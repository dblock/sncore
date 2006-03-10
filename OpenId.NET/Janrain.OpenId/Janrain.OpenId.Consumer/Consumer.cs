using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;

using Mono.Security.Cryptography;

using Janrain.OpenId.Store;

namespace Janrain.OpenId.Consumer
{
    public class ParseException : ApplicationException { }
    public class MissingParameterException : ApplicationException
    {
	public MissingParameterException(string key) : base(String.Format("NameValueCollection missing key: {0}", key))
	{
	}
    }

    public struct AuthRequest
    {
	public string token;
	public string nonce;
	public Uri serverId;
	public Uri serverUri;
    }


    /// <summary>
    ///  This class is the gateway to the OpenId consumer logic.
    ///  Instances of it maintain no per-request state, so they can be
    ///  reused (or even used by multiple threads concurrently) as needed.
    /// </summary>
    public class Consumer
    {
	public enum Mode {
	    IMMEDIATE,
	    SETUP
	}

	public enum Status {
	    SUCCESS,
	    FAILURE,
	    SETUP_NEEDED
	}

	static uint NONCE_LEN = 8;
	static byte[] NONCE_CHARS = new byte[62];
	static uint TOKEN_LIFETIME = 120;

	IAssociationStore store;
	Fetcher fetcher;

	static Consumer()
	{
	    uint i = 0;
	    uint j;
	    for (j = 97; j < 123; i++, j++)
		NONCE_CHARS[i] = (byte)j;

	    for (j = 65; j < 91; i++, j++)
		NONCE_CHARS[i] = (byte)j;

	    for (j = 48; j < 58; i++, j++)
		NONCE_CHARS[i] = (byte)j;
	}

	/// <summary>
	/// Initializes a new <see cref="Consumer"/> instance.
	/// </summary>
	public Consumer(IAssociationStore store, Fetcher fetcher)
	{
	    this.store = store;
	    this.fetcher = fetcher;
	}

	/// <summary>
	///  This method is called to start the OpenId login process.
	/// </summary>
	/// 
	/// <remarks>
	///  To create the <paramref name="userUrl"/> parameter from
	///  the text submitted by the user, use the NormalizeUri
	///  method of <see cref="OpenId.UriUtil" />.
	/// </remarks>
	/// 
	/// <param name="userUrl">
	///  This is the url the user entered as their OpenId. 
	/// </param>
	/// 
	/// <returns>
	///  This method returns an instance of
	///  <see cref="AuthRequest"/> representing the state of
	///  the authorization request.
	/// </returns>
	public AuthRequest BeginAuth(Uri userUrl)
	{
	    // Retrieve the users info from their openid page
	    FetchResponse response = this.fetcher.Get(userUrl);
	    AuthRequest request = ParseIdentityInfo(response);
            
	    // Create a nonce for this openid exchange
	    byte[] nonce = new byte[NONCE_LEN];
	    CryptUtil.RandomSelection(nonce, NONCE_CHARS);
	    request.nonce = ASCIIEncoding.ASCII.GetString(nonce);
	    
	    // Added a signed token to the request
	    GenToken(response.finalUri, ref request);
	    return request;
	}

	/// <summary>
	///  This method is called to construct the redirect URL sent
	///  to the browser to ask the server to verify its identity.
	///  The generated redirect should be sent to the browser
	///  which initiated the authorization request.
	/// </summary>
	///
	/// <param name="request">
	///  An instance of <see cref="AuthRequest"/> as returned
	///  from BeginAuth.
	/// </param>
	/// <param name="returnTo">
	///  The URL the identity server should redirect back to.
	/// </param>
	/// <param name="trustRoot">
	///  This represents the consumer to the identity server. For example,
	///  an ASP application would probably send an absolute URL using
	///  the Application path. The OpenId spec,
	///  http://www.openid.net/specs.bml#mode-checkid_immediate, 
	///  has more information on what the trust_root value is for
        ///  and what its form can be.
	/// </param>
	/// 
	/// <returns>
	///  This method returns a <see cref="System.Uri"/>
	///  representing the URL to redirect to when such a URL is
	///  successfully constructed.
	/// </returns>
	public string CreateRedirect(Mode mode, AuthRequest request, Uri returnTo, string trustRoot)
	{
	    Association assoc = GetAssociation(request.serverUri, true);

	    UriBuilder redir = new UriBuilder(request.serverUri);
	    UriUtil.AppendQueryArgument(redir, "openid.identity", request.serverId.AbsoluteUri);
	    UriUtil.AppendQueryArgument(redir, "openid.return_to", returnTo.AbsoluteUri);
	    UriUtil.AppendQueryArgument(redir, "openid.trust_root", trustRoot);

	    switch (mode) {
	    case Mode.IMMEDIATE:
		UriUtil.AppendQueryArgument(redir, "openid.mode", "checkid_immediate");
		break;
	    case Mode.SETUP:
		UriUtil.AppendQueryArgument(redir, "openid.mode", "checkid_setup");
		break;
	    }
	    
	    if (assoc != null)
		UriUtil.AppendQueryArgument(redir, "openid.assoc_handle", assoc.Handle);
	    
	    this.store.StoreNonce(request.nonce);

     return redir.ToString();
	}

	/// <summary>
	///  This method is called to interpret the server's response
	///  to an OpenID request.
	/// </summary>
	/// <param name="token">
	///  This is the token for this authentication transaction,
	///  generated by the call to BeginAuth.
	/// </param>
	/// <param name="query">
	///  Should contain the query parameters the OpenID server
	///  included in its redirect back to the return_to URL.  The
	///  keys and values should both be url-unescaped.
	/// </param>
	/// <param name="info">
	///  This output parameter is the data associated with the returned
	///  status.
	/// </param>
	/// <returns>
	///  When SUCCESS is returned, the additional information
	///  returned is either null or a <see cref="System.Uri"/>.
	///  If it is null, it means the user cancelled the login, and
	///  no further information can be determined.  If the
	///  additional information is a <see cref="System.Uri"/>, it
	///  is the identity that has been verified as belonging to
	///  the user making this request.
	///
        ///  When FAILURE is returned, the additional information is
        ///  either null or a <see cref="System.Uri"/>.  In either
        ///  case, this code means that the identity verification
        ///  failed.  If it can be determined, the identity that
        ///  failed to verify is returned. Otherwise null is returned.
	///  
        ///  When SETUP_NEEDED is returned, the additional information
        ///  is the user setup URL of type <see cref="System.Uri"/>.
        ///  This is a URL returned only as a response to requests
        ///  made with the IMMEDIATE mode, which indicates that the
        ///  login was unable to proceed, and the user should be sent
        ///  to that URL if they wish to proceed with the login.
	/// </returns>
	public Status CompleteAuth(string token, NameValueCollection query, out object info)
	{
	    string mode = query["openid.mode"];
	    if (mode != null)
	    {
		switch (mode) {
		case "cancel":
		    info = null;
		    return Status.SUCCESS;
		case "error":
		    string error = query["openid.error"];
		    if (error != null)
		    {
			// XXX: log this error
		    }
		    break;
		case "id_res":
		    return DoIdRes(token, query, out info);
		}
	    }
	    else
	    {
		// XXX: log this error
	    }
	    info = null;
	    return Status.FAILURE;
	}

	private Association GetAssociation(Uri serverUri, bool replace)
	{
	    if (this.store.IsDumb)
		return null;

	    Association assoc = this.store.GetAssociation(serverUri);
	    if (assoc == null || (replace && (assoc.ExpiresIn < TOKEN_LIFETIME)))
	    {
		DiffieHellman dh = CryptUtil.CreateDiffieHellman();
		byte[] body = CreateAssociateRequest(dh);
		assoc = FetchAssociation(dh, serverUri, body);
	    }
	    return assoc;
	}

	private void GenToken (Uri consumerId, ref AuthRequest request)
	{
	    string timestamp = DateTime.UtcNow.ToFileTimeUtc().ToString();

	    MemoryStream ms = new MemoryStream();
	    byte[] temp = ASCIIEncoding.ASCII.GetBytes(timestamp);
	    ms.Write(temp, 0, temp.Length);
	    ms.WriteByte(0);
            
	    temp = ASCIIEncoding.ASCII.GetBytes(request.nonce);
	    ms.Write(temp, 0, temp.Length);
	    ms.WriteByte(0);

	    temp = ASCIIEncoding.ASCII.GetBytes(consumerId.AbsoluteUri);
	    ms.Write(temp, 0, temp.Length);
	    ms.WriteByte(0);

	    temp = ASCIIEncoding.ASCII.GetBytes(request.serverId.AbsoluteUri);
	    ms.Write(temp, 0, temp.Length);
	    ms.WriteByte(0);

	    temp = ASCIIEncoding.ASCII.GetBytes(request.serverUri.AbsoluteUri);
	    ms.Write(temp, 0, temp.Length);

	    HMACSHA1 hmac = new HMACSHA1(this.store.AuthKey);
	    byte[] hash = hmac.ComputeHash(ms);
	    MemoryStream ms2 = new MemoryStream();
	    ms2.Write(hash, 0, hash.Length);
	    ms.WriteTo(ms2);
	    request.token = CryptUtil.ToBase64String(ms2.ToArray());
	}

	private Association FetchAssociation(DiffieHellman dh, Uri serverUri, byte[] body) 
	{
	    try {
		FetchResponse resp = this.fetcher.Post(serverUri, body);
		NameValueCollection results = KVUtil.KVToDict(resp.data);
		
		return ParseAssociation(results, dh, serverUri);
	    }
	    catch (FetchException e)
	    {
		if (e.response == null)
		{
		    // XXX: log network failure
		}
		else if (e.response.code == HttpStatusCode.BadRequest)
		{
		    // XXX: log this
		    /*
		      server_error = results.get('error', '<no message from server>')
		      fmt = 'Getting association: error returned from server %s: %s'
		      oidutil.log(fmt % (server_url, server_error))
		    */
		} 
		else 
		{
		    // XXX: log this
		    /*fmt = 'Getting association: bad status code from server %s: %s'
		      oidutil.log(fmt % (server_url, http_code))
		    */
		}
		return null;
	    }
	}

	private string GetParameter(NameValueCollection args, string key)
	{
	    string val = args[key];
	    if (val == null)
		throw new MissingParameterException(key);
	    return val;
	}
	
	protected Association ParseAssociation(NameValueCollection results, DiffieHellman dh, Uri serverUri)
	{
	    try
	    {
		if (GetParameter(results, "assoc_type") != "HMAC-SHA1")
		{
		    // XXX: log this
		    return null;
		}

		byte[] secret;
		string sessionType = results["session_type"];
		if (sessionType == null)
		{
		    string macKey = results["mac_key"];
		    if (macKey == null)
		    {
			// XXX: Log this
			return null;
		    }
		    secret = Convert.FromBase64String(macKey);
		}
		else
		{
		    if (sessionType != "DH-SHA1")
		    {
			// XXX: log this
			return null;
		    }

		    byte[] spub = Convert.FromBase64String(GetParameter(results, "dh_server_public"));
		    byte[] encMacKey = Convert.FromBase64String(GetParameter(results, "enc_mac_key"));
		    secret = CryptUtil.SHA1XorSecret(dh, spub, encMacKey);
		}
		
		string assocHandle = GetParameter(results, "assoc_handle");
		TimeSpan expiresIn = new TimeSpan(0, 0, Convert.ToInt32(GetParameter(results, "expires_in")));
		Association assoc = new HMACSHA1Association(assocHandle, secret, expiresIn);
		this.store.StoreAssociation(serverUri, assoc);

		return assoc;
	    }
	    catch (MissingParameterException e)
	    {
		// XXX: log this
		return null;
	    }
	}


	private Status DoIdRes (string token, NameValueCollection query, out object info)
	{
	    string nonce;
	    Uri consumerId, serverId, serverUri;
	    if ( ! SplitToken(token, out nonce, out consumerId, out serverId, out serverUri) )
	    {
		// XXX: log this
		info = null;
		return Status.FAILURE;
	    }
	    
	    try
	    {
		string userSetup = query["openid.user_setup_url"];
		if (userSetup != null)
		{
		    info = new Uri(userSetup);
		    return Status.SETUP_NEEDED;
		}
		
		GetParameter(query, "openid.return_to");
		string serverId2 = GetParameter(query, "openid.identity");
		string assocHandle = GetParameter(query, "openid.assoc_handle");

		if (serverId.AbsoluteUri != serverId2)
		{
		    // XXX: log this
		    info = null;
		    return Status.FAILURE;
		}

		Association assoc = this.store.GetAssociation(serverUri, assocHandle);
		if (assoc == null )
		{
		    // It's not an association we know about. Dumb
		    // mode is our only chance for recovery.
		    info = consumerId;
		    return CheckAuth(nonce, query, serverUri);
		}

		if (assoc.ExpiresIn <= 0)
		{
		    /*
		      XXX: It might be a good idea sometimes to
		      re-start the authentication with a new
		      association. Doing it automatically opens the
		      possibility for denial-of-service by a server
		      that just returns expired associations (or
		      really short-lived associations)
		     */
		    // XXX: Log this
		    info = consumerId;
		    return Status.FAILURE;
		}
		
		// Check the signature
		string sig = GetParameter(query, "openid.sig");
		string signed = GetParameter(query, "openid.signed");
		string[] signedArray = signed.Split(new char[] { ',', });
		string vSig = assoc.SignDict(signedArray, query, "openid.");
		if (vSig == sig)
		{
		    if (this.store.UseNonce(nonce))
		    {
			info = consumerId;
			return Status.SUCCESS;
		    }
		    else
		    {
			// XXX: log this
		    }
		}
		else
		{
		    // XXX: log this
		}
	    }
	    catch (MissingParameterException e)
	    { 
		// XXX: log this
	    }

	    info = consumerId;
	    return Status.FAILURE;
	}

	private bool SplitToken(string token, out string nonce, out Uri consumerId, out Uri serverId, out Uri serverUri)
	{
	    nonce = null;
	    consumerId = null;
	    serverId = null;
	    serverUri = null;
	    
	    byte[] tok = Convert.FromBase64String(token);
	    if (tok.Length < 21)
		return false;

	    byte[] sig = new byte[20];
	    for (uint i = 0; i < sig.Length; i++)
		sig[i] = tok[i];
	    
	    int idx, prev = 20;

	    byte delim = 0x00;

	    // Parse timestamp
	    if ((idx = Array.IndexOf(tok, delim, prev)) == -1) return false;
	    string timestamp = ASCIIEncoding.ASCII.GetString(
		tok, prev, idx - prev);
	    prev = idx + 1;

	    // Check if timestamp has expired
	    DateTime ts = DateTime.FromFileTimeUtc(Convert.ToInt64(timestamp));
	    ts += new TimeSpan(0, 0, (int) TOKEN_LIFETIME);
	    if (ts < DateTime.UtcNow)
	    {
		// XXX: log this
		return false;
	    }

	    // Parse nonce
	    if ((idx = Array.IndexOf(tok, delim, prev)) == -1)
	    {
		// XXX: log this
		return false;
	    }
	    nonce = ASCIIEncoding.ASCII.GetString(tok, prev, idx - prev);
	    prev = idx + 1;

	    // Parse consumerId
	    if ((idx = Array.IndexOf(tok, delim, prev)) == -1)
	    {
		// XXX: log this
		return false;
	    }
	    consumerId = new Uri(ASCIIEncoding.ASCII.GetString(tok, prev, idx - prev));
	    prev = idx + 1;

	    // Parse serverId
	    if ((idx = Array.IndexOf(tok, delim, prev)) == -1)
	    {
		// XXX: log this
		return false;
	    }
	    serverId = new Uri(ASCIIEncoding.ASCII.GetString(tok, prev, idx - prev));
	    prev = idx + 1;
	    
	    // Parse serverUri
	    serverUri = new Uri(ASCIIEncoding.ASCII.GetString(tok, prev, tok.Length - prev));

	    return true;
	}

	private Status CheckAuth(string nonce, NameValueCollection query, Uri serverUri)
	{
	    NameValueCollection checkArgs = new NameValueCollection();
	    foreach (string key in query.AllKeys)
	    {
		if (key.StartsWith("openid."))
		    checkArgs.Add(key, query[key]);
	    }
	    checkArgs["openid.mode"] = "check_authentication";
	    string postString = UriUtil.CreateQueryString(checkArgs);
	    byte[] postData = ASCIIEncoding.ASCII.GetBytes(postString);
	    
	    try 
	    {
		FetchResponse resp = this.fetcher.Post(serverUri, postData);
		NameValueCollection results = KVUtil.KVToDict(resp.data);
		string isValid = results["is_valid"];
		if (isValid == "true")
		{
		    string invalidateHandle = results["invalidate_handle"];
		    if (invalidateHandle != null)
			this.store.RemoveAssociation(serverUri, invalidateHandle);
		    
		    if (this.store.UseNonce(nonce))
			return Status.SUCCESS;
		}
		else
		{
		    string error = results["error"];
		    if (error != null)
		    {
			// XXX: Log this error string
		    }
		}
	    }
	    catch (FetchException e)
	    {
		// XXX: Log this fetch error
	    }

	    return Status.FAILURE;
	}


	/****************   Static Methods *****************/

	public static byte[] CreateAssociationRequest (DiffieHellman dh, NameValueCollection args)
	{
	    byte[] dhPublic = dh.CreateKeyExchange();
	    string cpub = CryptUtil.UnsignedToBase64(dhPublic);
	    DHParameters dhps = dh.ExportParameters(true);
	    
	    args.Add("openid.mode", "associate");
	    args.Add("openid.assoc_type", "HMAC-SHA1");
	    args.Add("openid.session_type", "DH-SHA1");
	    args.Add("openid.dh_modulus", CryptUtil.UnsignedToBase64(dhps.P));
	    args.Add("openid.dh_gen", CryptUtil.UnsignedToBase64(dhps.G));
	    args.Add("openid.dh_consumer_public", cpub);

	    return ASCIIEncoding.ASCII.GetBytes(UriUtil.CreateQueryString(args));
	}

	private static byte[] CreateAssociateRequest(DiffieHellman dh)
	{
	    NameValueCollection args = new NameValueCollection();
	    return CreateAssociationRequest(dh, args);
	}

	private static AuthRequest ParseIdentityInfo(FetchResponse response)
	{
	    string server = null;
	    string deleg = null;
	    string rel, href;
	    foreach (NameValueCollection attrs in LinkParser.ParseLinkAttrs(response.data, response.length, response.charset))
	    {
		rel = attrs["rel"];
		if (rel != null)
		{
		    href = attrs["href"];
		    if (rel == "openid.server" && server == null)
			if (href != null)
			    server = href;

		    if (rel == "openid.delegate" && deleg == null)
			if (href != null)
			    deleg = href;
		}
	    }

	    if (server == null)
		throw new ParseException();

	    AuthRequest request = new AuthRequest();

	    request.serverUri = UriUtil.NormalizeUri(server);

	    if (deleg == null)
		request.serverId = response.finalUri;
	    else
		request.serverId = UriUtil.NormalizeUri(deleg);

	    return request;
	}

    }
}

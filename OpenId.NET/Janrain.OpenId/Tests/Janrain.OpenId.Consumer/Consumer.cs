using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Mono.Security.Cryptography;

using Janrain.TestHarness;
using Janrain.OpenId;
using Janrain.OpenId.Consumer;
using Janrain.OpenId.Store;

namespace OpenIdTest.OpenId
{
    class AssociationInfo
    {
	public byte[] secret;
	public string handle;
	public AssociationInfo(byte[] secret, string handle)
	{
	    this.secret = secret;
	    this.handle = handle;
	}
    }

    class FormParser
    {
	private FormParser() {}

	private static void AddRawKeyValue(StringBuilder key, StringBuilder val, NameValueCollection form)
	{
	    form.Add (HttpUtility.UrlDecode(key.ToString (), Encoding.UTF8),
		      HttpUtility.UrlDecode(val.ToString (), Encoding.UTF8)
		      );

	    key.Length = 0;
	    val.Length = 0;
	}

	// Implementation taken from Mono's System.Web.HttpRequest
	public static NameValueCollection Parse(byte[] data)
	{
	    NameValueCollection form = new NameValueCollection();
	    Stream input = new MemoryStream(data);
	    StreamReader s = new StreamReader (input, Encoding.UTF8);
	    
	    StringBuilder key = new StringBuilder ();
	    StringBuilder value = new StringBuilder ();
	    int c;

	    while ((c = s.Read ()) != -1){
		if (c == '='){
		    value.Length = 0;
		    while ((c = s.Read ()) != -1){
			if (c == '&'){
			    AddRawKeyValue (key, value, form);
			    break;
			} else
			    value.Append ((char) c);
		    }
		    if (c == -1){
			AddRawKeyValue (key, value, form);
			return form;
		    }
		} else if (c == '&')
		    AddRawKeyValue (key, value, form);
		else
		    key.Append ((char) c);
	    }
	    if (c == -1)
		AddRawKeyValue (key, value, form);
	    
	    return form;
	}
	
    }

    class TestFetcher : Fetcher
    {
	public Hashtable getResponses;
	AssociationInfo assoc;
	
	public TestFetcher(Uri userUri, string userPage, AssociationInfo ainfo)
	{
	    this.assoc = ainfo;
	    this.getResponses = new Hashtable();

	    byte[] data = Encoding.UTF8.GetBytes(userPage);
	    FetchResponse resp = new FetchResponse(
		HttpStatusCode.OK, userUri, "UTF-8", data, data.Length);
	    this.getResponses.Add(userUri, resp);
	}

	private byte[] Associate(byte[] data)
	{
	    NameValueCollection q = FormParser.Parse(data);
	    TestTools.Assert(q.Count == 6);
	    TestTools.Assert(q["openid.mode"] == "associate");
	    TestTools.Assert(q["openid.assoc_type"] == "HMAC-SHA1");
	    TestTools.Assert(q["openid.session_type"] == "DH-SHA1");
	    
	    DiffieHellman d = new DiffieHellmanManaged(
	        Convert.FromBase64String(q["openid.dh_modulus"]),
		Convert.FromBase64String(q["openid.dh_gen"]), 1024);

	    byte[] enc_mac_key = CryptUtil.SHA1XorSecret(d, 
	        Convert.FromBase64String(q["openid.dh_consumer_public"]), 
		this.assoc.secret);

	    byte[] dhPublic = d.CreateKeyExchange();
	    string spub = CryptUtil.UnsignedToBase64(dhPublic);

	    NameValueCollection reply = new NameValueCollection();
	    reply.Add("assoc_type", "HMAC-SHA1");
	    reply.Add("assoc_handle", this.assoc.handle);
	    reply.Add("expires_in", "600");
	    reply.Add("session_type", "DH-SHA1");
	    reply.Add("dh_server_public", spub);
	    reply.Add("enc_mac_key", CryptUtil.ToBase64String(enc_mac_key));
	    return KVUtil.DictToKV(reply);
	}

	private FetchResponse Response(Uri url, byte[] data)
	{
	    if (data == null)
		throw new FetchException(
		    new FetchResponse(HttpStatusCode.NotFound, url, "UTF-8",
				      new byte[0], 0), "Not Found");
	    
	    return new FetchResponse(HttpStatusCode.OK, url, "UTF-8",
				     data, data.Length);
	}

        #region Fetcher Members

	public override FetchResponse Get(Uri uri, uint maxRead)
	{
	    FetchResponse ret = (FetchResponse) this.getResponses[uri];
	    if (ret == null)
		ret = Response(uri, null);
	    return ret;
	}

	public override FetchResponse Post(Uri uri, byte[] body, uint maxRead)
	{
	    if (Encoding.UTF8.GetString(body).IndexOf("openid.mode=associate") < 0)
		return Response(uri, null);
	    
	    return Response(uri, Associate(body));
	}

        #endregion
    }

    class BadFetcher : Fetcher
    {
	FetchResponse resp;
	string message;

	public BadFetcher(FetchResponse resp)
	{
	    this.resp = resp;
	    this.message = "barf";
	}

	public override FetchResponse Get(Uri uri, uint maxRead)
	{
	    throw new FetchException(this.resp, this.message);
	}
	
	public override FetchResponse Post(Uri uri, byte[] body, uint maxRead)
	{
	    throw new FetchException(this.resp, this.message);
	}
    }
    

    [TestSuite]
    public class ConsumerTestSuite
    {
	static string USER_PAGE_PAT = @"<html>
  <head>
    <title>A user page</title>
    {0}
  </head>
  <body>
    blah blah
  </body>
</html>";
	static Uri serverUri = new Uri("http://server.example.com/");
	static Uri consumerUri = new Uri("http://consumer.example.com/");

	private void Success(Uri userUri, Uri delegateUri, string links, Consumer.Mode immediate)
	{
	    MemoryStore store = new MemoryStore();
	    
	    string mode;
	    if (immediate == Consumer.Mode.IMMEDIATE)
		mode = "checkid_immediate";
	    else
		mode = "checkid_setup";

	    string userPage = String.Format(USER_PAGE_PAT, links);
	    string test_handle = "Snarky";
	    AssociationInfo info = new AssociationInfo(
                Encoding.ASCII.GetBytes("another 20-byte key."), test_handle);
	    Fetcher fetcher = new TestFetcher(userUri, userPage, info);

	    Consumer consumer = new Consumer(store, fetcher);

	    AuthRequest request = consumer.BeginAuth(userUri);
	    Uri returnTo = new Uri(consumerUri.AbsoluteUri, true);
	    string trustRoot = consumerUri.AbsoluteUri;
	    Uri redirectUri = consumer.CreateRedirect(immediate, request, returnTo, trustRoot);
	    
	    NameValueCollection q = FormParser.Parse(Encoding.UTF8.GetBytes(redirectUri.Query.Substring(1)));

	    string errmsg = redirectUri.AbsoluteUri;
	    TestTools.Assert(q.Count == 5, errmsg);
	    TestTools.Assert(q["openid.mode"] == mode, errmsg);
	    TestTools.Assert(q["openid.identity"] == delegateUri.AbsoluteUri, errmsg);
	    TestTools.Assert(q["openid.trust_root"] == trustRoot, errmsg);
	    TestTools.Assert(q["openid.assoc_handle"] == test_handle, errmsg);
	    TestTools.Assert(q["openid.return_to"] == returnTo.AbsoluteUri, errmsg);

	    TestTools.Assert(redirectUri.AbsoluteUri.StartsWith(serverUri.AbsoluteUri), errmsg);


	    NameValueCollection query = new NameValueCollection();
	    query.Add("openid.mode", "id_res");
	    query.Add("openid.return_to", returnTo.AbsoluteUri);
	    query.Add("openid.identity", delegateUri.AbsoluteUri);
	    query.Add("openid.assoc_handle", test_handle);

	    Association assoc = store.GetAssociation(serverUri, test_handle);
	    string sig = assoc.SignDict(new string[] { "mode", "return_to", "identity" }, query, "openid.");

	    query.Add("openid.sig", sig);
	    query.Add("openid.signed", "mode,return_to,identity");

	    object result;
	    Consumer.Status status = consumer.CompleteAuth(request.token, query, out result);
	    TestTools.Assert(status == Consumer.Status.SUCCESS);
	    TestTools.Assert(((Uri)result).AbsoluteUri == userUri.AbsoluteUri, String.Format("info:{0}\nuserUri:{1}", info, userUri));
	}

	[Test]
	public void Success()
	{
	    Uri userUri = new Uri("http://www.example.com/user.html");
	    string links = String.Format("<link rel=\"openid.server\" href=\"{0}\" />", serverUri);
	    
	    Uri delegateUri = new Uri("http://consumer.example.com/user");
	    string delegateLinks = String.Format(@"<link rel=""openid.server"" href=""{0}"" />
                <link rel=""openid.delegate"" href=""{1}"" />", serverUri, delegateUri);

	    Success(userUri, userUri, links, Consumer.Mode.SETUP);
	    Success(userUri, userUri, links, Consumer.Mode.IMMEDIATE);
	    Success(userUri, delegateUri, delegateLinks, Consumer.Mode.SETUP);
	    Success(userUri, delegateUri, delegateLinks, Consumer.Mode.IMMEDIATE);
	}

	[Test]
	public void BadFetch()
	{
	    Uri userUrl = new Uri("http://who.cares/");
	    IAssociationStore store = new MemoryStore();
	    Consumer consumer;
	    
	    
	    ArrayList cases = new ArrayList();
	    cases.Add(null);
	    cases.Add(HttpStatusCode.NotFound);
	    cases.Add(HttpStatusCode.BadRequest);
	    cases.Add(HttpStatusCode.InternalServerError);
	    	    
	    byte[] data = Encoding.UTF8.GetBytes("Who cares?");
	    FetchResponse resp;
	    foreach (object code in cases)
	    {
		if (code == null)
		    resp = null;
		else
		    resp = new FetchResponse((HttpStatusCode) code, userUrl, "UTF-8", data, data.Length);

		consumer = new Consumer(store, new BadFetcher(resp));
		try
		{
		    consumer.BeginAuth(userUrl);
		    TestTools.Assert(false, String.Format("Consumer failed to raise FetchException: {0}", code.ToString()));
		}
		catch (FetchException e) {}
	    }
	}

	[Test]
	public void BadParse()
	{
	    IAssociationStore store = new MemoryStore();
	    Uri userUrl = new Uri("http://user.example.com/");

	    string[] cases = 
		{ "", 
		  "http://not.in.a.link.tag/",
		  "<link rel=\"openid.server\" href=\"not.in.html.or.head\" />"
		};

	    Fetcher fetcher;
	    Consumer consumer;
	    foreach (string userPage in cases)
	    {
		fetcher = new TestFetcher(userUrl, userPage, null);
		consumer = new Consumer(store, fetcher);
		try {
		    consumer.BeginAuth(userUrl);
		    TestTools.Assert(false, String.Format("Shouldn't have succeeded with user_page=[{0}]", userPage));
		}
		catch (ParseException e) {}
		
	    }
	}


	class TestConsumer : Consumer
	{
	    static string CONSUMER_X = @"x/a0BNdiZWTdmJDCgDrjsPZFtIOaSMEi16u0W5LkExC3L+GHPbnJkFu/jjRTZXp5Lb7Q6FdaAonAgdpFRQbo7I8XdHrdCulFuz9+hv0mn5eqGamB27MosGZcZaNwhSNyTT6KY4DEpwX6ohlRxbofZWT7CFNAzUW8ike3/N/OgTA=";

	    static string CONSUMER_SPUB = @"AMwsoRFdgWDxtKRX40foZBCtnd50JT7+/MZcp6g3BNlwzz+4DN7eI5XQaqF52OKkDZPIy/2L/7PVAMhYxotXFHWyLprWoDELijzy6JmlqYDwK1UOmNqdzWo/mH+0PREjt1FbQfkda1YXcy10vLuFWfiIMHhCHew+uq9E9D8ErUEu";

	    static string CONSUMER_ENC_MAC_KEY = @"/8e7cN5TYwWibiKdvlhgx/q3Zq0=";

	    public TestConsumer (IAssociationStore store, Fetcher fetcher) : base (store, fetcher)
	    {
	    }
	    
	    public Association TestParseAssociation()
	    {
		NameValueCollection results = new NameValueCollection();
		results.Add("assoc_type", "HMAC-SHA1"); 
		results.Add("assoc_handle", "myhandle");
		results.Add("session_type", "DH-SHA1");
		results.Add("dh_server_public", CONSUMER_SPUB);
		results.Add("enc_mac_key", CONSUMER_ENC_MAC_KEY);
		results.Add("expires_in", "600");

		DiffieHellman dh = new DiffieHellmanManaged(CryptUtil.DEFAULT_MOD, CryptUtil.DEFAULT_GEN, Convert.FromBase64String(CONSUMER_X));
		return ParseAssociation(results, dh, new Uri("http://www.google.com/"));
	    }
	}

	class DumbStore : IAssociationStore
	{
	    public DumbStore()
	    {
	    }
	    
	    public byte[] AuthKey
	    {
		get { throw new NotImplementedException(); }
	    }
	    
	    public bool IsDumb
	    {
		get { return true; }
	    }

	    public void StoreAssociation(Uri serverUri, Association assoc)
	    {
	    }
	    
	    public Association GetAssociation(Uri serverUri)
	    {
		throw new NotImplementedException();
	    }
	    
	    public Association GetAssociation(Uri serverUri, string handle)
	    {
		throw new NotImplementedException();
	    }
	    
	    public bool RemoveAssociation(Uri serverUri, string handle)
	    {
		throw new NotImplementedException();
	    }
	    
	    public void StoreNonce(string nonce)
	    {
		throw new NotImplementedException();
	    }
	    
	    public bool UseNonce(string nonce)
	    {
		throw new NotImplementedException();
	    }
	}

	static string SERVER_SECRET = @"Md7XQVWxUOidZXpGWVmHFgKYqZs=";

	[Test]
	public void ParseAssociation()
	{
	    IAssociationStore store = new DumbStore();
	    TestConsumer consumer = new TestConsumer(store, new SimpleFetcher()); 
	    Association assoc = consumer.TestParseAssociation();
	    if (assoc == null)
	    {
		TestTools.Assert(false, "TestParseAssociation return a null association");
	    }
	    else
	    {
		string result = CryptUtil.ToBase64String(assoc.Secret);
		TestTools.Assert(SERVER_SECRET == result, result);
	    }
	}
    }
}

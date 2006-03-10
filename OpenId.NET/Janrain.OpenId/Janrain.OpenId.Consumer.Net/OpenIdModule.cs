using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Janrain.OpenId;
using Janrain.OpenId.Consumer;
using Janrain.OpenId.Store.Net;

namespace Janrain.OpenId.Consumer.Net
{
    public class OpenIdModule : IHttpModule
    {
	private static string TOKEN_KEY = "openidtoken";
	
	public void Init( HttpApplication context )
	{
            context.AuthenticateRequest += new 
                    EventHandler(this.AuthenticateRequest);
        }

	private void AuthenticateRequest(Object sender, EventArgs e)
	{
	    Consumer consumer =  new Consumer(new MemoryStore(), new SimpleFetcher());

	    HttpContext Context = HttpContext.Current;
	    HttpSessionState Session = Context.Session;
	    HttpRequest Request = Context.Request;
	    HttpResponse Response = Context.Response;

	    if (Request.Url.AbsolutePath != "/login.aspx")
		return;

	    if (Request.HttpMethod.ToLower() == "post")
	    {
		string urlStr = Request.Form["openid_url"];
		if (urlStr != null)
		{
		    Uri userUri = UriUtil.NormalizeUri(urlStr);
		    try
		    {
			AuthRequest oidreq = consumer.BeginAuth(userUri);
			
			// XXX: construct this from login path and ReturnTo param
			UriBuilder builder = new UriBuilder(Request.Url.AbsoluteUri);
			if (Session == null)
			    UriUtil.AppendQueryArgument(builder, TOKEN_KEY, oidreq.token);
			else
			    Session[TOKEN_KEY] = oidreq.token;

			Uri returnTo = new Uri(builder.ToString(), true);

			builder = new UriBuilder(Request.Url.AbsoluteUri);
			builder.Query = null;
			builder.Password = null;
			builder.UserName = null;
			builder.Fragment = null;
			builder.Path = Request.ApplicationPath;
			string trustRoot = builder.ToString();
			Uri redirectUri = consumer.CreateRedirect(Consumer.Mode.SETUP, oidreq, returnTo, trustRoot);
			Response.Redirect(redirectUri.AbsoluteUri);
		    }
		    catch (FetchException fe)
		    {
			Context.Items.Add("errmsg", "Failed to fetch identity page.");
		    }
		}
	    }
	    else if (Request.QueryString["openid.mode"] != null)
	    {
		// XXX: this needs to handle checkid_immediate
		string token;
		if (Session == null)
		    token = Request.QueryString[TOKEN_KEY];
		else
		{
		    token = (String) Session[TOKEN_KEY];
		    Session.Remove(TOKEN_KEY);
		}

		if (token == null)
		{
		    Context.Items.Add("errmsg", "Token was null.");
		}
		else
		{
		    object result;
		    Consumer.Status status = consumer.CompleteAuth(token, Request.QueryString, out result);
		    
		    switch (status)
		    {
		    case Consumer.Status.FAILURE:
			Context.Items.Add("errmsg", String.Format("Verification of {0} failed.", result.ToString()));
			break;
		    case Consumer.Status.SUCCESS:
			if (result != null)
			{
			    FormsAuthentication.RedirectFromLoginPage(
				((Uri) result).AbsoluteUri, false);
			    return;
			}
			else
			{
			    Context.Items.Add("errmsg", "Verification Cancelled.");
			}
			break;
		    }
		}
	    }
        }

	public void Dispose()
	{
	}
    }
}	
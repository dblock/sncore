using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SnCore.Tools.Web;

public class FacebookPageManager
{
    private SessionManager mSessionManager;

    public FacebookPageManager(SessionManager manager)
    {
        mSessionManager = manager;
    }

    public SortedList<string, string> GetFacebookCookies(HttpCookieCollection cookies)
    {
        SortedList<string, string> sortedCookies = new SortedList<string, string>();
        string cookiePrefix = FacebookAPIKey + "_";
        foreach (string cookieName in cookies)
        {
            if (cookieName.StartsWith(cookiePrefix))
            {
                var cookie = cookies[cookieName];
                sortedCookies.Add(cookie.Name.Substring(cookiePrefix.Length), cookie.Value);
            }
        }

        if (sortedCookies.Count == 0)
        {
            throw new Exception("Facebook Cookies not set.");
        }

        return sortedCookies;
    }

    public string FacebookAPIKey
    {
        get
        {
            return mSessionManager.GetCachedConfiguration("Facebook.APIKey", "");
        }
    }

    public string GetLoginUrl(string returnUrl)
    {
        return string.Format("http://www.facebook.com/login.php?api_key={0}&extern=1&fbconnect=1&req_perms=publish_stream,email&return_session=1&v=1.0&next={1}&fb_connect=1&cancel_url={1}",
            FacebookAPIKey, Renderer.UrlEncode(string.Format("{0}/FacebookConnect.aspx?connect=1&ReturnUrl={1}", mSessionManager.WebsiteUrl, Renderer.UrlEncode(returnUrl))));
    }
}

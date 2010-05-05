using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        return sortedCookies;
    }

    public string FacebookAPIKey
    {
        get
        {
            return mSessionManager.GetCachedConfiguration("Facebook.APIKey", "");
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using SnCore.Tools.Web;
using System.Web.Caching;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.BackEndServices;

public class SessionManager
{
    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";

    private Cache mCache = null;
    private HttpRequest mRequest = null;
    private HttpResponse mResponse = null;
    private string mTicket = string.Empty;
    private TransitAccountPermissions mAccountPermissions = null;
    private TransitAccount mAccount = null;

    private static object mLock = new Object();
    private static WebAccountService mAccountService = null;
    private static WebLocationService mWebLocationService = null;
    private static WebSystemService mWebSystemService = null;
    private static WebDiscussionService mWebDiscussionService = null;
    private static WebSocialService mWebSocialService = null;
    private static WebStoryService mWebStoryService = null;
    private static WebBugService mWebBugService = null;
    private static WebTagWordService mWebTagWordService = null;
    private static WebSyndicationService mWebSyndicationService = null;
    private static WebBackEndService mWebBackEndService = null;
    private static WebPlaceService mWebPlaceService = null;

    private string mWebsiteUrl = string.Empty;

    public Cache Cache
    {
        get
        {
            return mCache;
        }
    }

    public HttpRequest Request
    {
        get
        {
            return mRequest;
        }
    }

    public HttpResponse Response
    {
        get
        {
            return mResponse;
        }
    }

    public string WebsiteUrl
    {
        get
        {
            if (string.IsNullOrEmpty(mWebsiteUrl))
            {
                mWebsiteUrl = (string)Cache["settings:WebSiteUrl"];
                if (string.IsNullOrEmpty(mWebsiteUrl))
                {
                    Cache["settings:WebSiteUrl"] = mWebsiteUrl = SystemService.GetConfigurationByNameWithDefault(
                        "SnCore.WebSite.Url", "http://localhost/SnCoreWeb").Value;
                }
            }

            return mWebsiteUrl;
        }
    }

    public SessionManager(System.Web.UI.Page page)
        : this(page.Cache, page.Request, page.Response)
    {
    }

    public SessionManager(System.Web.UI.MasterPage page)
        : this(page.Cache, page.Request, page.Response)
    {
    }

    public SessionManager(Cache cache, HttpRequest request, HttpResponse response)
    {
        mCache = cache;
        mRequest = request;
        mResponse = response;

        HttpCookie authcookie = Request.Cookies[sSnCoreAuthCookieName];
        if (authcookie != null)
        {
            try
            {
                // cache a verified ticket for an hour
                mTicket = (string) Cache[string.Format("ticket:{0}", authcookie.Value)];
                if (string.IsNullOrEmpty(mTicket))
                {
                    AccountService.GetAccountId(authcookie.Value);
                    mTicket = authcookie.Value;
                    Cache.Insert(string.Format("ticket:{0}", authcookie.Value), 
                        mTicket, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }
            catch
            {
            }
        }
    }

    public DateTime Adjust(DateTime dt)
    {
        return dt.AddHours(UtcOffset);
    }

    public string AdjustToRFC822(DateTime dt)
    {
        return Adjust(dt).ToString("ddd, dd MMM yyyy HH:mm:ss") +
            " " + UtcOffset.ToString("00") + "00";
    }

    public string GetValue(string s, string defaultvalue)
    {
        return string.IsNullOrEmpty(s) ? defaultvalue : s;
    }

    public string GetValue(object s, string defaultvalue)
    {
        return (s == null) ? defaultvalue : GetValue(s.ToString(), defaultvalue);
    }

    public int UtcOffset
    {
        get
        {
            return IsLoggedIn ? Account.UtcOffset : TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
        }
    }

    public bool IsLoggedIn
    {
        get
        {
            return Account != null;
        }
    }

    public TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                if (! string.IsNullOrEmpty(Ticket))
                {
                    try
                    {
                        mAccount = (TransitAccount) Cache[string.Format("account:{0}", Ticket)];
                        if (mAccount == null)
                        {
                            mAccount = AccountService.GetAccount(Ticket);
                            Cache.Insert(string.Format("account:{0}", Ticket),
                                mAccount, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                        }
                    }
                    catch
                    {
                        
                    }
                }
            }
            return mAccount;
        }
    }

    public TransitAccountPermissions AccountPermissions
    {
        get
        {
            if (mAccountPermissions == null)
            {
                if (IsLoggedIn)
                {
                    mAccountPermissions = (TransitAccountPermissions)
                        Cache[string.Format("accountpermissions:{0}", Ticket)];

                    if (mAccountPermissions == null)
                    {
                        mAccountPermissions = AccountService.GetAccountPermissions(Ticket);
                        Cache.Insert(string.Format("accountpermissions:{0}", Ticket),
                            mAccountPermissions, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                    }
                }
            }
            return mAccountPermissions;
        }
    }

    public bool IsAdministrator
    {
        get
        {
            return AccountPermissions != null ? AccountPermissions.IsAdministrator : false;
        }
    }

    public string Ticket
    {
        get
        {
            return mTicket;
        }
    }

    public void Logout()
    {
        HttpCookie impersonateCookie = Request.Cookies[sSnCoreImpersonateCookieName];
        if (impersonateCookie != null)
        {
            Login(impersonateCookie.Value, false);
            Response.Cookies[sSnCoreImpersonateCookieName].Value = string.Empty;
            Response.Cookies[sSnCoreImpersonateCookieName].Expires = new DateTime(1970, 1, 1);
            return;
        }

        if (IsLoggedIn)
        {
            Cache.Remove(string.Format("ticket:{0}", Ticket));
            Cache.Remove(string.Format("account:{0}", Ticket));
            Cache.Remove(string.Format("accountpermissions:{0}", Ticket));
            Response.Cookies[sSnCoreAuthCookieName].Value = string.Empty;
            Response.Cookies[sSnCoreAuthCookieName].Expires = new DateTime(1970, 1, 1);
            mTicket = string.Empty;
            mAccount = null;
        }
    }

    public bool IsImpersonating
    {
        get
        {
            return Request.Cookies[sSnCoreImpersonateCookieName] != null;
        }
    }

    public void Impersonate(string newticket)
    {
        HttpCookie impersonateCookie = Request.Cookies[sSnCoreImpersonateCookieName];
        if (impersonateCookie != null)
        {
            throw new Exception("You are already impersonating a user. Logout first.");
        }

        Response.Cookies[sSnCoreImpersonateCookieName].Value = Ticket;

        Login(newticket, false);
    }

    public void Login(string ticket, bool rememberme)
    {
        HttpCookie c = new HttpCookie(sSnCoreAuthCookieName);
        c.Value = ticket;
        if (rememberme)
        {
            c.Expires = DateTime.Today.AddYears(1);
        }
        Response.Cookies.Add(c);
    }

    public string OpenIdToken
    {
        get
        {
            HttpCookie c = Request.Cookies[sSnCoreOpenIdTokenCookieName];
            if (c == null) return null;
            return c.Value;
        }
        set
        {
            HttpCookie c = new HttpCookie(sSnCoreOpenIdTokenCookieName);
            c.Value = value;
            Response.Cookies.Add(c);
        }
    }

    public bool RememberLogin
    {
        get
        {
            HttpCookie c = Request.Cookies[sSnCoreRememberLogin];
            if (c == null) return false;
            return bool.Parse(c.Value);
        }
        set
        {
            HttpCookie c = new HttpCookie(sSnCoreRememberLogin);
            c.Value = value.ToString();
            Response.Cookies.Add(c);
        }
    }

    public static WebAccountService AccountService
    {
        get
        {
            lock (mLock)
            {
                if (mAccountService == null)
                {
                    mAccountService = new WebAccountService();
                }
            }
            return mAccountService;
        }
    }

    public static WebBugService BugService
    {
        get
        {
            lock (mLock)
            {
                if (mWebBugService == null)
                {
                    mWebBugService = new WebBugService();
                }
            }
            return mWebBugService;
        }
    }

    public static WebTagWordService TagWordService
    {
        get
        {
            lock (mLock)
            {

                if (mWebTagWordService == null)
                {
                    mWebTagWordService = new WebTagWordService();
                }
            }
            return mWebTagWordService;
        }
    }

    public static WebSocialService SocialService
    {
        get
        {
            lock (mLock)
            {

                if (mWebSocialService == null)
                {
                    mWebSocialService = new WebSocialService();
                }
            }
            return mWebSocialService;
        }
    }

    public static WebStoryService StoryService
    {
        get
        {
            lock (mLock)
            {

                if (mWebStoryService == null)
                {
                    mWebStoryService = new WebStoryService();
                }
            }
            return mWebStoryService;
        }
    }

    public static WebLocationService LocationService
    {
        get
        {
            lock (mLock)
            {

                if (mWebLocationService == null)
                {
                    mWebLocationService = new WebLocationService();
                }
            }
            return mWebLocationService;
        }
    }

    public static WebSystemService SystemService
    {
        get
        {
            lock (mLock)
            {
                if (mWebSystemService == null)
                {
                    mWebSystemService = new WebSystemService();
                }
            }
            return mWebSystemService;
        }
    }

    public static WebDiscussionService DiscussionService
    {
        get
        {
            lock (mLock)
            {

                if (mWebDiscussionService == null)
                {
                    mWebDiscussionService = new WebDiscussionService();
                }
            }
            return mWebDiscussionService;
        }
    }

    public static WebSyndicationService SyndicationService
    {
        get
        {
            lock (mLock)
            {
                if (mWebSyndicationService == null)
                {
                    mWebSyndicationService = new WebSyndicationService();
                }
            }
            return mWebSyndicationService;
        }
    }

    public static WebBackEndService BackEndService
    {
        get
        {
            lock (mLock)
            {
                if (mWebBackEndService == null)
                {
                    mWebBackEndService = new WebBackEndService();
                }
            }
            return mWebBackEndService;
        }
    }

    public static WebPlaceService PlaceService
    {
        get
        {
            lock (mLock)
            {
                if (mWebPlaceService == null)
                {
                    mWebPlaceService = new WebPlaceService();
                }
            }
            return mWebPlaceService;
        }
    }

    static Regex EncodedCommentExpression = new Regex(@"^\&gt;(.*)", RegexOptions.Multiline);
    static Regex CommentExpression = new Regex(@"^>(.*)", RegexOptions.Multiline);

    private string CommentHandler(Match ParameterMatch)
    {
        return "<div class='sncore_post_quote'>" + ParameterMatch.Value + "</div>";
    }

    private string DeleteCommentHandler(Match ParameterMatch)
    {
        return string.Empty;
    }

    static Regex MarkupExpression = new Regex(@"(?<tag>[\[]+)(?<name>[\w\s]*):(?<value>[\w\s\']*)[\]]+", 
        RegexOptions.IgnoreCase);

    private string ReferenceHandler(Match ParameterMatch)
    {
        string tag = ParameterMatch.Groups["tag"].Value;
        string city = ParameterMatch.Groups["name"].Value;
        string name = ParameterMatch.Groups["value"].Value;

        if (tag == "[[")
        {
            return string.Format("[{0}:{1}]", city, name);
        }
        else
        {
            TransitPlace p = PlaceService.FindPlace(
                city, name);

            if (p == null)
            {
                return string.Format("<a href=\"{3}/PlaceView.aspx?city={0}&name={1}\">{2}</a>",
                    Renderer.UrlEncode(city), Renderer.UrlEncode(name), Renderer.Render(name), WebsiteUrl);
            }
            else
            {
                return string.Format("<a href=\"{2}/PlaceView.aspx?id={0}\">{1}</a>",
                    p.Id, Renderer.Render(p.Name), WebsiteUrl);
            }
        }
    }

    public string RenderMarkups(string s)
    {
        MatchEvaluator mhd = new MatchEvaluator(ReferenceHandler);
        return MarkupExpression.Replace(s, mhd);
    }

    public string RenderComments(string s)
    {
        MatchEvaluator mev = new MatchEvaluator(CommentHandler);
        return EncodedCommentExpression.Replace(s, mev);
    }

    public string DeleteComments(string s)
    {
        MatchEvaluator mev = new MatchEvaluator(DeleteCommentHandler);
        string result = CommentExpression.Replace(s, mev);

        for (int i = result.Length - 1; i > 0; i--)
        {
            if (result[i] == '\n' && result[i - 1] == '\n')
            {
                result = result.Remove(i, 1);
            }
        }

        return result;
    }


    public string MarkupClearHandler(Match ParameterMatch)
    {
        string tag = ParameterMatch.Groups["tag"].Value;
        string city = ParameterMatch.Groups["name"].Value;
        string name = ParameterMatch.Groups["value"].Value;

        if (tag == "[[")
        {
            return string.Format("[{0}:{1}]", city, name);
        }
        else
        {
            return name;
        }
    }

    public string ClearMarkups(string s)
    {
        MatchEvaluator mhd = new MatchEvaluator(MarkupClearHandler);
        return Renderer.ClearMarkups(MarkupExpression.Replace(s, mhd));
    }

    public static bool ShowAds
    {
        get
        {
            object ads = ConfigurationManager.AppSettings["Ads.Visible"];
            return (ads == null) || bool.Parse(ads.ToString());
        }
    }

    public static bool ShowCounter
    {
        get
        {
            object counter = ConfigurationManager.AppSettings["Counter.Visible"];
            return (counter == null) || bool.Parse(counter.ToString());
        }
    }
}

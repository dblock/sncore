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
using System.Collections.Generic;
using System.Text;

public class SessionManager
{
    static TimeSpan s_RequestCommitInterval = new TimeSpan(0, 1, 0); // commit every minute
    private static DateTime s_RequestsLastCommit = DateTime.UtcNow;
    private static List<TransitStatsRequest> s_Requests = new List<TransitStatsRequest>(1024);
    public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(0, 5, 0);

    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";
    const string sSnCoreLastVisit = "SnCore.lastvisit";

    private Cache mCache = null;
    private HttpRequest mRequest = null;
    private HttpResponse mResponse = null;
    private string mTicket = string.Empty;
    private TransitAccountPermissions mAccountPermissions = null;
    private TransitAccount mAccount = null;

    private WebContentService mContentService = null;
    private WebAccountService mAccountService = null;
    private WebLocationService mWebLocationService = null;
    private WebSystemService mWebSystemService = null;
    private WebDiscussionService mWebDiscussionService = null;
    private WebSocialService mWebSocialService = null;
    private WebStoryService mWebStoryService = null;
    private WebBugService mWebBugService = null;
    private WebTagWordService mWebTagWordService = null;
    private WebSyndicationService mWebSyndicationService = null;
    private WebBackEndService mWebBackEndService = null;
    private WebPlaceService mWebPlaceService = null;
    private WebBlogService mWebBlogService = null;
    private WebEventService mWebEventService = null;
    private WebStatsService mWebStatsService = null;

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
            return GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/SnCoreWeb");
        }
    }

    public string GetCachedConfiguration(string name, string defaultvalue)
    {
        object[] args = { name, defaultvalue };
        return GetCachedItem<TransitConfiguration>(SystemService, "GetConfigurationByNameWithDefault", args).Value.ToString();
    }

    public SessionManager(System.Web.UI.Page page)
        : this(page.Cache, page.Request, page.Response, true)
    {
    }

    public SessionManager(System.Web.UI.MasterPage page)
        : this(page.Cache, page.Request, page.Response, true)
    {
    }

    public SessionManager(PicturePage page)
        : this(page.Cache, page.Request, page.Response, false)
    {
    }

    public SessionManager(Cache cache, HttpRequest request, HttpResponse response, bool track)
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
                mTicket = (string)Cache[string.Format("ticket:{0}", authcookie.Value)];
                if (string.IsNullOrEmpty(mTicket))
                {
                    AccountService.GetAccountId(authcookie.Value);
                    mTicket = authcookie.Value;
                    Cache.Insert(string.Format("ticket:{0}", authcookie.Value),
                        mTicket, null, Cache.NoAbsoluteExpiration, SessionManager.DefaultCacheTimeSpan);
                }
            }
            catch
            {
            }
        }

        if (track)
        {
            Track(request, response);
        }
    }

    private void Track(HttpRequest request, HttpResponse response)
    {
        Nullable<DateTime> lastVisit = GetLastVisit(request);

        if ((!lastVisit.HasValue) || (lastVisit.Value.AddDays(1) < DateTime.UtcNow))
        {
            // cookie doesn't exist or exists with an invalid value
            // cookie older than a day
            HttpCookie lastVisitCookie = new HttpCookie(sSnCoreLastVisit, DateTime.UtcNow.ToString());
            lastVisitCookie.Expires = DateTime.UtcNow.AddYears(1);
            response.Cookies.Add(lastVisitCookie);
        }

        TransitStatsRequest tsr = new TransitStatsRequest(request, lastVisit);

        lock (s_Requests)
        {
            s_Requests.Add(tsr);
        }

        if (s_RequestsLastCommit.Add(s_RequestCommitInterval) >= tsr.Timestamp)
            return;

        // commit tracked requests
        lock (s_Requests)
        {
            if (s_RequestsLastCommit.Add(s_RequestCommitInterval) < tsr.Timestamp)
            {
                try
                {
                    StatsService.TrackMultipleRequests(s_Requests.ToArray());
                }
                finally
                {
                    s_Requests.Clear();
                    s_RequestsLastCommit = tsr.Timestamp;
                }
            }
        }
    }

    protected Nullable<DateTime> GetLastVisit(HttpRequest request)
    {
        // track request
        Nullable<DateTime> lastVisit = new Nullable<DateTime>();
        HttpCookie lastVisitCookie = request.Cookies[sSnCoreLastVisit];
        if (lastVisitCookie != null)
        {
            DateTime lvcDt;
            if (DateTime.TryParse(request.Cookies[sSnCoreLastVisit].Value, out lvcDt))
            {
                lastVisit = lvcDt;
            }
        }

        return lastVisit;
    }

    public DateTime Adjust(DateTime dt)
    {
        return dt.AddHours(UtcOffset);
    }

    public DateTime ToUTC(DateTime dt)
    {
        return dt.AddHours(-UtcOffset);
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
            return IsLoggedIn ? Account.UtcOffset : System.TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
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
                if (!string.IsNullOrEmpty(Ticket))
                {
                    try
                    {
                        mAccount = AccountService.GetAccount(Ticket);
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
                    object[] args = { Ticket };
                    mAccountPermissions = GetCachedItem<TransitAccountPermissions>(AccountService, "GetAccountPermissions", args);
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

    public WebContentService ContentService
    {
        get
        {
            if (mContentService == null)
            {
                mContentService = (WebContentService)HttpContext.Current.Cache["SnCore.SessionManager.ContentService"];
                if (mContentService == null)
                {
                    mContentService = new WebContentService();
                    HttpContext.Current.Cache["SnCore.SessionManager.ContentService"] = mContentService;
                }
            }
            return mContentService;
        }
    }

    public WebAccountService AccountService
    {
        get
        {
            if (mAccountService == null)
            {
                mAccountService = (WebAccountService) HttpContext.Current.Cache["SnCore.SessionManager.AccountService"];
                if (mAccountService == null)
                {
                    mAccountService = new WebAccountService();
                    HttpContext.Current.Cache["SnCore.SessionManager.AccountService"] = mAccountService;
                }
            }
            return mAccountService;
        }
    }

    public WebBugService BugService
    {
        get
        {
            if (mWebBugService == null)
            {
                mWebBugService = (WebBugService)HttpContext.Current.Cache["SnCore.SessionManager.BugService"];
                if (mWebBugService == null)
                {
                    mWebBugService = new WebBugService();
                    HttpContext.Current.Cache["SnCore.SessionManager.BugService"] = mWebBugService;
                }
            }
            return mWebBugService;
        }
    }

    public WebStatsService StatsService
    {
        get
        {
            if (mWebStatsService == null)
            {
                mWebStatsService = (WebStatsService)HttpContext.Current.Cache["SnCore.SessionManager.StatsService"];
                if (mWebStatsService == null)
                {
                    mWebStatsService = new WebStatsService();
                    HttpContext.Current.Cache["SnCore.SessionManager.StatsService"] = mWebStatsService;
                }
            }
            return mWebStatsService;
        }
    }

    public WebTagWordService TagWordService
    {
        get
        {
            if (mWebTagWordService == null)
            {
                mWebTagWordService = (WebTagWordService)HttpContext.Current.Cache["SnCore.SessionManager.TagWordService"];
                if (mWebTagWordService == null)
                {
                    mWebTagWordService = new WebTagWordService();
                    HttpContext.Current.Cache["SnCore.SessionManager.TagWordService"] = mWebTagWordService;
                }
            }

            return mWebTagWordService;
        }
    }

    public WebSocialService SocialService
    {
        get
        {
            if (mWebSocialService == null)
            {
                mWebSocialService = (WebSocialService)HttpContext.Current.Cache["SnCore.SessionManager.SocialService"];
                if (mWebSocialService == null)
                {
                    mWebSocialService = new WebSocialService();
                    HttpContext.Current.Cache["SnCore.SessionManager.SocialService"] = mWebSocialService;
                }
            }
            return mWebSocialService;
        }
    }

    public WebStoryService StoryService
    {
        get
        {
            if (mWebStoryService == null)
            {
                mWebStoryService = (WebStoryService)HttpContext.Current.Cache["SnCore.SessionManager.StoryService"];
                if (mWebStoryService == null)
                {
                    mWebStoryService = new WebStoryService();
                    HttpContext.Current.Cache["SnCore.SessionManager.StoryService"] = mWebStoryService;
                }
            }

            return mWebStoryService;
        }
    }

    public WebLocationService LocationService
    {
        get
        {
            if (mWebLocationService == null)
            {
                mWebLocationService = (WebLocationService)HttpContext.Current.Cache["SnCore.SessionManager.LocationService"];
                if (mWebLocationService == null)
                {
                    mWebLocationService = new WebLocationService();
                    HttpContext.Current.Cache["SnCore.SessionManager.LocationService"] = mWebLocationService;
                }
            }
            return mWebLocationService;
        }
    }

    public WebSystemService SystemService
    {
        get
        {
            if (mWebSystemService == null)
            {
                mWebSystemService = (WebSystemService)HttpContext.Current.Cache["SnCore.SessionManager.SystemService"];
                if (mWebSystemService == null)
                {
                    mWebSystemService = new WebSystemService();
                    HttpContext.Current.Cache["SnCore.SessionManager.SystemService"] = mWebSystemService;
                }
            }
            return mWebSystemService;
        }
    }

    public WebEventService EventService
    {
        get
        {
            if (mWebEventService == null)
            {
                mWebEventService = (WebEventService)HttpContext.Current.Cache["SnCore.SessionManager.EventService"];
                if (mWebEventService == null)
                {
                    mWebEventService = new WebEventService();
                    HttpContext.Current.Cache["SnCore.SessionManager.EventService"] = mWebEventService;
                }
            }
            return mWebEventService;
        }
    }

    public WebDiscussionService DiscussionService
    {
        get
        {
            if (mWebDiscussionService == null)
            {
                mWebDiscussionService = (WebDiscussionService)HttpContext.Current.Cache["SnCore.SessionManager.DiscussionService"];
                if (mWebDiscussionService == null)
                {
                    mWebDiscussionService = new WebDiscussionService();
                    HttpContext.Current.Cache["SnCore.SessionManager.DiscussionService"] = mWebDiscussionService;
                }
            }
            return mWebDiscussionService;
        }
    }

    public WebSyndicationService SyndicationService
    {
        get
        {
            if (mWebSyndicationService == null)
            {
                mWebSyndicationService = (WebSyndicationService)HttpContext.Current.Cache["SnCore.SessionManager.SyndicationService"];
                if (mWebSyndicationService == null)
                {
                    mWebSyndicationService = new WebSyndicationService();
                    HttpContext.Current.Cache["SnCore.SessionManager.SyndicationService"] = mWebSyndicationService;
                }
            }
            return mWebSyndicationService;
        }
    }

    public WebBackEndService BackEndService
    {
        get
        {
            if (mWebBackEndService == null)
            {
                mWebBackEndService = (WebBackEndService)HttpContext.Current.Cache["SnCore.SessionManager.BackEndService"];
                if (mWebBackEndService == null)
                {
                    mWebBackEndService = new WebBackEndService();
                    HttpContext.Current.Cache["SnCore.SessionManager.BackEndService"] = mWebBackEndService;
                }
            }
            return mWebBackEndService;
        }
    }

    public WebPlaceService PlaceService
    {
        get
        {
            if (mWebPlaceService == null)
            {
                mWebPlaceService = (WebPlaceService)HttpContext.Current.Cache["SnCore.SessionManager.PlaceService"];
                if (mWebPlaceService == null)
                {
                    mWebPlaceService = new WebPlaceService();
                    HttpContext.Current.Cache["SnCore.SessionManager.PlaceService"] = mWebPlaceService;
                }
            }
            return mWebPlaceService;
        }
    }

    public WebBlogService BlogService
    {
        get
        {
            if (mWebBlogService == null)
            {
                mWebBlogService = (WebBlogService)HttpContext.Current.Cache["SnCore.SessionManager.BlogService"];
                if (mWebBlogService == null)
                {
                    mWebBlogService = new WebBlogService();
                    HttpContext.Current.Cache["SnCore.SessionManager.BlogService"] = mWebBlogService;
                }
            }
            return mWebBlogService;
        }
    }

    static Regex CommentExpression = new Regex(@"^>(.*)", RegexOptions.Multiline);

    private string CommentHandler(Match ParameterMatch)
    {
        return "<span class='sncore_post_quote'>" + ParameterMatch.Value + "</span>";
    }

    private string DeleteCommentHandler(Match ParameterMatch)
    {
        return string.Empty;
    }

    static Regex MarkupExpression = new Regex(@"(?<tag>[\[]+)(?<name>[\w\s]*):(?<value>[\w\s\'\-!]*)[\]]+",
        RegexOptions.IgnoreCase);

    private string ReferenceHandler(Match ParameterMatch)
    {
        string tag = ParameterMatch.Groups["tag"].Value;
        string tagname = ParameterMatch.Groups["name"].Value.Trim();
        string tagvalue = ParameterMatch.Groups["value"].Value.Trim();

        if (tag == "[[")
        {
            return string.Format("[{0}:{1}]", tagname, tagvalue);
        }
        else if ((tagname == "user") || (tagname == "account"))
        {
            int userid = 0;
            if (int.TryParse(tagvalue, out userid))
            {
                object[] args = { userid };
                TransitAccount a = GetCachedItem<TransitAccount>(AccountService, "GetAccountById", args);

                if (a != null)
                {
                    return string.Format("<a href=\"{0}/AccountView.aspx?id={1}\">{2}</a>",
                        WebsiteUrl, a.Id, Renderer.Render(a.Name));
                }
            }

            return string.Format("[invalid user: {0}]", tagvalue);
        }
        else
        {
            object[] args = { tagname, tagvalue };
            TransitPlace p = GetCachedItem<TransitPlace>(PlaceService, "FindPlace", args);

            if (p == null)
            {
                return string.Format("<a href=\"{3}/PlaceView.aspx?city={0}&name={1}\">{2}</a>",
                    Renderer.UrlEncode(tagname), Renderer.UrlEncode(tagvalue), Renderer.Render(tagvalue), WebsiteUrl);
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
        if (string.IsNullOrEmpty(s)) return string.Empty;
        MatchEvaluator mhd = new MatchEvaluator(ReferenceHandler);
        return MarkupExpression.Replace(s, mhd);
    }

    public string RenderComments(object s)
    {
        if (s == null) return string.Empty;
        return RenderComments(s.ToString());
    }

    public string RenderComments(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        MatchEvaluator mev = new MatchEvaluator(CommentHandler);
        return CommentExpression.Replace(s, mev);
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

    public string RemoveMarkups(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        MatchEvaluator mhd = new MatchEvaluator(MarkupClearHandler);
        return Renderer.RemoveMarkups(MarkupExpression.Replace(s, mhd));
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

    public string TimeZone
    {
        get
        {
            switch (UtcOffset)
            {
                case -12: return "(GMT -12:00 hours)";
                case -11: return "(GMT -11:00 hours)";
                case -10: return "(GMT -10:00 hours) Hawaii";
                case -9: return "(GMT -9:00 hours) Alaska";
                case -8: return "(GMT -8:00 hours) Pacific Time (US & Canada)";
                // case -7: return "(GMT -7:00 hours) Arizona";
                case -7: return "(GMT -7:00 hours) Mountain Standard Time (US & Canada)";
                case -6: return "(GMT -6:00 hours) Central Time (US & Canada)";
                case -5: return "(GMT -5:00 hours) Eastern Time (US & Canada)";
                case -4: return "(GMT -4:00 hours) Atlantic Time (Canada)";
                case -3: return "(GMT -3:00 hours)";
                case -2: return "(GMT -2:00 hours) Mid-Atlantic";
                case -1: return "(GMT -1:00 hours)";
                case 0: return "(GMT) Western Europe Time, London";
                case 1: return "(GMT +1:00 hours) CET(Central Europe Time)";
                case 2: return "(GMT +2:00 hours) EET(Eastern Europe Time)";
                case 3: return "(GMT +3:00 hours) Moscow";
                case 4: return "(GMT +4:00 hours)";
                case 5: return "(GMT +5:00 hours)";
                case 6: return "(GMT +6:00 hours)";
                case 7: return "(GMT +7:00 hours) Bangkok, Jakarta";
                case 8: return "(GMT +8:00 hours) Singapore";
                case 9: return "(GMT +9:00 hours) Tokyo";
                case 10: return "(GMT +10:00 hours)";
                case 11: return "(GMT +11:00 hours)";
                case 12: return "(GMT +12:00 hours)";
            }

            return string.Empty;
        }
    }

    private static string GetCacheKey(string invoke, object[] args)
    {
        StringBuilder key = new StringBuilder(invoke);

        if (args != null)
        {
            foreach (object arg in args)
            {
                key.Append(":");
                key.Append(arg == null ? string.Empty : arg.GetHashCode().ToString());
            }
        }

        return key.ToString();
    }

    public List<TransitType> GetCachedCollection<TransitType>(
        WebService service, string invoke, object[] args)
    {
        return GetCachedCollection<TransitType>(service, invoke, args, DefaultCacheTimeSpan);
    }

    public List<TransitType> GetCachedCollection<TransitType>(
        WebService service, string invoke, object[] args, TimeSpan cacheduration)
    {
        string key = GetCacheKey(invoke, args);
        List<TransitType> result = (List<TransitType>)Cache[key];
        if (result == null || IsAdministrator)
        {
            result = (List<TransitType>)service.GetType().GetMethod(invoke).Invoke(service, args);
            if (result != null)
            {
                Cache.Insert(key, result, null, Cache.NoAbsoluteExpiration, cacheduration);
            }
        }

        return result;
    }

    public int GetCachedCollectionCount(
        WebService service, string invoke, object[] args)
    {
        return GetCachedCollectionCount(service, invoke, args, DefaultCacheTimeSpan);
    }

    public int GetCachedCollectionCount(
        WebService service, string invoke, object[] args, TimeSpan cacheduration)
    {
        string key = GetCacheKey(invoke, args);
        object count = Cache[key];
        if (count == null || IsAdministrator)
        {
            count = service.GetType().GetMethod(invoke).Invoke(service, args);
            Cache.Insert(key, count, null, Cache.NoAbsoluteExpiration, cacheduration);
        }
        return (int)count;
    }

    public TransitType GetCachedItem<TransitType>(
        WebService service, string invoke, object[] args)
    {
        return GetCachedItem<TransitType>(service, invoke, args, DefaultCacheTimeSpan);
    }

    public TransitType GetCachedItem<TransitType>(
        WebService service, string invoke, object[] args, TimeSpan cacheduration)
    {
        string key = GetCacheKey(invoke, args);
        TransitType result = (TransitType) Cache[key];
        if (result == null || IsAdministrator)
        {
            result = (TransitType)service.GetType().GetMethod(invoke).Invoke(service, args);
            if (result != null)
            {
                Cache.Insert(key, result, null, Cache.NoAbsoluteExpiration, cacheduration);
            }
        }

        return result;
    }
}

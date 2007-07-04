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
using System.Reflection;
using SnCore.Tools;
using System.Diagnostics;
using System.Web.Hosting;
using SnCore.Data.Hibernate;

public class SessionManager
{
    static TimeSpan s_RequestCommitInterval = new TimeSpan(0, 1, 0); // commit every three minutes
    private static DateTime s_RequestsLastCommit = DateTime.UtcNow;
    private static List<TransitStatsRequest> s_Requests = new List<TransitStatsRequest>(1024);
    public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(0, 5, 0);

    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";
    const string sSnCoreLastVisit = "SnCore.lastvisit";
    const string sSnCoreLastMonthVisit = "SnCore.lastmonthvisit";

    private EventLog mEventLog = null;
    private Cache mCache = null;
    private HttpRequest mRequest = null;
    private HttpResponse mResponse = null;
    private string mTicket = string.Empty;
    private TransitAccount mAccount = null;

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

    public Uri WebsiteUri
    {
        get
        {
            return new Uri(WebsiteUrl.TrimEnd("/".ToCharArray()) + "/");
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
        return GetInstance<TransitConfiguration, string, string>(
            name, defaultvalue, SystemService.GetConfigurationByNameWithDefault).Value;
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
        : this(page.Cache, page.Request, page.Response, page.PageType == PicturePage.PicturePageType.Bitmap)
    {

    }

    public SessionManager(Cache cache, HttpRequest request, HttpResponse response, bool track)
    {
        mCache = cache;
        mRequest = request;
        mResponse = response;

        CacheAuthCookie();

        if (!track || IsSystemRequest())
            return;

        Track(request, response);
    }

    private bool IsSystemRequest()
    {
        return (Request.UserAgent == GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0"));
    }

    private void CacheAuthCookie()
    {
        // there can be multiple cookies with the same name acceptable for this domain
        for (int i = 0; i < Request.Cookies.AllKeys.Length; i++)
        {
            HttpCookie authcookie = Request.Cookies[i];
            if (authcookie.Name != sSnCoreAuthCookieName)
                continue;

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

                // ticket was valid, account was valid
                break;
            }
            catch
            {
            }
        }
    }

    #region Login and Impersonate

    public bool IsLoggedIn
    {
        get
        {
            return Account != null;
        }
    }

    public int AccountId
    {
        get
        {
            if (!IsLoggedIn)
            {
                throw new Exception("User is not Logged In");
            }

            return Account.Id;
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
                        mAccount = (TransitAccount)Cache[string.Format("account:{0}", Ticket)];
                        if (mAccount == null)
                        {
                            mAccount = AccountService.GetAccount(Ticket, ! IsSystemRequest());
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

    public bool IsAdministrator
    {
        get
        {
            return Account != null ? Account.IsAdministrator : false;
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

    #endregion

    #region Web Services

    public WebContentService ContentService
    {
        get
        {
            return CachedWebService<WebContentService>.GetEndPoint(Cache);
        }
    }

    public WebLicenseService LicenseService
    {
        get
        {
            return CachedWebService<WebLicenseService>.GetEndPoint(Cache);
        }
    }

    public WebAccountService AccountService
    {
        get
        {
            return CachedWebService<WebAccountService>.GetEndPoint(Cache);
        }
    }

    public WebBugService BugService
    {
        get
        {
            return CachedWebService<WebBugService>.GetEndPoint(Cache);
        }
    }

    public WebStatsService StatsService
    {
        get
        {
            return CachedWebService<WebStatsService>.GetEndPoint(Cache);
        }
    }

    public WebTagWordService TagWordService
    {
        get
        {
            return CachedWebService<WebTagWordService>.GetEndPoint(Cache);
        }
    }

    public WebSocialService SocialService
    {
        get
        {
            return CachedWebService<WebSocialService>.GetEndPoint(Cache);
        }
    }

    public WebStoryService StoryService
    {
        get
        {
            return CachedWebService<WebStoryService>.GetEndPoint(Cache);
        }
    }

    public WebLocationService LocationService
    {
        get
        {
            return CachedWebService<WebLocationService>.GetEndPoint(Cache);
        }
    }

    public WebSystemService SystemService
    {
        get
        {
            return CachedWebService<WebSystemService>.GetEndPoint(Cache);
        }
    }

    public WebMarketingService MarketingService
    {
        get
        {
            return CachedWebService<WebMarketingService>.GetEndPoint(Cache);
        }
    }

    public WebMadLibService MadLibService
    {
        get
        {
            return CachedWebService<WebMadLibService>.GetEndPoint(Cache);
        }
    }

    public WebEventService EventService
    {
        get
        {
            return CachedWebService<WebEventService>.GetEndPoint(Cache);
        }
    }

    public WebDiscussionService DiscussionService
    {
        get
        {
            return CachedWebService<WebDiscussionService>.GetEndPoint(Cache);
        }
    }

    public WebSyndicationService SyndicationService
    {
        get
        {
            return CachedWebService<WebSyndicationService>.GetEndPoint(Cache);
        }
    }

    public WebPlaceService PlaceService
    {
        get
        {
            return CachedWebService<WebPlaceService>.GetEndPoint(Cache);
        }
    }

    public WebBlogService BlogService
    {
        get
        {
            return CachedWebService<WebBlogService>.GetEndPoint(Cache);
        }
    }

    public WebObjectService ObjectService
    {
        get
        {
            return CachedWebService<WebObjectService>.GetEndPoint(Cache);
        }
    }

    public WebGroupService GroupService
    {
        get
        {
            return CachedWebService<WebGroupService>.GetEndPoint(Cache);
        }
    }

    #endregion

    #region Kitchen Sink (TODO: move)

    public EventLog EventLog
    {
        get
        {
            if (mEventLog == null)
            {
                mEventLog = HostedApplication.CreateEventLog();
            }
            return mEventLog;
        }
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
                TransitAccount t_account = GetInstance<TransitAccount, int>(
                    userid, AccountService.GetAccountById);

                if (t_account != null)
                {
                    return string.Format("<a href=\"{0}/AccountView.aspx?id={1}\">{2}</a>",
                        WebsiteUrl, t_account.Id, Renderer.Render(t_account.Name));
                }
            }

            return string.Format("[invalid user: {0}]", tagvalue);
        }
        else
        {
            TransitPlace p = GetInstance<TransitPlace, string, string>(
                tagname, tagvalue, PlaceService.FindPlace);

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


    private void Track(HttpRequest request, HttpResponse response)
    {
        // new/returning users daily
        Nullable<DateTime> lastVisit = GetLastVisit(request, sSnCoreLastVisit);

        if ((!lastVisit.HasValue) || (lastVisit.Value.AddDays(1) < DateTime.UtcNow))
        {
            // cookie doesn't exist or exists with an invalid value
            // cookie older than a day
            HttpCookie lastVisitCookie = new HttpCookie(sSnCoreLastVisit, DateTime.UtcNow.ToString());
            lastVisitCookie.Expires = DateTime.UtcNow.AddYears(1);
            response.Cookies.Add(lastVisitCookie);
        }

        // unique users monthly
        Nullable<DateTime> lastMonthVisit = GetLastVisit(request, sSnCoreLastMonthVisit);
        if (!lastMonthVisit.HasValue)
        {
            // cookie doesn't exist, it has expired within the month
            HttpCookie lastMonthVisitCookie = new HttpCookie(sSnCoreLastMonthVisit, DateTime.UtcNow.ToString());
            DateTime nextmonth = DateTime.UtcNow.AddMonths(1);
            lastMonthVisitCookie.Expires = new DateTime(nextmonth.Year, nextmonth.Month, 1);
            response.Cookies.Add(lastMonthVisitCookie);
        }

        TransitStatsRequest tsr = new TransitStatsRequest(request, lastVisit, lastMonthVisit);

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
                catch (Exception ex)
                {
                    EventLog.WriteEntry(string.Format("Error tracking multiple requests.\n{0}", ex.Message),
                        EventLogEntryType.Warning);
                }
                finally
                {
                    s_Requests.Clear();
                    s_RequestsLastCommit = tsr.Timestamp;
                }
            }
        }
    }

    protected Nullable<DateTime> GetLastVisit(HttpRequest request, string cookie)
    {
        // track request
        Nullable<DateTime> lastVisit = new Nullable<DateTime>();
        HttpCookie lastVisitCookie = request.Cookies[cookie];
        if (lastVisitCookie != null)
        {
            DateTime lvcDt;
            if (DateTime.TryParse(request.Cookies[cookie].Value, out lvcDt))
            {
                lastVisit = lvcDt;
            }
        }

        return lastVisit;
    }

    public DateTime Adjust(DateTime dt)
    {
        if ((IsLoggedIn) && (Account.TimeZone >= 0))
        {
            TimeZoneInformation tz = TimeZoneInformation.FromIndex(Account.TimeZone);
            return tz.FromUniversalTime(dt);
        }

        return dt.AddHours(BrowserUtcOffset);
    }

    public DateTime ToUTC(DateTime dt)
    {
        if ((IsLoggedIn) && (Account.TimeZone >= 0))
        {
            TimeZoneInformation tz = TimeZoneInformation.FromIndex(Account.TimeZone);
            return tz.ToUniversalTime(dt);
        }

        return dt.AddHours(-BrowserUtcOffset);
    }

    public string ToAdjustedString(DateTime dt)
    {
        TimeSpan ts = DateTime.UtcNow.Subtract(dt);
        if (ts.TotalMinutes <= 1)
            return string.Format("{0} second{1} ago", ts.Seconds, ts.Seconds != 1 ? "s" : string.Empty);
        else if (ts.TotalHours <= 1)
            return string.Format("{0} minute{1} ago", ts.Minutes, ts.Minutes != 1 ? "s" : string.Empty);
        else if (ts.TotalDays <= 1)
            return string.Format("{0} hour{1} ago", ts.Hours, ts.Hours != 1 ? "s" : string.Empty);
        else if (ts.TotalDays < 2)
            return "yesterday";
        else if (ts.TotalDays <= 7)
            return string.Format("{0} day{1} ago", ts.Days, ts.Days != 1 ? "s" : string.Empty);
        else
            return string.Format("{0}", Adjust(dt).ToString("d"));
    }

    public string AdjustToRFC822(DateTime dt)
    {
        if ((IsLoggedIn) && (Account.TimeZone >= 0))
        {
            TimeZoneInformation tz = TimeZoneInformation.FromIndex(Account.TimeZone);
            return tz.FromUniversalTime(dt).ToString("ddd, dd MMM yyyy HH:mm:ss") +
                " " + tz.CurrentUtcBiasHours.ToString("00") + "00";
        }

        return dt.AddHours(BrowserUtcOffset).ToString("ddd, dd MMM yyyy HH:mm:ss") +
            " " + BrowserUtcOffset.ToString("00") + "00";
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
            if ((IsLoggedIn) && (Account.TimeZone >= 0))
            {
                TimeZoneInformation tz = TimeZoneInformation.FromIndex(Account.TimeZone);
                return tz.CurrentUtcBiasHours;
            }

            return BrowserUtcOffset;
        }
    }

    public int BrowserUtcOffset
    {
        get
        {
            int tz = System.TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;

            HttpCookie xtz = Request.Cookies["x-VisitorTimeZoneOffset"];
            if (xtz != null && !string.IsNullOrEmpty(xtz.Value))
                int.TryParse(xtz.Value, out tz);

            return tz;
        }
    }

    #endregion

    #region DomainModel
    
    public DomainClass GetDomainClass(string name)
    {
        string key = string.Format("DomainClass:{0}", name);
        DomainClass cs = (DomainClass) Cache.Get(key);
        if (cs == null)
        {
            cs = new DomainClass(name, SystemService.GetTypeColumns(name));
            Cache.Insert(key, cs);
        }
        return cs;
    }

    #endregion

    #region Strongly Typed

    #region Collection

    /// ticket + ServiceQueryOptions
    public IList<TransitType> GetCollection<TransitType>(
        ServiceQueryOptions options, WebClientImpl<TransitType>.GetCollectionDelegate functor)
    {
        return WebClientImpl<TransitType>.GetCollection(
            Ticket, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + ServiceQueryOptions
    public IList<TransitType> GetCollection<TransitType, ArgType1>(
        ArgType1 arg1, ServiceQueryOptions options,
        WebClientImpl<TransitType>.GetCollectionDelegate<ArgType1> functor)
    {
        return WebClientImpl<TransitType>.GetCollection<ArgType1>(
            Ticket, arg1, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + ServiceQueryOptions
    public IList<TransitType> GetCollection<TransitType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2, ServiceQueryOptions options,
        WebClientImpl<TransitType>.GetCollectionDelegate<ArgType1, ArgType2> functor)
    {
        return WebClientImpl<TransitType>.GetCollection<ArgType1, ArgType2>(
            Ticket, arg1, arg2, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3 + ServiceQueryOptions
    public IList<TransitType> GetCollection<TransitType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3, ServiceQueryOptions options,
        WebClientImpl<TransitType>.GetCollectionDelegate<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl<TransitType>.GetCollection<ArgType1, ArgType2, ArgType3>(
            Ticket, arg1, arg2, arg3, options, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region Instance

    /// ticket
    public TransitType GetInstance<TransitType>(
        WebClientImpl<TransitType>.GetItemDelegate functor)
    {
        return WebClientImpl<TransitType>.GetInstance(
            Ticket, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1
    public TransitType GetInstance<TransitType, ArgType1>(
        ArgType1 arg1,
        WebClientImpl<TransitType>.GetItemDelegate<ArgType1> functor)
    {
        return WebClientImpl<TransitType>.GetInstance<ArgType1>(
            Ticket, arg1, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2
    public TransitType GetInstance<TransitType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2,
        WebClientImpl<TransitType>.GetItemDelegate<ArgType1, ArgType2> functor)
    {
        return WebClientImpl<TransitType>.GetInstance<ArgType1, ArgType2>(
            Ticket, arg1, arg2, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3
    public TransitType GetInstance<TransitType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3,
        WebClientImpl<TransitType>.GetItemDelegate<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl<TransitType>.GetInstance<ArgType1, ArgType2, ArgType3>(
            Ticket, arg1, arg2, arg3, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region Count

    /// ticket
    public int GetCount<TransitType>(
        WebClientImpl<TransitType>.GetItemDelegateCount functor)
    {
        return WebClientImpl<TransitType>.GetCount(
            Ticket, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1
    public int GetCount<TransitType, ArgType1>(
        ArgType1 arg1, WebClientImpl<TransitType>.GetItemDelegateCount<ArgType1> functor)
    {
        return WebClientImpl<TransitType>.GetCount(
            Ticket, arg1, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2
    public int GetCount<TransitType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2,
        WebClientImpl<TransitType>.GetItemDelegateCount<ArgType1, ArgType2> functor)
    {
        return WebClientImpl<TransitType>.GetCount(
            Ticket, arg1, arg2, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3
    public int GetCount<TransitType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3,
        WebClientImpl<TransitType>.GetItemDelegateCount<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl<TransitType>.GetCount(
            Ticket, arg1, arg2, arg3, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region CreateOrUpdate

    public int CreateOrUpdate<TransitType>(
        TransitType t_instance, WebClientImpl<TransitType>.CreateOrUpdateItemDelegate functor)
    {
        return WebClientImpl<TransitType>.CreateOrUpdate(
            Ticket, t_instance, functor, Cache);
    }

    public int CreateOrUpdate<TransitType, ArgType1>(
        TransitType t_instance, ArgType1 arg1, WebClientImpl<TransitType>.CreateOrUpdateItemDelegate<ArgType1> functor)
    {
        return WebClientImpl<TransitType>.CreateOrUpdate(
            Ticket, t_instance, arg1, functor, Cache);
    }

    #endregion

    #region Delete

    public void Delete<TransitType>(
        int id, WebClientImpl<TransitType>.DeleteItemDelegate functor)
    {
        WebClientImpl<TransitType>.Delete(
            Ticket, id, functor, Cache);
    }

    #endregion

    #region Invalidate Cache

    public void InvalidateCache<TransitType>()
    {
        WebClientImpl<TransitType>.Invalidate(Cache);
    }

    #endregion

    #endregion
}

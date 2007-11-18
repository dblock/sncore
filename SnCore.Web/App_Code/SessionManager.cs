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

public class SessionManager : HostedSessionManager, IMarkupRendererHandler
{
    static TimeSpan s_RequestCommitInterval = new TimeSpan(0, 2, 0); // commit every two minutes
    private static DateTime s_RequestsLastCommit = DateTime.UtcNow;
    private static List<TransitStatsRequest> s_Requests = new List<TransitStatsRequest>(1024);
    public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(0, 5, 0);

    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";
    const string sSnCoreLastVisit = "SnCore.lastvisit";
    const string sSnCoreLastMonthVisit = "SnCore.lastmonthvisit";

    private string mTicket = string.Empty;
    private TransitAccount mAccount = null;

    protected override int SessionTimeZone
    {
        get
        {
            return Account.TimeZone;
        }
    }

    public Uri WebsiteUri
    {
        get
        {
            return new Uri(WebsiteUrl.TrimEnd("/".ToCharArray()) + "/");
        }
    }

    public Uri MobileWebsiteUri
    {
        get
        {
            return new Uri(MobileWebsiteUrl.TrimEnd("/".ToCharArray()) + "/");
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/SnCoreWeb");
        }
    }

    public string MobileWebsiteUrl
    {
        get
        {
            return GetCachedConfiguration("SnCore.MobileWebSite.Url", string.Empty);
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
        : base(cache, request, response)
    {
        mMarkupRenderer = new MarkupRenderer<SessionManager>(this);

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

    public override bool IsLoggedIn
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

    public WebObjectService ObjectService
    {
        get
        {
            return CachedWebService<WebObjectService>.GetEndPoint(Cache);
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

    public WebGroupService GroupService
    {
        get
        {
            return CachedWebService<WebGroupService>.GetEndPoint(Cache);
        }
    }

    #endregion

    #region Kitchen Sink (TODO: move)

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

    public string GetValue(string s, string defaultvalue)
    {
        return string.IsNullOrEmpty(s) ? defaultvalue : s;
    }

    public string GetValue(object s, string defaultvalue)
    {
        return (s == null) ? defaultvalue : GetValue(s.ToString(), defaultvalue);
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

    #region Markup Renderer

    private MarkupRenderer<SessionManager> mMarkupRenderer = null;
    
    public string RenderMarkups(string value)
    {
        return mMarkupRenderer.Render(value);
    }

    public delegate string ToString<TransitType>(TransitType instance);

    private string Handle<TransitType>(
        string tagname, string tagvalue, 
        WebClientImpl<TransitType>.GetItemDelegate<int> functor, 
        ToString<TransitType> transformer)
    {
        int id = 0;
        if (int.TryParse(tagvalue, out id))
        {
            TransitType t_instance = GetInstance<TransitType, int>(id, functor);
            if (t_instance != null)
            {
                return transformer(t_instance);
            }
        }

        return string.Format("[invalid {0}: {1}]", tagname, tagvalue);
    }

    private string Handle<TransitType, ArgType>(
        string tagname, string tagvalue, ArgType arg,
        WebClientImpl<TransitType>.GetItemDelegate<int, ArgType> functor,
        ToString<TransitType> transformer)
    {
        int id = 0;
        if (int.TryParse(tagvalue, out id))
        {
            TransitType t_instance = GetInstance<TransitType, int, ArgType>(id, arg, functor);
            if (t_instance != null)
            {
                return transformer(t_instance);
            }
        }

        return string.Format("[invalid {0}: {1}]", tagname, tagvalue);
    }

    public string Handle(string tag, string tagname, string tagvalue)
    {
        switch (tagname)
        {
            case "user":
            case "account":
                return Handle<TransitAccount>(tagname, tagvalue, AccountService.GetAccountById,
                    delegate(TransitAccount t_account)
                    {
                        return string.Format("<a href=\"{0}/AccountView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_account.Id, Renderer.Render(t_account.Name));
                    });
            case "place":
                return Handle<TransitPlace>(tagname, tagvalue, PlaceService.GetPlaceById,
                    delegate(TransitPlace t_place)
                    {
                        return string.Format("<a href=\"{0}/PlaceView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_place.Id, Renderer.Render(t_place.Name));
                    });
            case "group":
                return Handle<TransitAccountGroup>(tagname, tagvalue, GroupService.GetAccountGroupById,
                    delegate(TransitAccountGroup t_group)
                    {
                        return string.Format("<a href=\"{0}/AccountGroupView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_group.Id, Renderer.Render(t_group.Name));
                    });
            case "blog":
                return Handle<TransitAccountBlog>(tagname, tagvalue, BlogService.GetAccountBlogById,
                    delegate(TransitAccountBlog t_blog)
                    {
                        return string.Format("<a href=\"{0}/AccountBlogView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_blog.Id, Renderer.Render(t_blog.Name));
                    });
            case "event":
                return Handle<TransitAccountEvent, int>(tagname, tagvalue, 0, EventService.GetAccountEventById,
                    delegate(TransitAccountEvent t_event)
                    {
                        return string.Format("<a href=\"{0}/AccountEventView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_event.Id, Renderer.Render(t_event.Name));
                    });
            case "feed":
                return Handle<TransitAccountFeed>(tagname, tagvalue, SyndicationService.GetAccountFeedById,
                    delegate(TransitAccountFeed t_feed)
                    {
                        return string.Format("<a href=\"{0}/AccountFeedView.aspx?id={1}\">{2}</a>",
                            WebsiteUrl, t_feed.Id, Renderer.Render(t_feed.Name));
                    });
            case "discussion":
                return Handle<TransitDiscussion>(tagname, tagvalue, DiscussionService.GetDiscussionById,
                    delegate(TransitDiscussion t_discussion)
                    {
                        string name = string.IsNullOrEmpty(t_discussion.ParentObjectName) ? t_discussion.Name : t_discussion.ParentObjectName;
                        string uri = string.IsNullOrEmpty(t_discussion.ParentObjectUri) 
                            ? string.Format("DiscussionView.aspx?id={0}", t_discussion.Id)
                            : t_discussion.ParentObjectUri;
                        return string.Format("<a href=\"{0}/{1}\">{2}</a>",
                            WebsiteUrl, uri, Renderer.Render(name));
                    });
            default:
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

    #endregion
}

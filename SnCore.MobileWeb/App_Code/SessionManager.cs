using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Hosting;
using System.Web.Caching;
using SnCore.Tools;
using SnCore.Tools.Web;
using SnCore.WebServices;
using System.Text.RegularExpressions;

public class SessionManager : HostedSessionManager, IMarkupRendererHandler
{
    public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(0, 5, 0);

    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";

    private string mTicket = string.Empty;
    private AccountService.TransitAccount mAccount = null;

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

    public string WebsiteUrl
    {
        get
        {
            return GetCachedConfiguration("SnCore.MobileWebSite.Url", "http://localhost/SnCoreMobileWeb");
        }
    }

    public string WebsiteTitle
    {
        get
        {
            return GetCachedConfiguration("SnCore.Title", "SnCore");
        }
    }

    public string WebsiteName
    {
        get
        {
            return GetCachedConfiguration("SnCore.Name", "SnCore");
        }
    }

    public string GetCachedConfiguration(string name, string defaultvalue)
    {
        return GetInstance<SystemService.TransitConfiguration, SystemService.ServiceQueryOptions, string, string>(
            name, defaultvalue, SystemService.GetConfigurationByNameWithDefault).Value;
    }

    public SessionManager(System.Web.UI.Page page)
        : base(page)
    {
        CacheAuthCookie();
    }

    public SessionManager(System.Web.UI.MasterPage page)
        : base(page)
    {
        CacheAuthCookie();
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

    public AccountService.TransitAccount Account
    {
        get
        {
            if (mAccount == null)
            {
                if (!string.IsNullOrEmpty(Ticket))
                {
                    try
                    {
                        mAccount = (AccountService.TransitAccount)Cache[string.Format("account:{0}", Ticket)];
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

    public ObjectService.WebObjectService ObjectService
    {
        get
        {
            return CachedWebService<ObjectService.WebObjectService>.GetEndPoint(Cache);
        }
    }

    public AccountService.WebAccountService AccountService
    {
        get
        {
            return CachedWebService<AccountService.WebAccountService>.GetEndPoint(Cache);
        }
    }

    public PlaceService.WebPlaceService PlaceService
    {
        get
        {
            return CachedWebService<PlaceService.WebPlaceService>.GetEndPoint(Cache);
        }
    }

    public SocialService.WebSocialService SocialService
    {
        get
        {
            return CachedWebService<SocialService.WebSocialService>.GetEndPoint(Cache);
        }
    }

    public LocationService.WebLocationService LocationService
    {
        get
        {
            return CachedWebService<LocationService.WebLocationService>.GetEndPoint(Cache);
        }
    }

    public DiscussionService.WebDiscussionService DiscussionService
    {
        get
        {
            return CachedWebService<DiscussionService.WebDiscussionService>.GetEndPoint(Cache);
        }
    }

    public StatsService.WebStatsService StatsService
    {
        get
        {
            return CachedWebService<StatsService.WebStatsService>.GetEndPoint(Cache);
        }
    }

    public SystemService.WebSystemService SystemService
    {
        get
        {
            return CachedWebService<SystemService.WebSystemService>.GetEndPoint(Cache);
        }
    }

    public SyndicationService.WebSyndicationService SyndicationService
    {
        get
        {
            return CachedWebService<SyndicationService.WebSyndicationService>.GetEndPoint(Cache);
        }
    }

    public StoryService.WebStoryService StoryService
    {
        get
        {
            return CachedWebService<StoryService.WebStoryService>.GetEndPoint(Cache);
        }
    }

    public BlogService.WebBlogService BlogService
    {
        get
        {
            return CachedWebService<BlogService.WebBlogService>.GetEndPoint(Cache);
        }
    }

    #endregion

    #region Kitchen Sink (TODO: move)

    #region Markup Renderer

    private MarkupRenderer<SessionManager> mMarkupRenderer = null;

    public string RenderMarkups(string value)
    {
        return mMarkupRenderer.Render(value);
    }

    public string Handle(string tag, string tagname, string tagvalue)
    {
        if ((tagname == "user") || (tagname == "account"))
        {
            int userid = 0;
            if (int.TryParse(tagvalue, out userid))
            {
                AccountService.TransitAccount t_account = GetInstance<AccountService.TransitAccount, AccountService.ServiceQueryOptions, int>(
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
            PlaceService.TransitPlace p = GetInstance<PlaceService.TransitPlace, PlaceService.ServiceQueryOptions, string, string>(
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

    public static bool ShowAds
    {
        get
        {
            object ads = ConfigurationManager.AppSettings["Ads.Visible"];
            return (ads == null) || bool.Parse(ads.ToString());
        }
    }

    #endregion

    #region Strongly Typed

    #region Collection

    /// ticket + ServiceQueryOptionsType
    public IList<TransitType> GetCollection<TransitType, ServiceQueryOptionsType>(
        ServiceQueryOptionsType options, WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollectionDelegate functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollection(
            Ticket, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + ServiceQueryOptionsType
    public IList<TransitType> GetCollection<TransitType, ServiceQueryOptionsType, ArgType1>(
        ArgType1 arg1, ServiceQueryOptionsType options,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollectionDelegate<ArgType1> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollection<ArgType1>(
            Ticket, arg1, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + ServiceQueryOptionsType
    public IList<TransitType> GetCollection<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2, ServiceQueryOptionsType options,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollectionDelegate<ArgType1, ArgType2> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollection<ArgType1, ArgType2>(
            Ticket, arg1, arg2, options, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3 + ServiceQueryOptionsType
    public IList<TransitType> GetCollection<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3, ServiceQueryOptionsType options,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollectionDelegate<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCollection<ArgType1, ArgType2, ArgType3>(
            Ticket, arg1, arg2, arg3, options, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region Instance

    /// ticket
    public TransitType GetInstance<TransitType, ServiceQueryOptionsType>(
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegate functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetInstance(
            Ticket, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1
    public TransitType GetInstance<TransitType, ServiceQueryOptionsType, ArgType1>(
        ArgType1 arg1,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegate<ArgType1> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetInstance<ArgType1>(
            Ticket, arg1, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2
    public TransitType GetInstance<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegate<ArgType1, ArgType2> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetInstance<ArgType1, ArgType2>(
            Ticket, arg1, arg2, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3
    public TransitType GetInstance<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegate<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetInstance<ArgType1, ArgType2, ArgType3>(
            Ticket, arg1, arg2, arg3, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region Count

    /// ticket
    public int GetCount<TransitType, ServiceQueryOptionsType>(
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegateCount functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCount(
            Ticket, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1
    public int GetCount<TransitType, ServiceQueryOptionsType, ArgType1>(
        ArgType1 arg1, WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegateCount<ArgType1> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCount(
            Ticket, arg1, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2
    public int GetCount<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2>(
        ArgType1 arg1, ArgType2 arg2,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegateCount<ArgType1, ArgType2> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCount(
            Ticket, arg1, arg2, functor, Cache, DefaultCacheTimeSpan);
    }

    /// ticket + arg1 + arg2 + arg3
    public int GetCount<TransitType, ServiceQueryOptionsType, ArgType1, ArgType2, ArgType3>(
        ArgType1 arg1, ArgType2 arg2, ArgType3 arg3,
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetItemDelegateCount<ArgType1, ArgType2, ArgType3> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.GetCount(
            Ticket, arg1, arg2, arg3, functor, Cache, DefaultCacheTimeSpan);
    }

    #endregion

    #region CreateOrUpdate

    public int CreateOrUpdate<TransitType, ServiceQueryOptionsType>(
        TransitType t_instance, WebClientImpl2<TransitType, ServiceQueryOptionsType>.CreateOrUpdateItemDelegate functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.CreateOrUpdate(
            Ticket, t_instance, functor, Cache);
    }

    public int CreateOrUpdate<TransitType, ServiceQueryOptionsType, ArgType1>(
        TransitType t_instance, ArgType1 arg1, WebClientImpl2<TransitType, ServiceQueryOptionsType>.CreateOrUpdateItemDelegate<ArgType1> functor)
    {
        return WebClientImpl2<TransitType, ServiceQueryOptionsType>.CreateOrUpdate(
            Ticket, t_instance, arg1, functor, Cache);
    }

    #endregion

    #region Delete

    public void Delete<TransitType, ServiceQueryOptionsType>(
        int id, WebClientImpl2<TransitType, ServiceQueryOptionsType>.DeleteItemDelegate functor)
    {
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.Delete(
            Ticket, id, functor, Cache);
    }

    #endregion

    #region Invalidate Cache

    public void InvalidateCache<TransitType, ServiceQueryOptionsType>()
    {
        WebClientImpl2<TransitType, ServiceQueryOptionsType>.Invalidate(Cache);
    }

    #endregion

    #endregion
}

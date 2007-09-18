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

public class SessionManager
{
    public static TimeSpan DefaultCacheTimeSpan = new TimeSpan(0, 5, 0);

    const string sSnCoreOpenIdTokenCookieName = "SnCore.openidtoken";
    const string sSnCoreAuthCookieName = "SnCore.authcookie";
    const string sSnCoreImpersonateCookieName = "SnCore.impersonatecookie";
    const string sSnCoreRememberLogin = "SnCore.rememberlogin";

    private EventLog mEventLog = null;
    private Cache mCache = null;
    private HttpRequest mRequest = null;
    private HttpResponse mResponse = null;
    private string mTicket = string.Empty;
    private AccountService.TransitAccount mAccount = null;

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
        return GetInstance<SystemService.TransitConfiguration, SystemService.ServiceQueryOptions, string, string>(
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

    public SessionManager(Cache cache, HttpRequest request, HttpResponse response, bool track)
    {
        mCache = cache;
        mRequest = request;
        mResponse = response;

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

    public AccountService.WebAccountService AccountService
    {
        get
        {
            return CachedWebService<AccountService.WebAccountService>.GetEndPoint(Cache);
        }
    }

    public SocialService.WebSocialService SocialService
    {
        get
        {
            return CachedWebService<SocialService.WebSocialService>.GetEndPoint(Cache);
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

    #region Timezones
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
        if (ts.TotalSeconds <= 1)
            return "a second ago";
        else if (ts.TotalMinutes <= 1)
            return string.Format("{0} second{1} ago", ts.Seconds, ts.Seconds != 1 ? "s" : string.Empty);
        else if (ts.TotalHours <= 1)
            return string.Format("{0} minute{1} ago", ts.Minutes, ts.Minutes != 1 ? "s" : string.Empty);
        else if (ts.TotalHours <= 6)
            return string.Format("{0} hour{1} ago", ts.Hours, ts.Hours != 1 ? "s" : string.Empty);
        else if (ts.TotalDays <= 1)
            return "today";
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
    #endregion
}

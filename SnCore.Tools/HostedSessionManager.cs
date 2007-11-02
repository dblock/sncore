using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Diagnostics;

namespace SnCore.Tools.Web
{
    public abstract class HostedSessionManager
    {
        public HostedSessionManager(Page page)
            : this(page.Cache, page.Request, page.Response)
        {
        }

        public HostedSessionManager(MasterPage page)
            : this(page.Cache, page.Request, page.Response)
        {

        }

        public HostedSessionManager(Cache cache, HttpRequest request, HttpResponse response)
        {
            mCache = cache;
            mRequest = request;
            mResponse = response;
        }

        #region Infrastructure
        
        private EventLog mEventLog = null;

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

        #endregion

        #region Session

        public abstract bool IsLoggedIn { get; }

        private HttpRequest mRequest = null;
        private HttpResponse mResponse = null;
        private Cache mCache = null;

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

        #endregion

        #region Browser

        public int UtcOffset
        {
            get
            {
                if ((IsLoggedIn) && (SessionTimeZone >= 0))
                {
                    TimeZoneInformation tz = TimeZoneInformation.FromIndex(SessionTimeZone);
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

        #region Time Zones

        protected abstract int SessionTimeZone { get; }

        public DateTime Adjust(DateTime dt)
        {
            if ((IsLoggedIn) && (SessionTimeZone >= 0))
            {
                TimeZoneInformation tz = TimeZoneInformation.FromIndex(SessionTimeZone);
                return tz.FromUniversalTime(dt);
            }

            return dt.AddHours(BrowserUtcOffset);
        }

        public DateTime ToUTC(DateTime dt)
        {
            if ((IsLoggedIn) && (SessionTimeZone >= 0))
            {
                TimeZoneInformation tz = TimeZoneInformation.FromIndex(SessionTimeZone);
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
            if ((IsLoggedIn) && (SessionTimeZone >= 0))
            {
                TimeZoneInformation tz = TimeZoneInformation.FromIndex(SessionTimeZone);
                return tz.FromUniversalTime(dt).ToString("ddd, dd MMM yyyy HH:mm:ss") +
                    " " + tz.CurrentUtcBiasHours.ToString("00") + "00";
            }

            return dt.AddHours(BrowserUtcOffset).ToString("ddd, dd MMM yyyy HH:mm:ss") +
                " " + BrowserUtcOffset.ToString("00") + "00";
        }

        #endregion
    }
}

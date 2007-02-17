using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Collection;
using System.Diagnostics;
using System.Web;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitStatsSummary
    {
        private List<TransitSummarizedCounter> mHourly = new List<TransitSummarizedCounter>(25);

        public List<TransitSummarizedCounter> Hourly
        {
            get
            {
                return mHourly;
            }
            set
            {
                mHourly = value;
            }
        }

        private List<TransitSummarizedCounter> mReturningDaily = new List<TransitSummarizedCounter>(14);

        public List<TransitSummarizedCounter> ReturningDaily
        {
            get
            {
                return mReturningDaily;
            }
            set
            {
                mReturningDaily = value;
            }
        }

        private List<TransitSummarizedCounter> mNewDaily = new List<TransitSummarizedCounter>(15);

        public List<TransitSummarizedCounter> NewDaily
        {
            get
            {
                return mNewDaily;
            }
            set
            {
                mNewDaily = value;
            }
        }

        private List<TransitSummarizedCounter> mDaily = new List<TransitSummarizedCounter>(14);

        public List<TransitSummarizedCounter> Daily
        {
            get
            {
                return mDaily;
            }
            set
            {
                mDaily = value;
            }
        }

        private List<TransitSummarizedCounter> mWeekly = new List<TransitSummarizedCounter>(54);

        public List<TransitSummarizedCounter> Weekly
        {
            get
            {
                return mWeekly;
            }
            set
            {
                mWeekly = value;
            }
        }

        private List<TransitSummarizedCounter> mMonthly = new List<TransitSummarizedCounter>(12);

        public List<TransitSummarizedCounter> Monthly
        {
            get
            {
                return mMonthly;
            }
            set
            {
                mMonthly = value;
            }
        }

        private List<TransitSummarizedCounter> mYearly = new List<TransitSummarizedCounter>(6);

        public List<TransitSummarizedCounter> Yearly
        {
            get
            {
                return mYearly;
            }
            set
            {
                mYearly = value;
            }
        }

        private List<TransitSummarizedCounter> mAccountDaily = new List<TransitSummarizedCounter>(14);

        public List<TransitSummarizedCounter> AccountDaily
        {
            get
            {
                return mAccountDaily;
            }
            set
            {
                mAccountDaily = value;
            }
        }

        private List<TransitSummarizedCounter> mAccountWeekly = new List<TransitSummarizedCounter>(54);

        public List<TransitSummarizedCounter> AccountWeekly
        {
            get
            {
                return mAccountWeekly;
            }
            set
            {
                mAccountWeekly = value;
            }
        }

        private List<TransitSummarizedCounter> mAccountMonthly = new List<TransitSummarizedCounter>(12);

        public List<TransitSummarizedCounter> AccountMonthly
        {
            get
            {
                return mAccountMonthly;
            }
            set
            {
                mAccountMonthly = value;
            }
        }

        private List<TransitSummarizedCounter> mAccountYearly = new List<TransitSummarizedCounter>(6);

        public List<TransitSummarizedCounter> AccountYearly
        {
            get
            {
                return mAccountYearly;
            }
            set
            {
                mAccountYearly = value;
            }
        }

        private long mTotalHits = 0;

        public long TotalHits
        {
            get
            {
                return mTotalHits;
            }
            set
            {
                mTotalHits = value;
            }
        }
    }

    public class TransitStatsRequest
    {
        public TransitStatsRequest()
        {

        }

        public TransitStatsRequest(HttpRequest request, Nullable<DateTime> lastseen)
        {
            RequestUri = (request.Url != null) ? request.Url.ToString() : string.Empty;
            RefererUri = (request.UrlReferrer != null) ? request.UrlReferrer.ToString() : string.Empty;

            if (request.UrlReferrer != null)
            {
                HttpRequest q = new HttpRequest(null, RefererUri.ToString(), request.UrlReferrer.Query.TrimStart("?".ToCharArray()));
                RefererQuery = q.Params["q"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["s"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["query"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["search"];
            }

            if (!lastseen.HasValue) IncrementNewUser = true;
            else if (lastseen.Value.AddDays(1) < DateTime.UtcNow) IncrementReturningUser = true;

            Timestamp = DateTime.UtcNow;
        }

        private bool mIncrementNewUser = false;

        public bool IncrementNewUser
        {
            get
            {
                return mIncrementNewUser;
            }
            set
            {
                mIncrementNewUser = value;
            }
        }

        private bool mIncrementReturningUser = false;

        public bool IncrementReturningUser
        {
            get
            {
                return mIncrementReturningUser;
            }
            set
            {
                mIncrementReturningUser = value;
            }
        }

        private DateTime mTimestamp;

        public DateTime Timestamp
        {
            get
            {
                return mTimestamp;
            }
            set
            {
                mTimestamp = value;
            }
        }

        private string mRequestUri;

        public string RequestUri
        {
            get
            {
                return mRequestUri;
            }
            set
            {
                mRequestUri = value;
            }
        }

        private string mRefererUri;

        public string RefererUri
        {
            get
            {
                return mRefererUri;
            }
            set
            {
                mRefererUri = value;
            }
        }

        private string mRefererQuery;

        public string RefererQuery
        {
            get
            {
                return mRefererQuery;
            }
            set
            {
                mRefererQuery = value;
            }
        }
    }

    public class ManagedStats : ManagedServiceImpl
    {
        public ManagedStats(ISession session)
            : base(session)
        {

        }

        public void Track(TransitStatsRequest request)
        {
            ITransaction trans = Session.BeginTransaction();

            try
            {
                // global counters
                IncrementHourlyCounter();
                IncrementDailyCounter();
                IncrementWeeklyCounter();
                IncrementMonthlyCounter();
                IncrementYearlyCounter();
                // per-user counter
                IncrementReturningDailyCounter(request);
                // per-uri page counter
                IncrementRawCounter(request);
                // Referer hosts
                UpdateRefererHost(request);
                // Referer query
                UpdateRefererQuery(request);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        private void UpdateRefererHost(TransitStatsRequest request)
        {
            if (string.IsNullOrEmpty(request.RefererUri))
                return;

            // don't track navigation between pages
            Uri requesturi = new Uri(request.RequestUri);
            Uri refereruri = new Uri(request.RefererUri);
            if (refereruri.Host == requesturi.Host)
                return;

            RefererHost host = (RefererHost)Session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Eq("Host", refereruri.Host))
                .UniqueResult();

            if (host == null)
            {
                // find whether this is a dup host
                RefererHostDup duphost = (RefererHostDup)
                    Session.CreateSQLQuery(
                        "SELECT {R.*} FROM RefererHostDup {R}" +
                        " WHERE '" + Renderer.SqlEncode(refereruri.Host) + "' LIKE Host",
                        "R",
                        typeof(RefererHostDup)).SetMaxResults(1).UniqueResult();

                if (duphost != null)
                {
                    host = duphost.RefererHost;
                }
            }

            if (host == null)
            {
                host = new RefererHost();
                host.Created = DateTime.UtcNow;
                host.Host = refereruri.Host;
                host.Total = 0;
            }

            host.Updated = DateTime.UtcNow;
            
            //
            // hack: try our best to insert the host record
            //       when an exception happens this session can't be flushed any more
            //
            if (! string.IsNullOrEmpty(request.RefererUri) && (request.RefererUri.Length < 255)) 
                host.LastRefererUri = request.RefererUri;
            if (! string.IsNullOrEmpty(request.RequestUri) && (request.RequestUri.Length < 255)) 
                host.LastRequestUri = request.RequestUri;
            
            host.Total++;

            try
            {
                Session.Save(host);
            }
            catch
            {
                Session.Evict(host);
            }
        }

        private void UpdateRefererQuery(TransitStatsRequest request)
        {
            if (string.IsNullOrEmpty(request.RefererQuery))
                return;

            RefererQuery query = (RefererQuery)Session.CreateCriteria(typeof(RefererQuery))
                .Add(Expression.Eq("Keywords", request.RefererQuery))
                .UniqueResult();

            if (query == null)
            {
                query = new RefererQuery();
                query.Created = DateTime.UtcNow;
                query.Total = 0;
                query.Keywords = request.RefererQuery;
            }

            query.Updated = DateTime.UtcNow;
            query.Total++;

            try
            {
                Session.Save(query);
            }
            catch
            {
                Session.Evict(query);
            }
        }

        private void IncrementRawCounter(TransitStatsRequest request)
        {
            string uri = request.RequestUri;
            if (uri.Length > 255) uri = uri.Substring(0, 255);

            // increment the raw uri counter
            Counter counter_raw = (Counter)Session.CreateCriteria(typeof(Counter))
                .Add(Expression.Eq("Uri", uri)).UniqueResult();

            if (counter_raw == null)
            {
                counter_raw = new Counter();
                counter_raw.Uri = uri;
                counter_raw.Created = DateTime.UtcNow;
                counter_raw.Total = 0;
            }

            counter_raw.Total++;
            counter_raw.Modified = DateTime.UtcNow;
            try
            {
                Session.Save(counter_raw);
            }
            catch
            {
                Session.Evict(counter_raw);
            }
        }

        private void IncrementHourlyCounter()
        {
            DateTime now = DateTime.UtcNow;
            DateTime ts = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            CounterHourly cntr = (CounterHourly)Session.CreateCriteria(typeof(CounterHourly))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterHourly();
                cntr.Timestamp = ts;
                cntr.Total = 0;
            }

            cntr.Total++;
            Session.Save(cntr);
        }

        private void IncrementDailyCounter()
        {
            DateTime now = DateTime.UtcNow;
            DateTime ts = new DateTime(now.Year, now.Month, now.Day);
            CounterDaily cntr = (CounterDaily)Session.CreateCriteria(typeof(CounterDaily))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterDaily();
                cntr.Timestamp = ts;
                cntr.Total = 0;
            }

            cntr.Total++;
            Session.Save(cntr);
        }

        private void IncrementReturningDailyCounter(TransitStatsRequest request)
        {
            if (!request.IncrementNewUser && !request.IncrementReturningUser)
                return;

            DateTime now = DateTime.UtcNow;
            DateTime ts = new DateTime(now.Year, now.Month, now.Day);
            CounterReturningDaily cntr = (CounterReturningDaily)Session.CreateCriteria(typeof(CounterReturningDaily))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterReturningDaily();
                cntr.Timestamp = ts;
                cntr.ReturningTotal = 0;
                cntr.NewTotal = 0;
            }

            if (request.IncrementReturningUser) cntr.ReturningTotal++;
            if (request.IncrementNewUser) cntr.NewTotal++;

            Session.Save(cntr);
        }

        private void IncrementWeeklyCounter()
        {
            DateTime now = DateTime.UtcNow;

            while (now.DayOfWeek != DayOfWeek.Sunday)
                now = now.AddDays(-1);

            DateTime ts = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            CounterWeekly cntr = (CounterWeekly)Session.CreateCriteria(typeof(CounterWeekly))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterWeekly();
                cntr.Timestamp = ts;
                cntr.Total = 0;
            }

            cntr.Total++;
            Session.Save(cntr);
        }

        private void IncrementMonthlyCounter()
        {
            DateTime now = DateTime.UtcNow;
            DateTime ts = new DateTime(now.Year, now.Month, 1);
            CounterMonthly cntr = (CounterMonthly)Session.CreateCriteria(typeof(CounterMonthly))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterMonthly();
                cntr.Timestamp = ts;
                cntr.Total = 0;
            }

            cntr.Total++;
            Session.Save(cntr);
        }

        private void IncrementYearlyCounter()
        {
            DateTime now = DateTime.UtcNow;
            DateTime ts = new DateTime(now.Year, 1, 1);
            CounterYearly cntr = (CounterYearly)Session.CreateCriteria(typeof(CounterYearly))
                .Add(Expression.Eq("Timestamp", ts))
                .UniqueResult();

            if (cntr == null)
            {
                cntr = new CounterYearly();
                cntr.Timestamp = ts;
                cntr.Total = 0;
            }

            cntr.Total++;
            Session.Save(cntr);
        }

        public List<TransitSummarizedCounter> GetSummaryHourly()
        {
            List<TransitSummarizedCounter> result = new List<TransitSummarizedCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddHours(-24);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, ts.Hour, 0, 0);
                CounterHourly c = (CounterHourly)Session.CreateCriteria(typeof(CounterHourly))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitSummarizedCounter(ts_current, 0) : new TransitSummarizedCounter(c.Timestamp, c.Total));
                ts = ts.AddHours(1);
            }

            return result;
        }

        private delegate DateTime GetDt(DateTime dt);
        private delegate TransitSummarizedCounter GetSummarizedCounter<TotalType>(TimestampCounter<TotalType> tc);

        private List<TransitSummarizedCounter> GetSummary<CounterType, TotalType>(
            GetDt getfirst, GetDt getnext, 
            GetTimestampCounter<CounterType, TotalType> gettimestampcounter,
            GetSummarizedCounter<TotalType> getsummarizedcounter)
        {
            List<TransitSummarizedCounter> result = new List<TransitSummarizedCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = getfirst(now);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                CounterType c = Session.CreateCriteria(typeof(CounterType))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult<CounterType>();

                TransitSummarizedCounter tsc = null;
                if (c == null) 
                {
                    tsc = new TransitSummarizedCounter(ts_current, 0);
                }
                else
                {
                    TimestampCounter<TotalType> tc = gettimestampcounter(c);
                    tsc = getsummarizedcounter(tc);
                }

                result.Add(tsc);
                ts = getnext(ts);
            }

            return result;
        }

        public List<TransitSummarizedCounter> GetSummaryDaily()
        {
            return GetSummary<CounterDaily, int>(
                delegate(DateTime dt) { return dt.AddDays(-14); },
                delegate(DateTime dt) { return dt.AddDays(1); },
                delegate(CounterDaily counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryAccountDaily()
        {
            return GetSummary<CounterAccountDaily, int>(
                delegate(DateTime dt) { return dt.AddDays(-14); },
                delegate(DateTime dt) { return dt.AddDays(1); },
                delegate(CounterAccountDaily counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryReturningDaily()
        {
            return GetSummary<CounterReturningDaily, int>(
                delegate(DateTime dt) { return dt.AddDays(-14); },
                delegate(DateTime dt) { return dt.AddDays(1); },
                delegate(CounterReturningDaily counter) { return new TimestampCounter<int>(counter.Timestamp, counter.ReturningTotal); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryNewDaily()
        {
            return GetSummary<CounterReturningDaily, int>(
                delegate(DateTime dt) { return dt.AddDays(-14); },
                delegate(DateTime dt) { return dt.AddDays(1); },
                delegate(CounterReturningDaily counter) { return new TimestampCounter<int>(counter.Timestamp, counter.NewTotal); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryWeekly()
        {
            return GetSummary<CounterWeekly, int>(
                delegate(DateTime dt) { dt = dt.AddYears(-1); while (dt.DayOfWeek != DayOfWeek.Sunday) dt = dt.AddDays(-1); return dt; },
                delegate(DateTime dt) { return dt.AddDays(7); },
                delegate(CounterWeekly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryAccountWeekly()
        {
            return GetSummary<CounterAccountWeekly, int>(
                delegate(DateTime dt) { dt = dt.AddYears(-1); while (dt.DayOfWeek != DayOfWeek.Sunday) dt = dt.AddDays(-1); return dt; },
                delegate(DateTime dt) { return dt.AddDays(7); },
                delegate(CounterAccountWeekly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryMonthly()
        {
            return GetSummary<CounterMonthly, int>(
                delegate(DateTime dt) { dt = dt.AddMonths(-12); return new DateTime(dt.Year, dt.Month, 1); },
                delegate(DateTime dt) { return dt.AddMonths(1); },
                delegate(CounterMonthly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryAccountMonthly()
        {
            return GetSummary<CounterAccountMonthly, int>(
                delegate(DateTime dt) { dt = dt.AddMonths(-12); return new DateTime(dt.Year, dt.Month, 1); },
                delegate(DateTime dt) { return dt.AddMonths(1); },
                delegate(CounterAccountMonthly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<int> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryYearly()
        {
            return GetSummary<CounterYearly, long>(
                delegate(DateTime dt) { dt = dt.AddYears(-5); return new DateTime(dt.Year, 1, 1); },
                delegate(DateTime dt) { return dt.AddYears(1); },
                delegate(CounterYearly counter) { return new TimestampCounter<long>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<long> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public List<TransitSummarizedCounter> GetSummaryAccountYearly()
        {
            return GetSummary<CounterAccountYearly, long>(
                delegate(DateTime dt) { dt = dt.AddYears(-5); return new DateTime(dt.Year, 1, 1); },
                delegate(DateTime dt) { return dt.AddYears(1); },
                delegate(CounterAccountYearly counter) { return new TimestampCounter<long>(counter.Timestamp, counter.Total); },
                delegate(TimestampCounter<long> tsc) { return new TransitSummarizedCounter(tsc.Timestamp, tsc.Total); }
                );
        }

        public TransitStatsSummary GetSummary()
        {
            TransitStatsSummary summary = new TransitStatsSummary();

            summary.TotalHits = (long) Session.CreateQuery("SELECT SUM(c.Total) FROM CounterYearly c").UniqueResult();

            DateTime now = DateTime.UtcNow;

            summary.Hourly.AddRange(GetSummaryHourly());
            summary.Daily.AddRange(GetSummaryDaily());
            summary.Weekly.AddRange(GetSummaryWeekly());
            summary.Monthly.AddRange(GetSummaryMonthly());
            summary.Yearly.AddRange(GetSummaryYearly());
            
            summary.ReturningDaily.AddRange(GetSummaryReturningDaily());
            summary.NewDaily.AddRange(GetSummaryNewDaily());

            summary.AccountDaily.AddRange(GetSummaryAccountDaily());
            summary.AccountWeekly.AddRange(GetSummaryAccountWeekly());
            summary.AccountMonthly.AddRange(GetSummaryAccountMonthly());
            summary.AccountYearly.AddRange(GetSummaryAccountYearly());

            return summary;
        }

        public static TransitCounter FindByUri(ISession session, string pageviewfilename, int id, ManagedSecurityContext sec)
        {
            string uri = string.Format("{0}/{1}?id={2}", 
                ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                pageviewfilename, id);

            return ManagedCounter.FindByUri(session, uri, sec);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Expression;
using NHibernate.Collection;
using System.Diagnostics;
using System.Web;
namespace SnCore.Services
{
    public class TransitCounter
    {
        public TransitCounter(DateTime ts, long total)
        {
            mTimestamp = ts;
            mTotal = total;
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

        private long mTotal;

        public long Total
        {
            get
            {
                return mTotal;
            }
            set
            {
                mTotal = value;
            }
        }
    }

    public class TransitStatsSummary
    {
        private List<TransitCounter> mHourly = new List<TransitCounter>(24);

        public List<TransitCounter> Hourly
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

        private List<TransitCounter> mReturningDaily = new List<TransitCounter>(14);

        public List<TransitCounter> ReturningDaily
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

        private List<TransitCounter> mNewDaily = new List<TransitCounter>(14);

        public List<TransitCounter> NewDaily
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

        private List<TransitCounter> mDaily = new List<TransitCounter>(14);

        public List<TransitCounter> Daily
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

        private List<TransitCounter> mWeekly = new List<TransitCounter>(8);

        public List<TransitCounter> Weekly
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

        private List<TransitCounter> mMonthly = new List<TransitCounter>(12);

        public List<TransitCounter> Monthly
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

        private List<TransitCounter> mYearly = new List<TransitCounter>();

        public List<TransitCounter> Yearly
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
            RequestUri = request.Url;
            RefererUri = request.UrlReferrer;

            if (RefererUri != null)
            {
                HttpRequest q = new HttpRequest(null, RefererUri.ToString(), RefererUri.Query.TrimStart("?".ToCharArray()));
                RefererQuery = q.Params["q"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["s"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["query"];
                if (string.IsNullOrEmpty(RefererQuery)) RefererQuery = q.Params["search"];
            }

            if (!lastseen.HasValue) IncrementNewUser = true;
            else if (lastseen.Value.AddDays(1) < DateTime.UtcNow) IncrementReturningUser = true;

            SinkException = true;
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

        private bool mSinkException = true;

        public bool SinkException
        {
            get
            {
                return mSinkException;
            }
            set
            {
                mSinkException = value;
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

        private Uri mRequestUri;

        public Uri RequestUri
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

        private Uri mRefererUri;

        public Uri RefererUri
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

    /// <summary>
    /// Managed stats and tracker.
    /// </summary>
    public class ManagedStats : ManagedService
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
            if (request.RefererUri == null)
                return;

            // don't track navigation between pages
            if (request.RefererUri.Host == request.RequestUri.Host)
                return;

            RefererHost host = (RefererHost)Session.CreateCriteria(typeof(RefererHost))
                .Add(Expression.Eq("Host", request.RefererUri.Host))
                .UniqueResult();

            if (host == null)
            {
                // find whether this is a dup host
                RefererHostDup duphost = (RefererHostDup)Session.CreateCriteria(typeof(RefererHostDup))
                    .Add(Expression.Like("Host", request.RefererUri.Host))
                    .UniqueResult();

                if (duphost != null)
                {
                    host = duphost.RefererHost;
                }
            }

            if (host == null)
            {
                host = new RefererHost();
                host.Created = DateTime.UtcNow;
                host.Host = request.RefererUri.Host;
                host.Total = 0;
            }

            host.Updated = DateTime.UtcNow;
            host.LastRefererUri = request.RefererUri.ToString();
            host.LastRequestUri = request.RequestUri.ToString();
            host.Total++;

            Session.Save(host);
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

            Session.Save(query);
        }

        private void IncrementRawCounter(TransitStatsRequest request)
        {
            // increment the raw uri counter
            Counter counter_raw = (Counter)Session.CreateCriteria(typeof(Counter))
                .Add(Expression.Eq("Uri", request.RequestUri.ToString())).UniqueResult();

            if (counter_raw == null)
            {
                counter_raw = new Counter();
                counter_raw.Uri = request.RequestUri.ToString();
                counter_raw.Created = DateTime.UtcNow;
                counter_raw.Total = 0;
            }

            counter_raw.Total++;
            counter_raw.Modified = DateTime.UtcNow;
            Session.Save(counter_raw);
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

        public List<TransitCounter> GetSummaryHourly()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddHours(-24);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, ts.Hour, 0, 0);
                CounterHourly c = (CounterHourly)Session.CreateCriteria(typeof(CounterHourly))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                ts = ts.AddHours(1);
            }

            return result;
        }

        public List<TransitCounter> GetSummaryDaily()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddDays(-14);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                CounterDaily c = (CounterDaily)Session.CreateCriteria(typeof(CounterDaily))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                ts = ts.AddDays(1);
            }

            return result;
        }

        public List<TransitCounter> GetSummaryReturningDaily()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddDays(-14);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                CounterReturningDaily c = (CounterReturningDaily)Session.CreateCriteria(typeof(CounterReturningDaily))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.ReturningTotal));
                ts = ts.AddDays(1);
            }

            return result;
        }

        public List<TransitCounter> GetSummaryNewDaily()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddDays(-14);
            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                CounterReturningDaily c = (CounterReturningDaily)Session.CreateCriteria(typeof(CounterReturningDaily))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.NewTotal));
                ts = ts.AddDays(1);
            }

            return result;
        }

        public List<TransitCounter> GetSummaryWeekly()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddMonths(-2);

            while (ts.DayOfWeek != DayOfWeek.Sunday)
                ts = ts.AddDays(-1);

            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                CounterWeekly c = (CounterWeekly)Session.CreateCriteria(typeof(CounterWeekly))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                ts = ts.AddDays(7);
            }

            return result;
        }

        /// <summary>
        /// Monthly hits, twelve months.
        /// </summary>
        /// <returns></returns>
        public List<TransitCounter> GetSummaryMonthly()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;
            DateTime ts = now.AddMonths(-12);

            while (ts <= now)
            {
                DateTime ts_current = new DateTime(ts.Year, ts.Month, 1, 0, 0, 0);
                CounterMonthly c = (CounterMonthly)Session.CreateCriteria(typeof(CounterMonthly))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                ts = ts.AddMonths(1);
            }

            return result;
        }

        /// <summary>
        /// Yearly hits, five years.
        /// </summary>
        /// <returns></returns>
        public List<TransitCounter> GetSummaryYearly()
        {
            List<TransitCounter> result = new List<TransitCounter>();
            DateTime now = DateTime.UtcNow;

            for (int i = -5; i <= 0; i++)
            {
                DateTime ts_current = new DateTime(now.Year + i, 1, 1, 0, 0, 0);
                CounterYearly c = (CounterYearly)Session.CreateCriteria(typeof(CounterYearly))
                    .Add(Expression.Eq("Timestamp", ts_current))
                    .UniqueResult();

                result.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
            }

            return result;
        }

        public TransitStatsSummary GetSummary()
        {
            TransitStatsSummary summary = new TransitStatsSummary();

            summary.TotalHits = (long)Session.CreateQuery("SELECT SUM(c.Total) FROM CounterYearly c").UniqueResult();

            DateTime now = DateTime.UtcNow;

            summary.Hourly.AddRange(GetSummaryHourly());
            summary.Daily.AddRange(GetSummaryDaily());
            summary.Weekly.AddRange(GetSummaryWeekly());
            summary.Monthly.AddRange(GetSummaryMonthly());
            summary.Yearly.AddRange(GetSummaryYearly());
            summary.ReturningDaily.AddRange(GetSummaryReturningDaily());
            summary.NewDaily.AddRange(GetSummaryNewDaily());

            return summary;
        }
    }
}

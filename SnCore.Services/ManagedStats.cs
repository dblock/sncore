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

        public TransitStatsRequest(HttpRequest request)
        {
            ReferrerQuery = request.Params["q"];
            if (string.IsNullOrEmpty(ReferrerQuery)) ReferrerQuery = request.Params["search"];
            RequestUri = request.Url;
            ReferrerUri = request.UrlReferrer;
            SinkException = true;
            Timestamp = DateTime.UtcNow;
        }

        private bool mSinkException;

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

        private Uri mReferrerUri;

        public Uri ReferrerUri
        {
            get
            {
                return mReferrerUri;
            }
            set
            {
                mReferrerUri = value;
            }
        }

        private string mReferrerQuery;

        public string ReferrerQuery
        {
            get
            {
                return mReferrerQuery;
            }
            set
            {
                mReferrerQuery = value;
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
                // per-uri page counter
                IncrementRawCounter(request);
                // referrer hosts
                UpdateReferrerHost(request);
                // referrer query
                UpdateReferrerQuery(request);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        private void UpdateReferrerHost(TransitStatsRequest request)
        {
            if (request.ReferrerUri == null)
                return;

            // don't track navigation between pages
            if (request.ReferrerUri.Host == request.RequestUri.Host)
                return;

            ReferrerHost host = (ReferrerHost)Session.CreateCriteria(typeof(ReferrerHost))
                .Add(Expression.Eq("Host", request.ReferrerUri.Host))
                .UniqueResult();

            if (host == null)
            {
                host = new ReferrerHost();
                host.Created = DateTime.UtcNow;
                host.Host = request.ReferrerUri.Host;
                host.Total = 0;
            }

            host.Updated = DateTime.UtcNow;
            host.LastReferrerUri = request.ReferrerUri.ToString();
            host.LastUri = request.RequestUri.ToString();
            host.Total++;

            Session.Save(host);
        }

        private void UpdateReferrerQuery(TransitStatsRequest request)
        {
            if (string.IsNullOrEmpty(request.ReferrerQuery))
                return;

            ReferrerQuery query = (ReferrerQuery)Session.CreateCriteria(typeof(ReferrerQuery))
                .Add(Expression.Eq("Keywords", request.ReferrerQuery))
                .UniqueResult();

            if (query == null)
            {
                query = new ReferrerQuery();
                query.Created = DateTime.UtcNow;
                query.Total = 0;
                query.Keywords = request.ReferrerQuery;
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

        public TransitStatsSummary GetSummary()
        {
            TransitStatsSummary summary = new TransitStatsSummary();

            summary.TotalHits = (long)Session.CreateQuery("SELECT SUM(c.Total) FROM CounterYearly c").UniqueResult();

            DateTime now = DateTime.UtcNow;

            // hourly hits, -24 hours
            {
                DateTime ts = now.AddHours(-24);
                while (ts <= now)
                {
                    DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, ts.Hour, 0, 0);
                    CounterHourly c = (CounterHourly)Session.CreateCriteria(typeof(CounterHourly))
                        .Add(Expression.Eq("Timestamp", ts_current))
                        .UniqueResult();

                    summary.Hourly.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                    ts = ts.AddHours(1);
                }
            }

            // daily hits, -14 days
            {
                DateTime ts = now.AddDays(-14);
                while (ts <= now)
                {
                    DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                    CounterDaily c = (CounterDaily)Session.CreateCriteria(typeof(CounterDaily))
                        .Add(Expression.Eq("Timestamp", ts_current))
                        .UniqueResult();

                    summary.Daily.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                    ts = ts.AddDays(1);
                }
            }

            // weekly hits, two months
            {
                DateTime ts = now.AddMonths(-2);
                
                while (ts.DayOfWeek != DayOfWeek.Sunday)
                    ts = ts.AddDays(-1);

                while (ts <= now)
                {
                    DateTime ts_current = new DateTime(ts.Year, ts.Month, ts.Day, 0, 0, 0);
                    CounterWeekly c = (CounterWeekly)Session.CreateCriteria(typeof(CounterWeekly))
                        .Add(Expression.Eq("Timestamp", ts_current))
                        .UniqueResult();

                    summary.Weekly.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                    ts = ts.AddDays(7);
                }
            }

            // monthly hits, twelve months
            {
                DateTime ts = now.AddMonths(-12);

                while (ts <= now)
                {
                    DateTime ts_current = new DateTime(ts.Year, ts.Month, 1, 0, 0, 0);
                    CounterMonthly c = (CounterMonthly)Session.CreateCriteria(typeof(CounterMonthly))
                        .Add(Expression.Eq("Timestamp", ts_current))
                        .UniqueResult();

                    summary.Monthly.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
                    ts = ts.AddMonths(1);
                }
            }

            // yearly hits, five years
            for (int i = -5; i <= 0; i++)
            {
                DateTime ts_current = new DateTime(now.Year + i, 1, 1, 0, 0, 0);
                    CounterYearly c = (CounterYearly)Session.CreateCriteria(typeof(CounterYearly))
                        .Add(Expression.Eq("Timestamp", ts_current))
                        .UniqueResult();

                summary.Yearly.Add((c == null) ? new TransitCounter(ts_current, 0) : new TransitCounter(c.Timestamp, c.Total));
            }

            return summary;
        }
    }
}

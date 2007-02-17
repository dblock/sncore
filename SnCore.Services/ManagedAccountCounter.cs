using System;
using System.Collections.Generic;
using System.Text;
using SnCore.Data;
using NHibernate;

namespace SnCore.Services
{
    public class TimestampCounter<T>
    {
        private T mTotal;

        public T Total
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

        public TimestampCounter(DateTime ts, T tl)
        {
            Total = tl;
            Timestamp = ts;
        }
    }

    public delegate TimestampCounter<TotalType> GetTimestampCounter<CounterType, TotalType>(CounterType counter);
    public delegate void SetTimestampCounter<CounterType, TotalType>(CounterType counter, TimestampCounter<TotalType> tc);

    public class ManagedAccountCounterCollection
    {
        private SortedDictionary<DateTime, long> mYearly = new SortedDictionary<DateTime, long>();

        public SortedDictionary<DateTime, long> Yearly
        {
            get
            {
                return mYearly;
            }
        }

        private SortedDictionary<DateTime, int> mMonthly = new SortedDictionary<DateTime, int>();

        public SortedDictionary<DateTime, int> Monthly
        {
            get
            {
                return mMonthly;
            }
        }

        private SortedDictionary<DateTime, int> mWeekly = new SortedDictionary<DateTime, int>();

        public SortedDictionary<DateTime, int> Weekly
        {
            get
            {
                return mWeekly;
            }
        }

        private SortedDictionary<DateTime, int> mDaily = new SortedDictionary<DateTime, int>();

        public SortedDictionary<DateTime, int> Daily
        {
            get
            {
                return mDaily;
            }
        }

        public ManagedAccountCounterCollection()
        {

        }

        public bool Add(DateTime value)
        {
            bool result = AddDaily(value);
            result |= AddWeekly(value);
            result |= AddMonthly(value);
            result |= AddYearly(value);
            return result;
        }

        protected delegate T Inc<T>(T value);

        static int IncOne(int value) { return value + 1; }
        static long IncOne(long value) { return value + 1; }

        protected static bool Add<T>(SortedDictionary<DateTime, T> dict, DateTime key, Inc<T> incrementor, T defaultvalue)
        {
            T count = default(T);
            bool result = dict.TryGetValue(key, out count);
            if (result) count = incrementor(count); else count = defaultvalue;
            dict[key] = count;
            return result;
        }

        public static DateTime GetDailyTimestamp(DateTime value) { return value.Date; }
        public static DateTime GetWeeklyTimestamp(DateTime value) { while (value.DayOfWeek != DayOfWeek.Sunday) value = value.AddDays(-1); return value.Date; }
        public static DateTime GetMonthlyTimestamp(DateTime value) { return new DateTime(value.Year, value.Month, 1).Date; }
        public static DateTime GetYearlyTimestamp(DateTime value) { return new DateTime(value.Year, 1, 1).Date; }

        public bool AddDaily(DateTime value)
        {
            return Add<int>(mDaily, GetDailyTimestamp(value), IncOne, 1);
        }

        public bool AddMonthly(DateTime value)
        {
            return Add<int>(mMonthly, GetMonthlyTimestamp(value), IncOne, 1);
        }

        public bool AddYearly(DateTime value)
        {
            return Add<long>(mYearly, GetYearlyTimestamp(value), IncOne, 1);
        }

        public bool AddWeekly(DateTime value)
        {
            return Add<int>(mWeekly, GetWeeklyTimestamp(value), IncOne, 1);
        }

        public static void UpdateOrDelete<CounterType, TotalType>(ISession session, SortedDictionary<DateTime, TotalType> dict, CounterType instance,
            GetTimestampCounter<CounterType, TotalType> getTimestampcounter,
            SetTimestampCounter<CounterType, TotalType> setTimestampcounter)
        {
            TotalType total = default(TotalType);
            TimestampCounter<TotalType> tc = getTimestampcounter(instance);
            if (dict.TryGetValue(tc.Timestamp, out total))
            {
                if (!total.Equals(tc.Total))
                {
                    tc.Total = total;
                    setTimestampcounter(instance, tc);
                    session.Save(instance);
                }
                dict.Remove(tc.Timestamp);
            }
            else
            {
                session.Delete(instance);
            }
        }

        public static void Update<CounterType, TotalType>(ISession session, SortedDictionary<DateTime, TotalType> dict,
            GetTimestampCounter<CounterType, TotalType> getTimestampcounter,
            SetTimestampCounter<CounterType, TotalType> setTimestampcounter)
            where CounterType : IDbObject, new()
        {
            IEnumerator<CounterType> counterenumerator = session.CreateQuery(string.Format("FROM {0}", typeof(CounterType).Name))
                .Enumerable<CounterType>().GetEnumerator();

            while (counterenumerator.MoveNext())
            {
                UpdateOrDelete<CounterType, TotalType>(session, dict, counterenumerator.Current,
                    getTimestampcounter, setTimestampcounter);
            }

            SortedDictionary<DateTime, TotalType>.Enumerator remainingenumerator = dict.GetEnumerator();
            while (remainingenumerator.MoveNext())
            {
                CounterType counter = new CounterType();
                TimestampCounter<TotalType> tc = new TimestampCounter<TotalType>(
                    remainingenumerator.Current.Key,
                    remainingenumerator.Current.Value);
                setTimestampcounter(counter, tc);
                session.Save(counter);
            }
        }

        public void SaveAccountCounters(ISession session)
        {
            Update<CounterAccountDaily, int>(session, mDaily,
                delegate(CounterAccountDaily counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(CounterAccountDaily counter, TimestampCounter<int> value) { counter.Timestamp = value.Timestamp; counter.Total = value.Total; }
            );
            Update<CounterAccountWeekly, int>(session, mWeekly,
                delegate(CounterAccountWeekly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(CounterAccountWeekly counter, TimestampCounter<int> value) { counter.Timestamp = value.Timestamp; counter.Total = value.Total; }
            );
            Update<CounterAccountMonthly, int>(session, mMonthly,
                delegate(CounterAccountMonthly counter) { return new TimestampCounter<int>(counter.Timestamp, counter.Total); },
                delegate(CounterAccountMonthly counter, TimestampCounter<int> value) { counter.Timestamp = value.Timestamp; counter.Total = value.Total; }
            );
            Update<CounterAccountYearly, long>(session, mYearly,
               delegate(CounterAccountYearly counter) { return new TimestampCounter<long>(counter.Timestamp, counter.Total); },
               delegate(CounterAccountYearly counter, TimestampCounter<long> value) { counter.Timestamp = value.Timestamp; counter.Total = value.Total; }
            );
        }
    }
}

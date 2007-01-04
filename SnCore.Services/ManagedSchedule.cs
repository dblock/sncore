using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public enum RecurrencePattern
    {
        None = 0,
        Daily_EveryNDays = 1,
        Daily_EveryWeekday = 2,
        Weekly = 3,
        Monthly_DayNOfEveryNMonths = 4,
        Monthly_NthWeekDayOfEveryNMonth = 5,
        Yearly_DayNOfMonth = 6,
        Yearly_NthWeekDayOfMonth = 7
    }

    public enum DaysOfWeek
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    public enum DayIndex
    {
        first = 1,
        second = 2,
        third = 3,
        fourth = 4,
        last = -1
    }

    public enum DayName
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        day = 7,
        weekday = 8,
        weekendday = 9
    }

    public enum MonthName
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    [Serializable()]
    public class TransitSchedule : TransitService<Schedule>
    {
        private int mAccountId;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private bool mAllDay = false;

        public bool AllDay
        {
            get
            {

                return mAllDay;
            }
            set
            {
                mAllDay = value;
            }
        }

        private int mDailyEveryNDays = 1;

        public int DailyEveryNDays
        {
            get
            {

                return mDailyEveryNDays;
            }
            set
            {
                mDailyEveryNDays = value;
            }
        }

        private DateTime mEndDateTime = DateTime.UtcNow.AddHours(1);

        public DateTime EndDateTime
        {
            get
            {

                return mEndDateTime;
            }
            set
            {
                mEndDateTime = value;
            }
        }

        private bool mEndless = true;

        public bool Endless
        {
            get
            {

                return mEndless;
            }
            set
            {
                mEndless = value;
            }
        }

        private int mEndOccurrences = 10;

        public int EndOccurrences
        {
            get
            {

                return mEndOccurrences;
            }
            set
            {
                mEndOccurrences = value;
            }
        }

        private int mMonthlyDay = 1;

        public int MonthlyDay
        {
            get
            {

                return mMonthlyDay;
            }
            set
            {
                mMonthlyDay = value;
            }
        }

        private int mMonthlyExDayIndex = (int)DayIndex.first;

        public int MonthlyExDayIndex
        {
            get
            {

                return mMonthlyExDayIndex;
            }
            set
            {
                mMonthlyExDayIndex = value;
            }
        }

        private int mMonthlyExDayName = (int)DateTime.UtcNow.DayOfWeek;

        public int MonthlyExDayName
        {
            get
            {

                return mMonthlyExDayName;
            }
            set
            {
                mMonthlyExDayName = value;
            }
        }

        private int mMonthlyExMonth = 1;

        public int MonthlyExMonth
        {
            get
            {

                return mMonthlyExMonth;
            }
            set
            {
                mMonthlyExMonth = value;
            }
        }

        private int mMonthlyMonth = 1;

        public int MonthlyMonth
        {
            get
            {

                return mMonthlyMonth;
            }
            set
            {
                mMonthlyMonth = value;
            }
        }

        private RecurrencePattern mRecurrencePattern = RecurrencePattern.None;

        public RecurrencePattern RecurrencePattern
        {
            get
            {

                return mRecurrencePattern;
            }
            set
            {
                mRecurrencePattern = value;
            }
        }

        private DateTime mStartDateTime = DateTime.UtcNow;

        public DateTime StartDateTime
        {
            get
            {

                return mStartDateTime;
            }
            set
            {
                mStartDateTime = value;
            }
        }

        private short mWeeklyDaysOfWeek = (short)Math.Pow(2, (double)DateTime.UtcNow.DayOfWeek);

        public short WeeklyDaysOfWeek
        {
            get
            {

                return mWeeklyDaysOfWeek;
            }
            set
            {
                mWeeklyDaysOfWeek = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        private int mWeeklyEveryNWeeks = 1;

        public int WeeklyEveryNWeeks
        {
            get
            {

                return mWeeklyEveryNWeeks;
            }
            set
            {
                mWeeklyEveryNWeeks = value;
            }
        }

        private int mYearlyDay = DateTime.UtcNow.Day;

        public int YearlyDay
        {
            get
            {

                return mYearlyDay;
            }
            set
            {
                mYearlyDay = value;
            }
        }

        private int mYearlyExDayIndex = 0;

        public int YearlyExDayIndex
        {
            get
            {

                return mYearlyExDayIndex;
            }
            set
            {
                mYearlyExDayIndex = value;
            }
        }

        private int mYearlyExDayName = (int)DateTime.UtcNow.DayOfWeek;

        public int YearlyExDayName
        {
            get
            {

                return mYearlyExDayName;
            }
            set
            {
                mYearlyExDayName = value;
            }
        }

        private int mYearlyExMonth = DateTime.UtcNow.Month;

        public int YearlyExMonth
        {
            get
            {

                return mYearlyExMonth;
            }
            set
            {
                mYearlyExMonth = value;
            }
        }

        private int mYearlyMonth = DateTime.UtcNow.Month;

        public int YearlyMonth
        {
            get
            {

                return mYearlyMonth;
            }
            set
            {
                mYearlyMonth = value;
            }
        }

        public TransitSchedule()
        {

        }

        public TransitSchedule(Schedule value)
            : base(value)
        {

        }

        public override void SetInstance(Schedule value)
        {
            this.AccountId = value.Account.Id;
            this.RecurrencePattern = (RecurrencePattern) value.RecurrencePattern;
            this.Created = value.Created;
            this.Modified = value.Modified;

            switch ((RecurrencePattern) value.RecurrencePattern)
            {
                case RecurrencePattern.None:
                    this.AllDay = value.AllDay;
                    break;
                case RecurrencePattern.Daily_EveryNDays:
                    this.DailyEveryNDays = value.DailyEveryNDays;
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    break;
                case RecurrencePattern.Weekly:
                    this.WeeklyDaysOfWeek = value.WeeklyDaysOfWeek;
                    this.WeeklyEveryNWeeks = value.WeeklyEveryNWeeks;
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    this.MonthlyDay = value.MonthlyDay;
                    this.MonthlyMonth = value.MonthlyMonth;
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    this.MonthlyExDayIndex = value.MonthlyExDayIndex;
                    this.MonthlyExDayName = value.MonthlyExDayName;
                    this.MonthlyExMonth = value.MonthlyExMonth;
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    this.YearlyDay = value.YearlyDay;
                    this.YearlyMonth = value.YearlyMonth;
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    this.YearlyExDayIndex = value.YearlyExDayIndex;
                    this.YearlyExDayName = value.YearlyExDayName;
                    this.YearlyExMonth = value.YearlyExMonth;
                    break;
            }

            this.EndDateTime = value.EndDateTime;
            this.Endless = value.Endless;
            this.EndOccurrences = value.EndOccurrences;
            this.StartDateTime = value.StartDateTime;
            base.SetInstance(value);
        }

        public override Schedule GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Schedule instance = base.GetInstance(session, sec);
            instance.AllDay = this.AllDay;
            instance.DailyEveryNDays = this.DailyEveryNDays;
            instance.EndDateTime = this.EndDateTime;
            instance.Endless = this.Endless;
            instance.EndOccurrences = this.EndOccurrences;
            instance.MonthlyDay = this.MonthlyDay;
            instance.MonthlyMonth = this.MonthlyMonth;
            instance.MonthlyExDayIndex = this.MonthlyExDayIndex;
            instance.MonthlyExDayName = this.MonthlyExDayName;
            instance.MonthlyExMonth = this.MonthlyExMonth;
            instance.RecurrencePattern = (short)this.RecurrencePattern;
            instance.StartDateTime = this.StartDateTime;
            instance.WeeklyDaysOfWeek = this.WeeklyDaysOfWeek;
            instance.WeeklyEveryNWeeks = this.WeeklyEveryNWeeks;
            instance.YearlyDay = this.YearlyDay;
            instance.YearlyMonth = this.YearlyMonth;
            instance.YearlyExDayIndex = this.YearlyExDayIndex;
            instance.YearlyExDayName = this.YearlyExDayName;
            instance.YearlyExMonth = this.YearlyExMonth;
            if (Id == 0 && AccountId > 0) instance.Account = (Account)session.Load(typeof(Account), this.AccountId);
            return instance;
        }
    }

    public class ManagedSchedule : ManagedService<Schedule, TransitSchedule>
    {
        public ManagedSchedule()
        {

        }

        public ManagedSchedule(ISession session)
            : base(session)
        {

        }

        public ManagedSchedule(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedSchedule(ISession session, Schedule value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public string ToString(int offset)
        {
            StringBuilder result = new StringBuilder();
            DateTime start = mInstance.StartDateTime.AddHours(offset);
            DateTime end = mInstance.EndDateTime.AddHours(offset);

            if (mInstance.RecurrencePattern == (short)RecurrencePattern.None)
            {
                if (mInstance.AllDay)
                {
                    if (start.Date == end.Date)
                    {
                        result.AppendFormat("Runs all day {0}.", start.Date.ToString("dddd, dd MMMM yyyy"));
                    }
                    else
                    {
                        result.AppendFormat("Runs all day from {0} through {1}.",
                           start.Date.ToString("dddd, dd MMMM yyyy"),
                           end.Date.ToString("dddd, dd MMMM yyyy"));
                    }
                }
                else
                {
                    TimeSpan delta = (mInstance.EndDateTime - mInstance.StartDateTime);
                    result.AppendFormat("Runs {0} to {1} ({2} hour{3}).",
                        start.ToString("f"),
                        end.ToString(start.Date == end.Date ? "hh:mm tt" : "f"),
                        delta.TotalHours,
                        delta.TotalHours != 1 ? "s" : string.Empty);
                }
                return result.ToString();
            }

            result.AppendFormat("Occurs from {0}:{1} to {2}:{3} ",
                start.TimeOfDay.Hours.ToString("00"), start.TimeOfDay.Minutes.ToString("00"),
                end.TimeOfDay.Hours.ToString("00"), end.TimeOfDay.Minutes.ToString("00"));

            switch ((RecurrencePattern)mInstance.RecurrencePattern)
            {
                case RecurrencePattern.Daily_EveryNDays:
                    result.AppendFormat("every {0} day(s)", mInstance.DailyEveryNDays);
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    result.Append("every weekday");
                    break;
                case RecurrencePattern.Weekly:
                    result.AppendFormat("every {0} week(s) on", mInstance.WeeklyEveryNWeeks);
                    for (int i = 0; i < 7; i++)
                    {
                        if ((mInstance.WeeklyDaysOfWeek & (short)Math.Pow(2, i)) > 0)
                            result.Append(" " + ((DaysOfWeek)i).ToString());
                    }
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    result.AppendFormat("on day {0} of every {1} month(s)", mInstance.MonthlyDay, mInstance.MonthlyMonth);
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    result.AppendFormat("the {0} {1} of every {2} month(s)",
                        ((DayIndex)mInstance.MonthlyExDayIndex).ToString(),
                        ((DayName)mInstance.MonthlyExDayName).ToString(),
                        mInstance.MonthlyExMonth);
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    result.AppendFormat("yearly every {0} {1}",
                        ((MonthName)mInstance.YearlyMonth).ToString(),
                        mInstance.YearlyDay);
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    result.AppendFormat("yearly the {0} {1} of {2}",
                        ((DayIndex)mInstance.YearlyExDayIndex).ToString(),
                        ((DayName)mInstance.YearlyExDayName).ToString(),
                        ((MonthName)mInstance.YearlyExMonth).ToString());
                    break;
            }

            result.AppendFormat(" starting {0}", start.Date.ToString("dddd, dd MMMM yyyy"));

            if (!mInstance.Endless)
            {
                if (mInstance.EndOccurrences > 0)
                {
                    result.AppendFormat(". Ends after {0} occurrences.", mInstance.EndOccurrences);
                }
                else
                {
                    result.AppendFormat(" through {0}.", end.Date.ToString("dddd, dd MMMM yyyy"));
                }
            }
            else
            {
                result.Append(".");
            }

            return result.ToString();
        }

        public RecurrencePattern RecurrencePattern
        {
            get
            {
                return (RecurrencePattern)mInstance.RecurrencePattern;
            }
        }

        /// <summary>
        /// Update is needed if there're no schedule instances or schedule instances
        /// that have been modified before this schedule was last saved.
        /// </summary>
        public bool NeedsUpdate
        {
            get
            {
                if (mInstance.ScheduleInstances == null)
                    return true;

                foreach (ScheduleInstance instance in Collection<ScheduleInstance>.GetSafeCollection(mInstance.ScheduleInstances))
                {
                    if (instance.Modified < mInstance.Modified)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Generate/update instances of schedule.
        /// Reuse existing instances when possible.
        /// </summary>
        /// <returns>number of instances generated</returns>
        public int UpdateInstances()
        {
            int result =
                (RecurrencePattern == RecurrencePattern.None) ?
                    UpdateInstances_None() : UpdateInstances_Recurrent();

            Session.Save(mInstance);
            return result;
        }

        /// <summary>
        /// Get or create a schedule instance.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private ScheduleInstance GetOrCreateScheduleInstance(int index)
        {
            if (mInstance.ScheduleInstances == null)
            {
                mInstance.ScheduleInstances = new List<ScheduleInstance>();
            }

            if (index < mInstance.ScheduleInstances.Count)
            {
                ScheduleInstance instance = (ScheduleInstance)mInstance.ScheduleInstances[index];
                instance.Modified = mInstance.Modified;
                return instance;
            }

            ScheduleInstance result = new ScheduleInstance();
            result.Schedule = mInstance;
            result.Instance = index;
            result.Created = result.Modified = mInstance.Modified;

            mInstance.ScheduleInstances.Add(result);

            return result;
        }

        private int UpdateInstances_None()
        {
            ScheduleInstance instance = GetOrCreateScheduleInstance(0);
            instance.StartDateTime = mInstance.StartDateTime;
            instance.EndDateTime = mInstance.AllDay ? mInstance.EndDateTime.AddDays(1) : mInstance.EndDateTime;
            Session.Save(instance);
            return 1;
        }

        private const int ScheduleInstancesAhead = 25;

        public bool IsRecurrentPastDate(DateTime current, int instance)
        {
            // only schedule ScheduleInstancesAhead instances ahead
            if (instance >= ScheduleInstancesAhead)
                return true;

            // end after X occurrences
            if (mInstance.EndOccurrences > 0 && !mInstance.Endless && (instance >= mInstance.EndOccurrences))
                return true;

            // an event that is not endless ends on EndDateTime
            if ((mInstance.EndOccurrences == 0 && !mInstance.Endless && (current > mInstance.EndDateTime)))
                return true;

            return false;
        }

        public DateTime GetNextRecurrence(DateTime current)
        {
            switch (RecurrencePattern)
            {
                // every n days, simply adds number of days
                case RecurrencePattern.Daily_EveryNDays:
                    {
                        current = current.AddDays(mInstance.DailyEveryNDays);
                        break;
                    }
                // every week-day skips week-end days
                case RecurrencePattern.Daily_EveryWeekday:
                    {
                        do
                        {
                            current = current.AddDays(1);
                        }
                        while (current.DayOfWeek == DayOfWeek.Saturday || current.DayOfWeek == DayOfWeek.Sunday);
                        break;
                    }
                // weekly occurs on certain days only
                case RecurrencePattern.Weekly:
                    {
                        // if doesn't ever occur (should be disallowed by UI)
                        if (mInstance.WeeklyDaysOfWeek == 0)
                        {
                            throw new Exception("No days of week selected for a weekly schedule.");
                        }
                        else
                        {
                            do
                            {
                                current = current.AddDays(1);
                            } while (((short)Math.Pow(2, (short)current.DayOfWeek) & mInstance.WeeklyDaysOfWeek) == 0);
                        }

                        break;
                    }
                // occurs on a certain day of every N months
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    {
                        int skipped = 0;
                        do
                        {
                            do
                            {
                                current = current.AddDays(1);
                            }
                            while (current.Day != mInstance.MonthlyDay);

                            skipped++;
                        }
                        while (mInstance.MonthlyMonth != 0 && skipped != mInstance.MonthlyMonth);
                        break;
                    }

                // the ((DayIndex)mInstance.MonthlyExDayIndex) 
                // (DayName)mInstance.MonthlyExDayName 
                // of every mInstance.MonthlyExMonth month(s)
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    {
                        int skipped_matchingmonth = 0;
                        do
                        {
                            while (true)
                            {
                                current = current.AddDays(1);

                                // is this a matching day name
                                if (
                                    mInstance.MonthlyExDayName == (int)current.DayOfWeek ||
                                    mInstance.MonthlyExDayName == (short)DayName.day ||
                                    (mInstance.MonthlyExDayName == (short)DayName.weekday
                                        && current.DayOfWeek != DayOfWeek.Sunday
                                        && current.DayOfWeek != DayOfWeek.Saturday) ||
                                    (mInstance.MonthlyExDayName == (short)DayName.weekendday
                                        && (current.DayOfWeek == DayOfWeek.Sunday
                                            || current.DayOfWeek == DayOfWeek.Saturday))
                                   )
                                {
                                    // is it the last occurence this month?
                                    if (mInstance.MonthlyExDayIndex == (short)DayIndex.last &&
                                        CBusinessDay.IsLastDayOfWeekOccurrenceThisMonth(current))
                                        break;

                                    // which occurrence is it this month?
                                    if (CBusinessDay.GetDayOfWeekOccurrenceThisMonth(current) == mInstance.MonthlyExDayIndex)
                                        break;
                                }
                            }

                            skipped_matchingmonth++;
                        }
                        while (mInstance.MonthlyExMonth != 0 && skipped_matchingmonth != mInstance.MonthlyExMonth);
                        break;
                    }

                // yearly every ((MonthName)mInstance.YearlyMonth) YearlyDay
                case RecurrencePattern.Yearly_DayNOfMonth:
                    {
                        current = current.AddDays(1); // skip the current instance

                        // instance this year
                        DateTime result = current.AddYears(-1);

                        do
                        {
                            // get the instance next year
                            result = new DateTime(result.Year + 1, mInstance.YearlyMonth, mInstance.YearlyDay)
                                .Add(current.TimeOfDay);

                        } while (current > result);

                        current = result;
                        break;
                    }

                // yearly the ((DayIndex)mInstance.YearlyExDayIndex) ((DayName)mInstance.YearlyExDayName) of ((MonthName)mInstance.YearlyExMonth)
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    {
                        current = current.AddDays(1); // skip the current instance

                        // start a year ago, gets incremented in the first loop
                        DateTime result = current.AddYears(-1);

                        do
                        {
                            // get the instance next year
                            result = new DateTime(result.Year + 1, mInstance.YearlyExMonth, 1)
                                .Add(current.TimeOfDay);

                            while (true)
                            {
                                if ((short)result.DayOfWeek == mInstance.YearlyExDayName)
                                {
                                    // is it the last occurence this month?
                                    if (mInstance.YearlyExDayIndex == (short)DayIndex.last &&
                                        CBusinessDay.IsLastDayOfWeekOccurrenceThisMonth(result))
                                        break;

                                    // which occurrence is it this month?
                                    if (CBusinessDay.GetDayOfWeekOccurrenceThisMonth(result) == mInstance.YearlyExDayIndex)
                                        break;
                                }

                                result = result.AddDays(1);
                            }
                        } while (current > result);

                        current = result;
                        break;
                    }
            }

            return current;
        }

        public int UpdateInstances_Recurrent()
        {
            // find the first occurrence starting with yesterday (this morning)

            // daily schedule is an exception, it's not a search and starts today
            DateTime current = (RecurrencePattern == RecurrencePattern.Daily_EveryNDays)
                ? mInstance.StartDateTime
                : GetNextRecurrence(mInstance.StartDateTime.AddDays(-1));

            int instance = 0;
            while (!IsRecurrentPastDate(current, instance))
            {
                ScheduleInstance si = GetOrCreateScheduleInstance(instance);
                // starts on the date, with a timeofday delta (days may have 23 hours for daylight saving)
                si.StartDateTime = current.Date.Add(mInstance.StartDateTime.TimeOfDay);
                // add the duration of the recurrent event
                si.EndDateTime = current.Add(mInstance.EndDateTime.TimeOfDay - mInstance.StartDateTime.TimeOfDay);
                // instance number
                si.Instance = instance;
                // save
                Session.Save(si);
                // next occurence + every N days
                current = GetNextRecurrence(current);
                instance++;
            }

            return instance;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
            UpdateInstances();
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

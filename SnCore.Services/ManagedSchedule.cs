using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools;

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
        first = 0,
        second = 1,
        third = 2,
        fourth = 3,
        last = 4
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
    public class TransitSchedule : TransitService
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

        private int mMonthlyExDayIndex = 0;

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

        public TransitSchedule(Schedule s)
            : base(s.Id)
        {
            this.AccountId = s.Account.Id;
            this.RecurrencePattern = (RecurrencePattern)s.RecurrencePattern;
            this.Created = s.Created;
            this.Modified = s.Modified;

            switch ((RecurrencePattern)s.RecurrencePattern)
            {
                case RecurrencePattern.None:
                    this.AllDay = s.AllDay;
                    break;
                case RecurrencePattern.Daily_EveryNDays:
                    this.DailyEveryNDays = s.DailyEveryNDays;
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    break;
                case RecurrencePattern.Weekly:
                    this.WeeklyDaysOfWeek = s.WeeklyDaysOfWeek;
                    this.WeeklyEveryNWeeks = s.WeeklyEveryNWeeks;
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    this.MonthlyDay = s.MonthlyDay;
                    this.MonthlyMonth = s.MonthlyMonth;
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    this.MonthlyExDayIndex = s.MonthlyExDayIndex;
                    this.MonthlyExDayName = s.MonthlyExDayName;
                    this.MonthlyExMonth = s.MonthlyExMonth;
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    this.YearlyDay = s.YearlyDay;
                    this.YearlyMonth = s.YearlyMonth;
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    this.YearlyExDayIndex = s.YearlyExDayIndex;
                    this.YearlyExDayName = s.YearlyExDayName;
                    this.YearlyExMonth = s.YearlyExMonth;
                    break;
            }

            this.EndDateTime = s.EndDateTime;
            this.Endless = s.Endless;
            this.EndOccurrences = s.EndOccurrences;
            this.StartDateTime = s.StartDateTime;
        }

        public Schedule GetSchedule(ISession session)
        {
            Schedule p = (Id != 0) ? (Schedule)session.Load(typeof(Schedule), Id) : new Schedule();
            p.AllDay = this.AllDay;
            p.DailyEveryNDays = this.DailyEveryNDays;
            p.EndDateTime = this.EndDateTime;
            p.Endless = this.Endless;
            p.EndOccurrences = this.EndOccurrences;
            p.MonthlyDay = this.MonthlyDay;
            p.MonthlyMonth = this.MonthlyMonth;
            p.MonthlyExDayIndex = this.MonthlyExDayIndex;
            p.MonthlyExDayName = this.MonthlyExDayName;
            p.MonthlyExMonth = this.MonthlyExMonth;
            p.RecurrencePattern = (short)this.RecurrencePattern;
            p.StartDateTime = this.StartDateTime;
            p.WeeklyDaysOfWeek = this.WeeklyDaysOfWeek;
            p.WeeklyEveryNWeeks = this.WeeklyEveryNWeeks;
            p.YearlyDay = this.YearlyDay;
            p.YearlyMonth = this.YearlyMonth;
            p.YearlyExDayIndex = this.YearlyExDayIndex;
            p.YearlyExDayName = this.YearlyExDayName;
            p.YearlyExMonth = this.YearlyExMonth;
            if (Id == 0 && AccountId > 0) p.Account = (Account)session.Load(typeof(Account), this.AccountId);
            return p;
        }

    }

    public class ManagedSchedule : ManagedService
    {
        private Schedule mSchedule = null;

        public ManagedSchedule(ISession session)
            : base(session)
        {

        }

        public ManagedSchedule(ISession session, int id)
            : base(session)
        {
            mSchedule = (Schedule)session.Load(typeof(Schedule), id);
        }

        public ManagedSchedule(ISession session, Schedule value)
            : base(session)
        {
            mSchedule = value;
        }

        public int Id
        {
            get
            {
                return mSchedule.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mSchedule.Account.Id;
            }
        }

        public TransitSchedule TransitSchedule
        {
            get
            {
                return new TransitSchedule(mSchedule);
            }
        }

        public string ToString(int offset)
        {
            StringBuilder result = new StringBuilder();
            DateTime start = mSchedule.StartDateTime.AddHours(offset);
            DateTime end = mSchedule.EndDateTime.AddHours(offset);

            if (mSchedule.RecurrencePattern == (short)RecurrencePattern.None)
            {
                if (mSchedule.AllDay)
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
                    TimeSpan delta = (mSchedule.EndDateTime - mSchedule.StartDateTime);
                    result.AppendFormat("Runs {0} to {1} (for {2} hour(s) {3} minute(s)).",
                        start.ToString("f"),
                        end.ToString(start.Date == end.Date ? "hh:mm tt" : "f"),
                        (int)delta.TotalHours, delta.Minutes);
                }
                return result.ToString();
            }

            result.AppendFormat("Occurs from {0}:{1} to {2}:{3} ",
                start.TimeOfDay.Hours.ToString("00"), start.TimeOfDay.Minutes.ToString("00"),
                end.TimeOfDay.Hours.ToString("00"), end.TimeOfDay.Minutes.ToString("00"));

            switch ((RecurrencePattern)mSchedule.RecurrencePattern)
            {
                case RecurrencePattern.Daily_EveryNDays:
                    result.AppendFormat("every {0} day(s)", mSchedule.DailyEveryNDays);
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    result.Append("every weekday");
                    break;
                case RecurrencePattern.Weekly:
                    result.AppendFormat("every {0} week(s) on", mSchedule.WeeklyEveryNWeeks);
                    for (int i = 0; i < 7; i++)
                    {
                        if ((mSchedule.WeeklyDaysOfWeek & (short)Math.Pow(2, i)) > 0)
                            result.Append(" " + ((DaysOfWeek)i).ToString());
                    }
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    result.AppendFormat("on day {0} of every {1} month(s)", mSchedule.MonthlyDay, mSchedule.MonthlyMonth);
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    result.AppendFormat("the {0} {1} of every {2} month(s)",
                        ((DayIndex)mSchedule.MonthlyExDayIndex).ToString(),
                        ((DayName)mSchedule.MonthlyExDayName).ToString(),
                        mSchedule.MonthlyExMonth);
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    result.AppendFormat("yearly every {0} {1}",
                        ((MonthName)mSchedule.YearlyMonth).ToString(),
                        mSchedule.YearlyDay);
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    result.AppendFormat("yearly the {0} {1} of {2}",
                        ((DayIndex)mSchedule.YearlyExDayIndex).ToString(),
                        ((DayName)mSchedule.YearlyExDayName).ToString(),
                        ((MonthName)mSchedule.YearlyExMonth).ToString());
                    break;
            }

            result.AppendFormat(" starting {0}", start.Date.ToString("dddd, dd MMMM yyyy"));

            if (!mSchedule.Endless)
            {
                if (mSchedule.EndOccurrences > 0)
                {
                    result.AppendFormat(". Ends after {0} occurrences.", mSchedule.EndOccurrences);
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

        public void Delete()
        {
            Session.Delete(mSchedule);
        }

        public bool IsInRange(DateTime start)
        {
            return IsInRange(start, start);
        }

        public bool IsInRange(DateTime start, DateTime end)
        {
            if (start == DateTime.MinValue && end == DateTime.MaxValue)
                return true;

            if (start == DateTime.MinValue || end == DateTime.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Missing start or end date.");
            }

            if (start > end)
            {
                throw new ArgumentOutOfRangeException("Start date cannot be greater than end date.");
            }


            switch((RecurrencePattern)mSchedule.RecurrencePattern)
            {
                case RecurrencePattern.None:
                    
                    // an event that starts after the end?
                    if (mSchedule.StartDateTime > end)
                        return false;

                    // an event that ends before the start?
                    if (mSchedule.EndDateTime < start)
                        return false;

                    return true;
            }

            throw new NotImplementedException();

            //// an event that ends before start?
            //if ((RecurrencePattern)mSchedule.RecurrencePattern != RecurrencePattern.None
            //    && mSchedule.EndOccurrences == 0 
            //    && mSchedule.EndDateTime < start
            //    && ! mSchedule.Endless)
            //{
            //    return false;
            //}

            //DateTime current = start;
            //while (current <= end)
            //{
            //    switch ((RecurrencePattern)mSchedule.RecurrencePattern)
            //    {
            //        case RecurrencePattern.Daily_EveryNDays:
            //            {
            //                // event occurs every n days
            //                TimeSpan elapsed = current.Subtract(mSchedule.StartDateTime);

            //                int nthday = 0;
            //                int occurrences = Math.DivRem((int)elapsed.TotalDays, mSchedule.DailyEveryNDays, out nthday);

            //                if (!mSchedule.Endless && mSchedule.EndOccurrences > 0)
            //                {
            //                    // too many occurrences already
            //                    if (occurrences >= mSchedule.EndOccurrences)
            //                        return false;
            //                }

            //                // n'th day of week
            //                if (nthday == 0)
            //                    return true;
            //            }
            //            break;
            //        case RecurrencePattern.Daily_EveryWeekday:
            //            {
            //                if (!mSchedule.Endless && mSchedule.EndOccurrences > 0)
            //                {
            //                    // event occurs every business day
            //                    double totaloccurences = CBusinessDay.CalculateBDay(
            //                        mSchedule.StartDateTime,
            //                        current,
            //                        5,
            //                        0);

            //                    // too many occurrences already
            //                    if (totaloccurences >= mSchedule.EndOccurrences)
            //                        return false;
            //                }

            //                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
            //                    return true;

            //            }
            //            break;

            //        case RecurrencePattern.Weekly:
            //            {
            //                // weekly, every N weeks on certain days

            //                // not any of the selected days of week?
            //                if (((short)Math.Pow(2, (short)current.DayOfWeek) & mSchedule.WeeklyDaysOfWeek) == 0)
            //                    return false;

            //                // n-th week
            //                int elapsedweeks = CBusinessDay.GetWeeks(mSchedule.StartDateTime, current);

            //                if (!mSchedule.Endless && mSchedule.EndOccurrences > 0)
            //                {
            //                    // TODO: dumb
            //                    int totaloccurrences = 0;
            //                    DateTime _current = mSchedule.StartDateTime;
            //                    while (_current < current)
            //                    {
            //                        if ((mSchedule.WeeklyDaysOfWeek & (short)Math.Pow(2, (short) _current.DayOfWeek)) > 0)
            //                        {
            //                            if (++totaloccurrences > mSchedule.EndOccurrences)
            //                                return false;
            //                        }

            //                        _current = _current.AddDays(1);

            //                        // skip a number of weeks for every n-th week
            //                        if (_current.DayOfWeek == mSchedule.StartDateTime.DayOfWeek && mSchedule.WeeklyEveryNWeeks > 0)
            //                            _current = _current.AddDays((mSchedule.WeeklyEveryNWeeks - 1) * 7);
            //                    }
            //                }

            //                // every week
            //                if (mSchedule.WeeklyEveryNWeeks <= 1)
            //                    return true;

            //                int nthweek = 0;
            //                Math.DivRem(elapsedweeks, mSchedule.WeeklyEveryNWeeks, out nthweek);

            //                if (nthweek == 0)
            //                    return true;
            //            }
            //            break;
            //        case RecurrencePattern.Monthly_DayNOfEveryNMonths:
            //            {
            //                // is this Day N of the month
            //                if (current.Day != mSchedule.MonthlyDay)
            //                    return false;                            

            //                //this.MonthlyDay = s.MonthlyDay;
            //                //this.MonthlyMonth = s.MonthlyMonth;
            //            }
            //            break;
            //        case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
            //            //this.MonthlyExDayIndex = s.MonthlyExDayIndex;
            //            //this.MonthlyExDayName = s.MonthlyExDayName;
            //            //this.MonthlyExMonth = s.MonthlyExMonth;
            //            break;
            //        case RecurrencePattern.Yearly_DayNOfMonth:
            //            //this.YearlyDay = s.YearlyDay;
            //            //this.YearlyMonth = s.YearlyMonth;
            //            break;
            //        case RecurrencePattern.Yearly_NthWeekDayOfMonth:
            //            //this.YearlyExDayIndex = s.YearlyExDayIndex;
            //            //this.YearlyExDayName = s.YearlyExDayName;
            //            //this.YearlyExMonth = s.YearlyExMonth;
            //            break;
            //    }

            //    current = current.AddDays(1);
            //}

            //return false;
        }

    }

}

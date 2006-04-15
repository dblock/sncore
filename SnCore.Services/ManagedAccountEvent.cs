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
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitAccountEventQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public string Country;
        public string State;
        public string City;
        public string Name;
        public string Type;
        public DateTime StartDateTime = DateTime.MinValue;
        public DateTime EndDateTime = DateTime.MaxValue;

        public TransitAccountEventQueryOptions()
        {
        }

        public string CreateSubQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Place.City.Id = '{0}'", ManagedCity.GetCityId(session, City, State, Country));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Place.City.Country.Id = {0}", ManagedCountry.GetCountryId(session, Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Place.City.State.Id = {0}", ManagedState.GetStateId(session, State, Country));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Type))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.AccountEventType.Id = {0}", ManagedAccountEventType.FindId(session, Type));
            }

            // exclude non-published events
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Publish = 1");
            }

            return b.ToString();
        }

        public IQuery CreateCountQuery(ISession session)
        {
            return session.CreateQuery("SELECT COUNT(e) FROM AccountEvent e " + CreateSubQuery(session));
        }

        public IQuery CreateQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT e FROM AccountEvent e");
            b.Append(CreateSubQuery(session));
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return session.CreateQuery(b.ToString());
        }
    };

    public class TransitAccountEvent : TransitService
    {
        private int mPictureId = 0;

        public int PictureId
        {
            get
            {

                return mPictureId;
            }
            set
            {
                mPictureId = value;
            }
        }

        private int mPlaceId;

        public int PlaceId
        {
            get
            {
                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
            }
        }

        private string mPlaceName;

        public string PlaceName
        {
            get
            {
                return mPlaceName;
            }
            set
            {
                mPlaceName = value;
            }
        }

        private string mPlaceCity;

        public string PlaceCity
        {
            get
            {
                return mPlaceCity;
            }
            set
            {
                mPlaceCity = value;
            }
        }

        private string mPlaceState;

        public string PlaceState
        {
            get
            {
                return mPlaceState;
            }
            set
            {
                mPlaceState = value;
            }
        }

        private string mPlaceCountry;

        public string PlaceCountry
        {
            get
            {
                return mPlaceCountry;
            }
            set
            {
                mPlaceCountry = value;
            }
        }

        private string mAccountEventType;

        public string AccountEventType
        {
            get
            {

                return mAccountEventType;
            }
            set
            {
                mAccountEventType = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private int mAccountId = 0;

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

        private string mAccountName;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private int mScheduleId;

        public int ScheduleId
        {
            get
            {
                return mScheduleId;
            }
            set
            {
                mScheduleId = value;
            }
        }

        private bool mPublish;

        public bool Publish
        {
            get
            {
                return mPublish;
            }
            set
            {
                mPublish = value;
            }
        }

        private string mCost;

        public string Cost
        {
            get
            {
                return mCost;
            }
            set
            {
                mCost = value;
            }
        }

        private string mWebsite;

        public string Website
        {
            get
            {
                return mWebsite;
            }
            set
            {
                mWebsite = value;
            }
        }

        private string mEmail;

        public string Email
        {
            get
            {
                return mEmail;
            }
            set
            {
                mEmail = value;
            }
        }

        private string mPhone;

        public string Phone
        {
            get
            {
                return mPhone;
            }
            set
            {
                mPhone = value;
            }
        }

        private string mSchedule;

        public string Schedule
        {
            get
            {
                return mSchedule;
            }
            set
            {
                mSchedule = value;
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

        public TransitAccountEvent()
        {

        }

        public TransitAccountEvent(AccountEvent o)
            : base(o.Id)
        {
            AccountEventType = o.AccountEventType.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            ScheduleId = o.Schedule.Id;
            PlaceId = o.Place.Id;
            PlaceName = o.Place.Name;
            PlaceCity = o.Place.City.Name;
            PlaceCountry = o.Place.City.Country.Name;
            PlaceState = o.Place.City.State.Name;
            Name = o.Name;
            Phone = o.Phone;
            Email = o.Email;
            Website = o.Website;
            Cost = o.Cost;
            Publish = o.Publish;
            PictureId = ManagedService.GetRandomElementId(o.AccountEventPictures);
        }

        public AccountEvent GetAccountEvent(ISession session)
        {
            AccountEvent e = (Id != 0) ? (AccountEvent)session.Load(typeof(AccountEvent), Id) : new AccountEvent();
            e.Description = this.Description;
            e.Name = this.Name;
            e.Phone = this.Phone;
            e.Email = this.Email;
            e.Website = this.Website;
            e.Cost = this.Cost;
            e.Publish = this.Publish;
            e.AccountEventType = ManagedAccountEventType.Find(session, this.AccountEventType);

            if (Id == 0)
            {
                // the account and the Event cannot be switched after the relationship is created
                if (AccountId > 0) e.Account = (Account)session.Load(typeof(Account), AccountId);
                if (PlaceId > 0) e.Place = (Place)session.Load(typeof(Place), PlaceId);
                if (ScheduleId > 0) e.Schedule = (Schedule)session.Load(typeof(Schedule), ScheduleId);
            }

            return e;
        }

        public void CreateSchedule(ISession session, int offset)
        {
            ManagedSchedule m_s = new ManagedSchedule(session, ScheduleId);
            Schedule = m_s.ToString(offset);

            TransitSchedule s = m_s.TransitSchedule;

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

            this.EndDateTime = s.EndDateTime.AddHours(offset);
            this.Endless = s.Endless;
            this.EndOccurrences = s.EndOccurrences;
            this.StartDateTime = s.StartDateTime.AddHours(offset);
        }
    }

    public class ManagedAccountEvent : ManagedService
    {
        private AccountEvent mAccountEvent = null;

        public ManagedAccountEvent(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEvent(ISession session, int id)
            : base(session)
        {
            mAccountEvent = (AccountEvent)session.Load(typeof(AccountEvent), id);
        }

        public ManagedAccountEvent(ISession session, AccountEvent value)
            : base(session)
        {
            mAccountEvent = value;
        }

        public ManagedAccountEvent(ISession session, TransitAccountEvent value)
            : base(session)
        {
            mAccountEvent = value.GetAccountEvent(session);
        }

        public int Id
        {
            get
            {
                return mAccountEvent.Id;
            }
        }

        public TransitAccountEvent TransitAccountEvent
        {
            get
            {
                return new TransitAccountEvent(mAccountEvent);
            }
        }

        public Account Account
        {
            get
            {
                return mAccountEvent.Account;
            }
        }

        public void Delete()
        {
            Session.Delete(string.Format("from Feature f where f.DataObjectId = {0} AND f.DataRowId = {1}",
                ManagedDataObject.Find(Session, "AccountEvent"), Id));
            mAccountEvent.Account.AccountEvents.Remove(mAccountEvent);
            Session.Delete(mAccountEvent);
        }

        public bool IsInRange(DateTime start, DateTime end)
        {
            return new ManagedSchedule(Session, mAccountEvent.Schedule).IsInRange(start, end);
        }
    }
}

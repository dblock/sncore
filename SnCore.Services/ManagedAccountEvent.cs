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
using SnCore.Tools.Web;
using SnCore.Data.Hibernate;

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
        public string Neighborhood;
        public string Type;
        public DateTime StartDateTime = DateTime.MinValue;
        public DateTime EndDateTime = DateTime.MaxValue;

        public TransitAccountEventQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(Neighborhood))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.Neighborhood.Name = '{0}'", Renderer.SqlEncode(Neighborhood));
            }

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.Name = '{0}'", Renderer.SqlEncode(City));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.Country.Name = '{0}'", Renderer.SqlEncode(Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Place.City.State.Name = '{0}'", Renderer.SqlEncode(State));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Type))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.AccountEventType.Name = '{0}'", Renderer.SqlEncode(Type));
            }

            // exclude non-published events
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountEvent.Publish = 1");
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT AccountEvent FROM AccountEvent AccountEvent");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY AccountEvent.{0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return b.ToString();
        }
    };

    public class TransitAccountEvent : TransitService<AccountEvent>
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

        private string mPlaceNeighborhood;

        public string PlaceNeighborhood
        {
            get
            {
                return mPlaceNeighborhood;
            }
            set
            {
                mPlaceNeighborhood = value;
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

        private string mInstanceType;

        public string AccountEventType
        {
            get
            {

                return mInstanceType;
            }
            set
            {
                mInstanceType = value;
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

        private bool mNoEndDateTime = true;

        public bool NoEndDateTime
        {
            get
            {
                return mNoEndDateTime;
            }
            set
            {
                mNoEndDateTime = value;
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

        public TransitAccountEvent(AccountEvent instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountEvent instance)
        {
            AccountEventType = instance.AccountEventType.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            ScheduleId = instance.Schedule.Id;
            PlaceId = instance.Place.Id;
            PlaceName = instance.Place.Name;
            if (instance.Place.City != null) PlaceCity = instance.Place.City.Name;
            if (instance.Place.City != null && instance.Place.City.Country != null) PlaceCountry = instance.Place.City.Country.Name;
            if (instance.Place.City != null && instance.Place.City.State != null) PlaceState = instance.Place.City.State.Name;
            if (instance.Place.Neighborhood != null) PlaceNeighborhood = instance.Place.Neighborhood.Name;
            Name = instance.Name;
            Phone = instance.Phone;
            Email = instance.Email;
            Website = instance.Website;
            Cost = instance.Cost;
            Publish = instance.Publish;
            PictureId = ManagedService<AccountEventPicture, TransitAccountEventPicture>.GetRandomElementId(instance.AccountEventPictures);
            base.SetInstance(instance);
        }

        public override AccountEvent GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEvent instance = base.GetInstance(session, sec);
            instance.Description = this.Description;
            instance.Name = this.Name;
            instance.Phone = this.Phone;
            instance.Email = this.Email;
            instance.Website = this.Website;
            instance.Cost = this.Cost;
            instance.Publish = this.Publish;
            instance.AccountEventType = ManagedAccountEventType.Find(session, this.AccountEventType);

            if (Id == 0)
            {
                // the account and the Event cannot be switched after the relationship is created
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Schedule = session.Load<Schedule>(ScheduleId);
            }

            if (PlaceId > 0) instance.Place = session.Load<Place>(PlaceId);
            return instance;
        }

        public void CreateSchedule(ISession session, int offset, ManagedSecurityContext sec)
        {
            ManagedSchedule m_s = new ManagedSchedule(session, ScheduleId);
            Schedule = m_s.ToString(offset);

            TransitSchedule s = m_s.GetTransitInstance(sec);

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
            this.NoEndDateTime = s.NoEndDateTime;
            this.EndOccurrences = s.EndOccurrences;
            this.StartDateTime = s.StartDateTime.AddHours(offset);
        }
    }

    public class ManagedAccountEvent : ManagedService<AccountEvent, TransitAccountEvent>
    {
        public ManagedAccountEvent()
        {

        }

        public ManagedAccountEvent(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEvent(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEvent(ISession session, AccountEvent value)
            : base(session, value)
        {

        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.Account.Id, typeof(AccountEvent), mInstance.Id, sec);
            ManagedFeature.Delete(Session, "AccountEvent", Id);
            Collection<AccountEvent>.GetSafeCollection(mInstance.Account.AccountEvents).Remove(mInstance);
            base.Delete(sec);
        }

        public string ToVCalendarString(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);

            StringBuilder b = new StringBuilder();
            b.AppendLine("BEGIN:VCALENDAR");
            b.AppendLine("PRODID:-//Vestris Inc.//SnCore 1.0 MIME//EN");
            b.AppendLine("VERSION:2.0");
            b.AppendLine("METHOD:PUBLISH");
            b.AppendLine("BEGIN:VEVENT");
            b.AppendLine(string.Format("DTSTAMP:{0}", DateTime.UtcNow.ToString(@"yyyyMMdd\THHmmss\Z")));
            b.AppendLine(string.Format("UID:{0}", Id.ToString()));

            StringBuilder fb = new StringBuilder();
            if (!mInstance.Schedule.Endless)
            {
                fb.Append((mInstance.Schedule.EndOccurrences > 0)
                    ? string.Format("COUNT={0};", mInstance.Schedule.EndOccurrences)
                    : string.Format("UNTIL={0};", mInstance.Schedule.EndDateTime.ToString(@"yyyyMMdd\THHmmss\Z")));
            }

            if (mInstance.Schedule.AllDay)
            {
                b.AppendLine(string.Format("DTSTART;VALUE=DATE:{0}", mInstance.Schedule.StartDateTime.ToString(@"yyyyMMdd")));
                b.AppendLine(string.Format("DTEND;VALUE=DATE:{0}", mInstance.Schedule.EndDateTime.AddDays(1).ToString(@"yyyyMMdd")));
            }
            else
            {
                b.AppendLine(string.Format("DTSTART:{0}", mInstance.Schedule.StartDateTime.ToString(@"yyyyMMdd\THHmmss\Z")));
                b.AppendLine(string.Format("DTEND:{0}", mInstance.Schedule.EndDateTime.ToString(@"yyyyMMdd\THHmmss\Z")));
            }

            switch ((RecurrencePattern)mInstance.Schedule.RecurrencePattern)
            {
                case RecurrencePattern.None:
                    break;
                case RecurrencePattern.Daily_EveryNDays:
                    b.Append("RRULE:FREQ=DAILY;");
                    b.Append(fb.ToString());
                    b.AppendLine(string.Format("INTERVAL={0}", mInstance.Schedule.DailyEveryNDays));
                    break;
                case RecurrencePattern.Daily_EveryWeekday:
                    b.AppendFormat("RRULE:FREQ=DAILY;");
                    b.Append(fb.ToString());
                    b.AppendLine("INTERVAL=1;BYDAY=MO,TU,WE,TH,FR;WKST=SU");
                    break;
                case RecurrencePattern.Weekly:
                    b.AppendFormat("RRULE:FREQ=WEEKLY;");
                    b.Append(fb.ToString());
                    b.Append(string.Format("INTERVAL={0};", mInstance.Schedule.WeeklyEveryNWeeks));
                    StringBuilder wb = new StringBuilder();
                    for (int i = 0; i < 7; i++)
                    {
                        if ((mInstance.Schedule.WeeklyDaysOfWeek & (short)Math.Pow(2, i)) > 0)
                        {
                            if (wb.Length > 0) wb.Append(",");
                            wb.Append(((DayOfWeek)i).ToString().Substring(0, 2).ToUpper());
                        }
                    }
                    b.Append("BYDAY=");
                    b.Append(wb.ToString());
                    b.AppendLine(";WKST=SU");
                    break;
                case RecurrencePattern.Monthly_DayNOfEveryNMonths:
                    b.Append("RRULE:FREQ=MONTHLY;");
                    b.Append(fb.ToString());
                    b.AppendLine(string.Format("INTERVAL={1};BYMONTHDAY={0};WKST=SU",
                        mInstance.Schedule.MonthlyDay, mInstance.Schedule.MonthlyMonth));
                    break;
                case RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth:
                    // first thursday of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=TH;BYSETPOS=1;WKST=SU
                    // first day of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=SU,MO,TU,WE,TH,FR,SA;BYSETPOS=1;WKST=SU
                    // second day of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=SU,MO,TU,WE,TH,FR,SA;BYSETPOS=2;WKST=SU
                    // second weekday of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=2;WKST=SU
                    // last monday of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=MO;BYSETPOS=-1;WKST=SU
                    b.Append("RRULE:FREQ=MONTHLY;");
                    b.Append(fb.ToString());
                    b.Append(string.Format("INTERVAL={0};", mInstance.Schedule.MonthlyExMonth));
                    switch ((DayName)mInstance.Schedule.MonthlyExDayName)
                    {
                        case DayName.day:
                            b.Append("BYDAY=SU,MO,TU,WE,TH,FR,SA;");
                            break;
                        case DayName.weekday:
                            b.Append("BYDAY=MO,TU,WE,TH,FR;");
                            break;
                        case DayName.weekendday:
                            b.Append("BYDAY=SU,SA;");
                            break;
                        default:
                            b.Append(string.Format("BYDAY={0};", ((DayOfWeek)mInstance.Schedule.MonthlyExDayName)
                                .ToString().Substring(0, 2).ToUpper()));
                            break;
                    }
                    b.Append(string.Format("BYSETPOS={0};", mInstance.Schedule.MonthlyExDayIndex));
                    b.AppendLine("WKST=SU");
                    break;
                case RecurrencePattern.Yearly_DayNOfMonth:
                    // every april 23 RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTHDAY=23;BYMONTH=4;WKST=SU
                    b.Append("RRULE:FREQ=YEARLY;INTERVAL=1;");
                    b.Append(fb.ToString());
                    b.AppendFormat("BYMONTHDAY={0};", mInstance.Schedule.YearlyDay);
                    b.AppendFormat("BYMONTH={0};", mInstance.Schedule.YearlyMonth);
                    b.AppendLine("WKST=SU");
                    break;
                case RecurrencePattern.Yearly_NthWeekDayOfMonth:
                    // every first sunday of every fifth month RRULE:FREQ=YEARLY;COUNT=10;INTERVAL=1;BYDAY=SU;BYMONTH=5;BYSETPOS=1;WKST=SU
                    b.Append("RRULE:FREQ=YEARLY;INTERVAL=1;");
                    b.Append(fb.ToString());
                    switch ((DayName)mInstance.Schedule.YearlyExDayName)
                    {
                        case DayName.day:
                            b.Append("BYDAY=SU,MO,TU,WE,TH,FR,SA;");
                            break;
                        case DayName.weekday:
                            b.Append("BYDAY=MO,TU,WE,TH,FR;");
                            break;
                        case DayName.weekendday:
                            b.Append("BYDAY=SU,SA;");
                            break;
                        default:
                            b.Append(string.Format("BYDAY={0};", ((DayOfWeek)mInstance.Schedule.MonthlyExDayName)
                                .ToString().Substring(0, 2).ToUpper()));
                            break;
                    }
                    b.AppendFormat("BYSETPOS={0};", mInstance.Schedule.YearlyExDayIndex);
                    b.AppendFormat("BYMONTH={0};", mInstance.Schedule.YearlyExMonth);
                    b.AppendLine("WKST=SU");
                    break;
            }

            b.AppendLine(string.Format("LOCATION;ENCODING=QUOTED-PRINTABLE:{0}", QuotedPrintable.Encode(mInstance.Place.Name)));
            b.AppendLine(string.Format("SUMMARY;ENCODING=QUOTED-PRINTABLE:{0}", QuotedPrintable.Encode(mInstance.Name)));
            b.AppendLine(string.Format("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:{0}",
                QuotedPrintable.Encode(
                    string.Format("{0}\r\n{1}/AccountEventView.aspx?id={2}",
                        Renderer.RemoveHtml(mInstance.Description),
                        ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                        mInstance.Id))));
            b.AppendLine("PRIORITY:3");
            if (!string.IsNullOrEmpty(mInstance.Email))
            {
                b.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", mInstance.Email));
            }
            b.AppendLine("END:VEVENT");
            b.AppendLine("END:VCALENDAR");
            return b.ToString();
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountEvent t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.Account.AccountEvents);
            }
        }

        public void MigrateToAccount(Account newowner, ManagedSecurityContext sec)
        {
            // migrate pictures
            IList<AccountEventPicture> pictures = Session.CreateCriteria(typeof(AccountEventPicture))
                .Add(Expression.Eq("Account.Id", mInstance.Account.Id))
                .Add(Expression.Eq("AccountEvent.Id", mInstance.Id))
                .List<AccountEventPicture>();

            foreach (AccountEventPicture pp in pictures)
            {
                ManagedAccountEventPicture mpp = new ManagedAccountEventPicture(Session, pp);
                mpp.MigrateToAccount(newowner, sec);
            }

            // migrate review discussion
            Discussion d = ManagedDiscussion.Find(
                Session, mInstance.Account.Id, typeof(AccountEvent), mInstance.Id, sec);

            if (d != null)
            {
                ManagedDiscussion md = new ManagedDiscussion(Session, d);
                md.MigrateToAccount(newowner, sec);
            }

            mInstance.Account = newowner;
            Session.Save(mInstance);
        }
    }
}

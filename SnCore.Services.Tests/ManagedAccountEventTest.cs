using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using SnCore.Tools;
using System.IO;
using System.Text;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventTest : ManagedCRUDTest<AccountEvent, TransitAccountEvent, ManagedAccountEvent>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountEventTypeTest _eventtype = new ManagedAccountEventTypeTest();
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedScheduleTest _schedule = new ManagedScheduleTest();

        [SetUp]
        public override void SetUp()
        {
            _eventtype.SetUp();
            _schedule.SetUp();
            _account.SetUp();
            _place.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _place.TearDown();
            _account.TearDown();
            _schedule.TearDown();
            _eventtype.TearDown();
        }

        public ManagedAccountEventTest()
        {

        }

        public override TransitAccountEvent GetTransitInstance()
        {
            TransitAccountEvent t_instance = new TransitAccountEvent();
            t_instance.AccountEventType = _eventtype.Instance.Instance.Name;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.AllDay = false;
            t_instance.Cost = "3$";
            t_instance.Description = GetNewString();
            t_instance.EndDateTime = DateTime.UtcNow;
            t_instance.Name = GetNewString();
            t_instance.Phone = "1-800-123-3456";
            t_instance.StartDateTime = DateTime.UtcNow;
            t_instance.Website = GetNewUri();
            t_instance.PictureId = _place.Instance.Id;
            t_instance.ScheduleId = _schedule.Instance.Id;
            t_instance.NoEndDateTime = _schedule.Instance.Object.NoEndDateTime;
            return t_instance;
        }

        class TestData
        {
            public string Input;
            public string Output;

            public TestData(string input, string output)
            {
                Input = input;
                Output = output;
            }
        }

        [Test]
        public void TestQuotedPrintable()
        {
            Console.WriteLine("TestQuotedPrintable");

            TestData[] data = {
                new TestData("", ""),
                new TestData("foo", "foo"),
                new TestData(" ", "=20"),
                new TestData("\r\n\t", "=0D=0A=09"),
                new TestData("line1\r\nline2", "line1=0D=0Aline2"),
                new TestData("name=value", "name=3Dvalue"),
                new TestData(new string('x', QuotedPrintable.RFC_1521_MAX_CHARS_PER_LINE), new string('x', QuotedPrintable.RFC_1521_MAX_CHARS_PER_LINE)),
                new TestData(new string('x', QuotedPrintable.RFC_1521_MAX_CHARS_PER_LINE + 1), new string('x', QuotedPrintable.RFC_1521_MAX_CHARS_PER_LINE) + "=\r\nx"),
            };

            foreach (TestData test in data)
            {
                Console.WriteLine(string.Format("           Test: {0}", test.Input));
                string encoded = QuotedPrintable.Encode(test.Input);
                Console.WriteLine(string.Format("        Encoded: {0}", encoded));
                string decoded = QuotedPrintable.Decode(encoded);
                Console.WriteLine(string.Format("        Decoded: {0}", decoded));
                Assert.AreEqual(test.Output, encoded, "Encoded string is wrong.");
                Assert.AreEqual(test.Input, decoded, "Decoded string is wrong.");
            }
        }

        public AccountEvent CreateEvent()
        {
            AccountEventType type = new AccountEventType();
            type.Name = "test type";

            Country country = new Country();
            country.Name = "USA";

            State state = new State();
            state.Country = country;
            state.Name = "NY";

            City city = new City();
            city.Country = country;
            city.State = state;
            city.Name = "New York";

            Place place = new Place();
            place.City = city;
            place.Name = "My Space";

            AccountEvent evt = new AccountEvent();
            evt.Description = "event description\r\nhttp://www.vestris.com/\r\nмолоко";
            evt.AccountEventType = type;
            evt.Cost = "10$";
            evt.Created = evt.Modified = DateTime.UtcNow;
            evt.Name = "milk - молоко";
            evt.Phone = "(212) 123-1234";
            evt.Place = place;
            evt.Website = GetNewUri();
            evt.Email = "foo@bar.com";

            return evt;
        }

        [Test]
        public void TestRecurrence_None()
        {
            AccountEvent evt = CreateEvent();
            
            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short) RecurrencePattern.None;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_None.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.UTF8.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_AllDay()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = true;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddDays(1).AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.None;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_AllDay.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.UTF8.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_EveryNDays()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);            
            s.RecurrencePattern = (short)RecurrencePattern.Daily_EveryNDays;
            s.DailyEveryNDays = 2;
            s.EndOccurrences = 10;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_EveryNDays.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_EveryWeekday()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Daily_EveryWeekday;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_EveryNDays.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_Weekly()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Weekly;
            s.WeeklyDaysOfWeek = 2 + 8;
            s.WeeklyEveryNWeeks = 3;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Weekly.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_Monthly_DayNOfEveryNMonths()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Monthly_DayNOfEveryNMonths;
            s.MonthlyDay = 23;
            s.MonthlyMonth = 4;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Monthly_DayNOfEveryNMonths.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_Monthly_NthWeekDayOfEveryNMonth()
        {
            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            // first day of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=SU,MO,TU,WE,TH,FR,SA;BYSETPOS=1;WKST=SU
            // second day of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=SU,MO,TU,WE,TH,FR,SA;BYSETPOS=2;WKST=SU
            // second weekday of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=MO,TU,WE,TH,FR;BYSETPOS=2;WKST=SU

            {
                // last weekday of every 2 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=MO;BYSETPOS=-1;WKST=SU
                s.MonthlyExDayIndex = (int) DayIndex.last;
                s.MonthlyExDayName = (int) DayName.weekday;
                s.MonthlyExMonth = 2;

                string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Monthly_NthWeekDayOfEveryNMonth_LastWeekday.ics");
                FileStream sw = File.Create(filename);
                byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
                sw.Write(bytes, 0, bytes.Length);
                sw.Flush();
                sw.Close();
                Console.WriteLine(filename);
            }

            {
                // first thursday of every 3 months: RRULE:FREQ=MONTHLY;INTERVAL=2;BYDAY=TH;BYSETPOS=1;WKST=SU
                s.MonthlyExDayIndex = (int) DayIndex.first;
                s.MonthlyExDayName = (int) DayName.Thursday;
                s.MonthlyExMonth = 3;

                string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Monthly_NthWeekDayOfEveryNMonth_FirstThursday.ics");
                FileStream sw = File.Create(filename);
                byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
                sw.Write(bytes, 0, bytes.Length);
                sw.Flush();
                sw.Close();
                Console.WriteLine(filename);
            }
        }

        [Test]
        public void TestRecurrence_Yearly_DayNOfMonth()
        {
            // every april 23 RRULE:FREQ=YEARLY;INTERVAL=1;BYMONTHDAY=23;BYMONTH=4;WKST=SU

            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Yearly_DayNOfMonth;
            s.YearlyDay = 23;
            s.YearlyMonth = 4;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Yearly_DayNOfMonth.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }

        [Test]
        public void TestRecurrence_Yearly_NthWeekDayOfMonth()
        {
            // every first sunday of april RRULE:FREQ=YEARLY;COUNT=10;INTERVAL=1;BYDAY=SU;BYMONTH=5;BYSETPOS=1;WKST=SU

            AccountEvent evt = CreateEvent();

            Schedule s = new Schedule();
            s.AllDay = false;
            s.Created = s.Modified = DateTime.UtcNow;
            s.StartDateTime = DateTime.UtcNow;
            s.EndDateTime = s.StartDateTime.AddHours(1);
            s.RecurrencePattern = (short)RecurrencePattern.Yearly_NthWeekDayOfMonth;
            s.YearlyExDayIndex = (int) DayIndex.first;
            s.YearlyExDayName = (int) DayName.Sunday;
            s.YearlyExMonth = (int) MonthName.April;
            s.Endless = true;

            evt.Schedule = s;

            ManagedAccountEvent m_evt = new ManagedAccountEvent(Session, evt);

            string filename = Path.Combine(Path.GetTempPath(), "SnCore.Recurrence_Yearly_NthWeekDayOfMonth.ics");
            FileStream sw = File.Create(filename);
            byte[] bytes = Encoding.ASCII.GetBytes(m_evt.ToVCalendarString(AdminSecurityContext));
            sw.Write(bytes, 0, bytes.Length);
            sw.Flush();
            sw.Close();

            Console.WriteLine(filename);
        }
    }
}

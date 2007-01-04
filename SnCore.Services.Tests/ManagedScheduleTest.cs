using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;
using SnCore.Tools;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedScheduleTest : ManagedCRUDTest<Schedule, TransitSchedule, ManagedSchedule>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
        }

        public override void TearDown()
        {
            _account.TearDown();
            base.TearDown();
        }

        public ManagedScheduleTest()
        {

        }

        public override TransitSchedule GetTransitInstance()
        {
            TransitSchedule t_instance = new TransitSchedule();
            t_instance.StartDateTime = DateTime.UtcNow;
            t_instance.EndDateTime = DateTime.UtcNow;
            t_instance.EndOccurrences = 2;
            t_instance.RecurrencePattern = RecurrencePattern.Daily_EveryNDays;
            t_instance.AccountId = _account.Instance.Id;
            return t_instance;
        }

        [Test]
        public void TestNoRecurrence_StartEnd()
        {
            Console.WriteLine("TestNoRecurrence_StartEnd");

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.None;
                ts.EndDateTime = ts.StartDateTime.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.AreEqual(s.ScheduleInstances.Count, 1, "There's more than one instance on a non-recurrent schedule.");

                {
                    ScheduleInstance ts_instance = (ScheduleInstance)s.ScheduleInstances[0];

                    Assert.AreEqual(ts_instance.StartDateTime, s.StartDateTime, "Schedule instance start date/time doesn't match.");
                    Assert.AreEqual(ts_instance.EndDateTime, s.EndDateTime, "Schedule instance end date/time doesn't match.");
                    Assert.AreEqual(ts_instance.Schedule, s, "Schedule instance schedule object doesn't match.");
                    Assert.AreEqual(ts_instance.Instance, 0, "Instance index is not zero.");
                    Assert.AreEqual(ts_instance.Modified, s.Created, "Instance creation date/time doesn't match schedule.");
                }

                // force update again
                Assert.AreEqual(m_s.UpdateInstances(), 1, "More than one event instance has been generated.");

                {
                    Assert.AreEqual(s.ScheduleInstances.Count, 1, "There's more than one instance on a non-recurrent schedule.");

                    ScheduleInstance ts_instance = (ScheduleInstance)s.ScheduleInstances[0];

                    Assert.AreEqual(ts_instance.StartDateTime, s.StartDateTime, "Schedule instance start date/time doesn't match.");
                    Assert.AreEqual(ts_instance.EndDateTime, s.EndDateTime, "Schedule instance end date/time doesn't match.");
                    Assert.AreEqual(ts_instance.Schedule, s, "Schedule instance schedule object doesn't match.");
                    Assert.AreEqual(ts_instance.Instance, 0, "Instance index is not zero.");
                    Assert.AreEqual(ts_instance.Modified, s.Created, "Instance creation date/time doesn't match schedule.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestNoRecurrence_AllDay()
        {
            Console.WriteLine("TestNoRecurrence_AllDay");

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();
                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.None;
                ts.AllDay = true;
                ts.EndDateTime = ts.StartDateTime; // all that day, will be adjusted +1 and end within 24 hours

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                m_s.UpdateInstances();

                Assert.AreEqual(s.ScheduleInstances.Count, 1, "There's more than one instance on a non-recurrent schedule.");

                ScheduleInstance ts_instance = (ScheduleInstance)s.ScheduleInstances[0];

                Assert.AreEqual(s.StartDateTime, ts_instance.StartDateTime, "Schedule instance start date/time doesn't match.");
                Assert.AreEqual(s.EndDateTime.AddDays(1), ts_instance.EndDateTime, "Schedule instance end date/time doesn't match.");
                Assert.AreEqual(s, ts_instance.Schedule, "Schedule instance schedule object doesn't match.");
                Assert.AreEqual(0, ts_instance.Instance, "Instance index is not zero.");
                Assert.AreEqual(s.Created, ts_instance.Modified, "Instance creation date/time doesn't match schedule.");
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Daily_EveryNDays()
        {
            Console.WriteLine("TestRecurrence_Daily_EveryNDays");

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Daily_EveryNDays;
                ts.Endless = false;
                ts.DailyEveryNDays = 2;
                ts.EndOccurrences = 10;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.AreEqual(ts.EndOccurrences, s.ScheduleInstances.Count, "There's more than ten instance on this schedule.");

                ScheduleInstance ts_instance = (ScheduleInstance)s.ScheduleInstances[0];

                Assert.AreEqual(ts_instance.StartDateTime.TimeOfDay, s.StartDateTime.TimeOfDay, "Schedule instance start date/time doesn't match.");
                Assert.AreEqual(ts_instance.EndDateTime.TimeOfDay, s.EndDateTime.TimeOfDay, "Schedule instance end date/time doesn't match.");
                Assert.AreEqual(ts_instance.Schedule, s, "Schedule instance schedule object doesn't match.");
                Assert.AreEqual(ts_instance.Instance, 0, "Instance index is not zero.");
                Assert.AreEqual(ts_instance.Modified, s.Modified, "Instance creation date/time doesn't match schedule.");

                ScheduleInstance ts_instance_1 = (ScheduleInstance)s.ScheduleInstances[5];
                ScheduleInstance ts_instance_2 = (ScheduleInstance)s.ScheduleInstances[6];

                Assert.AreEqual(ts_instance_1.StartDateTime.TimeOfDay, ts_instance_2.StartDateTime.TimeOfDay, "Schedule instance start date/time doesn't match.");
                Assert.AreEqual(ts_instance_1.Instance + 1, ts_instance_2.Instance, "Sequent schedule instance indexes are wrong.");
                Assert.AreEqual(ts_instance_2.StartDateTime.Subtract(ts_instance_1.StartDateTime), new TimeSpan(2, 0, 0, 0), "Schedule interval is wrong.");
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_EveryWeekday()
        {
            Console.WriteLine("TestRecurrence_EveryWeekday");

            // event occurs every week-day

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Daily_EveryWeekday;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.IsTrue(ts_instance.StartDateTime.DayOfWeek != DayOfWeek.Saturday, "Event cannot occur Sunday.");
                    Assert.IsTrue(ts_instance.StartDateTime.DayOfWeek != DayOfWeek.Saturday, "Event cannot occur Saturday.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Weekly()
        {
            Console.WriteLine("TestRecurrence_Weekly");

            // event occurs weekly on tuesday and friday

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Weekly;
                ts.WeeklyDaysOfWeek = (short)
                    ((short)Math.Pow(2, (short)DayOfWeek.Tuesday) + (short)Math.Pow(2, (short)DayOfWeek.Friday));
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.IsTrue(ts_instance.StartDateTime.DayOfWeek == DayOfWeek.Tuesday 
                        || ts_instance.StartDateTime.DayOfWeek == DayOfWeek.Friday, 
                        "Event cannot occur any other day than Tuesday and Friday.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Monthly_DayNOfEveryNMonths()
        {
            Console.WriteLine("TestRecurrence_Monthly_DayNOfEveryNMonths");

            // event occurs weekly on tuesday and friday

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Monthly_DayNOfEveryNMonths;
                ts.MonthlyDay = 3;
                ts.MonthlyMonth = 2;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                ScheduleInstance ts_previous_instance = null;
                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.AreEqual(ts_instance.StartDateTime.Day, ts.MonthlyDay, "Day of month is wrong.");
                    if (ts_previous_instance != null)
                    {
                        Assert.AreEqual(
                            ts_instance.StartDateTime, ts_previous_instance.StartDateTime.AddMonths(ts.MonthlyMonth), 
                            "Previous instance delta is incorrect.");
                    }
                    ts_previous_instance = ts_instance;
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Monthly_NthWeekDayOfEveryNMonth_Last()
        {
            Console.WriteLine("TestRecurrence_Monthly_NthWeekDayOfEveryNMonth_Last");

            // event occurs on every last Thursday of every month

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth;
                ts.MonthlyExDayIndex = (short) DayIndex.last;
                ts.MonthlyExDayName = (short)DayName.Thursday;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.AreEqual(ts_instance.StartDateTime.DayOfWeek, (DayOfWeek) ts.MonthlyExDayName, "Day of month is wrong.");
                    Assert.IsTrue(CBusinessDay.IsLastDayOfWeekOccurrenceThisMonth(ts_instance.StartDateTime), "Day of month is not the last instance.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Monthly_NthWeekDayOfEveryNMonth_First()
        {
            Console.WriteLine("TestRecurrence_Monthly_NthWeekDayOfEveryNMonth_First");

            // event occurs on every last Thursday of every month

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Monthly_NthWeekDayOfEveryNMonth;
                ts.MonthlyExDayIndex = (short)DayIndex.first;
                ts.MonthlyExDayName = (short)DayName.Tuesday;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.AreEqual(ts_instance.StartDateTime.DayOfWeek, (DayOfWeek)ts.MonthlyExDayName, "Day of month is wrong.");
                    Assert.IsTrue(CBusinessDay.GetDayOfWeekOccurrenceThisMonth(ts_instance.StartDateTime) == 1, "Day of month is not the first instance.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Yearly_DayNOfMonth()
        {
            Console.WriteLine("TestRecurrence_Yearly_DayNOfMonth");

            // event occurs yearly every May 21st (Mishamishka's birthday)

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Yearly_DayNOfMonth;
                ts.YearlyDay = 21;
                ts.YearlyMonth = (short) MonthName.May;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.AreEqual(ts_instance.StartDateTime.Day, ts.YearlyDay, "Day is wrong.");
                    Assert.AreEqual(ts_instance.StartDateTime.Month, ts.YearlyMonth, "Month is wrong.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestRecurrence_Yearly_NthWeekDayOfMonth_First()
        {
            Console.WriteLine("TestRecurrence_Yearly_NthWeekDayOfMonth");

            // event occurs yearly every first Thursday of February

            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSchedule ts = new TransitSchedule();

                ts.AccountId = a.Id;
                ts.StartDateTime = DateTime.UtcNow;
                ts.RecurrencePattern = RecurrencePattern.Yearly_NthWeekDayOfMonth;
                ts.YearlyExDayIndex = (short) DayIndex.first;
                ts.YearlyExDayName = (short) DayName.Thursday;
                ts.YearlyExMonth = (short) MonthName.February;
                ts.Endless = true;
                ts.EndDateTime = DateTime.UtcNow.AddHours(1);

                ManagedSchedule m_s = new ManagedSchedule(Session);
                int schedule_id = m_s.CreateOrUpdate(ts, AdminSecurityContext);

                Schedule s = (Schedule)Session.Load(typeof(Schedule), schedule_id);

                Assert.IsNotNull(s.ScheduleInstances, "Schedule instances cannot be null.");

                foreach (ScheduleInstance ts_instance in s.ScheduleInstances)
                {
                    Console.WriteLine(string.Format("Event on {0}.", ts_instance.StartDateTime.ToLongDateString()));
                    Assert.AreEqual((int) ts_instance.StartDateTime.DayOfWeek, ts.YearlyExDayName, "Day of week is wrong.");
                    Assert.AreEqual(ts_instance.StartDateTime.Month, ts.YearlyExMonth, "Month is wrong.");
                    Assert.IsTrue(CBusinessDay.GetDayOfWeekOccurrenceThisMonth(ts_instance.StartDateTime) == 1, "Day of month is not the first instance.");
                }
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

    }
}

using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedScheduleTest : NHibernateTest
    {
        public ManagedScheduleTest()
        {

        }

        [Test]
        public void TestRecurrent_Daily_EveryNDays()
        {
            DateTime utcnow = DateTime.UtcNow;

            Schedule ts = new Schedule();
            ts.StartDateTime = utcnow;
            ts.Endless = false;
            ts.EndOccurrences = 4;
            ts.DailyEveryNDays = 2;
            ts.RecurrencePattern = (short)RecurrencePattern.Daily_EveryNDays;

            ManagedSchedule s = new ManagedSchedule(Session, ts);
            for (int i = 0; i < 7; i+=2)
            {
                DateTime start = utcnow.AddDays(i);
                Assert.IsTrue(s.IsInRange(start));
                Assert.IsFalse(s.IsInRange(start.AddDays(1)));
            }

            DateTime afteroccurrences = utcnow.AddDays(ts.EndOccurrences * ts.DailyEveryNDays);
            Assert.IsFalse(s.IsInRange(afteroccurrences));
            Assert.IsFalse(s.IsInRange(afteroccurrences.AddDays(1)));
        }

        [Test]
        public void TestRecurrent_Daily_EveryWeekday()
        {
            DateTime utcnow = DateTime.UtcNow;

            Schedule ts = new Schedule();
            ts.StartDateTime = utcnow;
            ts.Endless = true;
            ts.RecurrencePattern = (short)RecurrencePattern.Daily_EveryWeekday;

            ManagedSchedule s = new ManagedSchedule(Session, ts);
            for (int i = 0; i < 7; i ++)
            {
                DateTime start = utcnow.AddDays(i);
                if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
                {
                    Assert.IsFalse(s.IsInRange(start));
                }
                else
                {
                    Assert.IsTrue(s.IsInRange(start));
                }
            }

            ts.Endless = false;
            ts.EndOccurrences = 5;
            DateTime afteroccurrences = utcnow.AddDays(7);
            Assert.IsFalse(s.IsInRange(afteroccurrences));
            Assert.IsFalse(s.IsInRange(afteroccurrences.AddDays(1)));
        }

        [Test]
        public void TestRecurrent_Weekly_EveryNWeeks()
        {
            DateTime utcnow = DateTime.UtcNow;

            Schedule ts = new Schedule();
            ts.StartDateTime = utcnow;
            ts.Endless = true;
            ts.RecurrencePattern = (short)RecurrencePattern.Weekly;

            // every day of the week
            ts.WeeklyDaysOfWeek = 0;
            for (int i = 0; i < 7; i++)
            {
                ts.WeeklyDaysOfWeek |= (short)Math.Pow(2, i);
            }

            ts.WeeklyEveryNWeeks = 2;

            ManagedSchedule s = new ManagedSchedule(Session, ts);
            for (int i = 0; i < 4; i+=2)
            {
                DateTime start = utcnow.AddDays(i * 7);
                Assert.IsTrue(s.IsInRange(start));
                Assert.IsFalse(s.IsInRange(start.AddDays(7)));
            }
        }

        [Test]
        public void TestRecurrent_Weekly_EveryNWeeks_WithEndOccurrences()
        {
            DateTime utcnow = DateTime.UtcNow;

            Schedule ts = new Schedule();
            ts.StartDateTime = utcnow;
            ts.Endless = false;
            ts.RecurrencePattern = (short)RecurrencePattern.Weekly;
            ts.EndOccurrences = 2 * 7;
            ts.WeeklyEveryNWeeks = 2;

            // every day of the week
            ts.WeeklyDaysOfWeek = 0;
            for (int i = 0; i < 7; i++)
            {
                ts.WeeklyDaysOfWeek |= (short)Math.Pow(2, i);
            }

            ManagedSchedule s = new ManagedSchedule(Session, ts);
            DateTime start = utcnow.AddDays(4 * 7);

            Assert.IsTrue(s.IsInRange(start));
            Assert.IsFalse(s.IsInRange(start.AddDays(1)));
            Assert.IsFalse(s.IsInRange(start.AddDays(7)));
        }
    }
}

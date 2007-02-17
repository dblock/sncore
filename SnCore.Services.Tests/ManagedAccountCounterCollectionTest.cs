using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Services;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountCounterCollectionTest
    {
        [Test]
        public void AddSameDateTest()
        {
            DateTime utcnow = DateTime.UtcNow;
            ManagedAccountCounterCollection c = new ManagedAccountCounterCollection();
            for (int i = 1; i <= 7; i++)
            {
                c.Add(utcnow);
                Assert.AreEqual(1, c.Daily.Count);
                Assert.AreEqual(i, c.Daily[ManagedAccountCounterCollection.GetDailyTimestamp(utcnow)], "Daily count is wrong.");
                Assert.AreEqual(1, c.Weekly.Count);
                Assert.AreEqual(i, c.Weekly[ManagedAccountCounterCollection.GetWeeklyTimestamp(utcnow)], "Weekly count is wrong.");
                Assert.AreEqual(1, c.Monthly.Count);
                Assert.AreEqual(i, c.Monthly[ManagedAccountCounterCollection.GetMonthlyTimestamp(utcnow)], "Monthly count is wrong.");
                Assert.AreEqual(1, c.Yearly.Count);
                Assert.AreEqual(i, c.Yearly[ManagedAccountCounterCollection.GetYearlyTimestamp(utcnow)], "Yearly count is wrong.");
            }
        }

        [Test]
        public void AddWeekDateTest()
        {
            ManagedAccountCounterCollection c = new ManagedAccountCounterCollection();
            DateTime utcnow = DateTime.UtcNow;
            while (utcnow.Month > 1)
                utcnow = utcnow.AddMonths(-1);
            while (utcnow.DayOfWeek != DayOfWeek.Sunday)
                utcnow = utcnow.AddDays(-1);
            for (int i = 1; i < 7; i++)
            {
                c.Add(utcnow);
                Assert.AreEqual(i, c.Daily.Count);
                Assert.AreEqual(1, c.Daily[ManagedAccountCounterCollection.GetDailyTimestamp(utcnow)], "Daily count is wrong.");
                Assert.AreEqual(1, c.Weekly.Count);
                Assert.AreEqual(i, c.Weekly[ManagedAccountCounterCollection.GetWeeklyTimestamp(utcnow)], "Weekly count is wrong.");
                Assert.AreEqual(1, c.Monthly.Count);
                Assert.AreEqual(i, c.Monthly[ManagedAccountCounterCollection.GetMonthlyTimestamp(utcnow)], "Monthly count is wrong.");
                Assert.AreEqual(1, c.Yearly.Count);
                Assert.AreEqual(i, c.Yearly[ManagedAccountCounterCollection.GetYearlyTimestamp(utcnow)], "Yearly count is wrong.");
                utcnow = utcnow.AddDays(1);
            }
        }

        [Test]
        public void AddMonthDateTest()
        {
            ManagedAccountCounterCollection c = new ManagedAccountCounterCollection();
            DateTime utcnow = DateTime.UtcNow;
            while (utcnow.Month > 1)
                utcnow = utcnow.AddMonths(-1);
            for (int i = 1; i < 12; i++)
            {
                c.Add(utcnow);
                Assert.AreEqual(i, c.Daily.Count);
                Assert.AreEqual(1, c.Daily[ManagedAccountCounterCollection.GetDailyTimestamp(utcnow)], "Daily count is wrong.");
                Assert.AreEqual(i, c.Weekly.Count);
                Assert.AreEqual(1, c.Weekly[ManagedAccountCounterCollection.GetWeeklyTimestamp(utcnow)], "Weekly count is wrong.");
                Assert.AreEqual(i, c.Monthly.Count);
                Assert.AreEqual(1, c.Monthly[ManagedAccountCounterCollection.GetMonthlyTimestamp(utcnow)], "Monthly count is wrong.");
                Assert.AreEqual(1, c.Yearly.Count);
                Assert.AreEqual(i, c.Yearly[ManagedAccountCounterCollection.GetYearlyTimestamp(utcnow)], "Yearly count is wrong.");
                utcnow = utcnow.AddMonths(1);
            }
        }

    }
}

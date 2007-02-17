using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class StatsTest : WebServiceBaseTest<WebStatsServiceNoCache>
    {
        [Test]
        public void GetSummaryTest()
        {
            WebStatsService.TransitStatsSummary summary = EndPoint.GetSummary(GetAdminTicket());
            Assert.IsTrue(summary.TotalHits >= 0);
            Assert.IsTrue(summary.Yearly.Length >= 5);
            Assert.IsTrue(summary.Monthly.Length >= 12);
            Assert.IsTrue(summary.Weekly.Length >= 52);
            Assert.IsTrue(summary.Daily.Length >= 2 * 7);
            Assert.IsTrue(summary.NewDaily.Length >= 2 * 7);
            Assert.IsTrue(summary.Hourly.Length >= 24);
            Assert.IsTrue(summary.AccountYearly.Length >= 5);
            Assert.IsTrue(summary.AccountMonthly.Length >= 12);
            Assert.IsTrue(summary.AccountWeekly.Length >= 52);
            Assert.IsTrue(summary.AccountDaily.Length >= 2 * 7);
        }
    }
}

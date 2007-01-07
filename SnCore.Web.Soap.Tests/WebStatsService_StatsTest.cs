using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class StatsTest
    {
        [Test]
        public void GetSummaryTest()
        {
            WebStatsServiceNoCache endpoint = new WebStatsServiceNoCache();
            WebStatsService.TransitStatsSummary summary = endpoint.GetSummary();
            Assert.IsTrue(summary.TotalHits >= 0);
            Assert.IsTrue(summary.Yearly.Length >= 5);
            Assert.IsTrue(summary.Monthly.Length >= 12);
            Assert.IsTrue(summary.Weekly.Length >= 52);
            Assert.IsTrue(summary.Daily.Length >= 2 * 7);
            Assert.IsTrue(summary.NewDaily.Length >= 2 * 7);
            Assert.IsTrue(summary.Hourly.Length >= 24);
        }
    }
}

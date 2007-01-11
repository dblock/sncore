using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedStatsTest : ManagedServiceTest
    {
        public ManagedStatsTest()
        {

        }

        [Test]
        public void TestSummary()
        {
            ManagedStats stats = new ManagedStats(Session);
            TransitStatsSummary summary = stats.GetSummary();
            Console.WriteLine("Total hits: {0}", summary.TotalHits);
            Assert.IsTrue(summary.TotalHits >= 0);
            Assert.IsTrue(summary.Yearly.Count >= 5);
            Assert.IsTrue(summary.Monthly.Count >= 12);
            Assert.IsTrue(summary.Weekly.Count >= 52);
            Assert.IsTrue(summary.Daily.Count >= 2 * 7);
            Assert.IsTrue(summary.NewDaily.Count >= 2 * 7);
            Assert.IsTrue(summary.Hourly.Count >= 24);
        }

        [Test]
        public void TestTrack()
        {
            ManagedStats stats = new ManagedStats(Session);
            TransitStatsSummary summary1 = stats.GetSummary();
            string uri = string.Format("http://localhost/uri/{0}", Guid.NewGuid());
            string query = string.Format("q={0}", Guid.NewGuid());
            HttpRequest request = new HttpRequest(null, uri, query);
            TransitStatsRequest statsrequest = new TransitStatsRequest(request, null);
            stats.Track(statsrequest);
            TransitStatsSummary summary2 = stats.GetSummary();
            Assert.AreEqual(summary1.TotalHits + 1, summary2.TotalHits);
            Assert.AreEqual(summary1.Yearly[summary1.Yearly.Count - 1].Total + 1, summary2.Yearly[summary2.Yearly.Count - 1].Total);
            Assert.AreEqual(summary1.Monthly[summary1.Monthly.Count - 1].Total + 1, summary2.Monthly[summary2.Monthly.Count - 1].Total);
            Assert.AreEqual(summary1.Weekly[summary1.Weekly.Count - 1].Total + 1, summary2.Weekly[summary2.Weekly.Count - 1].Total);
            Assert.AreEqual(summary1.Daily[summary1.Daily.Count - 1].Total + 1, summary2.Daily[summary2.Daily.Count - 1].Total);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Web.Tests.SnCore.WebServices.WebStatsService;

namespace SnCore.Web.Tests
{
    [TestFixture]
    public class WebStatsServiceTest
    {
        [Test]
        public void TrackSingleRequest()
        {
            WebStatsService service = new WebStatsService();
            TransitStatsRequest r = new TransitStatsRequest();
            r.IncrementNewUser = true;
            r.IncrementReturningUser = true;
            r.RefererQuery = string.Empty;
            r.RefererUri = string.Empty;
            r.RequestUri = "/";
            r.Timestamp = DateTime.UtcNow;
            service.TrackSingleRequest(r);
        }

        [Test]
        public void TrackSingleRequestLongUrls()
        {
            WebStatsService service = new WebStatsService();
            TransitStatsRequest r = new TransitStatsRequest();
            r.IncrementNewUser = true;
            r.IncrementReturningUser = true;
            r.RefererQuery = string.Empty;
            r.RefererUri = "http://images.google.com/imgres?imgurl=http://static.flickr.com/62/182577288_cb999a1979.jpg&imgrefurl=http://www.foodcandy.com/AccountFeedView.aspx?id=126&h=375&w=500&sz=204&hl=en&start=6&tbnid=Tt22BfvkEDQQCM:&tbnh=98&tbnw=130&prev=/images?q=linguini+with+white+clam+sauce&svnum=10&hl=en&lr=&client=firefox-a&rls=org.mozilla:en-US:official_s&hs=1yK&sa=N";
            r.RequestUri = "http://localhost/AccountFeedView.aspx?id=126";
            r.Timestamp = DateTime.UtcNow;
            service.TrackSingleRequest(r);
        }

    }
}

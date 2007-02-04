using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class TrackTest : WebServiceBaseTest<WebStatsServiceNoCache>
    {
        [Test]
        public void TrackSingleRequestTest()
        {
            WebStatsService.TransitStatsRequest request = new WebStatsService.TransitStatsRequest();
            request.IncrementNewUser = true;
            request.IncrementReturningUser = true;
            request.RefererQuery = string.Format("{0}={0}", GetNewString());
            request.RefererUri = GetNewUri();
            request.RequestUri = GetNewUri();
            request.Timestamp = DateTime.UtcNow;
            EndPoint.TrackSingleRequest(request);            
        }

        [Test]
        public void TrackMultipleRequestsTest()
        {
            WebStatsServiceNoCache endpoint = new WebStatsServiceNoCache();

            List<WebStatsService.TransitStatsRequest> requests = new List<WebStatsService.TransitStatsRequest>();
            for (int i = 0; i < 10; i++)
            {
                WebStatsService.TransitStatsRequest request = new WebStatsService.TransitStatsRequest();
                request.IncrementNewUser = true;
                request.IncrementReturningUser = true;
                request.RefererQuery = string.Format("{0}={0}", GetNewString());
                request.RefererUri = GetNewUri();
                request.RequestUri = GetNewUri();
                request.Timestamp = DateTime.UtcNow;
                requests.Add(request);
            }

            EndPoint.TrackMultipleRequests(requests.ToArray());
        }

        [Test]
        public void TrackSingleRequestLongUrls()
        {
            WebStatsService.TransitStatsRequest r = new WebStatsService.TransitStatsRequest();
            r.IncrementNewUser = true;
            r.IncrementReturningUser = true;
            r.RefererQuery = string.Empty;
            r.RefererUri = "http://images.google.com/imgres?imgurl=http://static.flickr.com/62/182577288_cb999a1979.jpg&imgrefurl=http://www.foodcandy.com/AccountFeedView.aspx?id=126&h=375&w=500&sz=204&hl=en&start=6&tbnid=Tt22BfvkEDQQCM:&tbnh=98&tbnw=130&prev=/images?q=linguini+with+white+clam+sauce&svnum=10&hl=en&lr=&client=firefox-a&rls=org.mozilla:en-US:official_s&hs=1yK&sa=N";
            r.RequestUri = "http://localhost/AccountFeedView.aspx?id=126";
            r.Timestamp = DateTime.UtcNow;
            EndPoint.TrackSingleRequest(r);
        }
    }
}

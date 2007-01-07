using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class TrackTest
    {
        [Test]
        public void TrackSingleRequestTest()
        {
            WebStatsServiceNoCache endpoint = new WebStatsServiceNoCache();
            WebStatsService.TransitStatsRequest request = new WebStatsService.TransitStatsRequest();
            request.IncrementNewUser = true;
            request.IncrementReturningUser = true;
            request.RefererQuery = string.Format("{0}={0}", Guid.NewGuid());
            request.RefererUri = string.Format("http://uri/{0}", Guid.NewGuid());
            request.RequestUri = string.Format("http://uri/{0}", Guid.NewGuid());
            request.Timestamp = DateTime.UtcNow;
            endpoint.TrackSingleRequest(request);            
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
                request.RefererQuery = string.Format("{0}={0}", Guid.NewGuid());
                request.RefererUri = string.Format("http://uri/{0}", Guid.NewGuid());
                request.RequestUri = string.Format("http://uri/{0}", Guid.NewGuid());
                request.Timestamp = DateTime.UtcNow;
                requests.Add(request);
            }

            endpoint.TrackMultipleRequests(requests.ToArray());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebDiscussionServiceTests
{
    public class WebDiscussionServiceNoCache : WebDiscussionService.WebDiscussionService
    {
        public WebDiscussionServiceNoCache()
        {

        }

        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            System.Net.WebRequest request = base.GetWebRequest(uri);
            request.Headers.Add("Cache-Control", "no-cache");
            return request;
        }
    }

    [TestFixture]
    public class EndpointTests
    {
        [Test]
        public void TestEndpoint()
        {
            WebDiscussionServiceNoCache endpoint = new WebDiscussionServiceNoCache();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
        }
    }
}

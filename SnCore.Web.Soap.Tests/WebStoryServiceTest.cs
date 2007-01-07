using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStoryServiceTests
{
    public class WebStoryServiceNoCache : WebStoryService.WebStoryService
    {
        public WebStoryServiceNoCache()
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
            WebStoryServiceNoCache endpoint = new WebStoryServiceNoCache();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
        }
    }
}

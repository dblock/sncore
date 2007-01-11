using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSocialServiceTests
{
    public class WebSocialServiceNoCache : WebSocialService.WebSocialService
    {
        public WebSocialServiceNoCache()
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
            WebSocialServiceNoCache endpoint = new WebSocialServiceNoCache();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
        }
    }
}

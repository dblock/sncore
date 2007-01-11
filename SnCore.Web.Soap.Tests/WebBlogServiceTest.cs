using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBlogServiceTests
{
    public class WebBlogServiceNoCache : WebBlogService.WebBlogService
    {
        public WebBlogServiceNoCache()
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
            WebBlogServiceNoCache endpoint = new WebBlogServiceNoCache();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
        }
    }
}

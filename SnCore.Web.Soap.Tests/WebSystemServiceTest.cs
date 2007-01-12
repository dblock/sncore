using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSystemServiceTests
{
    public class WebSystemServiceNoCache : WebSystemService.WebSystemService
    {
        public WebSystemServiceNoCache()
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
            WebSystemService.WebSystemService endpoint = new WebSystemService.WebSystemService();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
            Assert.IsTrue(endpoint.GetUptime() > 0);
            Console.WriteLine("Uptime: {0}", endpoint.GetUptime());
            Assert.IsNotEmpty(endpoint.GetTitle());
            Console.WriteLine("Title: {0}", endpoint.GetTitle());
            Assert.IsNotEmpty(endpoint.GetDescription());
            Console.WriteLine("Description: {0}", endpoint.GetDescription());
            Assert.IsNotEmpty(endpoint.GetVersion());
            Console.WriteLine("Version: {0}", endpoint.GetVersion());
            Assert.IsNotEmpty(endpoint.GetCopyright());
            Console.WriteLine("Copyright: {0}", endpoint.GetCopyright());
        }
    }
}

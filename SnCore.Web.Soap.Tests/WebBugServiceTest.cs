using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebBugServiceTest
    {
        [Test]
        public void TestEndpoint()
        {
            WebBugService.WebBugService endpoint = new WebBugService.WebBugService();
            Assert.IsFalse(string.IsNullOrEmpty(endpoint.Url));
        }
    }
}

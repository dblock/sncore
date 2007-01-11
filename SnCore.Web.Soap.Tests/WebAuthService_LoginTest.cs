using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAuthServiceTests
{
    [TestFixture]
    public class LoginTest : WebServiceBaseTest<WebAuthServiceNoCache>
    {
        [Test, ExpectedException(typeof(SoapException))]
        public void TestEmptyLogin()
        {
            EndPoint.Login(string.Empty, string.Empty);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestNullLogin()
        {
            EndPoint.Login(null, null);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestInvalidLogin()
        {
            EndPoint.Login(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        public void TestAdminLogin()
        {
            string ticket = EndPoint.Login("admin@localhost.com", "password");
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }
    }
}

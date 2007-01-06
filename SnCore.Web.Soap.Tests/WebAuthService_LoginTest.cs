using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAuthServiceTests
{
    [TestFixture]
    public class LoginTest
    {
        [Test, ExpectedException(typeof(SoapException))]
        public void TestEmptyLogin()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            endpoint.Login(string.Empty, string.Empty);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestNullLogin()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            endpoint.Login(null, null);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestInvalidLogin()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            endpoint.Login(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        public void TestAdminLogin()
        {
            WebAuthService.WebAuthService endpoint = new WebAuthService.WebAuthService();
            string ticket = endpoint.Login("admin@localhost.com", "password");
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }
    }
}

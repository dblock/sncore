using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests
{
    [TestFixture]
    public class WebAccountService_LoginTest
    {
        [Test, ExpectedException(typeof(SoapException))]
        public void TestEmptyLogin()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            endpoint.Login(string.Empty, string.Empty);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestNullLogin()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            endpoint.Login(null, null);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void TestInvalidLogin()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            endpoint.Login(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        public void TestAdminLogin()
        {
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            string ticket = endpoint.Login("admin@localhost.com", "password");
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }
    }
}

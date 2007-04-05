using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Security.Cryptography;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class LoginTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        [Test, ExpectedException(typeof(SoapException))]
        public void EmptyLoginTest()
        {
            EndPoint.Login(string.Empty, string.Empty);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void NullLoginTest()
        {
            EndPoint.Login(null, null);
        }

        [Test, ExpectedException(typeof(SoapException))]
        public void InvalidLoginTest()
        {
            EndPoint.Login(GetNewString(), GetNewString());
        }

        [Test]
        public void AdminLoginTest()
        {
            string ticket = EndPoint.Login("admin@localhost.com", "password");
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }

        [Test]
        protected void LoginOpenIdTest()
        {

        }

        [Test]
        public void LoginMd5Test()
        {
            string password = Encoding.Default.GetString(
             new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes("password")));
            string ticket = EndPoint.LoginMd5("admin@localhost.com", password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
        }
    }
}

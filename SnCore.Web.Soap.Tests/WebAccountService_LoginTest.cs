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
        public void LoginUserTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            string ticket = EndPoint.Login(user.email, user.password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            DeleteUser(user.id);
        }

        [Test]
        protected void LoginOpenIdTest()
        {

        }

        [Test]
        public void LoginUserMd5Test()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            byte[] password_hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(user.password));
            Console.WriteLine("Password hash: {0}", password_hash);
            string ticket = EndPoint.LoginMd5(user.email, password_hash);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            DeleteUser(user.id);
        }
    }
}

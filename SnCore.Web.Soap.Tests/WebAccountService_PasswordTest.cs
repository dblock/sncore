using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class PasswordTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        [Test]
        public void ChangePasswordTest()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = EndPoint.Login(email, password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            string newpassword = GetNewString();
            EndPoint.ChangePassword(ticket, user_id, password, newpassword);

            try
            {
                // login with the old password now fails
                string oldticket = EndPoint.Login(email, password);
                Assert.IsTrue(string.IsNullOrEmpty(oldticket));
            }
            catch (SoapException)
            {
            }

            string newticket = EndPoint.Login(email, newpassword);
            Assert.IsFalse(string.IsNullOrEmpty(newticket));

            EndPoint.DeleteAccount(newticket, newpassword);
        }

        [Test]
        public void ResetPasswordTest()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            DateTime dateofbirth = DateTime.UtcNow.AddYears(-10);
            int user_id = CreateUser(email, password, dateofbirth);
            Assert.IsTrue(user_id > 0);
            string ticket = EndPoint.Login(email, password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            string newpassword = GetNewString();

            try
            {
                EndPoint.ResetPassword(email, DateTime.UtcNow);
                Assert.IsTrue(false, "Invalid date of birth should have thrown an exception.");
            }
            catch (SoapException)
            {
            }

            EndPoint.ResetPassword(email, dateofbirth);

            try
            {
                // login with the old password now fails
                string oldticket = EndPoint.Login(email, password);
                Assert.IsTrue(string.IsNullOrEmpty(oldticket));
            }
            catch (SoapException)
            {
            }

            EndPoint.DeleteAccountById(GetAdminTicket(), user_id, string.Empty);
        }
    }
}

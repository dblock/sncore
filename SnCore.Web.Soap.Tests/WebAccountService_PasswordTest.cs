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
            catch (Exception)
            {
            }

            string newticket = EndPoint.Login(email, newpassword);
            Assert.IsFalse(string.IsNullOrEmpty(newticket));

            EndPoint.DeleteAccount(newticket, user_id, newpassword);
        }

        [Test]
        public void ChangePasswordAdminTest()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            DateTime dateofbirth = DateTime.UtcNow.AddYears(-10);
            int user_id = CreateUser(email, password, dateofbirth);
            Assert.IsTrue(user_id > 0);
            WebAccountService.TransitAccount ta1 = EndPoint.GetAccountById(GetAdminTicket(), user_id);
            Assert.IsFalse(ta1.IsPasswordExpired, "Password is not expired after admin password reset.");
            string newpassword = GetNewString();
            EndPoint.ChangePassword(GetAdminTicket(), user_id, string.Empty, newpassword);
            // get the user and check whether password is properly expired
            WebAccountService.TransitAccount ta2 = EndPoint.GetAccountById(GetAdminTicket(), user_id);
            Assert.IsTrue(ta2.IsPasswordExpired, "Password is not expired after admin password reset.");
            // check that it resets back when the user logs in
            string ticket = EndPoint.Login(email, newpassword);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            EndPoint.ChangePassword(ticket, user_id, newpassword, GetNewString());
            WebAccountService.TransitAccount ta3 = EndPoint.GetAccountById(GetAdminTicket(), user_id);
            Assert.IsFalse(ta3.IsPasswordExpired, "Password is expired after user changes it.");
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
            catch (Exception)
            {
            }

            EndPoint.ResetPassword(email, dateofbirth);

            try
            {
                // login with the old password now fails
                string oldticket = EndPoint.Login(email, password);
                Assert.IsTrue(string.IsNullOrEmpty(oldticket));
            }
            catch (Exception)
            {
            }

            EndPoint.DeleteAccount(GetAdminTicket(), user_id, string.Empty);
        }

        [Test]
        public void IsPasswordValidTest()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            DateTime dateofbirth = DateTime.UtcNow.AddYears(-10);
            int user_id = CreateUser(email, password, dateofbirth);
            Assert.IsTrue(user_id > 0);
            string ticket = EndPoint.Login(email, password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            Assert.IsTrue(EndPoint.IsPasswordValid(ticket, user_id, password), "Password should be valid.");
            Assert.IsFalse(EndPoint.IsPasswordValid(ticket, user_id, GetNewString()), "Password should be invalid.");
            EndPoint.DeleteAccount(GetAdminTicket(), user_id, string.Empty);
        }
    }
}

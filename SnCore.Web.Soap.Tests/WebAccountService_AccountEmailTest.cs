using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountEmailTest : AccountBaseTest<WebAccountService.TransitAccountEmail>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public AccountEmailTest()
            : base("AccountEmail")
        {

        }

        public override WebAccountService.TransitAccountEmail GetTransitInstance()
        {
            WebAccountService.TransitAccountEmail t_instance = new WebAccountService.TransitAccountEmail();
            t_instance.Address = string.Format("{0}@localhost.com", Guid.NewGuid().ToString());
            return t_instance;
        }

        [Test]
        public void CreateAccountEmailTest()
        {
            // create a user
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = Guid.NewGuid().ToString();
            int user_id = CreateUser(email, password);
            Assert.IsTrue(user_id > 0);
            string ticket = Login(email, password);
            Assert.IsNotEmpty(ticket);
            // check that the e-mail is part of e-mails
            WebAccountService.TransitAccountEmail[] emails = EndPoint.GetAccountEmails(ticket, user_id, null);
            Assert.AreEqual(1, emails.Length);
            Assert.AreEqual(email, emails[0].Address, "E-mail addresses don't match.");

            string email2 = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password2 = Guid.NewGuid().ToString();
            int user2_id = CreateUser(email2, password2);
            string ticket2 = Login(email2, password2);

            try
            {
                WebAccountService.TransitAccountEmail[] emails2 = EndPoint.GetAccountEmails(ticket2, user_id, null);
                Assert.IsTrue(false, "Users cannot see each other's e-mails.");
            }
            catch (SoapException)
            {
            }

            DeleteUser(user_id);
        }

        [Test]
        protected void HasVerifiedEmailTest()
        {

        }

        [Test]
        protected void GetActiveEmailAddressTest()
        {
        }
    }
}

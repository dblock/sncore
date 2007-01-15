using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountInvitationTest : AccountBaseTest<WebAccountService.TransitAccountInvitation>
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

        public AccountInvitationTest()
            : base("AccountInvitation")
        {

        }

        public override WebAccountService.TransitAccountInvitation GetTransitInstance()
        {
            WebAccountService.TransitAccountInvitation t_instance = new WebAccountService.TransitAccountInvitation();
            t_instance.AccountId = _account_id;
            t_instance.Email = string.Format("{0}@localhost.com", Guid.NewGuid().ToString());
            t_instance.Message = Guid.NewGuid().ToString();
            t_instance.Code = Guid.NewGuid().ToString(); // only useful when admin
            return t_instance;
        }

        [Test]
        public void CreateAccountInvitationTest()
        {
            // create an invitation from admin to a new user
            WebAccountService.TransitAccountInvitation t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // sign-up with this invitation
            WebAccountService.TransitAccount t_account = new WebAccountService.TransitAccount();
            t_account.Name = Guid.NewGuid().ToString();
            t_account.Password = Guid.NewGuid().ToString();
            t_account.Birthday = DateTime.UtcNow.AddYears(-10);

            try
            {
                int impossible_id = EndPoint.CreateAccountWithInvitation(t_instance.Id, Guid.NewGuid().ToString(), t_account);
                Assert.IsTrue(false, "Invalid code should have thrown an exception.");
            }
            catch (SoapException)
            {

            }

            try
            {
                int impossible_id = EndPoint.CreateAccountWithInvitation(-1, t_instance.Code, t_account);
                Assert.IsTrue(false, "Invalid id should have thrown an exception.");
            }
            catch (SoapException)
            {

            }

            string ticket = EndPoint.CreateAccountWithInvitationAndLogin(t_instance.Id, t_instance.Code, t_account);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            int id = EndPoint.GetAccountId(ticket);
            Assert.IsTrue(id > 0);
            Console.WriteLine("New account: {0}", id);

            try
            {
                EndPoint.GetAccountInvitationById(GetAdminTicket(), t_instance.Id);
                Assert.IsTrue(false, "Invitation hasn't been deleted after the account was created.");
            }
            catch (SoapException)
            {
            }

            EndPoint.DeleteAccountById(ticket, id, t_account.Password);
        }

        [Test]
        public void DeclineInvitationTest()
        {
            // create an invitation from admin to a new user
            WebAccountService.TransitAccountInvitation t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // decline this invitation

            try
            {
                EndPoint.DeclineInvitation(t_instance.Id, Guid.NewGuid().ToString());
                Assert.IsTrue(false, "Invalid code should have thrown an exception.");
            }
            catch (SoapException)
            {

            }

            try
            {
                EndPoint.DeclineInvitation(-1, t_instance.Code);
                Assert.IsTrue(false, "Invalid id should have thrown an exception.");
            }
            catch (SoapException)
            {

            }

            EndPoint.DeclineInvitation(t_instance.Id, t_instance.Code);

            try
            {
                EndPoint.GetAccountInvitationById(GetAdminTicket(), t_instance.Id);
                Assert.IsTrue(false, "Invitation hasn't been deleted after it was declined.");
            }
            catch (SoapException)
            {
            }
        }
    }
}

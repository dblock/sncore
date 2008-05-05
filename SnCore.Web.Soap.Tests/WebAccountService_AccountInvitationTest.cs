using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountInvitationTest : WebServiceTest<WebAccountService.TransitAccountInvitation, WebAccountServiceNoCache>
    {
        private UserInfo _user = null;

        public override void SetUp()
        {
            base.SetUp();
            _user = CreateUserWithVerifiedEmailAddress();
        }

        public override void TearDown()
        {
            DeleteUser(_user.id);
            base.TearDown();
        }

        public AccountInvitationTest()
            : base("AccountInvitation")
        {

        }

        public override WebAccountService.TransitAccountInvitation GetTransitInstance()
        {
            WebAccountService.TransitAccountInvitation t_instance = new WebAccountService.TransitAccountInvitation();
            t_instance.AccountId = _user.id;
            t_instance.Email = GetNewEmailAddress();
            t_instance.Message = GetNewString();
            t_instance.Code = GetNewString(); // only useful when admin
            return t_instance;
        }

        [Test]
        protected void CreateAccountInvitationVerifyEmailTest()
        {
            // create an invitation from admin to a new user
            WebAccountService.TransitAccountInvitation t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // check that there's an email sent to the recepient
            // TODO: verify that there's an e-mail in AccountEmailMessage and that it contains the code
        }

        [Test]
        protected void GetAccountInvitationByIdAndCodeTest()
        {

        }

        [Test]
        public void CreateAccountInvitationTest()
        {
            // create an invitation from admin to a new user
            WebAccountService.TransitAccountInvitation t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            // sign-up with this invitation
            WebAccountService.TransitAccount t_account = new WebAccountService.TransitAccount();
            t_account.Name = GetNewString();
            t_account.Password = GetNewString();
            t_account.Birthday = DateTime.UtcNow.AddYears(-10);

            try
            {
                int impossible_id = EndPoint.CreateAccountWithInvitation(t_instance.Id, GetNewString(), t_account);
                Assert.IsTrue(false, "Invalid code should have thrown an exception.");
            }
            catch (Exception)
            {

            }

            try
            {
                int impossible_id = EndPoint.CreateAccountWithInvitation(-1, t_instance.Code, t_account);
                Assert.IsTrue(false, "Invalid id should have thrown an exception.");
            }
            catch (Exception)
            {

            }

            string ticket = EndPoint.CreateAccountWithInvitationAndLogin(t_instance.Id, t_instance.Code, t_account);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            int id = EndPoint.GetAccountId(ticket);
            Assert.IsTrue(id > 0);
            Console.WriteLine("New account: {0}", id);

            WebAccountService.TransitAccountInvitation t_instance_deleted = EndPoint.GetAccountInvitationById(GetAdminTicket(), t_instance.Id);
            Assert.IsNull(t_instance_deleted, "Invitation hasn't been deleted after the account was created.");

            EndPoint.DeleteAccount(ticket, id);
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
                EndPoint.DeclineInvitation(t_instance.Id, GetNewString());
                Assert.IsTrue(false, "Invalid code should have thrown an exception.");
            }
            catch (Exception)
            {

            }

            try
            {
                EndPoint.DeclineInvitation(-1, t_instance.Code);
                Assert.IsTrue(false, "Invalid id should have thrown an exception.");
            }
            catch (Exception)
            {

            }

            EndPoint.DeclineInvitation(t_instance.Id, t_instance.Code);

            WebAccountService.TransitAccountInvitation t_instance_deleted = EndPoint.GetAccountInvitationById(GetAdminTicket(), t_instance.Id);
            Assert.IsNull(t_instance_deleted, "Invitation hasn't been deleted after it was declined.");
        }

        [Test]
        public void GetAccountInvitationsByAccountIdTest()
        {
            int count1 = EndPoint.GetAccountInvitationsCountByAccountId(
                _user.ticket, _user.id);
            Console.WriteLine("Count: {0}", count1);
            Assert.IsTrue(count1 >= 0);
            WebAccountService.TransitAccountInvitation t_instance = GetTransitInstance();
            t_instance.AccountId = _user.id;
            int id = Create(_user.ticket, t_instance);
            int count2 = EndPoint.GetAccountInvitationsCountByAccountId(
                _user.ticket, _user.id);
            Console.WriteLine("Count: {0}", count2);
            Assert.AreEqual(count1 + 1, count2);
            WebAccountService.TransitAccountInvitation[] invitations = EndPoint.GetAccountInvitationsByAccountId(
                _user.ticket, _user.id, null);
            Console.WriteLine("Invitations: {0}", invitations.Length);
            Assert.AreEqual(invitations.Length, count2);
            Delete(_user.ticket, id);
            int count3 = EndPoint.GetAccountInvitationsCountByAccountId(
                _user.ticket, _user.id);
            Assert.AreEqual(count1, count3);
        }
    }
}

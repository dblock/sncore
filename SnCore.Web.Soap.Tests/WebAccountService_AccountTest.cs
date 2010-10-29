using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Web.Soap.Tests.WebDiscussionServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountTest : WebServiceTest<WebAccountService.TransitAccount, WebAccountServiceNoCache>
    {
        private UserInfo _user = null;

        public UserInfo User
        {
            get
            {
                return _user;
            }
        }

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

        public AccountTest()
            : base("Account")
        {

        }

        public override WebAccountService.TransitAccount GetTransitInstance()
        {
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Name = GetNewString();
            t_instance.Password = GetNewString();
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            return t_instance;
        }

        [Test]
        public void GetAccountTest()
        {
            WebAccountService.TransitAccount t_instance = GetTransitInstance();
            string email = GetNewEmailAddress();
            int id = EndPoint.CreateAccount(string.Empty, email, t_instance);
            Assert.IsTrue(id > 0);
            string ticket = EndPoint.Login(email, t_instance.Password);
            int id2 = EndPoint.GetAccountId(ticket);
            Assert.AreEqual(id, id2);
            WebAccountService.TransitAccount t_instance2 = EndPoint.GetAccount(ticket, true);
            Assert.AreEqual(t_instance2.Id, id);
            Assert.AreEqual(t_instance2.Name, t_instance.Name);
        }

        [Test]
        public void GetAccountIdTest()
        {
            Assert.AreEqual(GetAdminAccount().Id, EndPoint.GetAccountId(GetAdminTicket()));
            Assert.AreEqual(_user.id, EndPoint.GetAccountId(_user.ticket));
            Assert.AreNotEqual(GetAdminAccount().Id, EndPoint.GetAccountId(_user.ticket));
        }

        [Test]
        public void GetAccountByIdTest()
        {
            WebAccountService.TransitAccount t_instance = EndPoint.GetAccountById(_user.ticket, _user.id);
            Assert.IsTrue(string.IsNullOrEmpty(t_instance.Password));
            Assert.AreEqual(t_instance.Id, _user.id);
        }

        [Test]
        public void FindByEmailTest()
        {
            WebAccountService.TransitAccount t_instance = EndPoint.FindByEmail(_user.ticket, "admin@localhost.com");
            Assert.AreEqual(t_instance.Id, GetAdminAccount().Id);
        }

        [Test]
        public void SearchAccountsTest()
        {
            WebAccountService.TransitAccount[] accounts = EndPoint.SearchAccounts(_user.ticket, GetAdminAccount().Name, null);
            Console.WriteLine("Accounts: {0}", accounts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebAccountService.TransitAccount>(accounts).ContainsId(GetAdminAccount().Id));
        }

        [Test]
        public void SearchAccountsEmptyTest()
        {
            WebAccountService.TransitAccount[] accounts = EndPoint.SearchAccounts(
                _user.ticket, string.Empty, null);
            Assert.AreEqual(0, accounts.Length);
        }

        [Test]
        public void SendAccountEmailMessageTest()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            WebAccountService.TransitAccountEmailMessage t_instance = new WebAccountService.TransitAccountEmailMessage();
            t_instance.AccountId = user.id;
            t_instance.Body = GetNewString();
            t_instance.MailFrom = user.email;
            t_instance.MailTo = GetNewEmailAddress();
            t_instance.Subject = GetNewString();
            t_instance.Id = EndPoint.CreateOrUpdateAccountEmailMessage(user.ticket, t_instance);
            Console.WriteLine("Message: {0}", t_instance.Id);
            Assert.AreNotEqual(0, t_instance.Id);
            EndPoint.DeleteAccountEmailMessage(GetAdminTicket(), t_instance.Id);
            DeleteUser(user.id);
        }

        [Test]
        public void SendAccountEmailMessageInvalidEmailTest()
        {
            // alter the message mail from, should not let me send
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            try
            {
                WebAccountService.TransitAccountEmailMessage t_instance = new WebAccountService.TransitAccountEmailMessage();
                t_instance.AccountId = user.id;
                t_instance.Body = GetNewString();
                t_instance.MailFrom = GetNewEmailAddress();
                t_instance.MailTo = GetNewEmailAddress();
                t_instance.Subject = GetNewString();
                t_instance.Id = EndPoint.CreateOrUpdateAccountEmailMessage(_user.ticket, t_instance);
                Console.WriteLine("Message: {0}", t_instance.Id);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }

            DeleteUser(user.id);
        }

        [Test]
        public void GetAdminAccountTest()
        {
            WebAccountService.TransitAccount t_instance = EndPoint.GetAdminAccount(_user.ticket);
            Assert.IsNotNull(t_instance);
            Assert.IsTrue(t_instance.IsAdministrator);
        }

        [Test]
        public void FindAllByEmailTest()
        {
            WebAccountService.TransitAccount[] t_instances = EndPoint.FindAllByEmail(
                GetAdminTicket(), "admin@localhost.com", null);
            Console.WriteLine("Results: {0}", t_instances.Length);
            Assert.AreNotEqual(0, t_instances.Length);
        }

        [Test]
        public void SetCountryStateTest()
        {
            WebAccountService.TransitAccount t_instance = GetTransitInstance();
            string email = GetNewEmailAddress();
            t_instance.Id = EndPoint.CreateAccount(string.Empty, email, t_instance);
            Assert.IsTrue(t_instance.Id > 0);
            string ticket = EndPoint.Login(email, t_instance.Password);
            t_instance.State = "New York";
            t_instance.Country = "United States";
            EndPoint.CreateOrUpdateAccount(ticket, t_instance);
            WebAccountService.TransitAccount t_instance2 = EndPoint.GetAccount(ticket, true);
            Assert.AreEqual(t_instance2.State, t_instance.State);
            Assert.AreEqual(t_instance2.Country, t_instance.Country);
            t_instance.State = string.Empty;
            t_instance.Country = "New Zealand";
            EndPoint.CreateOrUpdateAccount(ticket, t_instance);
            WebAccountService.TransitAccount t_instance3 = EndPoint.GetAccount(ticket, true);
            Assert.AreEqual(t_instance3.State, t_instance.State);
            Assert.AreEqual(t_instance3.Country, t_instance.Country);
        }

        [Test]
        public void DeleteAccountWithoutContentTests()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            WebDiscussionService.TransitDiscussionPost t_post = post.GetTransitInstance();
            t_post.AccountId = user.id;
            t_post.Id = post.EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            int count1 = post.EndPoint.GetDiscussionPostsCount(GetAdminTicket(), t_post.DiscussionId);
            EndPoint.DeleteAccount(GetAdminTicket(), user.id);
            // deleting a user with default options doesn't delete all his posts
            int count2 = post.EndPoint.GetDiscussionPostsCount(GetAdminTicket(), t_post.DiscussionId);
            Assert.AreEqual(count1, count2);
            post.TearDown();
        }

        [Test]
        public void DeleteAccountWithContentTests()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();
            // create a post
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            WebDiscussionService.TransitDiscussionPost t_post = post.GetTransitInstance();
            t_post.AccountId = user.id;
            t_post.Id = post.EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            int count1 = post.EndPoint.GetDiscussionPostsCount(GetAdminTicket(), t_post.DiscussionId);
            WebAccountService.TransitAccountDeleteOptions options = new WebAccountService.TransitAccountDeleteOptions();
            options.DeleteContent = true;
            EndPoint.DeleteAccountWithOptions(GetAdminTicket(), user.id, options);
            // deleting a user with content
            int count2 = post.EndPoint.GetDiscussionPostsCount(GetAdminTicket(), t_post.DiscussionId);
            Assert.AreEqual(count1 - 1, count2);
            post.TearDown();
        }
    }
}

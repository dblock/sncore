using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupAccountInvitationTest : WebServiceTest<WebGroupService.TransitAccountGroupAccountInvitation, WebGroupServiceNoCache>
    {
        private AccountGroupTest _group = new AccountGroupTest();
        private int _group_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _group.Delete(GetAdminTicket(), _group_id);
            _group.TearDown();
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _group_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _group_id, options };
            return args;
        }

        public AccountGroupAccountInvitationTest()
            : base("AccountGroupAccountInvitation")
        {

        }

        public override WebGroupService.TransitAccountGroupAccountInvitation GetTransitInstance()
        {
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = new WebGroupService.TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Message = GetNewString();
            return t_instance;
        }

        [Test]
        public void GetAccountGroupAccountInvitationsByAccountIdTest()
        {
            // get the pending account's invitations
            WebGroupService.TransitAccountGroupAccountInvitation[] invitations1 = EndPoint.GetAccountGroupAccountInvitationsByAccountId(
                GetUserTicket(), GetUserAccount().Id, null);
            Assert.IsNotNull(invitations1);
            // update the group to private
            WebGroupService.TransitAccountGroup t_group = EndPoint.GetAccountGroupById(GetAdminTicket(), _group_id);
            t_group.IsPrivate = true;
            EndPoint.CreateOrUpdateAccountGroup(GetAdminTicket(), t_group);
            Console.WriteLine("Updated group to private: {0}", _group_id);
            // create an invitatoin for user
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = new WebGroupService.TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.RequesterId = GetAdminAccount().Id;
            t_instance.Message = GetNewString();
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetAdminTicket(), t_instance);
            Console.WriteLine("Created invitation: {0}", t_instance.Id);
            // make sure the user isn't member of the group
            Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(GetAdminTicket(), GetUserAccount().Id, _group_id));
            // get the pending account's invitations
            WebGroupService.TransitAccountGroupAccountInvitation[] invitations2 = EndPoint.GetAccountGroupAccountInvitationsByAccountId(
                GetUserTicket(), GetUserAccount().Id, null);
            Assert.IsNotNull(invitations2);
            Assert.AreEqual(invitations1.Length + 1, invitations2.Length);
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccountInvitation>(invitations2).ContainsId(
                t_instance.Id));
        }

        [Test]
        public void CreateOrUpdateAccountGroupAccountInvitationTest()
        {
            string email = GetNewEmailAddress();
            AccountTest _account = new AccountTest();
            WebAccountService.TransitAccount t_user = _account.GetTransitInstance();
            t_user.Id = _account.EndPoint.CreateAccount(string.Empty, email, t_user);
            Assert.AreNotEqual(0, t_user.Id);
            Console.WriteLine("Created account: {0}", t_user.Id);            
            // only members can create invitations
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = GetTransitInstance();
            t_instance.AccountId = t_user.Id;
            t_instance.RequesterId = GetUserAccount().Id;
            try
            {
                // make sure the user is not a member of the group
                Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(
                    GetAdminTicket(), GetUserAccount().Id, _group_id));
                // create the account group invitation
                EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetUserTicket(), t_instance);
                Assert.IsTrue(false, "Expected Access Denied exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
                    ex.Message.Split("\n".ToCharArray(), 2)[0],
                    string.Format("Unexpected exception: {0}", ex.Message));
            }
            // the user joins the group
            WebGroupService.TransitAccountGroupAccount t_accountinstance = new WebGroupService.TransitAccountGroupAccount();
            t_accountinstance.AccountGroupId = _group_id;
            t_accountinstance.AccountId = GetUserAccount().Id;
            t_accountinstance.IsAdministrator = false;
            t_accountinstance.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_accountinstance);
            // make sure the invitation can be now sent
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetUserTicket(), t_instance);
            Assert.AreNotEqual(0, t_instance.Id);

            // TODO: make sure another user can't see invitation
        }

        [Test]
        public void CreateOrUpdateAccountGroupExistingAccountInvitationTest()
        {
            // the user joins the group
            WebGroupService.TransitAccountGroupAccount t_accountinstance = new WebGroupService.TransitAccountGroupAccount();
            t_accountinstance.AccountGroupId = _group_id;
            t_accountinstance.AccountId = GetUserAccount().Id;
            t_accountinstance.IsAdministrator = false;
            t_accountinstance.Id = EndPoint.CreateOrUpdateAccountGroupAccount(GetAdminTicket(), t_accountinstance);
            Assert.AreNotEqual(0, t_accountinstance.Id);
            // this is the same user that is already a member of the group
            try
            {
                WebGroupService.TransitAccountGroupAccountInvitation t_instance = GetTransitInstance();
                t_instance.AccountId = GetUserAccount().Id;
                t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetUserTicket(), t_instance);
                Assert.IsTrue(false, "Expected user is already a member of the group exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.Contains("is already a member of"), string.Format("Unexpected exception: {0}", ex.Message));
            }
        }

        [Test]
        public void AcceptAccountGroupAccountInvitationTest()
        {
            // login user
            string email = GetNewEmailAddress();
            AccountTest _account = new AccountTest();
            WebAccountService.TransitAccount t_user = _account.GetTransitInstance();
            t_user.Id = _account.EndPoint.CreateAccount(string.Empty, email, t_user);
            Assert.AreNotEqual(0, t_user.Id);
            Console.WriteLine("Created account: {0}", t_user.Id);
            string userticket = _account.Login(email, t_user.Password);
            // join user to group
            WebGroupService.TransitAccountGroupAccount t_account = new WebGroupService.TransitAccountGroupAccount();
            t_account.AccountGroupId = _group_id;
            t_account.AccountId = t_user.Id;
            t_account.IsAdministrator = false;
            t_account.Id = EndPoint.CreateOrUpdateAccountGroupAccount(userticket, t_account);
            Assert.AreNotEqual(0, t_account.Id);
            Console.WriteLine("Joined user: {0}", t_account.Id);
            // update the group to private
            WebGroupService.TransitAccountGroup t_group = EndPoint.GetAccountGroupById(GetAdminTicket(), _group_id);
            t_group.IsPrivate = true;
            EndPoint.CreateOrUpdateAccountGroup(GetAdminTicket(), t_group);
            Console.WriteLine("Updated group to private: {0}", _group_id);
            // create an invitatoin for user
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = new WebGroupService.TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.RequesterId = t_account.AccountId;
            t_instance.Message = GetNewString();
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetAdminTicket(), t_instance);
            Console.WriteLine("Created invitation: {0}", t_instance.Id);
            // make sure the user isn't member of the group
            Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(GetAdminTicket(), GetUserAccount().Id, _group_id));
            // get the pending group membership requests
            WebGroupService.TransitAccountGroupAccountRequest[] requests1 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests1.Length);
            // accept invitation
            EndPoint.AcceptAccountGroupAccountInvitation(GetUserTicket(), t_instance.Id, GetNewString());
            // this has created a new request on the group, it should be +1
            WebGroupService.TransitAccountGroupAccountRequest[] requests2 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests2.Length);
            Assert.AreEqual(requests1.Length + 1, requests2.Length);
            // make sure there's an AccountId in the requests for the user
            WebGroupService.TransitAccountGroupAccountRequest request = null;
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccountRequest>(requests2).ContainsId(
                GetUserAccount().Id, "AccountId", out request));
            Assert.IsNotNull(request);
            Console.WriteLine("New request: {0}", request.Id);
            // accept the request by admin
            EndPoint.AcceptAccountGroupAccountRequest(GetAdminTicket(), request.Id, GetNewString());
            // make sure the invitation was deleted
            Assert.IsNull(EndPoint.GetAccountGroupAccountInvitationById(GetUserTicket(), t_instance.Id));
            // make sure the request cannot be found any more
            WebGroupService.TransitAccountGroupAccountRequest[] requests3 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests3.Length);
            Assert.AreEqual(requests1.Length, requests3.Length);
            // make sure the user is member of the group
            WebGroupService.TransitAccountGroupAccount t_groupaccount = EndPoint.GetAccountGroupAccountByAccountGroupId(
                GetAdminTicket(), GetUserAccount().Id, _group_id);
            Assert.IsNotNull(t_groupaccount);
            Console.WriteLine("Account: {0}", t_groupaccount.Id);
            Assert.AreEqual(t_groupaccount.AccountId, GetUserAccount().Id);
            _account.Delete(GetAdminTicket(), t_user.Id);
        }

        [Test]
        public void AcceptAccountGroupAccountInvitationAdminRequesterTest()
        {
            // update the group to private
            WebGroupService.TransitAccountGroup t_group = EndPoint.GetAccountGroupById(GetAdminTicket(), _group_id);
            t_group.IsPrivate = true;
            EndPoint.CreateOrUpdateAccountGroup(GetAdminTicket(), t_group);
            Console.WriteLine("Updated group to private: {0}", _group_id);
            // create an invitatoin for user
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = new WebGroupService.TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.RequesterId = GetAdminAccount().Id;
            t_instance.Message = GetNewString();
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetAdminTicket(), t_instance);
            Console.WriteLine("Created invitation: {0}", t_instance.Id);
            // make sure the user isn't member of the group
            Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(GetAdminTicket(), GetUserAccount().Id, _group_id));
            // get the pending group membership requests
            WebGroupService.TransitAccountGroupAccountRequest[] requests1 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests1.Length);
            // accept invitation
            EndPoint.AcceptAccountGroupAccountInvitation(GetUserTicket(), t_instance.Id, GetNewString());
            // since the invitation comes from a group admin, this has not created a new request on the group
            WebGroupService.TransitAccountGroupAccountRequest[] requests2 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests2.Length);
            Assert.AreEqual(requests1.Length, requests2.Length);
            // make sure the invitation was deleted
            Assert.IsNull(EndPoint.GetAccountGroupAccountInvitationById(GetUserTicket(), t_instance.Id));
            // make sure the user is member of the group
            WebGroupService.TransitAccountGroupAccount t_account = EndPoint.GetAccountGroupAccountByAccountGroupId(GetAdminTicket(), GetUserAccount().Id, _group_id);
            Assert.IsNotNull(t_account);
            Console.WriteLine("Account: {0}", t_account.Id);
            Assert.AreEqual(t_account.AccountId, GetUserAccount().Id);
        }

        [Test]
        public void RejectAccountGroupAccountInvitationTest()
        {
            // update the group to private
            WebGroupService.TransitAccountGroup t_group = EndPoint.GetAccountGroupById(GetAdminTicket(), _group_id);
            t_group.IsPrivate = true;
            EndPoint.CreateOrUpdateAccountGroup(GetAdminTicket(), t_group);
            Console.WriteLine("Updated group to private: {0}", _group_id);
            // create an invitatoin for user
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = new WebGroupService.TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.RequesterId = GetAdminAccount().Id;
            t_instance.Message = GetNewString();
            t_instance.Id = EndPoint.CreateOrUpdateAccountGroupAccountInvitation(GetAdminTicket(), t_instance);
            Console.WriteLine("Created invitation: {0}", t_instance.Id);
            // make sure the user isn't member of the group
            Assert.IsNull(EndPoint.GetAccountGroupAccountByAccountGroupId(GetAdminTicket(), GetUserAccount().Id, _group_id));
            // get the pending group membership requests
            WebGroupService.TransitAccountGroupAccountRequest[] requests1 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests1.Length);
            // accept invitation
            EndPoint.RejectAccountGroupAccountInvitation(GetUserTicket(), t_instance.Id, GetNewString());
            // make sure the invitation was deleted
            Assert.IsNull(EndPoint.GetAccountGroupAccountInvitationById(GetUserTicket(), t_instance.Id));
            // this has not created a new request on the group, it should be =
            WebGroupService.TransitAccountGroupAccountRequest[] requests2 = EndPoint.GetAccountGroupAccountRequests(
                GetAdminTicket(), _group_id, null);
            Console.WriteLine("Pending requests: {0}", requests2.Length);
            Assert.AreEqual(requests1.Length, requests2.Length);
        }
    }
}

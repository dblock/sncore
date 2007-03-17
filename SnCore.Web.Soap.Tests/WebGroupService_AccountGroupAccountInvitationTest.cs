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
        private AccountTest _account = new AccountTest();
        private int _account_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
            _account.SetUp();
            _account_id = _account.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _account.Delete(GetAdminTicket(), _account_id);
            _account.TearDown();
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
            t_instance.AccountId = _account_id;
            t_instance.Message = GetNewString();
            return t_instance;
        }

        [Test]
        protected void GetAccountGroupAccountInvitationsByAccountIdTest()
        {

        }

        [Test]
        public void CreateOrUpdateAccountGroupAccountInvitationTest()
        {            
            // only members can create invitations
            WebGroupService.TransitAccountGroupAccountInvitation t_instance = GetTransitInstance();
            t_instance.AccountId = _account_id;
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
            catch (SoapException ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.AreEqual("SnCore.Services.ManagedAccount+AccessDeniedException: Access denied",
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
            catch (SoapException ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.Contains("is already a member of"), string.Format("Unexpected exception: {0}", ex.Message));
            }
        }

        [Test]
        public void AcceptAccountGroupAccountInvitationTest()
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

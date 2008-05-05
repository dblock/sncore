using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;
using SnCore.Web.Soap.Tests;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupAccountRequestTest : WebServiceTest<WebGroupService.TransitAccountGroupAccountRequest, WebGroupServiceNoCache>
    {
        private AccountGroupTest _group = new AccountGroupTest();
        private int _group_id = 0;
        private AccountTest _account = new AccountTest();
        private int _account_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
            _account.SetUp();
            _account_id = _account.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
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

        public AccountGroupAccountRequestTest()
            : base("AccountGroupAccountRequest")
        {

        }

        public override WebGroupService.TransitAccountGroupAccountRequest GetTransitInstance()
        {
            WebGroupService.TransitAccountGroupAccountRequest t_instance = new WebGroupService.TransitAccountGroupAccountRequest();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = _account_id;
            t_instance.Message = GetNewString();
            return t_instance;
        }

        [Test]
        public void GetAccountGroupAccountRequestsByAccountIdTest()
        {
            WebGroupService.TransitAccountGroupAccountRequest t_request = new WebGroupService.TransitAccountGroupAccountRequest();
            t_request.AccountGroupId = _group_id;
            t_request.AccountId = _user.id;
            t_request.Message = GetNewString();
            t_request.Id = EndPoint.CreateOrUpdateAccountGroupAccountRequest(_user.ticket, t_request);
            WebGroupService.TransitAccountGroupAccountRequest[] t_instances = EndPoint.GetAccountGroupAccountRequestsByAccountId(
                _user.ticket, _user.id, null);
            Assert.IsNotNull(t_instances);
            Assert.IsTrue(t_instances.Length > 0);
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccountRequest>(t_instances)
                .ContainsId(t_request.Id));
            Delete(GetAdminTicket(), t_request.Id);
        }

        [Test]
        public void AcceptAccountGroupAccountRequestTest()
        {
            WebGroupService.TransitAccountGroupAccountRequest t_request = new WebGroupService.TransitAccountGroupAccountRequest();
            t_request.AccountGroupId = _group_id;
            t_request.AccountId = _user.id;
            t_request.Message = GetNewString();
            t_request.Id = EndPoint.CreateOrUpdateAccountGroupAccountRequest(_user.ticket, t_request);
            Assert.IsNotNull(EndPoint.GetAccountGroupAccountRequestById(_user.ticket, t_request.Id));
            EndPoint.AcceptAccountGroupAccountRequest(GetAdminTicket(), t_request.Id, GetNewString());
            Assert.IsNull(EndPoint.GetAccountGroupAccountRequestById(_user.ticket, t_request.Id));
            WebGroupService.TransitAccountGroupAccount[] accounts = EndPoint.GetAccountGroupAccounts(
                GetAdminTicket(), _group_id, null);
            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Length > 0);
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccount>(accounts)
                .ContainsId(_user.id, "AccountId"));            
        }

        [Test]
        public void RejectAccountGroupAccountRequestTest()
        {
            WebGroupService.TransitAccountGroupAccountRequest t_request = new WebGroupService.TransitAccountGroupAccountRequest();
            t_request.AccountGroupId = _group_id;
            t_request.AccountId = _user.id;
            t_request.Message = GetNewString();
            t_request.Id = EndPoint.CreateOrUpdateAccountGroupAccountRequest(_user.ticket, t_request);
            Assert.IsNotNull(EndPoint.GetAccountGroupAccountRequestById(_user.ticket, t_request.Id));
            EndPoint.RejectAccountGroupAccountRequest(GetAdminTicket(), t_request.Id, GetNewString());
            Assert.IsNull(EndPoint.GetAccountGroupAccountRequestById(_user.ticket, t_request.Id));
            WebGroupService.TransitAccountGroupAccount[] accounts = EndPoint.GetAccountGroupAccounts(
                GetAdminTicket(), _group_id, null);
            Assert.IsNotNull(accounts);
            Assert.IsFalse(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccount>(accounts)
                .ContainsId(_user.id, "AccountId"));            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;

namespace SnCore.Web.Soap.Tests.WebGroupServiceTests
{
    [TestFixture]
    public class AccountGroupAccountTest : WebServiceTest<WebGroupService.TransitAccountGroupAccount, WebGroupServiceNoCache>
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

        public AccountGroupAccountTest()
            : base("AccountGroupAccount")
        {

        }

        public override WebGroupService.TransitAccountGroupAccount GetTransitInstance()
        {
            WebGroupService.TransitAccountGroupAccount t_instance = new WebGroupService.TransitAccountGroupAccount();
            t_instance.AccountGroupId = _group_id;
            t_instance.AccountId = _account_id;
            t_instance.IsAdministrator = true;
            return t_instance;
        }

        [Test]
        public void GetAccountGroupAccountsByAccountIdTest()
        {
            int count1 = EndPoint.GetAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.IsTrue(count1 >= 0);
            WebGroupService.TransitAccountGroupAccount t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            int count2 = EndPoint.GetAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.AreEqual(count1 + 1, count2);
            WebGroupService.TransitAccountGroupAccount[] accounts = EndPoint.GetAccountGroupAccountsByAccountId(
                GetAdminTicket(), _account_id, null);
            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Length > 0);
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccount>(accounts).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
            int count3 = EndPoint.GetAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.AreEqual(count1, count3);
        }

        [Test]
        public void GetAccountGroupAccountByAccountGroupIdTest()
        {
            WebGroupService.TransitAccountGroupAccount t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);            
            WebGroupService.TransitAccountGroupAccount account = EndPoint.GetAccountGroupAccountByAccountGroupId(
                GetAdminTicket(), _account_id, _group_id);
            Assert.IsNotNull(account);
            Assert.AreEqual(account.Id, t_instance.Id);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetPublicAccountGroupAccountsByAccountIdTest()
        {
            int count1 = EndPoint.GetPublicAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.IsTrue(count1 >= 0);
            WebGroupService.TransitAccountGroupAccount t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            int count2 = EndPoint.GetPublicAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.AreEqual(count1 + 1, count2);
            WebGroupService.TransitAccountGroupAccount[] accounts = EndPoint.GetPublicAccountGroupAccountsByAccountId(
                GetAdminTicket(), _account_id, null);
            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Length > 0);
            Assert.IsTrue(new TransitServiceCollection<WebGroupService.TransitAccountGroupAccount>(accounts).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
            int count3 = EndPoint.GetPublicAccountGroupAccountsByAccountIdCount(
                GetAdminTicket(), _account_id);
            Assert.AreEqual(count1, count3);
        }
    }
}

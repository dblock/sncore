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
        protected void CreateOrUpdateAccountGroupAccountInvitationMemberTest()
        {
            // only members can create invitations
        }

        [Test]
        protected void AcceptAccountGroupAccountInvitationTest()
        {

        }

        [Test]
        protected void RejectAccountGroupAccountInvitationTest()
        {

        }

        [Test]
        protected void PublicGroupInvitationWorkflowTest()
        {
            // invite a friend: email sent to friend
            // accept invitation: a welcome email sent to acceptor, a ack mail sent to requester   
        }

        [Test]
        protected void PrivateGroupInvitationWorkflowTest()
        {
            // invite a friend: email sent to friend
            // accept invitation: ack mail sent to requester,
            // new request, a request email sent to admin
        }
    }
}

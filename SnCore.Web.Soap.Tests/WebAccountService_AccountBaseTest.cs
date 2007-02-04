using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    public abstract class AccountBaseTest<TransitType> : WebServiceTest<TransitType, WebAccountServiceNoCache>
    {
        public int _account_id = 0;
        private string _ticket;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            CreateTestUser();
        }

        private void CreateTestUser()
        {
            string email = GetNewEmailAddress();
            // account endpoint
            WebAccountService.WebAccountService endpoint = new WebAccountService.WebAccountService();
            // create an invitation from admin to a new user
            WebAccountService.TransitAccountInvitation t_invitation = new WebAccountService.TransitAccountInvitation();
            t_invitation.Email = email;
            t_invitation.Code = GetNewString();
            t_invitation.AccountId = GetAdminAccount().Id;
            t_invitation.Id = endpoint.CreateOrUpdateAccountInvitation(GetAdminTicket(), t_invitation);
            // create an account with the invitation
            WebAccountService.TransitAccount t_account = new WebAccountService.TransitAccount();
            t_account.Name = GetNewString();
            t_account.Password = GetNewString();
            t_account.Birthday = DateTime.UtcNow.AddYears(-10);
            _ticket = EndPoint.CreateAccountWithInvitationAndLogin(t_invitation.Id, t_invitation.Code, t_account);
            _account_id = EndPoint.GetAccountId(_ticket);
            Console.WriteLine("New account: {0}", _account_id);
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_account_id);
            base.TearDown();
        }

        public override string GetTestTicket()
        {
            return _ticket;
        }

        public AccountBaseTest(string one)
            : base(one)
        {

        }

        public AccountBaseTest(string one, string many)
            : base(one, many)
        {

        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _account_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _account_id };
            return args;
        }
    }
}

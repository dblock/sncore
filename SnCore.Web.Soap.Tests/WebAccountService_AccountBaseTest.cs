using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    public abstract class AccountBaseTest<TransitType> : WebServiceTest<TransitType, WebAccountServiceNoCache>
    {
        private AccountTest _account = new AccountTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            base.TearDown();
        }

        public int GetTestAccountId()
        {
            return _account.User.id;
        }

        public override string GetTestTicket()
        {
            return _account.User.ticket;
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
            object[] args = { ticket, _account.User.id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _account.User.id };
            return args;
        }
    }
}

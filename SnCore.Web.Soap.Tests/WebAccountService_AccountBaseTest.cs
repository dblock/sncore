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
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = Guid.NewGuid().ToString();
            _account_id = CreateUser(email, password);
            _ticket = Login(email, password);
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

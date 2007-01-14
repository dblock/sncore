using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountOpenIdTest : WebServiceTest<WebAccountService.TransitAccountOpenId, WebAccountServiceNoCache>
    {
        public AccountOpenIdTest()
            : base("AccountOpenId")
        {

        }

        public override WebAccountService.TransitAccountOpenId GetTransitInstance()
        {
            WebAccountService.TransitAccountOpenId t_instance = new WebAccountService.TransitAccountOpenId();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }
    }
}

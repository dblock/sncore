using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountMessageTest : WebServiceTest<WebAccountService.TransitAccountMessage, WebAccountServiceNoCache>
    {
        public AccountMessageTest()
            : base("AccountMessage")
        {
        }


        public override WebAccountService.TransitAccountMessage GetTransitInstance()
        {
            WebAccountService.TransitAccountMessage t_instance = new WebAccountService.TransitAccountMessage();
            return t_instance;
        }
    }
}

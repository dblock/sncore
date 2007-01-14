using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountAddressTest : WebServiceTest<WebAccountService.TransitAccountAddress, WebAccountServiceNoCache>
    {
        public AccountAddressTest()
            : base("AccountAddress", "AccountAddresses")
        {
        }


        public override WebAccountService.TransitAccountAddress GetTransitInstance()
        {
            WebAccountService.TransitAccountAddress t_instance = new WebAccountService.TransitAccountAddress();
            return t_instance;
        }
    }
}

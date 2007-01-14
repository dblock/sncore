using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountWebsiteTest : WebServiceTest<WebAccountService.TransitAccountWebsite, WebAccountServiceNoCache>
    {
        public AccountWebsiteTest()
            : base("AccountWebsite")
        {
        }


        public override WebAccountService.TransitAccountWebsite GetTransitInstance()
        {
            WebAccountService.TransitAccountWebsite t_instance = new WebAccountService.TransitAccountWebsite();
            return t_instance;
        }
    }
}

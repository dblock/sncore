using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPropertyValueTest : WebServiceTest<WebAccountService.TransitAccountPropertyValue, WebAccountServiceNoCache>
    {
        public AccountPropertyValueTest()
            : base("AccountPropertyValue")
        {
        }


        public override WebAccountService.TransitAccountPropertyValue GetTransitInstance()
        {
            WebAccountService.TransitAccountPropertyValue t_instance = new WebAccountService.TransitAccountPropertyValue();
            return t_instance;
        }
    }
}

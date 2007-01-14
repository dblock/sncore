using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPropertyTest : WebServiceTest<WebAccountService.TransitAccountProperty, WebAccountServiceNoCache>
    {
        public AccountPropertyTest()
            : base("AccountProperty", "AccountProperties")
        {
        }


        public override WebAccountService.TransitAccountProperty GetTransitInstance()
        {
            WebAccountService.TransitAccountProperty t_instance = new WebAccountService.TransitAccountProperty();
            return t_instance;
        }
    }
}

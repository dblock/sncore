using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPropertyGroupTest : WebServiceTest<WebAccountService.TransitAccountPropertyGroup, WebAccountServiceNoCache>
    {
        public AccountPropertyGroupTest()
            : base("AccountPropertyGroup")
        {
        }


        public override WebAccountService.TransitAccountPropertyGroup GetTransitInstance()
        {
            WebAccountService.TransitAccountPropertyGroup t_instance = new WebAccountService.TransitAccountPropertyGroup();
            return t_instance;
        }
    }
}

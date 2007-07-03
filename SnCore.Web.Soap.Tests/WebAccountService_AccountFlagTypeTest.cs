using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountFlagTypeTest : WebServiceTest<WebAccountService.TransitAccountFlagType, WebAccountServiceNoCache>
    {
        public AccountFlagTypeTest()
            : base("AccountFlagType")
        {
        }

        public override WebAccountService.TransitAccountFlagType GetTransitInstance()
        {
            WebAccountService.TransitAccountFlagType t_instance = new WebAccountService.TransitAccountFlagType();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

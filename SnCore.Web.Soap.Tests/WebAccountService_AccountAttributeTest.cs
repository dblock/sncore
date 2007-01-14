using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountAttributeTest : WebServiceTest<WebAccountService.TransitAccountAttribute, WebAccountServiceNoCache>
    {
        public AccountAttributeTest()
            : base("AccountAttribute")
        {
        }


        public override WebAccountService.TransitAccountAttribute GetTransitInstance()
        {
            WebAccountService.TransitAccountAttribute t_instance = new WebAccountService.TransitAccountAttribute();
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountRedirectTest : WebServiceTest<WebAccountService.TransitAccountRedirect, WebAccountServiceNoCache>
    {
        public AccountRedirectTest()
            : base("AccountRedirect")
        {
        }


        public override WebAccountService.TransitAccountRedirect GetTransitInstance()
        {
            WebAccountService.TransitAccountRedirect t_instance = new WebAccountService.TransitAccountRedirect();
            return t_instance;
        }
    }
}

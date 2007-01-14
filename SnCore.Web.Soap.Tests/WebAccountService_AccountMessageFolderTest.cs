using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountMessageFolderTest : WebServiceTest<WebAccountService.TransitAccountMessageFolder, WebAccountServiceNoCache>
    {
        public AccountMessageFolderTest()
            : base("AccountMessageFolder")
        {
        }


        public override WebAccountService.TransitAccountMessageFolder GetTransitInstance()
        {
            WebAccountService.TransitAccountMessageFolder t_instance = new WebAccountService.TransitAccountMessageFolder();
            return t_instance;
        }
    }
}

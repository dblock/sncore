using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPictureTest : WebServiceTest<WebAccountService.TransitAccountPicture, WebAccountServiceNoCache>
    {
        public AccountPictureTest()
            : base("AccountPicture")
        {
        }


        public override WebAccountService.TransitAccountPicture GetTransitInstance()
        {
            WebAccountService.TransitAccountPicture t_instance = new WebAccountService.TransitAccountPicture();
            return t_instance;
        }
    }
}

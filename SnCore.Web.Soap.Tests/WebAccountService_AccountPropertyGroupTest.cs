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
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public AccountPropertyGroupTest()
            : base("AccountPropertyGroup")
        {
        }


        public override WebAccountService.TransitAccountPropertyGroup GetTransitInstance()
        {
            WebAccountService.TransitAccountPropertyGroup t_instance = new WebAccountService.TransitAccountPropertyGroup();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

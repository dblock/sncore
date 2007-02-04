using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountOpenIdTest : AccountBaseTest<WebAccountService.TransitAccountOpenId>
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

        public AccountOpenIdTest()
            : base("AccountOpenId")
        {

        }

        public override WebAccountService.TransitAccountOpenId GetTransitInstance()
        {
            WebAccountService.TransitAccountOpenId t_instance = new WebAccountService.TransitAccountOpenId();
            t_instance.AccountId = _account_id;
            t_instance.IdentityUrl = GetNewUri();
            return t_instance;
        }
    }
}

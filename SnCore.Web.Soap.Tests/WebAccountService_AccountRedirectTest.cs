using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountRedirectTest : AccountBaseTest<WebAccountService.TransitAccountRedirect>
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

        public AccountRedirectTest()
            : base("AccountRedirect")
        {
        }

        public override WebAccountService.TransitAccountRedirect GetTransitInstance()
        {
            WebAccountService.TransitAccountRedirect t_instance = new WebAccountService.TransitAccountRedirect();
            t_instance.AccountId = _account_id;
            t_instance.SourceUri = Guid.NewGuid().ToString().Replace('-', 'x');
            t_instance.TargetUri = Guid.NewGuid().ToString().Replace('-', 'x');
            return t_instance;
        }

        [Test]
        protected void GetAccountRedirectByTargetUriTest()
        {

        }

        [Test]
        protected void GetAccountRedirectBySourceUriTest()
        {

        }
    }
}

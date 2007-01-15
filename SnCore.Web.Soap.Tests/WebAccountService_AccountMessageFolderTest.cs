using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountMessageFolderTest : AccountBaseTest<WebAccountService.TransitAccountMessageFolder>
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

        public AccountMessageFolderTest()
            : base("AccountMessageFolder")
        {

        }

        public override WebAccountService.TransitAccountMessageFolder GetTransitInstance()
        {
            WebAccountService.TransitAccountMessageFolder t_instance = new WebAccountService.TransitAccountMessageFolder();
            t_instance.AccountId = _account_id;
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.System = false;
            return t_instance;
        }

        [Test]
        protected void GetAccountMessageSystemFolderTest()
        {

        }

        [Test]
        protected void CreateAccountSystemMessageFoldersTest()
        {

        }
    }
}

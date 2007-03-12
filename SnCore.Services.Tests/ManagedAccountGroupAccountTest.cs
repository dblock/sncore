using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupAccountTest : ManagedCRUDTest<AccountGroupAccount, TransitAccountGroupAccount, ManagedAccountGroupAccount>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountGroupTest _group = new ManagedAccountGroupTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
            _group.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _group.SetUp();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedAccountGroupAccountTest()
        {

        }

        public override TransitAccountGroupAccount GetTransitInstance()
        {
            TransitAccountGroupAccount t_instance = new TransitAccountGroupAccount();
            t_instance.AccountGroupId = _group.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.IsAdministrator = true;
            return t_instance;
        }

        [Test]
        protected void PromoteDemoteTest()
        {

        }

        [Test]
        protected void DemoteLastGroupAdministratorTest()
        {
            // test that demoting the last group administrator doesn't work
        }
    }
}

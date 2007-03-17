using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupAccountInvitationTest : ManagedCRUDTest<AccountGroupAccountInvitation, TransitAccountGroupAccountInvitation, ManagedAccountGroupAccountInvitation>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedAccountGroupTest _group = new ManagedAccountGroupTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _group.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _group.TearDown();
            _account.TearDown();
        }

        public ManagedAccountGroupAccountInvitationTest()
        {

        }

        public override TransitAccountGroupAccountInvitation GetTransitInstance()
        {
            TransitAccountGroupAccountInvitation t_instance = new TransitAccountGroupAccountInvitation();
            t_instance.AccountGroupId = _group.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Message = GetNewString();
            return t_instance;
        }

        [Test, ExpectedException(typeof(Exception))]
        public override void TestUpdateAndRetrieve()
        {
            base.TestUpdateAndRetrieve();
        }
    }
}

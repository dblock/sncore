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
    }
}

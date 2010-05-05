using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountInvitationGroupTest : ManagedCRUDTest<AccountInvitation, TransitAccountInvitation, ManagedAccountInvitation>
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

        public ManagedAccountInvitationGroupTest()
        {

        }

        public override TransitAccountInvitation GetTransitInstance()
        {
            TransitAccountInvitation t_instance = new TransitAccountInvitation();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Code = GetNewString();
            t_instance.Email = string.Format("{0}@{1}.com", Guid.NewGuid(), Guid.NewGuid());
            t_instance.Message = GetNewString();
            t_instance.AccountGroupId = _group.Instance.Id;
            return t_instance;
        }
    }
}

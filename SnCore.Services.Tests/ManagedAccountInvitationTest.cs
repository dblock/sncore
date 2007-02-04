using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountInvitationTest : ManagedCRUDTest<AccountInvitation, TransitAccountInvitation, ManagedAccountInvitation>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            base.TearDown();
        }

        public ManagedAccountInvitationTest()
        {

        }

        public override TransitAccountInvitation GetTransitInstance()
        {
            TransitAccountInvitation t_instance = new TransitAccountInvitation();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Code = GetNewString();
            t_instance.Email = string.Format("{0}@{1}.com", Guid.NewGuid(), Guid.NewGuid());
            t_instance.Message = GetNewString();
            return t_instance;
        }
    }
}

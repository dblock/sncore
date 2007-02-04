using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountOpenIdTest : ManagedCRUDTest<AccountOpenId, TransitAccountOpenId, ManagedAccountOpenId>
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

        public ManagedAccountOpenIdTest()
        {

        }

        public override TransitAccountOpenId GetTransitInstance()
        {
            TransitAccountOpenId t_instance = new TransitAccountOpenId();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.IdentityUrl = GetNewUri();
            return t_instance;
        }
    }
}

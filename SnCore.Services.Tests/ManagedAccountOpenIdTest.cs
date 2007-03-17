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
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
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

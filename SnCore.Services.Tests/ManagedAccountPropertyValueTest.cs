using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPropertyValueTest : ManagedCRUDTest<AccountPropertyValue, TransitAccountPropertyValue, ManagedAccountPropertyValue>
    {
        private ManagedAccountPropertyTest _property = new ManagedAccountPropertyTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            _property.SetUp();
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
            _property.TearDown();
        }

        public ManagedAccountPropertyValueTest()
        {

        }

        public override TransitAccountPropertyValue GetTransitInstance()
        {
            TransitAccountPropertyValue t_instance = new TransitAccountPropertyValue();
            t_instance.AccountPropertyId = _property.Instance.Id;
            t_instance.Value = GetNewString();
            t_instance.AccountId = _account.Instance.Id;
            return t_instance;
        }
    }
}

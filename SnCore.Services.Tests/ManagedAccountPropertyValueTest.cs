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
            base.SetUp();
            _property.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.SetUp();
            _property.TearDown();
            base.TearDown();
        }

        public ManagedAccountPropertyValueTest()
        {

        }

        public override TransitAccountPropertyValue GetTransitInstance()
        {
            TransitAccountPropertyValue t_instance = new TransitAccountPropertyValue();
            t_instance.AccountProperty = new TransitAccountProperty();
            t_instance.AccountProperty.Id = _property.Instance.Id;
            t_instance.Value = Guid.NewGuid().ToString();
            t_instance.AccountId = _account.Instance.Id;
            return t_instance;
        }
    }
}

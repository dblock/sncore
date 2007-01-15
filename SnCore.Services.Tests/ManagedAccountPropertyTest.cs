using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPropertyTest : ManagedCRUDTest<AccountProperty, TransitAccountProperty, ManagedAccountProperty>
    {
        private ManagedAccountPropertyGroupTest _accountpropertygroup = new ManagedAccountPropertyGroupTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _accountpropertygroup.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountpropertygroup.TearDown();
            base.TearDown();
        }

        public ManagedAccountPropertyTest()
        {

        }

        public override TransitAccountProperty GetTransitInstance()
        {
            TransitAccountProperty t_instance = new TransitAccountProperty();
            t_instance.AccountPropertyGroupId = _accountpropertygroup.Instance.Id;
            t_instance.DefaultValue = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Type = "System.String";
            return t_instance;
        }
    }
}

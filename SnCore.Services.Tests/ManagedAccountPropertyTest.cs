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
            _accountpropertygroup.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountpropertygroup.TearDown();
        }

        public ManagedAccountPropertyTest()
        {

        }

        public override TransitAccountProperty GetTransitInstance()
        {
            TransitAccountProperty t_instance = new TransitAccountProperty();
            t_instance.AccountPropertyGroupId = _accountpropertygroup.Instance.Id;
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Type = "System.String";
            return t_instance;
        }
    }
}

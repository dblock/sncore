using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedReminderAccountPropertyTest : ManagedCRUDTest<ReminderAccountProperty, TransitReminderAccountProperty, ManagedReminderAccountProperty>
    {
        private ManagedReminderTest _reminder = new ManagedReminderTest();
        private ManagedAccountPropertyTest _accountproperty = new ManagedAccountPropertyTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _reminder.SetUp();
            _accountproperty.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountproperty.TearDown();
            _reminder.TearDown();
            base.TearDown();
        }

        public ManagedReminderAccountPropertyTest()
        {

        }

        public override TransitReminderAccountProperty GetTransitInstance()
        {
            TransitReminderAccountProperty t_instance = new TransitReminderAccountProperty();
            t_instance.AccountPropertyId = _accountproperty.Instance.Id;
            t_instance.ReminderId = _reminder.Instance.Id;
            t_instance.Value = GetNewString();
            return t_instance;
        }
    }
}

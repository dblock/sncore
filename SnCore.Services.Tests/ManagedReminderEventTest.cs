using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedReminderEventTest : ManagedCRUDTest<ReminderEvent, TransitReminderEvent, ManagedReminderEvent>
    {
        private ManagedReminderTest _reminder = new ManagedReminderTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            _reminder.SetUp();
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
            _reminder.TearDown();
        }

        public ManagedReminderEventTest()
        {

        }

        public override TransitReminderEvent GetTransitInstance()
        {
            TransitReminderEvent t_instance = new TransitReminderEvent();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.ReminderId = _reminder.Instance.Id;
            return t_instance;
        }
    }
}

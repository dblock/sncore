using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedReminderEventTest : ManagedCRUDTest<ReminderEvent, TransitReminderEvent, ManagedReminderEvent>
    {
        public ManagedReminderEventTest()
        {

        }

        public override TransitReminderEvent GetTransitInstance()
        {
            TransitReminderEvent t_instance = new TransitReminderEvent();
            return t_instance;
        }
    }
}

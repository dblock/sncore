using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedReminderAccountPropertyTest : ManagedCRUDTest<ReminderAccountProperty, TransitReminderAccountProperty, ManagedReminderAccountProperty>
    {
        public ManagedReminderAccountPropertyTest()
        {

        }

        public override TransitReminderAccountProperty GetTransitInstance()
        {
            TransitReminderAccountProperty t_instance = new TransitReminderAccountProperty();
            return t_instance;
        }
    }
}

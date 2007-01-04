using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountEventTypeTest : ManagedCRUDTest<AccountEventType, TransitAccountEventType, ManagedAccountEventType>
    {
        public ManagedAccountEventTypeTest()
        {

        }

        public override TransitAccountEventType GetTransitInstance()
        {
            TransitAccountEventType t_instance = new TransitAccountEventType();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

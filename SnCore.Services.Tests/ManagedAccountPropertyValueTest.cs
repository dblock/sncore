using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPropertyValueTest : ManagedCRUDTest<AccountPropertyValue, TransitAccountPropertyValue, ManagedAccountPropertyValue>
    {
        public ManagedAccountPropertyValueTest()
        {

        }

        public override TransitAccountPropertyValue GetTransitInstance()
        {
            TransitAccountPropertyValue t_instance = new TransitAccountPropertyValue();
            return t_instance;
        }
    }
}

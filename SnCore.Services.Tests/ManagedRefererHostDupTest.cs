using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererHostDupTest : ManagedCRUDTest<RefererHostDup, TransitRefererHostDup, ManagedRefererHostDup>
    {
        public ManagedRefererHostDupTest()
        {

        }

        public override TransitRefererHostDup GetTransitInstance()
        {
            TransitRefererHostDup t_instance = new TransitRefererHostDup();
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererHostTest : ManagedCRUDTest<RefererHost, TransitRefererHost, ManagedRefererHost>
    {
        public ManagedRefererHostTest()
        {

        }

        public override TransitRefererHost GetTransitInstance()
        {
            TransitRefererHost t_instance = new TransitRefererHost();
            return t_instance;
        }
    }
}

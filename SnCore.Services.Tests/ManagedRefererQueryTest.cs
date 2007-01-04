using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedRefererQueryTest : ManagedCRUDTest<RefererQuery, TransitRefererQuery, ManagedRefererQuery>
    {
        public ManagedRefererQueryTest()
        {

        }

        public override TransitRefererQuery GetTransitInstance()
        {
            TransitRefererQuery t_instance = new TransitRefererQuery();
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedMadLibInstanceTest : ManagedCRUDTest<MadLibInstance, TransitMadLibInstance, ManagedMadLibInstance>
    {
        public ManagedMadLibInstanceTest()
        {

        }

        public override TransitMadLibInstance GetTransitInstance()
        {
            TransitMadLibInstance t_instance = new TransitMadLibInstance();
            return t_instance;
        }
    }
}

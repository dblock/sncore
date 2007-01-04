using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedMadLibTest : ManagedCRUDTest<MadLib, TransitMadLib, ManagedMadLib>
    {
        public ManagedMadLibTest()
        {

        }

        public override TransitMadLib GetTransitInstance()
        {
            TransitMadLib t_instance = new TransitMadLib();
            return t_instance;
        }
    }
}

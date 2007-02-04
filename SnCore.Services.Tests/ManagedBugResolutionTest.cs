using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugResolutionTest : ManagedCRUDTest<BugResolution, TransitBugResolution, ManagedBugResolution>
    {
        public ManagedBugResolutionTest()
        {

        }

        public override TransitBugResolution GetTransitInstance()
        {
            TransitBugResolution t_instance = new TransitBugResolution();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

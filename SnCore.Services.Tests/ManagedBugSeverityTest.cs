using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugSeverityTest : ManagedCRUDTest<BugSeverity, TransitBugSeverity, ManagedBugSeverity>
    {
        public ManagedBugSeverityTest()
        {

        }

        public override TransitBugSeverity GetTransitInstance()
        {
            TransitBugSeverity t_instance = new TransitBugSeverity();
            return t_instance;
        }
    }
}

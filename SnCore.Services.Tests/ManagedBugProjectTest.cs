using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugProjectTest : ManagedCRUDTest<BugProject, TransitBugProject, ManagedBugProject>
    {
        public ManagedBugProjectTest()
        {

        }

        public override TransitBugProject GetTransitInstance()
        {
            TransitBugProject t_instance = new TransitBugProject();
            return t_instance;
        }
    }
}

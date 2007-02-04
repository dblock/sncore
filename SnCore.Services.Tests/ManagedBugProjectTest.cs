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
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

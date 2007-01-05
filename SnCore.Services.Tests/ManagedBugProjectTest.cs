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
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

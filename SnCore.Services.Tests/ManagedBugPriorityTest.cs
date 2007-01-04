using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugPriorityTest : ManagedCRUDTest<BugPriority, TransitBugPriority, ManagedBugPriority>
    {
        public ManagedBugPriorityTest()
        {

        }

        public override TransitBugPriority GetTransitInstance()
        {
            TransitBugPriority t_instance = new TransitBugPriority();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

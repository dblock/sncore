using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugStatusTest : ManagedCRUDTest<BugStatu, TransitBugStatus, ManagedBugStatus>
    {
        public ManagedBugStatusTest()
        {

        }

        public override TransitBugStatus GetTransitInstance()
        {
            TransitBugStatus t_instance = new TransitBugStatus();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

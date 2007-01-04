using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugLinkTest : ManagedCRUDTest<BugLink, TransitBugLink, ManagedBugLink>
    {
        public ManagedBugLinkTest()
        {

        }

        public override TransitBugLink GetTransitInstance()
        {
            TransitBugLink t_instance = new TransitBugLink();
            return t_instance;
        }
    }
}

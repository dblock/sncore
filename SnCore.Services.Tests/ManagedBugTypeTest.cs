using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugTypeTest : ManagedCRUDTest<BugType, TransitBugType, ManagedBugType>
    {
        public ManagedBugTypeTest()
        {

        }

        public override TransitBugType GetTransitInstance()
        {
            TransitBugType t_instance = new TransitBugType();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

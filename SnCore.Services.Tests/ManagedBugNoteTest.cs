using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBugNoteTest : ManagedCRUDTest<BugNote, TransitBugNote, ManagedBugNote>
    {
        public ManagedBugNoteTest()
        {

        }

        public override TransitBugNote GetTransitInstance()
        {
            TransitBugNote t_instance = new TransitBugNote();
            return t_instance;
        }
    }
}

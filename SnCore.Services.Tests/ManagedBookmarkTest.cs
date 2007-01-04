using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedBookmarkTest : ManagedCRUDTest<Bookmark, TransitBookmark, ManagedBookmark>
    {
        public ManagedBookmarkTest()
        {

        }

        public override TransitBookmark GetTransitInstance()
        {
            TransitBookmark t_instance = new TransitBookmark();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

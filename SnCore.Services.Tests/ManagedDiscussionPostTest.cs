using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDiscussionPostTest : ManagedCRUDTest<DiscussionPost, TransitDiscussionPost, ManagedDiscussionPost>
    {
        public ManagedDiscussionPostTest()
        {

        }

        public override TransitDiscussionPost GetTransitInstance()
        {
            TransitDiscussionPost t_instance = new TransitDiscussionPost();
            return t_instance;
        }
    }
}

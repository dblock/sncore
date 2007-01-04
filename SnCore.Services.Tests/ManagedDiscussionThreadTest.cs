using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedDiscussionThreadTest : ManagedCRUDTest<DiscussionThread, TransitDiscussionThread, ManagedDiscussionThread>
    {
        public ManagedDiscussionThreadTest()
        {

        }

        public override TransitDiscussionThread GetTransitInstance()
        {
            TransitDiscussionThread t_instance = new TransitDiscussionThread();
            return t_instance;
        }
    }
}

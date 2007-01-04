using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedFeedTypeTest : ManagedCRUDTest<FeedType, TransitFeedType, ManagedFeedType>
    {
        public ManagedFeedTypeTest()
        {

        }

        public override TransitFeedType GetTransitInstance()
        {
            TransitFeedType t_instance = new TransitFeedType();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

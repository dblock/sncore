using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceQueueItemTest : ManagedCRUDTest<PlaceQueueItem, TransitPlaceQueueItem, ManagedPlaceQueueItem>
    {
        public ManagedPlaceQueueItemTest()
        {

        }

        public override TransitPlaceQueueItem GetTransitInstance()
        {
            TransitPlaceQueueItem t_instance = new TransitPlaceQueueItem();
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceQueueItemTest : ManagedCRUDTest<PlaceQueueItem, TransitPlaceQueueItem, ManagedPlaceQueueItem>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedPlaceQueueTest _queue = new ManagedPlaceQueueTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _place.SetUp();
            _queue.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _queue.TearDown();
            _place.TearDown();
            base.TearDown();
        }

        public ManagedPlaceQueueItemTest()
        {

        }

        public override TransitPlaceQueueItem GetTransitInstance()
        {
            TransitPlaceQueueItem t_instance = new TransitPlaceQueueItem();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.PlaceQueueId = _queue.Instance.Id;
            return t_instance;
        }
    }
}

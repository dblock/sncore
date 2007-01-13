using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceQueueItemTest : WebServiceTest<WebPlaceService.TransitPlaceQueueItem, WebPlaceServiceNoCache>
    {
        private PlaceQueueTest _queue = new PlaceQueueTest();
        private int _queue_id = 0;
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
            _queue_id = _queue.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _queue.Delete(GetAdminTicket(), _queue_id);
        }

        public PlaceQueueItemTest()
            : base("PlaceQueueItem")
        {

        }

        public override WebPlaceService.TransitPlaceQueueItem GetTransitInstance()
        {
            WebPlaceService.TransitPlaceQueueItem t_instance = new WebPlaceService.TransitPlaceQueueItem();
            t_instance.PlaceQueueId = _queue_id;
            t_instance.PlaceId = _place_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _queue_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _queue_id };
            return args;
        }

        [Test]
        protected void GetFriendsPlaceQueueItemsTest()
        {

        }
    }
}

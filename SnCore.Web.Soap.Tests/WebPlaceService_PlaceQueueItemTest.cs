using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebSocialServiceTests;

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
        public void GetFriendsPlaceQueueItemsTest()
        {
            AccountFriendTest friend = new AccountFriendTest();
            // make a friend
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = friend.EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            string ticket = Login(email, password);
            friend.EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, Guid.NewGuid().ToString());
            // create a queue for the friend
            WebPlaceService.TransitPlaceQueue t_queue = new WebPlaceService.TransitPlaceQueue();
            t_queue.Name = Guid.NewGuid().ToString();
            t_queue.AccountId = user_id;
            t_queue.PublishAll = t_queue.PublishFriends = true;
            t_queue.Id = EndPoint.CreateOrUpdatePlaceQueue(ticket, t_queue);
            // create a place
            PlaceTest place = new PlaceTest();
            place.SetUp();
            int place_id = place.Create(GetAdminTicket());
            // add a place to the queue
            WebPlaceService.TransitPlaceQueueItem t_placequeueitem = new WebPlaceService.TransitPlaceQueueItem();
            t_placequeueitem.PlaceId = place_id;
            t_placequeueitem.PlaceQueueId = t_queue.Id;
            t_placequeueitem.Id = EndPoint.CreateOrUpdatePlaceQueueItem(ticket, t_placequeueitem);
            // get the friends queue items
            int count = EndPoint.GetFriendsPlaceQueueItemsCount(GetAdminTicket(), GetAdminAccount().Id);
            Console.WriteLine("Count: {0}", count);
            Assert.IsTrue(count > 0);
            WebPlaceService.TransitFriendsPlaceQueueItem[] items = EndPoint.GetFriendsPlaceQueueItems(GetAdminTicket(), GetAdminAccount().Id, null);
            Console.WriteLine("Length: {0}", items.Length);
            Assert.AreEqual(count, items.Length);
            bool bFound = false;
            foreach (WebPlaceService.TransitFriendsPlaceQueueItem item in items)
            {
                if (item.Place.Id == place_id)
                {
                    bFound = true;
                    break;
                }                
            }
            Assert.IsTrue(bFound, "Didn't find the place in the friends queue.");
            place.Delete(GetAdminTicket(), place_id);
            place.TearDown();
            DeleteUser(user_id);
        }
    }
}

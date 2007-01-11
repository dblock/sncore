using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebSocialServiceTests
{
    [TestFixture]
    public class AccountFriendTest : WebServiceBaseTest<WebSocialServiceNoCache>
    {
        [Test]
        public void CreateOrUpdateAccountFriendRequestTest()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);

            {
                WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
                bool bFound = new TransitServiceCollection<WebSocialService.TransitAccountFriendRequest>(requests).ContainsId(friend_request_id);
                Assert.IsTrue(bFound);
            }

            {
                string ticket = Login(email, password);
                WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetAccountFriendRequests(ticket, user_id, null);
                bool bFound = new TransitServiceCollection<WebSocialService.TransitAccountFriendRequest>(requests).ContainsId(friend_request_id);
                Assert.IsTrue(bFound);
            }

            EndPoint.DeleteAccountFriendRequest(GetAdminTicket(), friend_request_id);
            DeleteUser(user_id);
        }

        [Test]
        public void AcceptAccountFriendRequestTest()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            string ticket = Login(email, password);
            EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, Guid.NewGuid().ToString());
            // TODO: check that these two are friends
            // check that the friend request is deleted
            WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
            foreach (WebSocialService.TransitAccountFriendRequest request in requests)
                Assert.IsFalse(request.Id == friend_request_id);
            DeleteUser(user_id);
        }

        [Test]
        public void RejectAccountFriendRequestTest()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            string ticket = Login(email, password);
            EndPoint.RejectAccountFriendRequest(ticket, friend_request_id, Guid.NewGuid().ToString());
            // TODO: check that these two are NOT friends
            // check that the friend request is deleted
            WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
            foreach (WebSocialService.TransitAccountFriendRequest request in requests)
                Assert.IsFalse(request.Id == friend_request_id);
            DeleteUser(user_id);
        }

        [Test]
        public void TestSentAccountFriendRequests()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            int count = EndPoint.GetSentAccountFriendRequestsCount(GetAdminTicket(), GetAdminAccount().Id);
            Console.WriteLine("Request count: {0}", count);
            Assert.IsTrue(count > 0);
            WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
            bool bFound = new TransitServiceCollection<WebSocialService.TransitAccountFriendRequest>(requests).ContainsId(friend_request_id);
            Assert.IsTrue(bFound);
            DeleteUser(user_id);
        }

        [Test]
        public void GetAccountFriendsTest()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = "password";
            int user_id = CreateUser(email, password);
            string ticket = Login(email, password);
            // the user has no friends
            int count = EndPoint.GetAccountFriendsCount(ticket, user_id);
            Assert.AreEqual(0, count, "New user has friends he shouldn't have.");
            WebSocialService.TransitAccountFriend[] friends = EndPoint.GetAccountFriends(ticket, user_id, null);
            Assert.AreEqual(0, friends.Length, "New user has friends he shouldn't have.");
            // admin makes a friends request
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, Guid.NewGuid().ToString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, Guid.NewGuid().ToString());
            // the admin user is now part of friends
            count = EndPoint.GetAccountFriendsCount(ticket, user_id);
            Assert.AreEqual(1, count, "New user has no friends after accepting a request.");
            friends = EndPoint.GetAccountFriends(ticket, user_id, null);
            Assert.AreEqual(1, friends.Length, "New user has no friends after accepting a request.");
            bool bFound = new TransitServiceCollection<WebSocialService.TransitAccountFriend>(friends).ContainsId(GetAdminAccount().Id, "FriendId");
            Assert.IsTrue(bFound, "New user doesn't have admin among his friends.");
            // user removes admin from friends
            EndPoint.DeleteAccountFriend(ticket, friends[0].Id);
            count = EndPoint.GetAccountFriendsCount(ticket, user_id);
            Assert.AreEqual(0, count, "New user has friends after removing his last friend.");
            friends = EndPoint.GetAccountFriends(ticket, user_id, null);
            Assert.AreEqual(0, friends.Length, "New user has friends after removing his last friend.");
            // delete the user
            DeleteUser(user_id);
        }
    }
}
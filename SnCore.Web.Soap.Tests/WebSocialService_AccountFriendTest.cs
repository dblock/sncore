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
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
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
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            Console.WriteLine("Email: {0}", email);
            // verify the user e-mail
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            WebAccountService.TransitAccountEmailConfirmation[] confirmations = account_endpoint.GetAccountEmailConfirmations(GetAdminTicket(), user_id, null);
            string verifiedemail = account_endpoint.VerifyAccountEmail(confirmations[0].Id, confirmations[0].Code);
            Console.WriteLine("Verified: {0}", verifiedemail);
            // admin is not a friend with the new user
            WebSocialService.TransitAccountFriend[] friends_before = EndPoint.GetAccountFriends(GetAdminTicket(), GetAdminAccount().Id, null);
            Assert.IsFalse(new TransitServiceCollection<WebSocialService.TransitAccountFriend>(friends_before).ContainsId(user_id, "FriendId"));
            // admin requests to be friends with the new user
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            // there's an e-mail to the user that admin has requested to be friends with
            WebAccountService.TransitAccountEmailMessage[] messages_request = account_endpoint.GetAccountEmailMessages(GetAdminTicket(), null);
            Assert.IsTrue(messages_request.Length > 0);
            WebAccountService.TransitAccountEmailMessage message_request = messages_request[messages_request.Length - 1];
            Console.WriteLine("Email: {0} to {1}", message_request.Subject, message_request.MailTo);
            Assert.IsTrue(message_request.Subject.Contains("wants to be your friend"));
            Assert.IsTrue(message_request.MailTo.Contains(email));
            // accept the friends request
            string ticket = Login(email, password);
            EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, GetNewString());
            // check that these two are friends
            WebSocialService.TransitAccountFriend[] friends_after = EndPoint.GetAccountFriends(GetAdminTicket(), GetAdminAccount().Id, null);
            Assert.IsTrue(new TransitServiceCollection<WebSocialService.TransitAccountFriend>(friends_after).ContainsId(user_id, "FriendId"));
            Assert.AreEqual(friends_before.Length + 1, friends_after.Length);
            // there's an e-mail to the administrator confirming that the user accepted the friends request
            WebAccountService.TransitAccountEmailMessage[] messages_accepted = account_endpoint.GetAccountEmailMessages(GetAdminTicket(), null);
            Assert.IsTrue(messages_accepted.Length > 0);
            WebAccountService.TransitAccountEmailMessage message_accepted = messages_accepted[messages_accepted.Length - 1];
            Console.WriteLine("Email: {0} to {1}", message_accepted.Subject, message_accepted.MailTo);
            Assert.IsTrue(message_accepted.Subject.Contains("accepted your request"));
            Assert.IsFalse(message_accepted.MailTo.Contains(email));
            // check that the friend request is deleted
            WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
            Assert.IsFalse(new TransitServiceCollection<WebSocialService.TransitAccountFriendRequest>(requests).ContainsId(friend_request_id));
            DeleteUser(user_id);
        }

        [Test]
        public void RejectAccountFriendRequestTest()
        {
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            string ticket = Login(email, password);
            EndPoint.RejectAccountFriendRequest(ticket, friend_request_id, GetNewString());
            // TODO: check that these two are NOT friends
            // check that the friend request is deleted
            WebSocialService.TransitAccountFriendRequest[] requests = EndPoint.GetSentAccountFriendRequests(GetAdminTicket(), GetAdminAccount().Id, null);
            foreach (WebSocialService.TransitAccountFriendRequest request in requests)
                Assert.IsFalse(request.Id == friend_request_id);
            DeleteUser(user_id);
        }

        [Test]
        public void SentAccountFriendRequestsTest()
        {
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
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
            string email = GetNewEmailAddress();
            string password = "password";
            int user_id = CreateUser(email, password);
            string ticket = Login(email, password);
            // the user has no friends
            int count = EndPoint.GetAccountFriendsCount(ticket, user_id);
            Assert.AreEqual(0, count, "New user has friends he shouldn't have.");
            WebSocialService.TransitAccountFriend[] friends = EndPoint.GetAccountFriends(ticket, user_id, null);
            Assert.AreEqual(0, friends.Length, "New user has friends he shouldn't have.");
            // admin makes a friends request
            int friend_request_id = EndPoint.CreateOrUpdateAccountFriendRequest(GetAdminTicket(), user_id, GetNewString());
            Console.WriteLine("Created friend request: {0}", friend_request_id);
            EndPoint.AcceptAccountFriendRequest(ticket, friend_request_id, GetNewString());
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
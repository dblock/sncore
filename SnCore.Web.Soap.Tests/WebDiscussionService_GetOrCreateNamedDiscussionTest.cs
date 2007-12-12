using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;
using SnCore.Web.Soap.Tests.WebBlogServiceTests;
using SnCore.Web.Soap.Tests.WebEventServiceTests;
using SnCore.Web.Soap.Tests.WebPlaceServiceTests;
using SnCore.Web.Soap.Tests.WebMadLibServiceTests;
using SnCore.Web.Soap.Tests.WebStoryServiceTests;
using SnCore.Web.Soap.Tests.WebSyndicationServiceTests;
using SnCore.Web.Soap.Tests.WebAccountService;
using SnCore.Web.Soap.Tests.WebGroupServiceTests;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebDiscussionServiceTests
{
    [TestFixture]
    public class GetOrCreateNamedDiscussionTest : WebServiceBaseTest<WebDiscussionServiceNoCache>
    {
        public delegate int CreateInstanceDelegate(string ticket);

        protected void GetOrCreateDiscussionIdTest<TransitType, WebServiceType>(
            string typename,
            WebServiceTest<TransitType, WebServiceType> objecttest)
            where WebServiceType : new()
        {
            UserInfo user = CreateUserWithVerifiedEmailAddress();

            // create a discussion

            Console.WriteLine("Type: {0}", typename);

            objecttest.SetUp();
            int objecttest_id = objecttest.Create(user.ticket);
            Assert.IsTrue(objecttest_id > 0);

            int discussion_id = EndPoint.GetOrCreateDiscussionId(user.ticket, typename, objecttest_id);
            Assert.IsTrue(discussion_id > 0);

            WebDiscussionService.TransitDiscussion t_discussion = EndPoint.GetDiscussionById(
                user.ticket, discussion_id);

            Console.WriteLine("Discussion: {0}", t_discussion.Name);

            string redirecturi = EndPoint.GetDiscussionRedirectUri(user.ticket, discussion_id);
            Console.WriteLine("Uri: {0}", redirecturi);

            Assert.AreEqual(t_discussion.ParentObjectUri, redirecturi);
            Assert.AreEqual(t_discussion.ParentObjectType, typename);

            Console.WriteLine("Parent name: {0}", t_discussion.ParentObjectName);

            // post to a discussion as a regular user
            WebDiscussionService.TransitDiscussionPost t_post = new WebDiscussionService.TransitDiscussionPost();
            t_post.DiscussionId = discussion_id;
            t_post.AccountId = user.id;
            t_post.Body = GetNewString();
            t_post.Subject = GetNewString();
            t_post.Id = EndPoint.CreateOrUpdateDiscussionPost(user.ticket, t_post);

            objecttest.Delete(user.ticket, objecttest_id);
            objecttest.TearDown();

            t_discussion = EndPoint.GetDiscussionById(user.ticket, discussion_id);
            Assert.IsNull(t_discussion, "Discussion has not been deleted with object.");
            DeleteUser(user.id);
        }

        [Test]
        public void GetOrCreateAccountBlogPostDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountBlogPost", new AccountBlogPostTest());
        }

        [Test]
        public void GetOrCreateAccountEventDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountEvent", new AccountEventTest());
        }

        [Test]
        public void GetOrCreateAccountGroupDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountGroup", new AccountGroupTest());
        }

        [Test]
        public void GetOrCreateAccountEventPictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountEventPicture", new AccountEventPictureTest());
        }

        [Test]
        public void GetOrCreateAccountFeedItemDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountFeedItem", new AccountFeedItemTest());
        }

        [Test]
        public void GetOrCreateAccountPictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountPicture", new AccountPictureTest());
        }

        [Test]
        public void GetOrCreateAccountStoryDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountStory", new AccountStoryTest());
        }

        [Test]
        public void GetOrCreateAccountStoryPictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("AccountStoryPicture", new AccountStoryPictureTest());
        }

        [Test]
        public void GetOrCreateAccountTagsDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("Account", new AccountTest());
        }

        [Test]
        public void GetOrCreatePlaceDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("Place", new PlaceTest());
        }

        [Test]
        public void GetOrCreatePlacePictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest("PlacePicture", new PlacePictureTest());
        }

        [Test]
        public void GetDiscussionByObjectIdTest()
        {
            // create a group
            AccountGroupTest group = new AccountGroupTest();
            group.SetUp();
            int group_id = group.Create(GetAdminTicket());
            Assert.IsTrue(group_id > 0);
            Console.WriteLine("Group: {0}", group_id);
            // the group should have no pre-built discussions
            int count = EndPoint.GetDiscussionsByObjectIdCount(GetAdminTicket(), "AccountGroup", group_id);
            Console.WriteLine("Group discussions: {0}", count);
            Assert.AreEqual(0, count);
            // create the default group discussion
            int discussion_id = EndPoint.GetOrCreateDiscussionId(GetAdminTicket(), "AccountGroup", group_id);
            Console.WriteLine("Discussion: {0}", discussion_id);
            int discussion2_id = EndPoint.GetOrCreateDiscussionId(GetAdminTicket(), "AccountGroup", group_id);
            Assert.AreEqual(discussion_id, discussion2_id);
            // get count, now should be 1
            int count2 = EndPoint.GetDiscussionsByObjectIdCount(GetAdminTicket(), "AccountGroup", group_id);
            Console.WriteLine("Group discussions: {0}", count2);
            Assert.AreEqual(1, count2);
            // TODO
            // ...
            // delete the group
            group.Delete(GetAdminTicket(), group_id);
            group.TearDown();
        }
    }
}

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
            Console.WriteLine("Type: {0}", typename);

            objecttest.SetUp();
            int objecttest_id = objecttest.Create(GetAdminTicket());
            Assert.IsTrue(objecttest_id > 0);

            int discussion_id = EndPoint.GetOrCreateDiscussionId(GetAdminTicket(), typename, objecttest_id);
            Assert.IsTrue(discussion_id > 0);

            WebDiscussionService.TransitDiscussion t_discussion = EndPoint.GetDiscussionById(
                GetAdminTicket(), discussion_id);

            Console.WriteLine("Discussion: {0}", t_discussion.Name);

            string redirecturi = EndPoint.GetDiscussionRedirectUri(GetAdminTicket(), discussion_id);
            Console.WriteLine("Uri: {0}", redirecturi);

            Assert.AreEqual(t_discussion.ParentObjectUri, redirecturi);
            Assert.AreEqual(t_discussion.ParentObjectType, typename);

            Console.WriteLine("Parent name: {0}", t_discussion.ParentObjectName);

            objecttest.Delete(GetAdminTicket(), objecttest_id);
            objecttest.TearDown();

            t_discussion = EndPoint.GetDiscussionById(
                GetAdminTicket(), discussion_id);

            Assert.IsNull(t_discussion, "Discussion has not been deleted with object.");
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
    }
}

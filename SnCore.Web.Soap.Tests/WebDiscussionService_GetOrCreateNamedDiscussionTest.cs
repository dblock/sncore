using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
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
        public delegate string GetDiscussionNameDelegate();
        public delegate int GetOrCreateDiscussionIdDelegate(string ticket, int id);
        public delegate int CreateInstanceDelegate(string ticket);

        protected void GetOrCreateDiscussionIdTest<TransitType, WebServiceType>(
            GetDiscussionNameDelegate GetDiscussionName,
            GetOrCreateDiscussionIdDelegate GetOrCreateDiscussionId,
            WebServiceTest<TransitType, WebServiceType> objecttest)
            where WebServiceType : new()
        {
            string name = GetDiscussionName();
            Assert.IsNotEmpty(name);
            Console.WriteLine("Name: {0}", name);

            objecttest.SetUp();
            int objecttest_id = objecttest.Create(GetAdminTicket());
            Assert.IsTrue(objecttest_id > 0);

            int discussion_id = GetOrCreateDiscussionId(GetAdminTicket(), objecttest_id);
            Assert.IsTrue(discussion_id > 0);

            WebDiscussionService.TransitDiscussion t_discussion = EndPoint.GetDiscussionById(
                GetAdminTicket(), discussion_id);

            Assert.AreEqual(t_discussion.Name, name);
            Console.WriteLine("Discussion: {0}", t_discussion.Name);

            objecttest.Delete(GetAdminTicket(), objecttest_id);
            objecttest.TearDown();

            t_discussion = EndPoint.GetDiscussionById(
                GetAdminTicket(), discussion_id);

            Assert.IsNull(t_discussion, "Discussion has not been deleted with object.");
        }

        [Test]
        public void GetOrCreateAccountBlogPostDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountBlogPostDiscussionName,
                 EndPoint.GetOrCreateAccountBlogPostDiscussionId,
                 new AccountBlogPostTest());
        }

        [Test]
        public void GetOrCreateAccountEventDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountEventDiscussionName,
                 EndPoint.GetOrCreateAccountEventDiscussionId,
                 new AccountEventTest());
        }

        [Test]
        public void GetOrCreateAccountEventPictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountEventPictureDiscussionName,
                 EndPoint.GetOrCreateAccountEventPictureDiscussionId,
                 new AccountEventPictureTest());
        }

        [Test]
        public void GetOrCreateAccountFeedItemDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountFeedItemDiscussionName,
                 EndPoint.GetOrCreateAccountFeedItemDiscussionId,
                 new AccountFeedItemTest());
        }

        [Test]
        protected void GetOrCreateAccountPictureDiscussionIdTest()
        {
            // EndPoint.GetOrCreateAccountPictureDiscussionId();
        }

        [Test]
        public void GetOrCreateAccountStoryDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountStoryDiscussionName,
                 EndPoint.GetOrCreateAccountStoryDiscussionId,
                 new AccountStoryTest());
        }

        [Test]
        public void GetOrCreateAccountStoryPictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetAccountStoryPictureDiscussionName,
                 EndPoint.GetOrCreateAccountStoryPictureDiscussionId,
                 new AccountStoryPictureTest());
        }

        [Test]
        protected void GetOrCreateAccountTagsDiscussionIdTest()
        {
            // EndPoint.GetOrCreateAccountTagsDiscussionId();
        }

        [Test]
        public void GetOrCreatePlaceDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetPlaceDiscussionName,
                 EndPoint.GetOrCreatePlaceDiscussionId,
                 new PlaceTest());
        }

        [Test]
        public void GetOrCreatePlacePictureDiscussionIdTest()
        {
            GetOrCreateDiscussionIdTest(
                 EndPoint.GetPlacePictureDiscussionName,
                 EndPoint.GetOrCreatePlacePictureDiscussionId,
                 new PlacePictureTest());
        }
    }
}

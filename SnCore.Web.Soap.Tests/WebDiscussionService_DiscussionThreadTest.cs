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
    public class DiscusisonThreadTest : WebServiceBaseTest<WebDiscussionServiceNoCache>
    {
        [Test]
        public void GetDiscussionThreadsTest()
        {
            int count = EndPoint.GetDiscussionThreadsCount(GetAdminTicket());
            Console.WriteLine("Count: {0}", count);
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetDiscussionThreads(GetAdminTicket(), null);
            Console.WriteLine("Length: {0}", posts.Length);
            Assert.AreEqual(posts.Length, count);
            for (int i = 0; i < posts.Length - 1; i++)
            {
                for (int j = i + 1; j < posts.Length; j++)
                {
                    Assert.AreNotEqual(posts[i].DiscussionThreadId, posts[j].DiscussionThreadId);
                    Assert.AreNotEqual(posts[i].Id, posts[j].Id);
                }
            }
        }

        [Test]
        public void GetDiscussionTopOfThreadsTest()
        {
            int count = EndPoint.GetDiscussionTopOfThreadsCount(GetAdminTicket());
            Console.WriteLine("Count: {0}", count);
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetDiscussionTopOfThreads(GetAdminTicket(), null);
            Console.WriteLine("Length: {0}", posts.Length);
            Assert.AreEqual(posts.Length, count);
            foreach (WebDiscussionService.TransitDiscussionPost post in posts)
            {
                Assert.AreEqual(post.DiscussionPostParentId, 0);
            }
        }

        [Test]
        protected void GetDiscussionThreadsByDiscussionIdTest()
        {

        }

        [Test]
        protected void GetDiscussionThreadPostTest()
        {

        }

        [Test]
        public void GetUserDiscussionThreadsTest()
        {
            WebDiscussionService.DiscussionQueryOptions qopt = new WebDiscussionService.DiscussionQueryOptions();
            qopt.AccountId = GetAdminAccount().Id;
            int count = EndPoint.GetUserDiscussionThreadsCount(GetAdminTicket(), qopt);
            Console.WriteLine("Count: {0}", count);
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetUserDiscussionThreads(GetAdminTicket(), qopt, null);
            Console.WriteLine("Length: {0}", posts.Length);
            Assert.AreEqual(posts.Length, count);
        }

        [Test]
        protected void MoveDiscussionThreadTest()
        {

        }

        [Test]
        protected void GetDiscussionThreadByIdTest()
        {

        }
    }
}

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
        public void GetDiscussionThreadsByDiscussionIdTest()
        {
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            int post_id = post.Create(GetAdminTicket());
            int count = EndPoint.GetDiscussionThreadsCountByDiscussionId(GetUserTicket(), post._discussion_id);
            Console.WriteLine("Count: {0}", count);
            Assert.AreEqual(count, 1);
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetDiscussionThreadsByDiscussionId(GetUserTicket(), post._discussion_id, null);
            Console.WriteLine("Length: {0}", posts.Length);
            Assert.AreEqual(count, posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(post_id));
            post.Delete(GetAdminTicket(), post_id);
            post.TearDown();
        }

        [Test]
        public void GetDiscussionThreadPostTest()
        {
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            int post_id = post.Create(GetAdminTicket());
            WebDiscussionService.TransitDiscussionPost t_post = EndPoint.GetDiscussionPostById(GetAdminTicket(), post_id);
            Console.WriteLine("Thread Id: {0}", t_post.DiscussionThreadId);
            WebDiscussionService.TransitDiscussionPost t_threadpost = EndPoint.GetDiscussionThreadPost(GetAdminTicket(), t_post.DiscussionThreadId);
            Assert.AreEqual(t_post.Id, t_threadpost.Id);
            post.Delete(GetAdminTicket(), post_id);
            post.TearDown();
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
        public void MoveDiscussionThreadTest()
        {
            DiscussionTest discussion = new DiscussionTest();
            int discussion_id = discussion.Create(GetAdminTicket());
            discussion.SetUp();
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            int post_id = post.Create(GetAdminTicket());
            WebDiscussionService.TransitDiscussionPost t_post = EndPoint.GetDiscussionPostById(GetAdminTicket(), post_id);
            Console.WriteLine("Thread Id: {0}", t_post.DiscussionThreadId);
            EndPoint.MoveDiscussionThread(GetAdminTicket(), t_post.DiscussionThreadId, discussion_id);
            WebDiscussionService.TransitDiscussionThread t_thread = EndPoint.GetDiscussionThreadById(GetAdminTicket(), t_post.DiscussionThreadId);
            Assert.AreEqual(t_thread.DiscussionId, discussion_id);
            post.Delete(GetAdminTicket(), post_id);
            post.TearDown();
            discussion.Delete(GetAdminTicket(), discussion_id);
            discussion.TearDown();
        }

        [Test]
        public void GetDiscussionThreadByIdTest()
        {
            DiscussionPostTest post = new DiscussionPostTest();
            post.SetUp();
            int post_id = post.Create(GetAdminTicket());
            WebDiscussionService.TransitDiscussionPost t_post = EndPoint.GetDiscussionPostById(GetAdminTicket(), post_id);
            Console.WriteLine("Thread Id: {0}", t_post.DiscussionThreadId);
            WebDiscussionService.TransitDiscussionThread t_thread = EndPoint.GetDiscussionThreadById(GetAdminTicket(), t_post.DiscussionThreadId);
            Assert.AreEqual(t_post.DiscussionThreadId, t_thread.Id);
            post.Delete(GetAdminTicket(), post_id);
            post.TearDown();
        }

        [Test]
        protected void GetDiscussionThreadPostsByOrderTest()
        {

        }

        [Test]
        protected void GetDiscussionThreadPostsTest()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

namespace SnCore.Web.Soap.Tests.WebDiscussionServiceTests
{
    [TestFixture]
    public class DiscussionPostTest : WebServiceTest<WebDiscussionService.TransitDiscussionPost, WebDiscussionServiceNoCache>
    {
        public DiscussionTest _discussion = new DiscussionTest();
        public int _discussion_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _discussion.SetUp();
            _discussion_id = _discussion.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _discussion.Delete(GetAdminTicket(), _discussion_id);
            _discussion.TearDown();
        }

        public DiscussionPostTest()
            : base("DiscussionPost")
        {

        }

        public override WebDiscussionService.TransitDiscussionPost GetTransitInstance()
        {
            WebDiscussionService.TransitDiscussionPost t_instance = new WebDiscussionService.TransitDiscussionPost();
            t_instance.Body = Guid.NewGuid().ToString();
            t_instance.DiscussionId = _discussion_id;
            t_instance.Subject = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _discussion_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _discussion_id };
            return args;
        }

        [Test]
        public void GetLatestDiscussionPostsTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetLatestDiscussionPosts(GetUserTicket(), null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void GetLatestDiscussionPostsByIdTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.GetLatestDiscussionPostsById(
                GetUserTicket(), t_post.DiscussionId, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void SearchDiscussionPostsTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            Thread.Sleep(2000); // wait for reindex
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.SearchDiscussionPosts(
                GetUserTicket(), t_post.Subject, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void SearchDiscussionPostsByIdTest()
        {
            WebDiscussionService.TransitDiscussionPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            Thread.Sleep(2000); // wait for reindex
            WebDiscussionService.ServiceQueryOptions options = new WebDiscussionService.ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            WebDiscussionService.TransitDiscussionPost[] posts = EndPoint.SearchDiscussionPostsById(
                GetUserTicket(), t_post.DiscussionId, t_post.Subject, null);
            Assert.IsTrue(posts.Length > 0);
            Console.WriteLine("Posts: {0}", posts.Length);
            Assert.IsTrue(new TransitServiceCollection<WebDiscussionService.TransitDiscussionPost>(posts).ContainsId(t_post.Id));
            Delete(GetAdminTicket(), t_post.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;
using SnCore.Web.Soap.Tests.WebDiscussionService;

namespace SnCore.Web.Soap.Tests.WebBlogServiceTests
{
    [TestFixture]
    public class AccountBlogPostTest : WebServiceTest<WebBlogService.TransitAccountBlogPost, WebBlogServiceNoCache>
    {
        private AccountBlogTest _blog = new AccountBlogTest();
        private int _blog_id = 0;
        private UserInfo _user = null;

        public AccountBlogPostTest()
            : base("AccountBlogPost")
        {

        }

        [SetUp]
        public override void SetUp()
        {
            _blog_id = _blog.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
            _blog.Delete(GetAdminTicket(), _blog_id);
        }

        public override WebBlogService.TransitAccountBlogPost GetTransitInstance()
        {
            WebBlogService.TransitAccountBlogPost t_instance = new WebBlogService.TransitAccountBlogPost();
            t_instance.AccountBlogId = _blog_id;
            t_instance.AccountId = _user.id;
            t_instance.Body = GetNewString();
            t_instance.Title = GetNewString();
            t_instance.EnableComments = true;
            t_instance.Sticky = false;
            t_instance.Publish = true;
            return t_instance;
        }

        private WebBlogService.TransitAccountBlogPostQueryOptions GetBlogPostQueryOptions()
        {
            WebBlogService.TransitAccountBlogPostQueryOptions qopt = new WebBlogService.TransitAccountBlogPostQueryOptions();
            qopt.PublishedOnly = false;
            qopt.BlogId = _blog_id;
            return qopt;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetBlogPostQueryOptions(), options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetBlogPostQueryOptions() };
            return args;
        }

        [Test]
        public void SearchAccountBlogPostsEmptyTest()
        {
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.SearchAccountBlogPosts(
                GetAdminTicket(), string.Empty, null);
            Assert.AreEqual(0, posts.Length);
        }

        [Test]
        protected void SearchAccountBlogPostsTest()
        {
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            DatabaseTestInstance.UpdateSearchIndex("AccountBlogPost");
            int count = EndPoint.SearchAccountBlogPostsCount(GetAdminTicket(), t_post.Title);
            Console.WriteLine("Found {0} posts.", count);
            Assert.IsTrue(count > 0);
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.SearchAccountBlogPosts(GetAdminTicket(), t_post.Title, null);
            Assert.IsTrue(posts.Length > 0);
            bool bFound = new TransitServiceCollection<WebBlogService.TransitAccountBlogPost>(posts).ContainsId(t_post.Id);
            Assert.IsTrue(bFound);
            // unpublish the post
            t_post.Publish = false;
            EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            // search, should not be found
            WebBlogService.TransitAccountBlogPost[] posts_after = EndPoint.SearchAccountBlogPosts(GetAdminTicket(), t_post.Title, null);
            Assert.AreEqual(posts.Length, posts_after.Length + 1);
            bFound = new TransitServiceCollection<WebBlogService.TransitAccountBlogPost>(posts_after).ContainsId(t_post.Id);
            Assert.IsFalse(bFound);
            // delete everything
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void EnableDisableCommentsTest()
        {
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            int post_id = Create(GetAdminTicket(), t_post);
            Console.WriteLine("Post: {0}", post_id);
            // post a comment
            WebDiscussionService.WebDiscussionService DiscussionEndpoint = new SnCore.Web.Soap.Tests.WebDiscussionService.WebDiscussionService();
            int discussion_id = DiscussionEndpoint.GetOrCreateDiscussionId(GetAdminTicket(), "AccountBlogPost", post_id);
            Console.WriteLine("Discussion: {0}", discussion_id);
            WebDiscussionService.TransitDiscussionPost t_discussion_post_1 = new WebDiscussionService.TransitDiscussionPost();
            t_discussion_post_1.Body = GetNewString();
            t_discussion_post_1.DiscussionId = discussion_id;
            t_discussion_post_1.Subject = GetNewString();
            t_discussion_post_1.Id = DiscussionEndpoint.CreateOrUpdateDiscussionPost(_user.ticket, t_discussion_post_1);
            Console.WriteLine("Post: {0}", t_discussion_post_1.Id);
            // disable comments on the blog
            WebBlogService.TransitAccountBlog t_blog = EndPoint.GetAccountBlogById(GetAdminTicket(), _blog_id);
            t_blog.EnableComments = false;
            EndPoint.CreateOrUpdateAccountBlog(GetAdminTicket(), t_blog);
            // try to post again
            try
            {
                WebDiscussionService.TransitDiscussionPost t_discussion_post_2 = new WebDiscussionService.TransitDiscussionPost();
                t_discussion_post_2.Body = GetNewString();
                t_discussion_post_2.DiscussionId = discussion_id;
                t_discussion_post_2.Subject = GetNewString();
                t_discussion_post_2.Id = DiscussionEndpoint.CreateOrUpdateDiscussionPost(_user.ticket, t_discussion_post_2);
                Console.WriteLine("Post: {0}", t_discussion_post_2.Id);
                Assert.IsTrue(false, "Expected an access denied.");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Expected exception: {0}", ex.Message);
                Assert.IsTrue(ex.Message.StartsWith("System.Web.Services.Protocols.SoapException: Server was unable to process request. ---> SnCore.Services.ManagedAccount+AccessDeniedException: Access denied"));
            }
            Delete(GetAdminTicket(), post_id);
        }

        [Test]
        public void MoveAccountBlogPostTest()
        {
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            t_post.Id = Create(GetAdminTicket(), t_post);
            Console.WriteLine("Post: {0} in blog {1}", t_post.Id, _blog_id);
            // create another blog
            AccountBlogTest blog2 = new AccountBlogTest();
            blog2.SetUp();
            int blog2_id = blog2.Create(GetAdminTicket());
            // move the post to blog2
            EndPoint.MoveAccountBlogPost(GetAdminTicket(), t_post.Id, blog2_id);
            // check that the id was updated
            WebBlogService.TransitAccountBlogPost t_post2 = EndPoint.GetAccountBlogPostById(
                GetAdminTicket(), t_post.Id);
            Console.WriteLine("Post: {0} in blog {1}", t_post2.Id, t_post2.AccountBlogId);
            Assert.AreNotEqual(t_post.AccountBlogId, t_post2.AccountBlogId);
            Assert.AreEqual(t_post.Id, t_post2.Id);
            blog2.TearDown();
            Delete(GetAdminTicket(), t_post.Id);
        }

        [Test]
        public void MoveDiscussionPostToAccountBlogTest()
        {
            WebDiscussionServiceTests.DiscussionPostTest _post = new WebDiscussionServiceTests.DiscussionPostTest();
            _post.SetUp();
            int post_id = _post.Create(GetAdminTicket());
            Console.WriteLine("Post: {0}", post_id);
            // query options
            WebBlogService.TransitAccountBlogPostQueryOptions qopt = new WebBlogService.TransitAccountBlogPostQueryOptions();
            qopt.BlogId = _blog_id;
            qopt.PublishedOnly = false;
            // make sure there're no posts in the blog
            int blog_posts_count = EndPoint.GetAccountBlogPostsCount(GetAdminTicket(), qopt);
            Assert.AreEqual(0, blog_posts_count);
            // move the discussion post
            int moved_post_id = EndPoint.MoveDiscussionPost(GetAdminTicket(), post_id, _blog_id);
            Console.WriteLine("Moved Post: {0}", moved_post_id);
            Assert.AreNotEqual(0, moved_post_id);
            int blog_posts_count2 = EndPoint.GetAccountBlogPostsCount(GetAdminTicket(), qopt);
            Assert.AreEqual(1, blog_posts_count2);
            _post.TearDown();
        }

        [Test]
        public void AccountBlogPostStickynessTest()
        {
            // check that the system admin can create and flip the stickyness of the post
            // create a first post
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            // create a second post
            System.Threading.Thread.Sleep(1000);
            WebBlogService.TransitAccountBlogPost t_post2 = GetTransitInstance();
            t_post2.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post2);
            Console.WriteLine("Post: {0}", t_post2.Id);
            // query options
            WebBlogService.TransitAccountBlogPostQueryOptions qopt = new WebBlogService.TransitAccountBlogPostQueryOptions();
            qopt.BlogId = _blog_id;
            qopt.PublishedOnly = false;
            // make sure these are in order
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.GetAccountBlogPosts(GetAdminTicket(), qopt, null);
            Assert.AreEqual(2, posts.Length);
            Assert.AreEqual(t_post.Id, posts[1].Id); // newest post first
            Assert.AreEqual(t_post2.Id, posts[0].Id);
            // flip stickiness of the first post
            WebBlogService.TransitAccountBlogPost t_post_copy = EndPoint.GetAccountBlogPostById(
                GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            t_post.Sticky = true;
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetAccountBlogPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(true, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            // get posts, make sure the last one stuck
            WebBlogService.TransitAccountBlogPost[] posts_after = EndPoint.GetAccountBlogPosts(
                GetAdminTicket(), qopt, null);
            Assert.AreEqual(2, posts_after.Length);
            Assert.AreEqual(t_post2.Id, posts_after[1].Id); // newest post first
            Assert.AreEqual(t_post.Id, posts_after[0].Id);
            // reset stickyness
            t_post.Sticky = false;
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetAccountBlogPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Sticky);
            Assert.AreEqual(t_post.Sticky, t_post_copy.Sticky);
            // recheck after stickyness
            WebBlogService.TransitAccountBlogPost[] posts_final = EndPoint.GetAccountBlogPosts(
                GetAdminTicket(), qopt, null);
            Assert.AreEqual(2, posts_final.Length);
            Assert.AreEqual(t_post.Id, posts_final[1].Id); // newest post first
            Assert.AreEqual(t_post2.Id, posts_final[0].Id);
            // delete everything
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post.Id);
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post2.Id);
        }

        [Test]
        public void AccountBlogPostPublishTest()
        {
            // create a first post
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            Console.WriteLine("Post: {0}", t_post.Id);
            // create a second post
            System.Threading.Thread.Sleep(1000);
            WebBlogService.TransitAccountBlogPost t_post2 = GetTransitInstance();
            t_post2.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post2);
            Console.WriteLine("Post: {0}", t_post2.Id);
            // query options
            WebBlogService.TransitAccountBlogPostQueryOptions qopt = new WebBlogService.TransitAccountBlogPostQueryOptions();
            qopt.BlogId = _blog_id;
            qopt.PublishedOnly = true;
            // make sure these are in order
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.GetAccountBlogPosts(GetAdminTicket(), qopt, null);
            Assert.AreEqual(2, posts.Length);
            // flip publish of the first post
            WebBlogService.TransitAccountBlogPost t_post_copy = EndPoint.GetAccountBlogPostById(
                GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(true, t_post_copy.Publish);
            Assert.AreEqual(t_post.Publish, t_post_copy.Publish);
            t_post.Publish = false;
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetAccountBlogPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(false, t_post_copy.Publish);
            Assert.AreEqual(t_post.Publish, t_post_copy.Publish);
            // get posts, it should be one less
            WebBlogService.TransitAccountBlogPost[] posts_after = EndPoint.GetAccountBlogPosts(
                GetAdminTicket(), qopt, null);
            Assert.AreEqual(1, posts_after.Length);
            Assert.AreEqual(t_post2.Id, posts_after[0].Id); // newest post first
            // reset publish
            t_post.Publish = true;
            t_post.Id = EndPoint.CreateOrUpdateAccountBlogPost(GetAdminTicket(), t_post);
            t_post_copy = EndPoint.GetAccountBlogPostById(GetAdminTicket(), t_post.Id);
            Assert.AreEqual(t_post.Id, t_post_copy.Id);
            Assert.AreEqual(true, t_post_copy.Publish);
            Assert.AreEqual(t_post.Publish, t_post_copy.Publish);
            // recheck after change
            WebBlogService.TransitAccountBlogPost[] posts_final = EndPoint.GetAccountBlogPosts(
                GetAdminTicket(), qopt, null);
            Assert.AreEqual(2, posts_final.Length);
            Assert.AreEqual(t_post2.Id, posts_final[0].Id); // newest post first
            Assert.AreEqual(t_post.Id, posts_final[1].Id);
            // delete everything
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post.Id);
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post2.Id);
        }
    }
}

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

        public AccountBlogPostTest()
            : base("AccountBlogPost")
        {

        }

        [SetUp]
        public override void SetUp()
        {
            _blog_id = _blog.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _blog.Delete(GetAdminTicket(), _blog_id);
        }

        public override WebBlogService.TransitAccountBlogPost GetTransitInstance()
        {
            WebBlogService.TransitAccountBlogPost t_instance = new WebBlogService.TransitAccountBlogPost();
            t_instance.AccountBlogId = _blog_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Body = GetNewString();
            t_instance.Title = GetNewString();
            t_instance.EnableComments = true;
            t_instance.Sticky = false;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _blog_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _blog_id };
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
        public void SearchAccountBlogPostsTest()
        {
            WebBlogService.TransitAccountBlogPost t_post = GetTransitInstance();
            int post_id = Create(GetAdminTicket(), t_post);
            Thread.Sleep(2000);
            int count = EndPoint.SearchAccountBlogPostsCount(GetAdminTicket(), t_post.Title);
            Console.WriteLine("Found {0} posts.", count);
            Assert.IsTrue(count > 0);
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.SearchAccountBlogPosts(GetAdminTicket(), t_post.Title, null);
            Assert.IsTrue(posts.Length > 0);
            bool bFound = new TransitServiceCollection<WebBlogService.TransitAccountBlogPost>(posts).ContainsId(post_id);
            Assert.IsTrue(bFound);
            Delete(GetAdminTicket(), post_id);
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
            t_discussion_post_1.Id = DiscussionEndpoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_discussion_post_1);
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
                t_discussion_post_2.Id = DiscussionEndpoint.CreateOrUpdateDiscussionPost(GetUserTicket(), t_discussion_post_2);
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
            // make sure there're no posts in the blog
            int blog_posts_count = EndPoint.GetAccountBlogPostsCount(GetAdminTicket(), _blog_id);
            Assert.AreEqual(0, blog_posts_count);
            // move the discussion post
            int moved_post_id = EndPoint.MoveDiscussionPost(GetAdminTicket(), post_id, _blog_id);
            Console.WriteLine("Moved Post: {0}", moved_post_id);
            Assert.AreNotEqual(0, moved_post_id);
            int blog_posts_count2 = EndPoint.GetAccountBlogPostsCount(GetAdminTicket(), _blog_id);
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
            // make sure these are in order
            WebBlogService.TransitAccountBlogPost[] posts = EndPoint.GetAccountBlogPosts(GetAdminTicket(), _blog_id, null);
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
                GetAdminTicket(), _blog_id, null);
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
                GetAdminTicket(), _blog_id, null);
            Assert.AreEqual(2, posts_final.Length);
            Assert.AreEqual(t_post.Id, posts_final[1].Id); // newest post first
            Assert.AreEqual(t_post2.Id, posts_final[0].Id);
            // delete everything
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post.Id);
            EndPoint.DeleteAccountBlogPost(GetAdminTicket(), t_post2.Id);
        }
    }
}

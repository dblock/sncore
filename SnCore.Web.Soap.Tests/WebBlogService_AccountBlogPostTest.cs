using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using System.Threading;

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
    }
}

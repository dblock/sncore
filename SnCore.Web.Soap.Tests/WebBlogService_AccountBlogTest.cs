using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBlogServiceTests
{
    [TestFixture]
    public class AccountBlogTest : WebServiceTest<WebBlogService.TransitAccountBlog, WebBlogServiceNoCache>
    {
        public AccountBlogTest()
            : base("AccountBlog")
        {
        }

        public override WebBlogService.TransitAccountBlog GetTransitInstance()
        {
            WebBlogService.TransitAccountBlog t_instance = new WebBlogService.TransitAccountBlog();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }

        [Test]
        public void SyndicateAccountBlogTest()
        {
            int blog_id = Create(GetAdminTicket());
            int feed_id = EndPoint.SyndicateAccountBlog(GetAdminTicket(), blog_id);
            int feed_id_again = EndPoint.SyndicateAccountBlog(GetAdminTicket(), blog_id);
            Assert.AreEqual(feed_id, feed_id_again);
            WebSyndicationService.WebSyndicationService syndication_endpoint = new WebSyndicationService.WebSyndicationService();
            WebSyndicationService.TransitAccountFeed t_feed = syndication_endpoint.GetAccountFeedById(GetAdminTicket(), feed_id);
            Assert.IsTrue(t_feed.FeedUrl.EndsWith(string.Format("/AccountBlogRss.aspx?id={0}", blog_id)));
            syndication_endpoint.DeleteAccountFeed(GetAdminTicket(), t_feed.Id);
            Delete(GetAdminTicket(), blog_id);
        }

        [Test]
        public void GetAuthoredAccountBlogsTest()
        {
            int blog_id = Create(GetAdminTicket());
            int count = EndPoint.GetAuthoredAccountBlogsCount(GetUserTicket(), GetUserAccount().Id);
            Console.WriteLine("Blogs: {0}", count);
            WebBlogService.TransitAccountBlogAuthor t_author = new WebBlogService.TransitAccountBlogAuthor();
            t_author.AccountBlogId = blog_id;
            t_author.AccountId = GetUserAccount().Id;
            t_author.AllowDelete = t_author.AllowEdit = t_author.AllowPost = true;
            int author_id = EndPoint.CreateOrUpdateAccountBlogAuthor(GetAdminTicket(), t_author);
            Console.WriteLine("Created author: {0}", author_id);
            WebBlogService.TransitAccountBlog[] blogs = EndPoint.GetAuthoredAccountBlogs(GetUserTicket(), GetUserAccount().Id, null);
            Console.WriteLine("Blogs: {0}", blogs.Length);
            Assert.AreEqual(count + 1, blogs.Length);
            Delete(GetAdminTicket(), blog_id);
        }
    }
}

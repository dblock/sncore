using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebBlogServiceTests
{
    [TestFixture]
    public class AccountBlogAuthorTest : WebServiceTest<WebBlogService.TransitAccountBlogAuthor, WebBlogServiceNoCache>
    {
        private AccountBlogTest _blog = new AccountBlogTest();
        private int _blog_id = 0;

        public AccountBlogAuthorTest()
            : base("AccountBlogAuthor")
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

        public override WebBlogService.TransitAccountBlogAuthor GetTransitInstance()
        {
            WebBlogService.TransitAccountBlogAuthor t_instance = new WebBlogService.TransitAccountBlogAuthor();
            t_instance.AccountBlogId = _blog_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.AllowDelete = t_instance.AllowEdit = t_instance.AllowPost = true;
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
    }
}

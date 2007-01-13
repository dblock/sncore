using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

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
        protected void GetLatestDiscussionPostsTest()
        {

        }

        [Test]
        protected void GetLatestDiscussionPostsByIdTest()
        {

        }

        [Test]
        protected void SearchDiscussionPostsTest()
        {

        }

        [Test]
        protected void SearchDiscussionPostsByIdTest()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class AccountFeedTest : WebServiceTest<WebSyndicationService.TransitAccountFeed, WebSyndicationServiceNoCache>
    {
        private FeedTypeTest _feedtype = new FeedTypeTest();
        private int _feedtype_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _feedtype_id = _feedtype.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
            _feedtype.Delete(GetAdminTicket(), _feedtype_id);
        }

        public AccountFeedTest()
            : base("AccountFeed")
        {

        }

        public override object[] GetCountArgs(string ticket)
        {
            WebSyndicationService.TransitAccountFeedQueryOptions qopt = new WebSyndicationService.TransitAccountFeedQueryOptions();
            qopt.AccountId = _user.id;
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebSyndicationService.TransitAccountFeedQueryOptions qopt = new WebSyndicationService.TransitAccountFeedQueryOptions();
            qopt.AccountId = _user.id;
            object[] args = { ticket, qopt, options };
            return args;
        }

        public override WebSyndicationService.TransitAccountFeed GetTransitInstance()
        {
            WebSyndicationService.TransitAccountFeed t_instance = new WebSyndicationService.TransitAccountFeed();
            t_instance.AccountId = _user.id;
            t_instance.Description = GetNewString();
            t_instance.FeedType = (string) _feedtype.GetInstancePropertyById(_user.ticket, _feedtype_id, "Name");
            t_instance.FeedUrl = GetNewUri();
            t_instance.LinkUrl = GetNewUri();
            t_instance.Name = GetNewString();
            t_instance.Publish = true;
            t_instance.PublishImgs = true;
            t_instance.PublishMedia = true;
            t_instance.Hidden = false;
            return t_instance;
        }

        [Test]
        public void GetAccountFeedsTest()
        {
            WebSyndicationService.TransitAccountFeedQueryOptions options = new WebSyndicationService.TransitAccountFeedQueryOptions();
            int count = EndPoint.GetAccountFeedsCount(_user.ticket, options);
            Assert.IsTrue(count >= 0);
            WebSyndicationService.TransitAccountFeed[] feeds = EndPoint.GetAccountFeeds(_user.ticket, options, null);
        }
    }
}

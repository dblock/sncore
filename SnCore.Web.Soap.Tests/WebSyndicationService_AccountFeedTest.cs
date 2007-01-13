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

        [SetUp]
        public override void SetUp()
        {
            _feedtype_id = _feedtype.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _feedtype.Delete(GetAdminTicket(), _feedtype_id);
        }

        public AccountFeedTest()
            : base("AccountFeed")
        {

        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetUserAccount().Id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetUserAccount().Id, options };
            return args;
        }

        public override WebSyndicationService.TransitAccountFeed GetTransitInstance()
        {
            WebSyndicationService.TransitAccountFeed t_instance = new WebSyndicationService.TransitAccountFeed();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.FeedType = (string) _feedtype.GetInstancePropertyById(GetUserTicket(), _feedtype_id, "Name");
            t_instance.FeedUrl = string.Format("http://locacalhost/SnCoreWeb/{0}", Guid.NewGuid());
            t_instance.LinkUrl = string.Format("http://locacalhost/SnCoreWeb/{0}", Guid.NewGuid());
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        [Test]
        public void GetAllAccountFeedsTest()
        {
            int count = EndPoint.GetAllAccountFeedsCount(GetUserTicket());
            Assert.IsTrue(count >= 0);
            WebSyndicationService.TransitAccountFeed[] feeds = EndPoint.GetAllAccountFeeds(GetUserTicket(), null);

        }
    }
}

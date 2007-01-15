using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class AccountFeedItemTest : WebServiceTest<WebSyndicationService.TransitAccountFeedItem, WebSyndicationServiceNoCache>
    {
        private AccountFeedTest _accountfeed = new AccountFeedTest();
        private int _accountfeed_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _accountfeed.SetUp();
            _accountfeed_id = _accountfeed.Create(GetUserTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _accountfeed.Delete(GetUserTicket(), _accountfeed_id);
            _accountfeed.TearDown();
        }

        public AccountFeedItemTest()
            : base("AccountFeedItem")
        {

        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _accountfeed_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _accountfeed_id, options };
            return args;
        }

        public override WebSyndicationService.TransitAccountFeedItem GetTransitInstance()
        {
            WebSyndicationService.TransitAccountFeedItem t_instance = new WebSyndicationService.TransitAccountFeedItem();
            t_instance.AccountFeedId = _accountfeed_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Guid = Guid.NewGuid().ToString();
            t_instance.Link = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Title = Guid.NewGuid().ToString();
            return t_instance;
        }

        [Test]
        public void SearchAccountFeedItemsTest()
        {
            string s = Guid.NewGuid().ToString();
            int count = EndPoint.SearchAccountFeedItemsCount(GetUserTicket(), s);
            Assert.IsTrue(count >= 0);
            WebSyndicationService.TransitAccountFeedItem[] items = EndPoint.SearchAccountFeedItems(GetUserTicket(), s, null);
            Assert.IsNotNull(items);
            Console.WriteLine("Feed items: {0}", items.Length);
        }

        [Test]
        public void GetAllAccountFeedItemsTest()
        {
            int count = EndPoint.GetAllAccountFeedItemsCount(GetAdminTicket());
            Console.WriteLine("Count: {0}", count);
            WebSyndicationService.TransitAccountFeedItem[] items = EndPoint.GetAllAccountFeedItems(GetAdminTicket(), null);
            Console.WriteLine("Length: {0}", items.Length);
            Assert.AreEqual(count, items.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedItemTest : ManagedCRUDTest<AccountFeedItem, TransitAccountFeedItem, ManagedAccountFeedItem>
    {
        ManagedAccountFeedTest _accountfeed = new ManagedAccountFeedTest();

        public override void SetUp()
        {
            base.SetUp();
            _accountfeed.SetUp();
        }

        public override void TearDown()
        {
            _accountfeed.SetUp();
            base.TearDown();
        }

        public ManagedAccountFeedItemTest()
        {

        }

        public override TransitAccountFeedItem GetTransitInstance()
        {
            TransitAccountFeedItem t_instance = new TransitAccountFeedItem();
            t_instance.AccountFeedId = _accountfeed.Instance.Id;
            t_instance.AccountFeedLinkUrl = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.AccountFeedName = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Title = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

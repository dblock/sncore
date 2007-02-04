using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedItemMediaTest : ManagedCRUDTest<AccountFeedItemMedia, TransitAccountFeedItemMedia, ManagedAccountFeedItemMedia>
    {
        private ManagedAccountFeedItemTest _accountfeeditem = new ManagedAccountFeedItemTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _accountfeeditem.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountfeeditem.TearDown();
            base.TearDown();
        }

        public ManagedAccountFeedItemMediaTest()
        {

        }

        public override TransitAccountFeedItemMedia GetTransitInstance()
        {
            TransitAccountFeedItemMedia t_instance = new TransitAccountFeedItemMedia();
            t_instance.AccountFeedId = _accountfeeditem.Instance.Instance.AccountFeed.Id;
            t_instance.AccountFeedItemId = _accountfeeditem.Instance.Id;
            t_instance.AccountId = _accountfeeditem.Instance.Instance.AccountFeed.Account.Id;
            t_instance.EmbeddedHtml = GetNewString();
            t_instance.Type = GetNewString();
            return t_instance;
        }
    }
}

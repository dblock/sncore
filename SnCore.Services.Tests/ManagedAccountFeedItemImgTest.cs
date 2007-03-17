using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountFeedItemImgTest : ManagedCRUDTest<AccountFeedItemImg, TransitAccountFeedItemImg, ManagedAccountFeedItemImg>
    {
        private ManagedAccountFeedItemTest _accountfeeditem = new ManagedAccountFeedItemTest();

        [SetUp]
        public override void SetUp()
        {
            _accountfeeditem.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountfeeditem.TearDown();
        }

        public ManagedAccountFeedItemImgTest()
        {

        }

        public override TransitAccountFeedItemImg GetTransitInstance()
        {
            TransitAccountFeedItemImg t_instance = new TransitAccountFeedItemImg();
            t_instance.AccountFeedId = _accountfeeditem.Instance.Instance.AccountFeed.Id;
            t_instance.AccountFeedItemId = _accountfeeditem.Instance.Id;
            t_instance.AccountId = _accountfeeditem.Instance.Instance.AccountFeed.Account.Id;
            t_instance.Description = GetNewString();
            t_instance.Url = GetNewUri();
            return t_instance;
        }
    }
}

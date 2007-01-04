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
            base.SetUp();
            _accountfeeditem.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountfeeditem.TearDown();
            base.TearDown();
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
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }
    }
}

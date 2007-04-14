using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountRssWatchTest : ManagedCRUDTest<AccountRssWatch, TransitAccountRssWatch, ManagedAccountRssWatch>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();

        public ManagedAccountRssWatchTest()
        {

        }

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
        }

        public override TransitAccountRssWatch GetTransitInstance()
        {
            TransitAccountRssWatch t_instance = new TransitAccountRssWatch();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Name = GetNewString();
            t_instance.UpdateFrequency = 24;
            t_instance.Enabled = true;
            t_instance.Url = GetNewUri();
            return t_instance;
        }
    }
}

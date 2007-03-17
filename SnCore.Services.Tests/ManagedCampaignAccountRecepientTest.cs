using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedCampaignAccountRecepientTest : ManagedCRUDTest<CampaignAccountRecepient, TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedCampaignTest _campaign = new ManagedCampaignTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _campaign.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _campaign.TearDown();
            _account.TearDown();
        }

        public ManagedCampaignAccountRecepientTest()
        {

        }

        public override TransitCampaignAccountRecepient GetTransitInstance()
        {
            TransitCampaignAccountRecepient t_instance = new TransitCampaignAccountRecepient();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.CampaignId = _campaign.Instance.Id;
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedCampaignAccountRecepientTest : ManagedCRUDTest<CampaignAccountRecepient, TransitCampaignAccountRecepient, ManagedCampaignAccountRecepient>
    {
        public ManagedCampaignAccountRecepientTest()
        {

        }

        public override TransitCampaignAccountRecepient GetTransitInstance()
        {
            TransitCampaignAccountRecepient t_instance = new TransitCampaignAccountRecepient();
            return t_instance;
        }
    }
}

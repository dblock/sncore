using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedCampaignTest : ManagedCRUDTest<Campaign, TransitCampaign, ManagedCampaign>
    {
        public ManagedCampaignTest()
        {

        }

        public override TransitCampaign GetTransitInstance()
        {
            TransitCampaign t_instance = new TransitCampaign();
            return t_instance;
        }
    }
}

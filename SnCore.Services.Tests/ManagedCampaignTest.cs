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
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.SenderEmailAddress = string.Format("{0}@x.com", Guid.NewGuid());
            t_instance.SenderName = GetNewString();
            t_instance.Url = GetNewUri();
            return t_instance;
        }
    }
}

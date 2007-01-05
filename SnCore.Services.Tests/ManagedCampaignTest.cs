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
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.SenderEmailAddress = string.Format("{0}@x.com", Guid.NewGuid());
            t_instance.SenderName = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }
    }
}

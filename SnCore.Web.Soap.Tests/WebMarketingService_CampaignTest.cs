using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebMarketingServiceTests
{
    [TestFixture]
    public class CampaignTest : WebServiceTest<WebMarketingService.TransitCampaign, WebMarketingServiceNoCache>
    {
        public CampaignTest()
            : base("Campaign")
        {

        }

        public override WebMarketingService.TransitCampaign GetTransitInstance()
        {
            WebMarketingService.TransitCampaign t_instance = new WebMarketingService.TransitCampaign();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.SenderEmailAddress = string.Format("{0}@localhost.com", Guid.NewGuid());
            t_instance.SenderName = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }
    }
}

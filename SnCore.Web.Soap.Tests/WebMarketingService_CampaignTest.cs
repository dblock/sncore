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
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.SenderEmailAddress = GetNewEmailAddress();
            t_instance.SenderName = GetNewString();
            t_instance.Url = GetNewUri();
            return t_instance;
        }
    }
}

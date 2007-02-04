using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererHostTest : WebServiceTest<WebStatsService.TransitRefererHost, WebStatsServiceNoCache>
    {
        public RefererHostTest()
            : base("RefererHost")
        {

        }

        public override WebStatsService.TransitRefererHost GetTransitInstance()
        {
            WebStatsService.TransitRefererHost t_instance = new WebStatsService.TransitRefererHost();
            t_instance.Host = GetNewString();
            t_instance.LastRefererUri = GetNewUri();
            t_instance.LastRequestUri = GetNewUri();
            t_instance.Total = 1;
            return t_instance;
        }
    }
}

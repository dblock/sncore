using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererHostTest : WebServiceTest<WebStatsService.TransitRefererHost>
    {
        public RefererHostTest()
            : base("RefererHost", new WebStatsServiceNoCache())
        {

        }

        public override WebStatsService.TransitRefererHost GetTransitInstance()
        {
            WebStatsService.TransitRefererHost t_instance = new WebStatsService.TransitRefererHost();
            t_instance.Host = Guid.NewGuid().ToString();
            t_instance.LastRefererUri = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.LastRequestUri = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Total = 1;
            return t_instance;
        }
    }
}

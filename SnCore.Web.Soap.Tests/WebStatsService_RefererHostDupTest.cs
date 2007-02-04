using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererHostDupTest : WebServiceTest<WebStatsService.TransitRefererHostDup, WebStatsServiceNoCache>
    {
        public RefererHostDupTest()
            : base("RefererHostDup")
        {

        }

        public override WebStatsService.TransitRefererHostDup GetTransitInstance()
        {
            WebStatsService.TransitRefererHostDup t_instance = new WebStatsService.TransitRefererHostDup();
            t_instance.Host = GetNewString();
            t_instance.RefererHost = GetNewString();
            return t_instance;
        }
    }
}

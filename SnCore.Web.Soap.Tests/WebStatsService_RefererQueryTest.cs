using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererQueryTest : WebServiceTest<WebStatsService.TransitRefererQuery, WebStatsServiceNoCache>
    {
        public RefererQueryTest()
            : base("RefererQuery", "RefererQueries")
        {

        }

        public override WebStatsService.TransitRefererQuery GetTransitInstance()
        {
            WebStatsService.TransitRefererQuery t_instance = new WebStatsService.TransitRefererQuery();
            t_instance.Keywords = GetNewString();
            t_instance.Total = 1;
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererQueryTest : WebServiceTest<WebStatsService.TransitRefererQuery>
    {
        public RefererQueryTest()
            : base("RefererQuery", "RefererQueries", new WebStatsServiceNoCache())
        {

        }

        public override WebStatsService.TransitRefererQuery GetTransitInstance()
        {
            WebStatsService.TransitRefererQuery t_instance = new WebStatsService.TransitRefererQuery();
            t_instance.Keywords = Guid.NewGuid().ToString();
            t_instance.Total = 1;
            return t_instance;
        }
    }
}

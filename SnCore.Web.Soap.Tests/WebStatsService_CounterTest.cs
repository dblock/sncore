using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class CounterTest : WebServiceTest<WebStatsService.TransitCounter, WebStatsServiceNoCache>
    {
        public CounterTest()
            : base("Counter")
        {

        }

        public override WebStatsService.TransitCounter GetTransitInstance()
        {
            WebStatsService.TransitCounter t_instance = new WebStatsService.TransitCounter();
            t_instance.Uri = GetNewUri();
            t_instance.Total = 1;
            return t_instance;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSystemServiceTests
{
    [TestFixture]
    public class ScheduleTest : WebServiceTest<WebSystemService.TransitSchedule, WebSystemServiceNoCache>
    {
        public ScheduleTest()
            : base("Schedule")
        {
        }

        public override WebSystemService.TransitSchedule GetTransitInstance()
        {
            WebSystemService.TransitSchedule t_instance = new WebSystemService.TransitSchedule();
            t_instance.StartDateTime = DateTime.UtcNow;
            t_instance.EndDateTime = DateTime.UtcNow.AddDays(1);
            t_instance.RecurrencePattern = WebSystemService.RecurrencePattern.Daily_EveryNDays;
            return t_instance;
        }
    }
}

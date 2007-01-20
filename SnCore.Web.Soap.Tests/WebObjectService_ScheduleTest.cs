using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class ScheduleTest : WebServiceTest<WebObjectService.TransitSchedule, WebObjectServiceNoCache>
    {
        public ScheduleTest()
            : base("Schedule")
        {
        }

        public override WebObjectService.TransitSchedule GetTransitInstance()
        {
            WebObjectService.TransitSchedule t_instance = new WebObjectService.TransitSchedule();
            t_instance.StartDateTime = DateTime.UtcNow;
            t_instance.EndDateTime = DateTime.UtcNow.AddDays(1);
            t_instance.RecurrencePattern = WebObjectService.RecurrencePattern.Daily_EveryNDays;
            return t_instance;
        }

        [Test]
        public void GetScheduleStringTest()
        {
            WebObjectService.TransitSchedule t_instance = GetTransitInstance();
            string schedule = EndPoint.GetScheduleString(GetAdminTicket(), t_instance, 1);
            Assert.IsNotEmpty(schedule);
            Console.WriteLine(schedule);
        }
    }
}

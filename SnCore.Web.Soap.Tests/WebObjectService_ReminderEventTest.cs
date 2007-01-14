using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class ReminderEventTest : WebServiceTest<WebObjectService.TransitReminderEvent, WebObjectServiceNoCache>
    {
        public ReminderEventTest()
            : base("ReminderEvent")
        {
        }


        public override WebObjectService.TransitReminderEvent GetTransitInstance()
        {
            WebObjectService.TransitReminderEvent t_instance = new WebObjectService.TransitReminderEvent();
            return t_instance;
        }
    }
}

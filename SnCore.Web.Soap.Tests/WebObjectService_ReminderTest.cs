using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class ReminderTest : WebServiceTest<WebObjectService.TransitReminder, WebObjectServiceNoCache>
    {
        public ReminderTest()
            : base("Reminder")
        {
        }


        public override WebObjectService.TransitReminder GetTransitInstance()
        {
            WebObjectService.TransitReminder t_instance = new WebObjectService.TransitReminder();
            return t_instance;
        }
    }
}

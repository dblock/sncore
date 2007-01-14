using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class ReminderAccountPropertyTest : WebServiceTest<WebObjectService.TransitReminderAccountProperty, WebObjectServiceNoCache>
    {
        public ReminderAccountPropertyTest()
            : base("ReminderAccountProperty", "ReminderAccountProperties")
        {
        }


        public override WebObjectService.TransitReminderAccountProperty GetTransitInstance()
        {
            WebObjectService.TransitReminderAccountProperty t_instance = new WebObjectService.TransitReminderAccountProperty();
            return t_instance;
        }
    }
}

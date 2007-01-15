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
        public ReminderTest _reminder = new ReminderTest();
        public int _reminder_id = 0;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _reminder.SetUp();
            _reminder_id = _reminder.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _reminder.Delete(GetAdminTicket(), _reminder_id);
            _reminder.TearDown();
            base.TearDown();
        }

        public ReminderEventTest()
            : base("ReminderEvent")
        {

        }

        public override WebObjectService.TransitReminderEvent GetTransitInstance()
        {
            WebObjectService.TransitReminderEvent t_instance = new WebObjectService.TransitReminderEvent();
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.ReminderId = _reminder_id;
            return t_instance;
        }
    }
}

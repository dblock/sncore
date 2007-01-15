using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebAccountServiceTests;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class ReminderAccountPropertyTest : WebServiceTest<WebObjectService.TransitReminderAccountProperty, WebObjectServiceNoCache>
    {
        public ReminderTest _reminder = new ReminderTest();
        public int _reminder_id = 0;
        public AccountPropertyTest _accountproperty = new AccountPropertyTest();
        public int _accountproperty_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _reminder.SetUp();
            _reminder_id = _reminder.Create(GetAdminTicket());
            _accountproperty.SetUp();
            _accountproperty_id = _accountproperty.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _accountproperty.Delete(GetAdminTicket(), _accountproperty_id);
            _accountproperty.TearDown();
            _reminder.Delete(GetAdminTicket(), _reminder_id);
            _reminder.TearDown();
        }

        public ReminderAccountPropertyTest()
            : base("ReminderAccountProperty", "ReminderAccountProperties")
        {
        }


        public override WebObjectService.TransitReminderAccountProperty GetTransitInstance()
        {
            WebObjectService.TransitReminderAccountProperty t_instance = new WebObjectService.TransitReminderAccountProperty();
            t_instance.ReminderId = _reminder_id;
            t_instance.AccountPropertyId = _accountproperty_id;
            t_instance.Value = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _reminder_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _reminder_id };
            return args;
        }
    }
}

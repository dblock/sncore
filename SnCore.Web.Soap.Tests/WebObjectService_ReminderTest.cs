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
        public DataObjectTest _dataobject = new DataObjectTest();
        public int _dataobject_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _dataobject.SetUp();
            _dataobject_id = _dataobject.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _dataobject.Delete(GetAdminTicket(), _dataobject_id);
            _dataobject.TearDown();
        }

        public ReminderTest()
            : base("Reminder")
        {
        }


        public override WebObjectService.TransitReminder GetTransitInstance()
        {
            WebObjectService.TransitReminder t_instance = new WebObjectService.TransitReminder();
            t_instance.DataObject_Id = _dataobject_id;
            t_instance.DataObjectField = GetNewString();
            t_instance.DeltaHours = 24;
            t_instance.Enabled = false;
            t_instance.Url = GetNewUri();
            return t_instance;
        }

        [Test]
        protected void CanSendReminderTest()
        {

        }
    }
}

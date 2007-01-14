using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebObjectServiceTests;
using SnCore.Web.Soap.Tests.WebPlaceServiceTests;

namespace SnCore.Web.Soap.Tests.WebEventServiceTests
{
    [TestFixture]
    public class AccountEventTest : WebServiceTest<WebEventService.TransitAccountEvent, WebEventServiceNoCache>
    {
        private AccountEventTypeTest _type = new AccountEventTypeTest();
        private int _type_id = 0;
        private ScheduleTest _schedule = new ScheduleTest();
        private int _schedule_id = 0;
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _type_id = _type.Create(GetAdminTicket());
            _schedule_id = _schedule.Create(GetAdminTicket());
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
            _schedule.Delete(GetAdminTicket(), _schedule_id);
            _type.Delete(GetAdminTicket(), _type_id);
        }

        public AccountEventTest()
            : base("AccountEvent")
        {
        }

        public override WebEventService.TransitAccountEvent GetTransitInstance()
        {
            WebEventService.TransitAccountEvent t_instance = new WebEventService.TransitAccountEvent();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.AccountEventType = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.Cost = "10$";
            t_instance.Email = string.Format("{0}@localhost.com", Guid.NewGuid());
            t_instance.EndDateTime = DateTime.UtcNow.AddDays(1);
            t_instance.StartDateTime = DateTime.UtcNow.AddDays(-1);
            t_instance.Website = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.YearlyMonth = 6;
            t_instance.RecurrencePattern = WebEventService.RecurrencePattern.Yearly_DayNOfMonth;
            t_instance.ScheduleId = _schedule_id;
            t_instance.PlaceId = _place_id;
            t_instance.Publish = true;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebEventService.TransitAccountEventQueryOptions qopt = new WebEventService.TransitAccountEventQueryOptions();
            qopt.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebEventService.TransitAccountEventQueryOptions qopt = new WebEventService.TransitAccountEventQueryOptions();
            qopt.Type = (string)_type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            object[] args = { ticket, 0, qopt, options };
            return args;
        }

        public override object[] GetArg(string ticket, int id)
        {
            object[] args = { ticket, id, 0 };
            return args;
        }

        [Test]
        protected void GetAccountEventInstancesTest()
        {

        }

        [Test]
        protected void GetAccountEventsByAccountIdTest()
        {

        }

        [Test]
        protected void GetAccountEventVCalendarByIdTest()
        {

        }
    }
}

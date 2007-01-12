using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebEventServiceTests
{
    [TestFixture]
    public class AccountEventPictureTest : WebServiceTest<WebEventService.TransitAccountEventPicture, WebEventServiceNoCache>
    {
        private AccountEventTest _event = new AccountEventTest();
        private int _event_id = 0;

        [SetUp]
        public void SetUp()
        {
            _event_id = _event.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _event.Delete(GetAdminTicket(), _event_id);
        }

        public AccountEventPictureTest()
            : base("AccountEventPicture")
        {
        }

        public override WebEventService.TransitAccountEventPicture GetTransitInstance()
        {
            WebEventService.TransitAccountEventPicture t_instance = new WebEventService.TransitAccountEventPicture();
            t_instance.AccountEventId = _event_id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

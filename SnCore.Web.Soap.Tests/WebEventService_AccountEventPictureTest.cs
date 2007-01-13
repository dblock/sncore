using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebEventServiceTests
{
    [TestFixture]
    public class AccountEventPictureTest : WebServiceTest<WebEventService.TransitAccountEventPicture, WebEventServiceNoCache>
    {
        private AccountEventTest _event = new AccountEventTest();
        private int _event_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _event.SetUp();
            _event_id = _event.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _event.Delete(GetAdminTicket(), _event_id);
            _event.TearDown();
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
            t_instance.Picture = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 12, 240, 100);
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _event_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _event_id };
            return args;
        }
    }
}

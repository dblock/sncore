using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceQueueTest : WebServiceTest<WebPlaceService.TransitPlaceQueue, WebPlaceServiceNoCache>
    {
        public PlaceQueueTest()
            : base("PlaceQueue")
        {
        }

        public override WebPlaceService.TransitPlaceQueue GetTransitInstance()
        {
            WebPlaceService.TransitPlaceQueue t_instance = new WebPlaceService.TransitPlaceQueue();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.PublishAll = false;
            t_instance.PublishFriends = true;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }

        [Test]
        public void GetOrCreatePlaceQueueByNameTest()
        {
            string name = GetNewString();
            WebPlaceService.TransitPlaceQueue t_instance = EndPoint.GetOrCreatePlaceQueueByName(GetAdminTicket(), GetUserAccount().Id, name);
            Assert.IsNotNull(t_instance);
            Assert.IsTrue(t_instance.Id > 0);
            EndPoint.DeletePlaceQueue(GetAdminTicket(), t_instance.Id);
        }
    }
}

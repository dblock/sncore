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
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
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
        protected void GetOrCreatePlaceQueueByNameTest()
        {

        }
    }
}

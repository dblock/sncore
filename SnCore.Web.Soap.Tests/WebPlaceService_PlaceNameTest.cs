using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceNameTest : WebServiceTest<WebPlaceService.TransitPlaceName, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
        }

        public PlaceNameTest()
            : base("PlaceName")
        {
        }

        public override WebPlaceService.TransitPlaceName GetTransitInstance()
        {
            WebPlaceService.TransitPlaceName t_instance = new WebPlaceService.TransitPlaceName();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.PlaceId = _place_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _place_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _place_id };
            return args;
        }
    }
}

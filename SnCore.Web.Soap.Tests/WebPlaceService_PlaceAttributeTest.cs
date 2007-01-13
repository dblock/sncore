using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebSystemServiceTests;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceAttributeTest : WebServiceTest<WebPlaceService.TransitPlaceAttribute, WebPlaceServiceNoCache>
    {
        private AttributeTest _attribute = new AttributeTest();
        private int _attribute_id = 0;
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _attribute_id = _attribute.Create(GetAdminTicket());
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
            _attribute.Delete(GetAdminTicket(), _attribute_id);
        }

        public PlaceAttributeTest()
            : base("PlaceAttribute")
        {
        }

        public override WebPlaceService.TransitPlaceAttribute GetTransitInstance()
        {
            WebPlaceService.TransitPlaceAttribute t_instance = new WebPlaceService.TransitPlaceAttribute();
            t_instance.AttributeId = _attribute_id;
            t_instance.PlaceId = _place_id;
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Value = Guid.NewGuid().ToString();
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

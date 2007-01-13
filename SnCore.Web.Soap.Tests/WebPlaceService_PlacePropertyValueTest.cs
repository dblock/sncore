using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePropertyValueTest : WebServiceTest<WebPlaceService.TransitPlacePropertyValue, WebPlaceServiceNoCache>
    {
        public PlacePropertyTest _property = new PlacePropertyTest();
        public int _property_id = 0;
        public PlaceTest _place = new PlaceTest();
        public int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _property.SetUp();
            _property_id = _property.Create(GetAdminTicket());
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
            _property.Delete(GetAdminTicket(), _property_id);
            _property.TearDown();
        }

        public PlacePropertyValueTest()
            : base("PlacePropertyValue")
        {
        }

        public override WebPlaceService.TransitPlacePropertyValue GetTransitInstance()
        {
            WebPlaceService.TransitPlacePropertyValue t_instance = new WebPlaceService.TransitPlacePropertyValue();
            t_instance.PlacePropertyId = _property_id;
            t_instance.PlaceId = _place_id;
            t_instance.Value = Guid.NewGuid().ToString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _place_id, _property._group_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _place_id, _property._group_id, options };
            return args;
        }

        [Test]
        public void GetDistinctPropertyValuesTest()
        {
            int id = Create(GetAdminTicket());
            string groupname = (string) _property._group.GetInstancePropertyById(GetUserTicket(), _property._group_id, "Name");
            string propertyname = (string) _property.GetInstancePropertyById(GetUserTicket(), _property_id, "Name");
            WebPlaceService.TransitDistinctPlacePropertyValue[] values = EndPoint.GetDistinctPropertyValues(
                GetUserTicket(), groupname, propertyname);
            Console.WriteLine("Property values for [{0}/{1}]: {2}", groupname, propertyname, values.Length);
            Assert.IsTrue(values.Length >= 1);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        protected void GetAllPlacePropertyValuesByIdTest()
        {

        }

        [Test]
        protected void GetPlacesByPropertyValueTest()
        {

        }
    }
}

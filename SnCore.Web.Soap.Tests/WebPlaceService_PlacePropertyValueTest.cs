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
            t_instance.Value = GetNewString();
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
                GetUserTicket(), groupname, propertyname, null);
            Console.WriteLine("Property values for [{0}/{1}]: {2}", groupname, propertyname, values.Length);
            Assert.IsTrue(values.Length >= 1);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void GetAllPlacePropertyValuesByIdTest()
        {
            WebPlaceService.TransitPlacePropertyValue t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            string groupname = (string)_property._group.GetInstancePropertyById(GetUserTicket(), _property._group_id, "Name");
            string propertyname = (string)_property.GetInstancePropertyById(GetUserTicket(), _property_id, "Name");
            WebPlaceService.TransitPlacePropertyValue[] values = EndPoint.GetAllPlacePropertyValuesById(GetAdminTicket(), _place_id, _property._group_id);
            Console.WriteLine("Property values for [{0}/{1}]: {2}", groupname, propertyname, values.Length);
            Assert.IsTrue(values.Length >= 1);
            Assert.IsTrue(new TransitServiceCollection<WebPlaceService.TransitPlacePropertyValue>(values).ContainsId(t_instance.Id));
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void GetPlacesByPropertyValueTest()
        {
            WebPlaceService.TransitPlacePropertyValue t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            string groupname = (string)_property._group.GetInstancePropertyById(GetUserTicket(), _property._group_id, "Name");
            string propertyname = (string)_property.GetInstancePropertyById(GetUserTicket(), _property_id, "Name");
            int count = EndPoint.GetPlacesByPropertyValueCount(
                GetUserTicket(), groupname, propertyname, t_instance.Value);
            Console.WriteLine("Count: {0}", count);
            Assert.IsTrue(count > 0);
            WebPlaceService.TransitPlace[] places = EndPoint.GetPlacesByPropertyValue(
                GetUserTicket(), groupname, propertyname, t_instance.Value, null);
            Console.WriteLine("Properties for [{0}/{1}/{2}]: {3}", groupname, propertyname, t_instance.Value, places.Length);
            Assert.IsTrue(places.Length >= 1);
            Assert.IsTrue(new TransitServiceCollection<WebPlaceService.TransitPlace>(places).ContainsId(_place_id));
            Delete(GetAdminTicket(), id);
        }
    }
}

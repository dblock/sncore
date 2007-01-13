using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebLocationServiceTests;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceTest : WebServiceTest<WebPlaceService.TransitPlace, WebPlaceServiceNoCache>
    {
        public NeighborhoodTest _neighborhood = new NeighborhoodTest();
        public int _neighborhood_id = 0;
        public PlaceTypeTest _type = new PlaceTypeTest();
        public int _type_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _neighborhood.SetUp();
            _neighborhood_id = _neighborhood.Create(GetAdminTicket());
            _type.SetUp();
            _type_id = _type.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _neighborhood.Delete(GetAdminTicket(), _neighborhood_id);
            _neighborhood.TearDown();
            _type.Delete(GetAdminTicket(), _type_id);
            _type.TearDown();
        }

        public PlaceTest()
            : base("Place")
        {
        }

        public override WebPlaceService.TransitPlace GetTransitInstance()
        {
            WebPlaceService.TransitPlace t_instance = new WebPlaceService.TransitPlace();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.CrossStreet = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Email = string.Format("{0}@localhost.com", Guid.NewGuid());
            t_instance.Fax = "(123) 123-4567";
            t_instance.Phone = "(123) 123-4567";
            t_instance.Street = Guid.NewGuid().ToString();
            t_instance.Website = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Zip = "12345";
            t_instance.Neighborhood = (string) _neighborhood.GetInstancePropertyById(GetAdminTicket(), _neighborhood_id, "Name");
            t_instance.City = (string) _neighborhood._city.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city_id, "Name");
            t_instance.State = (string)_neighborhood._city._state.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city._state_id, "Name");
            t_instance.Country = (string)_neighborhood._city._state._country.GetInstancePropertyById(GetAdminTicket(), _neighborhood._city._state._country_id, "Name");
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            WebPlaceService.TransitPlaceQueryOptions qopt = new WebPlaceService.TransitPlaceQueryOptions();
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebPlaceService.TransitPlaceQueryOptions qopt = new WebPlaceService.TransitPlaceQueryOptions();
            object[] args = { ticket, qopt, options };
            return args;
        }

        [Test]
        public void GetNewPlacesTest()
        {
            WebPlaceService.TransitPlace[] places = EndPoint.GetNewPlaces(GetAdminTicket(), null);
            Console.WriteLine("Places: {0}", places.Length);
        }

        [Test]
        protected void FindPlaceTest()
        {

        }

        [Test]
        protected void SearchPlacesTest()
        {

        }
    }
}

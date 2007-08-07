using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceChangeRequestTest : WebServiceTest<WebPlaceService.TransitPlaceChangeRequest, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;
        private PlaceTypeTest _type = new PlaceTypeTest();
        private int _type_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _type_id = _type.Create(GetAdminTicket());
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _type.Delete(GetAdminTicket(), _type_id);
            _place.TearDown();
        }

        public PlaceChangeRequestTest()
            : base("PlaceChangeRequest")
        {
        }

        public override WebPlaceService.TransitPlaceChangeRequest GetTransitInstance()
        {
            WebPlaceService.TransitPlaceChangeRequest t_instance = new WebPlaceService.TransitPlaceChangeRequest();
            t_instance.Name = GetNewString();
            t_instance.Phone = (string) _place.GetInstancePropertyById(GetAdminTicket(), _place_id, "Phone");
            t_instance.Website = GetNewString();
            t_instance.Zip = (string) _place.GetInstancePropertyById(GetAdminTicket(), _place_id, "Zip");
            t_instance.Description = GetNewString();
            t_instance.PlaceId = _place_id;
            t_instance.City = (string) _place._neighborhood._city.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city_id, "Name");
            t_instance.State = (string) _place._neighborhood._city._state.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city._state_id, "Name");
            t_instance.Country = (string) _place._neighborhood._city._state._country.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city._state._country_id, "Name");
            t_instance.Neighborhood = (string) _place._neighborhood.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood_id, "Name");
            t_instance.Type = (string)_type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
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
    }
}

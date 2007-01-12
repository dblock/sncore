using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class DistinctPlaceNeighborhoodTest : WebServiceBaseTest<WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
        }

        [Test]
        public void GetPlaceNeighborhoodsTest()
        {
            string city = (string) _place._neighborhood._city.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city_id, "Name");
            string state = (string) _place._neighborhood._city._state.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city._state_id, "Name");
            string country = (string) _place._neighborhood._city._state._country.GetInstancePropertyById(GetAdminTicket(), _place._neighborhood._city._state._country_id, "Name");
            WebPlaceService.TransitDistinctPlaceNeighborhood[] nhs = EndPoint.GetPlaceNeighborhoods(
                GetAdminTicket(), country, state, city, null);
            Console.WriteLine("Neighborhoods: {0}", nhs.Length);
            Assert.IsTrue(nhs.Length >= 1);
        }
    }
}

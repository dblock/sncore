using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class CityTest : WebServiceTest<WebLocationService.TransitCity, WebLocationServiceNoCache>
    {
        public StateTest _state = new StateTest();
        public int _state_id = 0;

        [SetUp]
        public void SetUp()
        {
            _state.SetUp();
            _state_id = _state.Create(GetAdminTicket());
        }

        [TearDown]
        public void TearDown()
        {
            _state.Delete(GetAdminTicket(), _state_id);
            _state.TearDown();
        }

        public CityTest()
            : base("City", "Cities")
        {

        }

        public override WebLocationService.TransitCity GetTransitInstance()
        {
            WebLocationService.TransitCity t_instance = new WebLocationService.TransitCity();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.State = (string)_state.GetInstancePropertyById(GetAdminTicket(), _state_id, "Name");
            t_instance.Country = (string)_state._country.GetInstancePropertyById(GetAdminTicket(), _state._country_id, "Name");
            t_instance.Tag = Guid.NewGuid().ToString().Substring(0, 5);
            return t_instance;
        }

        [Test]
        public void GetCitiesByCountryIdTest()
        {
            WebLocationService.TransitCity t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance);
            int count = EndPoint.GetCitiesByCountryIdCount(GetAdminTicket(), _state._country_id);
            Assert.IsTrue(count > 0);
            WebLocationService.TransitCity[] cities = EndPoint.GetCitiesByCountryId(GetAdminTicket(), _state._country_id, null);
            bool bFound = new TransitServiceCollection<WebLocationService.TransitCity>(cities).ContainsId(id);
            Assert.IsTrue(bFound, "City was not returned from GetCitiesByCountryId");
            EndPoint.DeleteCity(GetAdminTicket(), id);
        }

        [Test]
        public void GetCitiesByStateIdTest()
        {
            WebLocationService.TransitCity t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance);
            int count = EndPoint.GetCitiesByStateIdCount(GetAdminTicket(), _state_id);
            Assert.IsTrue(count > 0);
            WebLocationService.TransitCity[] cities = EndPoint.GetCitiesByStateId(GetAdminTicket(), _state_id, null);
            bool bFound = new TransitServiceCollection<WebLocationService.TransitCity>(cities).ContainsId(id);
            Assert.IsTrue(bFound, "City was not returned from GetCitiesByStateId");
            EndPoint.DeleteCity(GetAdminTicket(), id);
        }

        [Test]
        public void GetCityByTagTest()
        {
            WebLocationService.TransitCity t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance);
            WebLocationService.TransitCity city = EndPoint.GetCityByTag(GetAdminTicket(), t_instance.Tag);
            Assert.IsNotNull(city, "City was not returned from GetCityByTag");
            EndPoint.DeleteCity(GetAdminTicket(), id);
        }

        [Test]
        public void SearchCityByNameTest()
        {
            WebLocationService.TransitCity t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance);
            WebLocationService.TransitCity[] cities = EndPoint.SearchCitiesByName(GetAdminTicket(), t_instance.Name, null);
            bool bFound = new TransitServiceCollection<WebLocationService.TransitCity>(cities).ContainsId(id);
            Assert.IsTrue(bFound, "City was not returned from SearchCitiesByName");
            EndPoint.DeleteCity(GetAdminTicket(), id);
        }

        [Test]
        public void MergeCitiesTest()
        {
            WebLocationService.TransitCity t_instance1 = GetTransitInstance();
            int id1 = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance1);
            WebLocationService.TransitCity t_instance2 = GetTransitInstance();
            int id2 = EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance2);
            EndPoint.MergeCities(GetAdminTicket(), id1, id2);
            EndPoint.DeleteCity(GetAdminTicket(), id1);
            // TODO: implement a GetCityById that doesn't throw, check that id2 doesn't exist any more
        }
    }
}
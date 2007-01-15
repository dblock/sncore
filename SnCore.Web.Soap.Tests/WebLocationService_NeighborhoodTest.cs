using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class NeighborhoodTest : WebServiceTest<WebLocationService.TransitNeighborhood, WebLocationServiceNoCache>
    {
        public CityTest _city = new CityTest();
        public int _city_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _city.SetUp();
            _city_id = _city.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _city.Delete(GetAdminTicket(), _city_id);
            _city.TearDown();
        }

        public NeighborhoodTest()
            : base("Neighborhood")
        {
        }


        public override WebLocationService.TransitNeighborhood GetTransitInstance()
        {
            WebLocationService.TransitNeighborhood t_instance = new WebLocationService.TransitNeighborhood();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Country = (string)_city._state._country.GetInstancePropertyById(GetAdminTicket(), _city._state._country_id, "Name");
            t_instance.State = (string)_city._state.GetInstancePropertyById(GetAdminTicket(), _city._state_id, "Name");
            t_instance.City = (string)_city.GetInstancePropertyById(GetAdminTicket(), _city_id, "Name");
            return t_instance;
        }

        [Test]
        public void GetNeighborhoodsByCityIdTest()
        {
            WebLocationService.TransitNeighborhood t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateNeighborhood(GetAdminTicket(), t_instance);
            int count = EndPoint.GetNeighborhoodsByCityIdCount(GetAdminTicket(), _city_id);
            Assert.IsTrue(count > 0);
            WebLocationService.TransitNeighborhood[] neighborhoods = EndPoint.GetNeighborhoodsByCityId(GetAdminTicket(), _city_id, null);
            bool bFound = new TransitServiceCollection<WebLocationService.TransitNeighborhood>(neighborhoods).ContainsId(id); 
            Assert.IsTrue(bFound, "Neighborhood was not returned from GetNeighborhoodsByCityId");
            EndPoint.DeleteNeighborhood(GetAdminTicket(), id);
        }

        [Test]
        public void SearchNeighborhoodByNameTest()
        {
            WebLocationService.TransitNeighborhood t_instance = GetTransitInstance();
            int id = EndPoint.CreateOrUpdateNeighborhood(GetAdminTicket(), t_instance);
            WebLocationService.TransitNeighborhood[] neighborhoods = EndPoint.SearchNeighborhoodsByName(GetAdminTicket(), t_instance.Name, null);
            bool bFound = new TransitServiceCollection<WebLocationService.TransitNeighborhood>(neighborhoods).ContainsId(id);            
            Assert.IsTrue(bFound, "Neighborhood was not returned from SearchNeighborhoodsByName");
            EndPoint.DeleteNeighborhood(GetAdminTicket(), id);
        }

        [Test]
        public void MergeNeighborhoodsTest()
        {
            WebLocationService.TransitNeighborhood t_instance1 = GetTransitInstance();
            int id1 = EndPoint.CreateOrUpdateNeighborhood(GetAdminTicket(), t_instance1);
            WebLocationService.TransitNeighborhood t_instance2 = GetTransitInstance();
            int id2 = EndPoint.CreateOrUpdateNeighborhood(GetAdminTicket(), t_instance2);
            EndPoint.MergeNeighborhoods(GetAdminTicket(), id1, id2);
            EndPoint.DeleteNeighborhood(GetAdminTicket(), id1);
            // TODO: implement a GetCityById that doesn't throw, check that id2 doesn't exist any more
        }

        [Test]
        protected void GetNeighborhoodsByLocationTest()
        {

        }
    }
}

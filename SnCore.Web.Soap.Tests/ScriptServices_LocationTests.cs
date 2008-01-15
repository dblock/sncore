using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SnCore.Web.Soap.Tests.WebLocationServiceTests;

namespace SnCore.Web.Soap.Tests.ScriptServicesTests
{
    [TestFixture]
    public class ScriptServices_LocationTests : WebServiceBaseTest<ScriptServices.ScriptServices> 
    {
        [Test]
        public void GetNeighborhoodsCompletionListTest()
        {
            NeighborhoodTest _nh = new NeighborhoodTest();
            _nh.SetUp();
            WebLocationService.TransitCity t_city = _nh.EndPoint.GetCityById(GetAdminTicket(), _nh._city_id);
            string key = string.Format("{0};{1};{2}", t_city.Country, t_city.State, t_city.Name);
            Console.WriteLine("Key: {0}", key);
            WebLocationService.TransitNeighborhood t_instance = _nh.GetTransitInstance();
            int id = _nh.EndPoint.CreateOrUpdateNeighborhood(GetAdminTicket(), t_instance);
            string[] neighborhoods = EndPoint.GetNeighborhoodsCompletionList(
                t_instance.Name.Substring(0, 3), 100, key);
            Assert.AreEqual(1, neighborhoods.Length);
            Assert.AreEqual(t_instance.Name, neighborhoods[0]);
            Console.WriteLine(neighborhoods[0]);
            _nh.EndPoint.DeleteNeighborhood(GetAdminTicket(), id);
            _nh.TearDown();
        }

        [Test]
        public void GetCitiesCompletionListTest()
        {
            CityTest _ct = new CityTest();
            _ct.SetUp();
            WebLocationService.TransitState t_state = _ct.EndPoint.GetStateById(GetAdminTicket(), _ct._state_id);
            string key = string.Format("{0};{1}", t_state.Country, t_state.Name);
            Console.WriteLine("Key: {0}", key);
            WebLocationService.TransitCity t_instance = _ct.GetTransitInstance();
            int id = _ct.EndPoint.CreateOrUpdateCity(GetAdminTicket(), t_instance);
            string[] cities = EndPoint.GetCitiesCompletionList(
                t_instance.Name.Substring(0, 3), 100, key);
            Assert.AreEqual(1, cities.Length);
            Assert.AreEqual(t_instance.Name, cities[0]);
            Console.WriteLine(cities[0]);
            _ct.EndPoint.DeleteCity(GetAdminTicket(), id);
            _ct.TearDown();
        }
    }
}

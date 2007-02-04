using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class CountryTest : WebServiceTest<WebLocationService.TransitCountry, WebLocationServiceNoCache>
    {
        public CountryTest()
            : base("Country", "Countries")
        {
        }


        public override WebLocationService.TransitCountry GetTransitInstance()
        {
            WebLocationService.TransitCountry t_instance = new WebLocationService.TransitCountry();
            t_instance.Name = GetNewString();
            return t_instance;
        }

        [Test]
        public void GetCountriesWithDefaultTest()
        {
            string name = "United States";
            WebLocationService.TransitCountry[] countries1 = EndPoint.GetCountries(GetAdminTicket(), null);
            WebLocationService.TransitCountry[] countries2 = EndPoint.GetCountriesWithDefault(GetAdminTicket(), name, null);
            Assert.AreEqual(countries1.Length + 2, countries2.Length);
            Assert.AreEqual(countries2[0].Name, name);
            Assert.AreEqual(countries2[1].Id, 0);
        }
    }
}

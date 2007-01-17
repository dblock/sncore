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
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }

        [Test]
        protected void GetCountriesWithDefaultTest()
        {

        }
    }
}

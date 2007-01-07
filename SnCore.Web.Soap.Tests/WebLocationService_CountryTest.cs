using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class CountryTest : WebServiceTest<WebLocationService.TransitCountry>
    {
        public CountryTest()
            : base("Country", "Countries", new WebLocationService.WebLocationService())
        {
        }


        public override WebLocationService.TransitCountry GetTransitInstance()
        {
            WebLocationService.TransitCountry t_instance = new WebLocationService.TransitCountry();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

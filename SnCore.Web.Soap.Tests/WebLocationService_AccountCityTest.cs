using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebLocationServiceTests
{
    [TestFixture]
    public class AccountCityTest : WebServiceBaseTest<WebLocationServiceNoCache>
    {
        private UserInfo _user = null;

        [SetUp]
        public void SetUp()
        {
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteUser(_user.id);
        }

        public AccountCityTest()
        {

        }

        [Test]
        public void GetAccountCitiesTest()
        {
            WebLocationService.TransitAccountCity[] t_collection = EndPoint.GetAccountCities(
                _user.ticket, null);
            Console.WriteLine("Cities: {0}", t_collection.Length);
            foreach (WebLocationService.TransitAccountCity t_instance in t_collection)
            {
                Console.WriteLine("{0} ({1}, {2} => {4}): {3}", 
                    t_instance.Name, t_instance.Country, t_instance.State, t_instance.Total, t_instance.Id);
            }
        }
    }
}

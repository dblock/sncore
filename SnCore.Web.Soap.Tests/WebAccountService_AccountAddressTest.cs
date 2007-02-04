using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebLocationServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountAddressTest : AccountBaseTest<WebAccountService.TransitAccountAddress>
    {
        public CityTest _city = new CityTest();
        public int _city_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _city.SetUp();
            _city_id = _city.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _city.Delete(GetAdminTicket(), _city_id);
            _city.TearDown();
        }

        public AccountAddressTest()
            : base("AccountAddress", "AccountAddresses")
        {

        }

        public override WebAccountService.TransitAccountAddress GetTransitInstance()
        {
            WebAccountService.TransitAccountAddress t_instance = new WebAccountService.TransitAccountAddress();
            t_instance.AccountId = _account_id;
            t_instance.Apt = "#1";
            t_instance.Name = GetNewString();
            t_instance.Street = GetNewString();
            t_instance.Zip = "10001";
            t_instance.Country = (string)_city._state._country.GetInstancePropertyById(GetAdminTicket(), _city._state._country_id, "Name");
            t_instance.State = (string)_city._state.GetInstancePropertyById(GetAdminTicket(), _city._state_id, "Name");
            t_instance.City = (string)_city.GetInstancePropertyById(GetAdminTicket(), _city_id, "Name");
            return t_instance;
        }
    }
}

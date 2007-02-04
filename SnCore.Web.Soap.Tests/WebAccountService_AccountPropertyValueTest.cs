using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPropertyValueTest : AccountBaseTest<WebAccountService.TransitAccountPropertyValue>
    {
        public AccountPropertyTest _property = new AccountPropertyTest();
        public int _property_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _property.SetUp();
            _property_id = _property.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _property.Delete(GetAdminTicket(), _property_id);
            _property.TearDown();
        }

        public AccountPropertyValueTest()
            : base("AccountPropertyValue")
        {
        }


        public override WebAccountService.TransitAccountPropertyValue GetTransitInstance()
        {
            WebAccountService.TransitAccountPropertyValue t_instance = new WebAccountService.TransitAccountPropertyValue();
            t_instance.AccountId = _account_id;
            t_instance.AccountPropertyId = _property_id;
            t_instance.Value = GetNewString();
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _account_id, _property._group_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _account_id, _property._group_id };
            return args;
        }

        [Test]
        public void GetAllAccountPropertyValuesTest()
        {
            int property1_id = _property.Create(GetAdminTicket());
            WebAccountService.TransitAccountPropertyValue t_value1 = GetTransitInstance();
            t_value1.AccountPropertyId = property1_id;
            t_value1.AccountId = GetUserAccount().Id;
            t_value1.Value = GetNewString();
            t_value1.Id = Create(GetUserTicket(), t_value1);
            int property2_id = _property.Create(GetAdminTicket());
            WebAccountService.TransitAccountPropertyValue t_value2 = GetTransitInstance();
            t_value2.AccountPropertyId = property2_id;
            t_value2.AccountId = GetUserAccount().Id;
            t_value2.Value = GetNewString();
            t_value2.Id = Create(GetUserTicket(), t_value2);
            WebAccountService.TransitAccountPropertyValue[] values = EndPoint.GetAllAccountPropertyValues(
                GetUserTicket(), GetUserAccount().Id, _property._group_id);
            Console.WriteLine("Properties: {0}", values.Length);
            Delete(GetUserTicket(), t_value1.Id);
            Delete(GetUserTicket(), t_value2.Id);
            _property.Delete(GetAdminTicket(), property1_id);
            _property.Delete(GetAdminTicket(), property2_id);
        }

        [Test]
        public void GetAccountPropertyValueByNameTest()
        {
            string groupname = (string) _property._group.GetInstancePropertyById(GetUserTicket(), _property._group_id, "Name");
            string propertyname = (string) _property.GetInstancePropertyById(GetUserTicket(), _property_id, "Name");
            Console.WriteLine("Property: {0}/{1}", groupname, propertyname);
            WebAccountService.TransitAccountPropertyValue t_value = GetTransitInstance();
            t_value.AccountPropertyId = _property_id;
            t_value.AccountId = GetUserAccount().Id;
            t_value.Value = GetNewString();
            t_value.Id = Create(GetUserTicket(), t_value);
            WebAccountService.TransitAccountPropertyValue t_instance = EndPoint.GetAccountPropertyValueByName(
                GetUserTicket(), GetUserAccount().Id, groupname, propertyname);
            Console.WriteLine("Value: {0}:{1}", t_instance.Id, t_instance.Value);
            Delete(GetUserTicket(), t_value.Id);
        }

        [Test]
        public void GetAccountsByPropertyValueTest()
        {
            string groupname = (string)_property._group.GetInstancePropertyById(GetUserTicket(), _property._group_id, "Name");
            string propertyname = (string)_property.GetInstancePropertyById(GetUserTicket(), _property_id, "Name");
            Console.WriteLine("Property: {0}/{1}", groupname, propertyname);
            WebAccountService.TransitAccountPropertyValue t_value = GetTransitInstance();
            t_value.AccountPropertyId = _property_id;
            t_value.AccountId = GetUserAccount().Id;
            t_value.Value = GetNewString();
            t_value.Id = Create(GetUserTicket(), t_value);
            int count = EndPoint.GetAccountsByPropertyValueCount(
                GetUserTicket(), groupname, propertyname, t_value.Value);
            Assert.AreEqual(count, 1);
            WebAccountService.TransitAccount[] t_accounts = EndPoint.GetAccountsByPropertyValue(
                GetUserTicket(), groupname, propertyname, t_value.Value, null);
            Assert.AreEqual(t_accounts.Length, 1);
            Assert.AreEqual(t_accounts[0].Id, GetUserAccount().Id);
            Delete(GetUserTicket(), t_value.Id);
        }
    }
}

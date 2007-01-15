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
            t_instance.Value = Guid.NewGuid().ToString();
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
        protected void GetAllAccountPropertyValuesTest()
        {

        }

        [Test]
        protected void GetAccountPropertyValueByNameTest()
        {

        }

        [Test]
        protected void GetAccountsByPropertyValueTest()
        {

        }
    }
}

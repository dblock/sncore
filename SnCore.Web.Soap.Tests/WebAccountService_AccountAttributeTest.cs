using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebObjectServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountAttributeTest : AccountBaseTest<WebAccountService.TransitAccountAttribute>
    {
        public AttributeTest _attribute = new AttributeTest();
        public int _attribute_id = 0;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _attribute.SetUp();
            _attribute_id = _attribute.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _attribute.Delete(GetAdminTicket(), _attribute_id);
            _attribute.TearDown();
            base.TearDown();
        }

        public AccountAttributeTest()
            : base("AccountAttribute")
        {
        }

        public override string GetTestTicket()
        {
            return GetAdminTicket();
        }

        public override WebAccountService.TransitAccountAttribute GetTransitInstance()
        {
            WebAccountService.TransitAccountAttribute t_instance = new WebAccountService.TransitAccountAttribute();
            t_instance.AccountId = _account_id;
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Value = Guid.NewGuid().ToString();
            t_instance.AttributeId = _attribute_id;
            return t_instance;
        }
    }
}

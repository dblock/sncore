using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountPropertyTest : WebServiceTest<WebAccountService.TransitAccountProperty, WebAccountServiceNoCache>
    {
        public AccountPropertyGroupTest _group = new AccountPropertyGroupTest();
        public int _group_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group.SetUp();
            _group_id = _group.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _group.Delete(GetAdminTicket(), _group_id);
        }

        public AccountPropertyTest()
            : base("AccountProperty", "AccountProperties")
        {
        }


        public override WebAccountService.TransitAccountProperty GetTransitInstance()
        {
            WebAccountService.TransitAccountProperty t_instance = new WebAccountService.TransitAccountProperty();
            t_instance.AccountPropertyGroupId = _group_id;
            t_instance.DefaultValue = GetNewString();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Publish = true;
            t_instance.Type = "System.String";
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _group_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _group_id };
            return args;
        }
    }
}

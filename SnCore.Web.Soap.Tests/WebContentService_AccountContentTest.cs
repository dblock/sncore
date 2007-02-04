using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebContentServiceTests
{
    [TestFixture]
    public class AccountContentTest : WebServiceTest<WebContentService.TransitAccountContent, WebContentServiceNoCache>
    {
        private AccountContentGroupTest _group = new AccountContentGroupTest();
        int _group_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _group_id = _group.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _group.Delete(GetAdminTicket(), _group_id);
        }

        public AccountContentTest()
            : base("AccountContent")
        {

        }

        public override WebContentService.TransitAccountContent GetTransitInstance()
        {
            WebContentService.TransitAccountContent t_instance = new WebContentService.TransitAccountContent();
            t_instance.AccountContentGroupId = _group_id;
            t_instance.Tag = GetNewString();
            t_instance.Text = GetNewString();
            t_instance.Timestamp = DateTime.UtcNow;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _group_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _group_id, options };
            return args;
        }
    }
}

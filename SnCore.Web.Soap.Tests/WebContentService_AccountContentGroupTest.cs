using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebContentServiceTests
{
    [TestFixture]
    public class AccountContentGroupTest : WebServiceTest<WebContentService.TransitAccountContentGroup, WebContentServiceNoCache>
    {
        public AccountContentGroupTest()
            : base("AccountContentGroup")
        {

        }

        public override WebContentService.TransitAccountContentGroup GetTransitInstance()
        {
            WebContentService.TransitAccountContentGroup t_instance = new WebContentService.TransitAccountContentGroup();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Description = GetNewString();
            t_instance.Login = true;
            t_instance.Name = GetNewString();
            t_instance.Trusted = false;
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetAdminAccount().Id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetAdminAccount().Id, options };
            return args;
        }
    }
}

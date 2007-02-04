using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountEmailMessageTest : WebServiceTest<WebAccountService.TransitAccountEmailMessage, WebAccountServiceNoCache>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        public AccountEmailMessageTest()
            : base("AccountEmailMessage")
        {

        }

        public override WebAccountService.TransitAccountEmailMessage GetTransitInstance()
        {
            WebAccountService.TransitAccountEmailMessage t_instance = new WebAccountService.TransitAccountEmailMessage();
            t_instance.Body = GetNewString();
            t_instance.DeleteSent = true;
            t_instance.MailFrom = GetNewEmailAddress();
            t_instance.MailTo = GetNewEmailAddress();
            t_instance.Subject = GetNewString();
            t_instance.Sent = false;
            t_instance.AccountId = GetAdminAccount().Id;
            return t_instance;
        }
    }
}

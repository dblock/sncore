using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class AccountRssWatchTest : WebServiceTest<WebSyndicationService.TransitAccountRssWatch, WebSyndicationServiceNoCache>
    {
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            DeleteUser(_user.id);
        }

        public AccountRssWatchTest()
            : base("AccountRssWatch")
        {

        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _user.id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _user.id, options };
            return args;
        }

        public override WebSyndicationService.TransitAccountRssWatch GetTransitInstance()
        {
            WebSyndicationService.TransitAccountRssWatch t_instance = new WebSyndicationService.TransitAccountRssWatch();
            t_instance.AccountId = _user.id;
            t_instance.Name = GetNewString();
            t_instance.Enabled = true;
            t_instance.UpdateFrequency = 24;
            t_instance.Url = GetNewUri();
            return t_instance;
        }

        [Test]
        protected void GetAccountRssWatchItemsTest()
        {

        }
    }
}

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
        [SetUp]
        public override void SetUp()
        {
        }

        [TearDown]
        public override void TearDown()
        {
        }

        public AccountRssWatchTest()
            : base("AccountRssWatch")
        {

        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, GetUserAccount().Id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, GetUserAccount().Id, options };
            return args;
        }

        public override WebSyndicationService.TransitAccountRssWatch GetTransitInstance()
        {
            WebSyndicationService.TransitAccountRssWatch t_instance = new WebSyndicationService.TransitAccountRssWatch();
            t_instance.AccountId = GetUserAccount().Id;
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

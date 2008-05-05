using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class AccountFeedItemMediaTest : WebServiceTest<WebSyndicationService.TransitAccountFeedItemMedia, WebSyndicationServiceNoCache>
    {
        private AccountFeedItemTest _accountfeeditem = new AccountFeedItemTest();
        private int _accountfeeditem_id = 0;
        private UserInfo _user = null;

        [SetUp]
        public override void SetUp()
        {
            _accountfeeditem.SetUp();
            _accountfeeditem_id = _accountfeeditem.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            _accountfeeditem.Delete(GetAdminTicket(), _accountfeeditem_id);
            _accountfeeditem.TearDown();
            DeleteUser(_user.id);
        }

        public AccountFeedItemMediaTest()
            : base("AccountFeedItemMedia")
        {

        }

        public override WebSyndicationService.TransitAccountFeedItemMedia GetTransitInstance()
        {
            WebSyndicationService.TransitAccountFeedItemMedia t_instance = new WebSyndicationService.TransitAccountFeedItemMedia();
            t_instance.AccountFeedItemId = _accountfeeditem_id;
            t_instance.AccountId = _user.id;
            t_instance.EmbeddedHtml = GetNewString();
            t_instance.Type = GetNewString();
            return t_instance;
        }


        public override object[] GetCountArgs(string ticket)
        {
            WebSyndicationService.TransitAccountFeedItemMediaQueryOptions qopt = new WebSyndicationService.TransitAccountFeedItemMediaQueryOptions();
            qopt.InterestingOnly = false;
            qopt.VisibleOnly = false;
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebSyndicationService.TransitAccountFeedItemMediaQueryOptions qopt = new WebSyndicationService.TransitAccountFeedItemMediaQueryOptions();
            qopt.InterestingOnly = false;
            qopt.VisibleOnly = false;
            object[] args = { ticket, qopt, options };
            return args;
        }
    }
}

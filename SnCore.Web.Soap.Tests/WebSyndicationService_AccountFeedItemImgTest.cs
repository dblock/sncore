using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class AccountFeedItemImgTest : WebServiceTest<WebSyndicationService.TransitAccountFeedItemImg, WebSyndicationServiceNoCache>
    {
        private AccountFeedItemTest _accountfeeditem = new AccountFeedItemTest();
        private int _accountfeeditem_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _accountfeeditem.SetUp();
            _accountfeeditem_id = _accountfeeditem.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _accountfeeditem.Delete(GetAdminTicket(), _accountfeeditem_id);
            _accountfeeditem.TearDown();
        }

        public AccountFeedItemImgTest()
            : base("AccountFeedItemImg")
        {

        }

        public override WebSyndicationService.TransitAccountFeedItemImg GetTransitInstance()
        {
            WebSyndicationService.TransitAccountFeedItemImg t_instance = new WebSyndicationService.TransitAccountFeedItemImg();
            t_instance.AccountFeedItemId = _accountfeeditem_id;
            t_instance.AccountId = GetUserAccount().Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Thumbnail = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 10, 240, 100);
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }


        public override object[] GetCountArgs(string ticket)
        {
            WebSyndicationService.TransitAccountFeedItemImgQueryOptions qopt = new WebSyndicationService.TransitAccountFeedItemImgQueryOptions();
            qopt.InterestingOnly = false;
            qopt.VisibleOnly = false;
            object[] args = { ticket, qopt };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            WebSyndicationService.TransitAccountFeedItemImgQueryOptions qopt = new WebSyndicationService.TransitAccountFeedItemImgQueryOptions();
            qopt.InterestingOnly = false;
            qopt.VisibleOnly = false;
            object[] args = { ticket, qopt, options };
            return args;
        }
    }
}

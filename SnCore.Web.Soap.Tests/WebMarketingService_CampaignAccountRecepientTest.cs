using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebMarketingServiceTests
{
    [TestFixture]
    public class CampaignAccountRecepientTest : WebServiceTest<WebMarketingService.TransitCampaignAccountRecepient, WebMarketingServiceNoCache>
    {
        private CampaignTest _campaign = new CampaignTest();
        private int _campaign_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _campaign_id = _campaign.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _campaign.Delete(GetAdminTicket(), _campaign_id);
        }

        public CampaignAccountRecepientTest()
            : base("CampaignAccountRecepient")
        {

        }

        public override WebMarketingService.TransitCampaignAccountRecepient GetTransitInstance()
        {
            WebMarketingService.TransitCampaignAccountRecepient t_instance = new WebMarketingService.TransitCampaignAccountRecepient();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.CampaignId = _campaign_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _campaign_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _campaign_id };
            return args;
        }

        [Test]
        public void ImportCampaignAccountRecepientsTest()
        {
            WebMarketingService.TransitCampaignAccountRecepient recepient1 = new WebMarketingService.TransitCampaignAccountRecepient();
            recepient1.AccountId = GetUserAccount().Id;
            recepient1.CampaignId = _campaign_id;
            WebMarketingService.TransitCampaignAccountRecepient recepient2 = new WebMarketingService.TransitCampaignAccountRecepient();
            recepient2.AccountId = GetAdminAccount().Id;
            recepient2.CampaignId = _campaign_id;
            WebMarketingService.TransitCampaignAccountRecepient[] recepients = { recepient1, recepient2, recepient1 };
            int count = EndPoint.ImportCampaignAccountRecepients(GetAdminTicket(), recepients);
            Assert.AreEqual(2, count);
            count = EndPoint.ImportCampaignAccountRecepients(GetAdminTicket(), recepients);
            Assert.AreEqual(0, count);
            count = EndPoint.GetCampaignAccountRecepientsCount(GetAdminTicket(), _campaign_id);
            Assert.AreEqual(count, 2);
            EndPoint.DeleteCampaignAccountRecepients(GetAdminTicket(), _campaign_id);
            count = EndPoint.GetCampaignAccountRecepientsCount(GetAdminTicket(), _campaign_id);
            Assert.AreEqual(count, 0);
        }

        [Test]
        public void ImportCampaignAccountEmailsTest()
        {
            int count_unverified = EndPoint.ImportCampaignAccountEmails(GetAdminTicket(), _campaign_id, true, false);
            Console.WriteLine("Imported {0} recepients.", count_unverified);
            int count_verified = EndPoint.ImportCampaignAccountEmails(GetAdminTicket(), _campaign_id, false, true);
            Console.WriteLine("Imported {0} recepients.", count_verified);
            Assert.IsTrue(count_unverified + count_verified > 0);
            int count_all = EndPoint.ImportCampaignAccountEmails(GetAdminTicket(), _campaign_id, true, true);
            Assert.AreEqual(0, count_all, "There should be no e-mails left.");
            EndPoint.DeleteCampaignAccountRecepients(GetAdminTicket(), _campaign_id);
            int count_all_2 = EndPoint.ImportCampaignAccountEmails(GetAdminTicket(), _campaign_id, true, true);
            Console.WriteLine("Imported {0} recepients.", count_all_2);
            Assert.AreEqual(count_unverified + count_verified, count_all_2, "Campaign import e-mail counts don't match.");
        }

        [Test]
        protected void ImportCampaignAccountPropertyValuesTest()
        {
            // TODO: when AccountPropertyValues are implemented
            // EndPoint.ImportCampaignAccountPropertyValues();
        }
    }
}

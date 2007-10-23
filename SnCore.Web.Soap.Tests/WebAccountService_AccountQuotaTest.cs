using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountQuotaTest : AccountBaseTest<WebAccountService.TransitAccountQuota>
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

        public AccountQuotaTest()
            : base("AccountQuota")
        {
        }

        public override WebAccountService.TransitAccountQuota GetTransitInstance()
        {
            WebAccountService.TransitAccountQuota t_instance = new WebAccountService.TransitAccountQuota();
            t_instance.AccountId = _account_id;
            t_instance.DataObjectName = "Place";
            t_instance.Limit = 10;
            return t_instance;
        }

        public override string GetTestTicket()
        {
            // only the administrator can create quotas
            return base.GetAdminTicket();
        }

        [Test]
        public void GetAccountQuotaByObjectNameTest()
        {
            WebAccountService.TransitAccountQuota t_instance = GetTransitInstance();
            int id = Create(GetAdminTicket(), t_instance);
            WebAccountService.TransitAccountQuota t_instance_copy = EndPoint.GetAccountQuotaByObjectName(
                GetAdminTicket(), t_instance.AccountId, t_instance.DataObjectName);
            Assert.IsNotNull(t_instance_copy);
            Assert.AreEqual(t_instance_copy.Id, id);
            Delete(GetAdminTicket(), id);
        }

        [Test]
        public void WithQuotaTest()
        {
            int max = 100;
            string ticket = base.GetTestTicket();

            WebAccountService.TransitAccountQuota t_instance = new WebAccountService.TransitAccountQuota();
            t_instance.AccountId = _account_id;
            t_instance.DataObjectName = "AccountWebsite";
            t_instance.Limit = 6;
            t_instance.Id = EndPoint.CreateOrUpdateAccountQuota(GetAdminTicket(), t_instance);
            Console.WriteLine("Quota id: {0}", t_instance.Id);

            for (int i = 0; i < t_instance.Limit; i++)
            {
                WebAccountService.TransitAccountWebsite website = new WebAccountService.TransitAccountWebsite();
                website.Name = GetNewString();
                website.Url = GetNewUri();
                int id = EndPoint.CreateOrUpdateAccountWebsite(ticket, website);
                Console.WriteLine("Created website: {0}:{1}", website.Url, id);
            }

            try
            {
                WebAccountService.TransitAccountWebsite website = new WebAccountService.TransitAccountWebsite();
                website.Name = GetNewString();
                website.Url = GetNewUri();
                EndPoint.CreateOrUpdateAccountWebsite(ticket, website);
                Assert.IsTrue(false, "Missing exception at quota limit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception at quota limit: {0}", ex.Message);
            }

            EndPoint.DeleteAccountQuota(GetAdminTicket(), t_instance.Id);

            for (int i = t_instance.Limit; i < max; i++)
            {
                WebAccountService.TransitAccountWebsite website = new WebAccountService.TransitAccountWebsite();
                website.Name = GetNewString();
                website.Url = GetNewUri();
                int id = EndPoint.CreateOrUpdateAccountWebsite(ticket, website);
                Console.WriteLine("Created website: {0}:{1}", website.Url, id);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebStatsServiceTests
{
    [TestFixture]
    public class RefererAccountTest : WebServiceTest<WebStatsService.TransitRefererAccount, WebStatsServiceNoCache>
    {
        RefererHostTest _host = new RefererHostTest();
        private int _host_id = 0;
        private UserInfo _user = null;

        public RefererAccountTest()
            : base("RefererAccount")
        {

        }

        [SetUp]
        public override void SetUp()
        {
            _host_id = _host.Create(GetAdminTicket());
            _user = CreateUserWithVerifiedEmailAddress();
        }

        [TearDown]
        public override void TearDown()
        {
            _host.Delete(GetAdminTicket(), _host_id);
            DeleteUser(_user.id);
        }

        public override WebStatsService.TransitRefererAccount GetTransitInstance()
        {
            WebStatsService.TransitRefererAccount t_instance = new WebStatsService.TransitRefererAccount();
            t_instance.AccountId = _user.id;
            t_instance.RefererHostLastRefererUri = GetNewUri();
            t_instance.RefererHostName = (string)_host.GetInstancePropertyById(GetAdminTicket(), _host_id, "Host");
            t_instance.RefererHostTotal = 1;
            return t_instance;
        }

        [Test]
        public void FindRefererAccountsTest()
        {
            WebAccountServiceTests.AccountWebsiteTest websitetest = new WebAccountServiceTests.AccountWebsiteTest();
            WebAccountService.TransitAccountWebsite t_website = websitetest.GetTransitInstance();
            t_website.AccountId = _user.id;
            int website_id = websitetest.Create(_user.ticket, t_website);
            try
            {
                WebStatsService.TransitAccount[] t_accounts = EndPoint.FindRefererAccounts(_user.ticket, t_website.Url, null);
                Assert.IsNotNull(t_accounts);
                Console.WriteLine("Accounts: {0}", t_accounts.Length);
                Assert.AreEqual(1, t_accounts.Length);
                Assert.AreEqual(t_accounts[0].Id, _user.id);
            }
            finally
            {
                websitetest.Delete(_user.ticket, website_id);
            }
        }
    }
}

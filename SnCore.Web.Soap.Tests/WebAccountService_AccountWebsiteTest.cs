using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountWebsiteTest : AccountBaseTest<WebAccountService.TransitAccountWebsite>
    {
        public AccountWebsiteTest()
            : base("AccountWebsite")
        {

        }

        public override WebAccountService.TransitAccountWebsite GetTransitInstance()
        {
            WebAccountService.TransitAccountWebsite t_instance = new WebAccountService.TransitAccountWebsite();
            t_instance.AccountId = GetTestAccountId();
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Url = GetNewUri();
            return t_instance;
        }

        [Test]
        public void WithQuotaTest()
        {
            string email = GetNewEmailAddress();
            string password = GetNewString();
            int user_id = CreateUser(email, password);
            string ticket = Login(email, password);

            const int max = 100;
            for (int i = 0; i < max; i++)
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

            DeleteUser(user_id);
        }
    }
}

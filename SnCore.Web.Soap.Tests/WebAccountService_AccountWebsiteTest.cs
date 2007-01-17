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
            t_instance.AccountId = _account_id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://www.{0}.com", Guid.NewGuid());
            return t_instance;
        }

        [Test]
        public void TestWithQuota()
        {
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            string password = Guid.NewGuid().ToString();
            int user_id = CreateUser(email, password);
            string ticket = Login(email, password);

            const int max = 100;
            for (int i = 0; i < max; i++)
            {
                WebAccountService.TransitAccountWebsite website = new WebAccountService.TransitAccountWebsite();
                website.Name = Guid.NewGuid().ToString();
                website.Url = string.Format("http://uri/{0}", Guid.NewGuid());
                int id = EndPoint.CreateOrUpdateAccountWebsite(ticket, website);
                Console.WriteLine("Created website: {0}:{1}", website.Url, id);
            }

            try
            {
                WebAccountService.TransitAccountWebsite website = new WebAccountService.TransitAccountWebsite();
                website.Name = Guid.NewGuid().ToString();
                website.Url = string.Format("http://uri/{0}", Guid.NewGuid());
                EndPoint.CreateOrUpdateAccountWebsite(ticket, website);
                Assert.IsTrue(false, "Missing exception at quota limit.");
            }
            catch (SoapException ex)
            {
                Console.WriteLine("Exception at quota limit: {0}", ex.Message);
            }

            DeleteUser(user_id);
        }
    }
}

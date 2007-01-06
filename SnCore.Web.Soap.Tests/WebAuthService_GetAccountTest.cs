using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebAuthServiceTests
{

    [TestFixture]
    public class GetAccountTest
    {
        [Test]
        public void TestGetAccount()
        {
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Password = Guid.NewGuid().ToString();
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            int id = account_endpoint.CreateAccount(string.Empty, email, t_instance);
            Assert.IsTrue(id > 0);
            WebAuthService.WebAuthService auth_endpoint = new WebAuthService.WebAuthService();
            string ticket = auth_endpoint.Login(email, t_instance.Password);
            int id2 = auth_endpoint.GetAccountId(ticket);
            Assert.AreEqual(id, id2);
            WebAuthService.TransitAccount t_instance2 = auth_endpoint.GetAccount(ticket);
            Assert.AreEqual(t_instance2.Id, id);
            Assert.AreEqual(t_instance2.Name, t_instance.Name);
        }
    }
}

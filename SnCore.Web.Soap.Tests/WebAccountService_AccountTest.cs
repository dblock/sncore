using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        [Test]
        public void TestGetAccount()
        {
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Password = Guid.NewGuid().ToString();
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            int id = EndPoint.CreateAccount(string.Empty, email, t_instance);
            Assert.IsTrue(id > 0);
            string ticket = EndPoint.Login(email, t_instance.Password);
            int id2 = EndPoint.GetAccountId(ticket);
            Assert.AreEqual(id, id2);
            WebAccountService.TransitAccount t_instance2 = EndPoint.GetAccount(ticket);
            Assert.AreEqual(t_instance2.Id, id);
            Assert.AreEqual(t_instance2.Name, t_instance.Name);
        }

        [Test]
        public void GetAccountIdTest()
        {
            Assert.AreEqual(GetAdminAccount().Id, EndPoint.GetAccountId(GetAdminTicket()));
            Assert.AreEqual(GetUserAccount().Id, EndPoint.GetAccountId(GetUserTicket()));
            Assert.AreNotEqual(GetAdminAccount().Id, EndPoint.GetAccountId(GetUserTicket()));
        }

        [Test]
        public void GetAccountByIdTest()
        {
            WebAccountService.TransitAccount t_instance = EndPoint.GetAccountById(GetUserTicket(), GetUserAccount().Id);
            Assert.IsTrue(string.IsNullOrEmpty(t_instance.Password));
            Assert.AreEqual(t_instance.Id, GetUserAccount().Id);
        }

        [Test]
        public void FindByEmailTest()
        {
            WebAccountService.TransitAccount t_instance = EndPoint.FindByEmail(GetUserTicket(), "admin@localhost.com");
            Assert.AreEqual(t_instance.Id, GetAdminAccount().Id);
        }
    }
}

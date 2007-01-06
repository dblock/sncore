using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebAuthServiceTests
{
    [TestFixture]
    public class SignUpTest
    {
        [Test]
        public void TestAccountCRUD()
        {
            //signup
            string email = string.Format("{0}@localhost.com", Guid.NewGuid());
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Password = Guid.NewGuid().ToString();
            WebAccountService.WebAccountService account_endpoint = new WebAccountService.WebAccountService();
            WebAuthService.WebAuthService auth_endpoint = new WebAuthService.WebAuthService();
            int id = account_endpoint.CreateAccount(string.Empty, email, t_instance);
            Console.WriteLine("Created account: {0}", id);
            Assert.IsTrue(id > 0);
            // login
            string ticket = auth_endpoint.Login(email, t_instance.Password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            // retreive
            WebAccountService.TransitAccount t_instance2 = account_endpoint.GetAccountById(ticket, id);
            Console.WriteLine("Retreived account: {0}", t_instance2.Id);
            Assert.AreEqual(id, t_instance2.Id);
            // update
            t_instance2.Name = Guid.NewGuid().ToString();
            account_endpoint.CreateOrUpdateAccount(ticket, t_instance2);
            // object is cached, use admin ticket
            string ticket2 = auth_endpoint.Login("admin@localhost.com", "password");
            // check that the name was updated
            WebAccountService.TransitAccount t_instance3 = account_endpoint.GetAccountById(ticket2, id);
            Console.WriteLine("Retreived account: {0}", t_instance3.Id);
            Assert.AreEqual(id, t_instance3.Id);
            Assert.AreEqual(t_instance2.Name, t_instance3.Name);
            // delete
            account_endpoint.DeleteAccount(ticket, t_instance.Password);
            Console.WriteLine("Deleted account: {0}", id);
        }
    }    
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class SignUpTest : WebServiceBaseTest<WebAccountServiceNoCache>
    {
        [Test]
        public void CreateAccountTest()
        {
            //signup
            string email = GetNewEmailAddress();
            WebAccountService.TransitAccount t_instance = new WebAccountService.TransitAccount();
            t_instance.Birthday = DateTime.UtcNow.AddYears(-10);
            t_instance.Name = GetNewString();
            t_instance.Password = GetNewString();
            int id = EndPoint.CreateAccount(string.Empty, email, t_instance);
            Console.WriteLine("Created account: {0}", id);
            Assert.IsTrue(id > 0);
            // login
            string ticket = EndPoint.Login(email, t_instance.Password);
            Assert.IsFalse(string.IsNullOrEmpty(ticket));
            // retreive
            WebAccountService.TransitAccount t_instance2 = EndPoint.GetAccountById(ticket, id);
            Console.WriteLine("Retreived account: {0}", t_instance2.Id);
            Assert.AreEqual(id, t_instance2.Id);
            // update
            t_instance2.Name = GetNewString();
            EndPoint.CreateOrUpdateAccount(ticket, t_instance2);
            // object may be cached, use admin ticket
            string ticket2 = EndPoint.Login("admin@localhost.com", "password");
            // check that the name was updated
            WebAccountService.TransitAccount t_instance3 = EndPoint.GetAccountById(ticket2, id);
            Console.WriteLine("Retreived account: {0}", t_instance3.Id);
            Assert.AreEqual(id, t_instance3.Id);
            Assert.AreEqual(t_instance2.Name, t_instance3.Name);
            // delete
            EndPoint.DeleteAccount(ticket, t_instance.Password);
            Console.WriteLine("Deleted account: {0}", id);
        }
    }    
}

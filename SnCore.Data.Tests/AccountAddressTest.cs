using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Expression;
using System.Collections.Generic;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountAddressTest : NHibernateTest
    {
        public AccountAddressTest()
        {

        }

        [Test]
        public void TestCrud()
        {
            Account acct = new Account();
            AccountAddress addr = new AccountAddress();
            Country c = new Country();
            State s = new State();
                
            try
            {
                c.Name = Guid.NewGuid().ToString();
                Session.Save(c);

                s.Name = Guid.NewGuid().ToString();
                s.Country = c;
                Session.Save(s);

                acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
                acct.Name = "Test User";
                acct.Password = "password";
                acct.Birthday = new DateTime(1976, 9, 7);
                acct.Enabled = false;
                Session.Save(acct);

                addr.Account = acct;

                addr.Apt = "6G";
                addr.City = "New York";
                addr.Created = addr.Modified = DateTime.UtcNow;

                addr.Country = c;
                addr.State = s;

                addr.Street = "89 Bleecker St.";
                addr.Zip = "10012-1234";

                if (acct.AccountAddresses == null) acct.AccountAddresses = new List<AccountAddress>();
                acct.AccountAddresses.Add(addr);

                Session.Save(addr);
                Session.Flush();

                Assert.IsTrue(addr.Id > 0);
                Assert.IsTrue(acct.Id > 0);
            }
            finally
            {
                Session.Delete(acct);
                Session.Delete(s);
                Session.Delete(c);
                Session.Flush();
            }
        }
    }
}

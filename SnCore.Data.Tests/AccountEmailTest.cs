using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountEmailTest : NHibernateTest
    {
        [Test]
        public void TestCrud()
        {
            Account acct = new Account();
            acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
            acct.Name = "Test User";
            acct.Password = "password";
            acct.Birthday = new DateTime(1976, 9, 7);

            AccountEmail email = new AccountEmail();
            email.Account = acct;

            email.Address = "foo@bar.com";
            email.Verified = false;
            email.Failed = false;
            email.Created = email.Modified = DateTime.UtcNow;

            if (acct.AccountEmails == null) acct.AccountEmails = new List<AccountEmail>();
            acct.AccountEmails.Add(email);

            Session.Save(acct);
            Session.Save(email);
            Session.Flush();

            Assert.IsTrue(email.Id > 0);
            Assert.IsTrue(acct.Id > 0);

            Session.Delete(acct);
            Session.Flush();
        }
    }
}

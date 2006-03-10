using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using System.Collections;
using System.Collections.Generic;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountEmailMessageTest : NHibernateTest
    {
        [Test]
        public void TestCrud()
        {
            Account acct = new Account();
            AccountEmailMessage email = new AccountEmailMessage();

            try
            {
                acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
                acct.Name = "Test User";
                acct.Password = "password";
                acct.Birthday = new DateTime(1976, 9, 7);

                email.Account = acct;
                email.Body = "Hello World";
                email.DeleteSent = false;
                email.MailFrom = "foo@bar.com";
                email.MailTo = "bar@foo.com";
                email.SendError = string.Empty;
                email.Sent = false;
                email.Subject = "no subject";
                email.Created = email.Modified = DateTime.UtcNow;

                if (acct.AccountEmailMessages == null) acct.AccountEmailMessages = new ArrayList();
                acct.AccountEmailMessages.Add(email);

                Session.Save(acct);
                Session.Save(email);
                Session.Flush();

                Assert.IsTrue(email.Id > 0);
                Assert.IsTrue(acct.Id > 0);
            }
            finally
            {
                Session.Delete(acct);
            }

            Session.Flush();
        }
    }
}

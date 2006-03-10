using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace SnCore.Data.Tests
{
    [TestFixture]
    class AccountRightsTest : NHibernateTest
    {
        public AccountRightsTest()
        {

        }

        [Test]
        public void TestCrud()
        {
            Account acct = new Account();
            DataObject obj = new DataObject();
            AccountRight right = new AccountRight();

            try
            {
                acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
                acct.Name = "Test User";
                acct.Password = "password";
                Session.Save(acct);
                Session.Flush();

                obj.Name = "Test Object";
                Session.Save(obj);

                right.Account = acct;
                right.DataObject = obj;
                right.AllowCreate = right.AllowDelete = right.AllowRetrieve = right.AllowUpdate;
                Session.Save(right);

                Assert.IsTrue(acct.Id > 0);
                Assert.IsTrue(obj.Id > 0);
                Assert.IsTrue(right.Id > 0);
            }
            finally
            {
                Session.Delete(acct);
                Session.Delete(obj);
                Session.Delete(right);
            }

            Session.Flush();
        }
    }
}

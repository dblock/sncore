using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using System.Collections;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountpicTest : NHibernateTest
    {
        [Test]
        public void TestCrud()
        {
            Account acct = new Account();
            AccountPicture pic = new AccountPicture();

            try
            {
                acct.Created = acct.LastLogin = acct.Modified = DateTime.UtcNow;
                acct.Name = "Test User";
                acct.Password = "password";
                acct.Birthday = new DateTime(1976, 9, 7);

                pic.Account = acct;
                byte[] myBytes = { 0x01, 0x02, 0x03 };
                pic.Bitmap = myBytes;
                pic.Name = "Name";
                pic.Description = "Description";
                pic.Created = pic.Modified = DateTime.UtcNow;

                if (acct.AccountPictures == null) acct.AccountPictures = new ArrayList();
                acct.AccountPictures.Add(pic);

                Session.Save(acct);
                Session.Save(pic);
                Session.Flush();

                Assert.IsTrue(pic.Id > 0);
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

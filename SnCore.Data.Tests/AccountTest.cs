using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountTest : NHibernateCrudTest
    {
        public Account mAccount = null;

        public AccountTest()
        {
            mAccount = new Account();
            mAccount.Name = Guid.NewGuid().ToString();
            mAccount.Password = "password";
            mAccount.Enabled = false;
            mAccount.Birthday = new DateTime(1976, 9, 7);
            mAccount.Created = mAccount.Modified = mAccount.LastLogin = DateTime.UtcNow;
        }

        public override object Object
        {
            get
            {
                return mAccount;
            }
        }

        public override int ObjectId
        {
            get
            {
                return mAccount.Id;
            }
        }
    }
}

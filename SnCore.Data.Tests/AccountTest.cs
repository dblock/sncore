using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    [TestFixture]
    public class AccountTest : NHibernateCrudTest<Account>
    {
        public AccountTest()
        {
        }

        public override Account GetNewObject()
        {
            Account obj = new Account();
            obj.Name = Guid.NewGuid().ToString();
            obj.Password = "password";
            obj.Enabled = false;
            obj.Birthday = new DateTime(1976, 9, 7);
            obj.Created = obj.Modified = obj.LastLogin = DateTime.UtcNow;
            return obj;
        }
    }
}

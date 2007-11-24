using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using SnCore.Data.Hibernate;
using System.Data.SqlClient;

namespace SnCore.Data.Tests
{
    /// <summary>
    /// NHibernate test foundation.
    /// </summary>
    public class NHibernateTest
    {
        private ISession mSession;

        public NHibernateTest()
        {

        }

        public ISession Session
        {
            get
            {
                return mSession;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            mSession = SnCore.Data.Hibernate.Session.Factory.OpenSession();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Session.Close();
        }

        public static string GetNewEmailAddress()
        {
            return string.Format("{0}@localhost.com", GetNewString());
        }

        public static string GetNewUri()
        {
            return string.Format("http://uri/{0}", GetNewString());
        }

        public static string GetNewString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

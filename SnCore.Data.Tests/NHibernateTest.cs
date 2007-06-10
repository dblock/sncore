using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect.Function;

namespace SnCore.Data.Tests
{
    /// <summary>
    /// NHibernate test foundation.
    /// </summary>
    public class NHibernateTest
    {
        private static NHibernate.Cfg.Configuration mConfiguration = null;
        private static ISessionFactory mFactory = null;
        private ISession mSession;

        public NHibernateTest()
        {

        }

        public static NHibernate.Cfg.Configuration Configuration
        {
            get
            {
                if (mConfiguration == null)
                {
                    mConfiguration = new NHibernate.Cfg.Configuration();
                    mConfiguration.Properties.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2005Dialect");
                    mConfiguration.Properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    mConfiguration.Properties.Add("hibernate.connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    mConfiguration.Properties.Add("hibernate.connection.connection_string", "Server=localhost;initial catalog=SnCore;Integrated Security=SSPI");
                    mConfiguration.AddAssembly("SnCore.Data");
                    mConfiguration.AddAssembly("SnCore.Data.Hibernate");
                }
                return mConfiguration;
            }
            set
            {
                mConfiguration = value;
            }
        }

        public static ISessionFactory Factory
        {
            get
            {
                if (mFactory == null)
                {
                    mFactory = Configuration.BuildSessionFactory();
                    mFactory.Dialect.Functions["count"] = new ClassicCountFunction();
                    mFactory.Dialect.Functions["sum"] = new ClassicSumFunction();
                    mFactory.Dialect.Functions["avg"] = new ClassicAvgFunction();
                }
                return mFactory;
            }
            set
            {
                mFactory = value;
            }
        }

        public ISession Session
        {
            get
            {
                return mSession;
            }
            set
            {
                mSession = value;
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            Session = Factory.OpenSession();
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

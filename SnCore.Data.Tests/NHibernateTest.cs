using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using NHibernate.Cfg;

namespace SnCore.Data.Tests
{
    /// <summary>
    /// NHibernate test foundation.
    /// </summary>
    public class NHibernateTest
    {
        private static ISessionFactory mFactory = null;
        private ISession mSession;

        public NHibernateTest()
        {

        }

        public static ISessionFactory Factory
        {
            get
            {
                if (mFactory == null)
                {
                    NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
                    cfg.Properties.Add("hibernate.dialect", "NHibernate.Dialect.MsSql2000Dialect");
                    cfg.Properties.Add("hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    cfg.Properties.Add("hibernate.connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    cfg.Properties.Add("hibernate.connection.connection_string", "Server=localhost;initial catalog=SnCore;Integrated Security=SSPI");
                    cfg.AddAssembly("SnCore.Data");
                    mFactory = cfg.BuildSessionFactory();
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
        public void SetUp()
        {
            Session = Factory.OpenSession();
        }

        [TearDown]
        public void TearDown()
        {
            Session.Close();
        }

    }
}

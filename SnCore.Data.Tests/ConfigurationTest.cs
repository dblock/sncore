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
    public class ConfigurationTest : NHibernateTest
    {
        [Test]
        public void TestCrud()
        {
            Configuration c = new Configuration();

            try
            {
                c.OptionName = "name";
                c.OptionValue = "value";
                Session.Save(c);
                Session.Flush();

                Assert.IsTrue(c.Id > 0);
            }
            finally
            {
                Session.Delete(c);
            }

            Session.Flush();
        }
    }
}

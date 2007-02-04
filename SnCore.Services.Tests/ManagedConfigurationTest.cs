using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedConfigurationTest : ManagedServiceTest
    {
        public ManagedConfigurationTest()
        {

        }

        [Test]
        public void TestCrud()
        {
            ManagedConfiguration c = null;
            try
            {
                string name = GetNewString();
                string value = GetNewString();
                c = ManagedConfiguration.SetValue(Session, name, value);
                Assert.AreEqual(value, ManagedConfiguration.GetValue(Session, name));
            }
            finally
            {
                c.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void TestGetValue()
        {
            string defaultvalue = GetNewString();
            string v = ManagedConfiguration.GetValue(Session, GetNewString(), defaultvalue);
            Assert.AreEqual(v, defaultvalue);
        }

        [Test]
        [ExpectedException(typeof(ManagedConfiguration.InvalidConfigurationException))]
        public void TestGetValueInvalid()
        {
            ManagedConfiguration.GetValue(Session, GetNewString());
        }

    }
}

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
                string name = Guid.NewGuid().ToString();
                string value = Guid.NewGuid().ToString();
                c = ManagedConfiguration.SetValue(Session, name, value);
                Assert.AreEqual(value, ManagedConfiguration.GetValue(Session, name));
            }
            finally
            {
                c.Delete();
            }
        }

        [Test]
        public void TestGetValue()
        {
            string defaultvalue = Guid.NewGuid().ToString();
            string v = ManagedConfiguration.GetValue(Session, Guid.NewGuid().ToString(), defaultvalue);
            Assert.AreEqual(v, defaultvalue);
        }

        [Test]
        [ExpectedException(typeof(ManagedConfiguration.InvalidConfigurationException))]
        public void TestGetValueInvalid()
        {
            ManagedConfiguration.GetValue(Session, Guid.NewGuid().ToString());
        }

    }
}

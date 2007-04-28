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
        public void TestGetValue()
        {
            string defaultvalue = GetNewString();
            string v = ManagedConfiguration.GetValue(Session, GetNewString(), defaultvalue);
            Assert.AreEqual(v, defaultvalue);
        }

        [Test]
        public void TestTryGetConfiguration()
        {
            Configuration result = null;
            Assert.IsFalse(ManagedConfiguration.TryGetConfiguration(Session, GetNewString(), out result));
            Assert.IsNull(result);
        }
    }
}

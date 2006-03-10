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
    public class ManagedCountryTest : NHibernateTest
    {
        public ManagedCountryTest()
        {

        }

        [Test]
        public void CreateCountry()
        {
            TransitCountry t = new TransitCountry();
            t.Name = Guid.NewGuid().ToString();
            ManagedCountry c = new ManagedCountry(Session);
            c.Create(t);
            Session.Flush();
            c.Delete();
            Session.Flush();
        }

        [Test]
        public void GetCountryId()
        {
            TransitCountry t = new TransitCountry();
            t.Name = Guid.NewGuid().ToString();
            ManagedCountry c = new ManagedCountry(Session);
            c.Create(t);
            Session.Flush();
            try
            {
                Assert.AreEqual(ManagedCountry.GetCountryId(Session, t.Name) > 0, true);
            }
            finally
            {
                c.Delete();
            }
            Session.Flush();
        }

        [Test]
        [ExpectedException(typeof(ManagedCountry.InvalidCountryException))]
        public void GetCountryIdInvalid()
        {
            ManagedCountry.GetCountryId(Session, Guid.NewGuid().ToString());
        }

    }
}

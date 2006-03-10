using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedCityTest : NHibernateTest
    {
        public ManagedCityTest()
        {

        }

        [Test]
        public void CreateCity()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState t = new ManagedState(Session);
            ManagedCity s = new ManagedCity(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = Guid.NewGuid().ToString();

                TransitState tt = new TransitState();
                tt.Name = Guid.NewGuid().ToString();
                tt.Country = tc.Name;

                TransitCity ts = new TransitCity();
                ts.Name = Guid.NewGuid().ToString();
                ts.Country = tc.Name;
                ts.State = tt.Name;

                c.Create(tc);
                t.Create(tt);
                s.Create(ts);
                Session.Flush();
            }
            finally
            {
                s.Delete();
                t.Delete();
                c.Delete();
            }
        }

        [Test]
        public void GetCityId()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState t = new ManagedState(Session);
            ManagedCity s = new ManagedCity(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = Guid.NewGuid().ToString();
                TransitState tt = new TransitState();
                tt.Name = Guid.NewGuid().ToString();
                tt.Country = tc.Name;
                TransitCity ts = new TransitCity();
                ts.Name = Guid.NewGuid().ToString();
                ts.Country = tc.Name;
                ts.State = tt.Name;

                c.Create(tc);
                t.Create(tt);
                Session.Flush();
                s.Create(ts);
                Session.Flush();
                ManagedCity.GetCityId(Session, ts.Name, tt.Name, tc.Name);
            }
            finally
            {
                s.Delete();
                t.Delete();
                c.Delete();
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedCity.InvalidCityException))]
        public void GetCityIdInvalid()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedCity s = new ManagedCity(Session);
            ManagedState t = new ManagedState(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = Guid.NewGuid().ToString();
                TransitState tt = new TransitState();
                tt.Name = Guid.NewGuid().ToString();
                tt.Country = tc.Name;

                c.Create(tc);
                t.Create(tt);
                Session.Flush();

                ManagedCity.GetCityId(Session, Guid.NewGuid().ToString(), tt.Name, tc.Name);
            }
            finally
            {
                t.Delete();
                c.Delete();
            }
        }

    }
}

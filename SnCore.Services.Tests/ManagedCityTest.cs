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
    public class ManagedCityTest : ManagedCRUDTest<City, TransitCity, ManagedCity>
    {
        private ManagedStateTest _state = new ManagedStateTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _state.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _state.TearDown();
            base.TearDown();
        }

        public ManagedCityTest()
        {

        }

        public override TransitCity GetTransitInstance()
        {
            TransitCity t_instance = new TransitCity();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Country = _state.Instance.Instance.Country.Name;
            t_instance.State = _state.Instance.Name;
            return t_instance;
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

                c.CreateOrUpdate(tc, AdminSecurityContext);
                t.CreateOrUpdate(tt, AdminSecurityContext);
                s.CreateOrUpdate(ts, AdminSecurityContext);
                Session.Flush();
            }
            finally
            {
                s.Delete(AdminSecurityContext);
                t.Delete(AdminSecurityContext);
                c.Delete(AdminSecurityContext);
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

                c.CreateOrUpdate(tc, AdminSecurityContext);
                t.CreateOrUpdate(tt, AdminSecurityContext);
                Session.Flush();
                s.CreateOrUpdate(ts, AdminSecurityContext);
                Session.Flush();
                ManagedCity.GetCityId(Session, ts.Name, tt.Name, tc.Name);
            }
            finally
            {
                s.Delete(AdminSecurityContext);
                t.Delete(AdminSecurityContext);
                c.Delete(AdminSecurityContext);
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

                c.CreateOrUpdate(tc, AdminSecurityContext);
                t.CreateOrUpdate(tt, AdminSecurityContext);
                Session.Flush();

                ManagedCity.GetCityId(Session, Guid.NewGuid().ToString(), tt.Name, tc.Name);
            }
            finally
            {
                t.Delete(AdminSecurityContext);
                c.Delete(AdminSecurityContext);
            }
        }

    }
}

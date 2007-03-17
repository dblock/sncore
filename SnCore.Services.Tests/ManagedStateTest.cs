using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;
using System.Diagnostics;
using SnCore.Tools.Web;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedStateTest : ManagedCRUDTest<State, TransitState, ManagedState>
    {
        private ManagedCountryTest _country = new ManagedCountryTest();

        [SetUp]
        public override void SetUp()
        {
            _country.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _country.TearDown();
        }

        public override TransitState GetTransitInstance()
        {
            TransitState t_instance = new TransitState();
            t_instance.Country = _country.Instance.Name;
            t_instance.Name = GetNewString();
            return t_instance;
        }

        public ManagedStateTest()
        {
            
        }


        [Test]
        public void CreateState()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState s = new ManagedState(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = GetNewString();
                TransitState ts = new TransitState();
                ts.Name = GetNewString();
                ts.Country = tc.Name;

                c.CreateOrUpdate(tc, AdminSecurityContext);
                s.CreateOrUpdate(ts, AdminSecurityContext);
                Session.Flush();
            }
            finally
            {
                s.Delete(AdminSecurityContext);
                c.Delete(AdminSecurityContext);
            }
        }

        [Test]
        public void GetStateId()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState s = new ManagedState(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = GetNewString();
                TransitState ts = new TransitState();
                ts.Name = GetNewString();
                ts.Country = tc.Name;

                c.CreateOrUpdate(tc, AdminSecurityContext);
                Session.Flush();
                s.CreateOrUpdate(ts, AdminSecurityContext);
                Session.Flush();
                ManagedState.GetStateId(Session, ts.Name, tc.Name);
            }
            finally
            {
                s.Delete(AdminSecurityContext);
                c.Delete(AdminSecurityContext);
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedState.InvalidStateException))]
        public void GetStateIdInvalid()
        {
            ManagedCountry c = new ManagedCountry(Session);
            ManagedState s = new ManagedState(Session);

            try
            {
                TransitCountry tc = new TransitCountry();
                tc.Name = GetNewString();

                c.CreateOrUpdate(tc, AdminSecurityContext);
                Session.Flush();

                ManagedState.GetStateId(Session, GetNewString(), tc.Name);
            }
            finally
            {
                c.Delete(AdminSecurityContext);
            }
        }

    }
}

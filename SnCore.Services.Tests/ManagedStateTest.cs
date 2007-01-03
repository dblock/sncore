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
    public class ManagedStateTest : ManagedServiceTest
    {
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
                tc.Name = Guid.NewGuid().ToString();
                TransitState ts = new TransitState();
                ts.Name = Guid.NewGuid().ToString();
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
                tc.Name = Guid.NewGuid().ToString();
                TransitState ts = new TransitState();
                ts.Name = Guid.NewGuid().ToString();
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
                tc.Name = Guid.NewGuid().ToString();

                c.CreateOrUpdate(tc, AdminSecurityContext);
                Session.Flush();

                ManagedState.GetStateId(Session, Guid.NewGuid().ToString(), tc.Name);
            }
            finally
            {
                c.Delete(AdminSecurityContext);
            }
        }

    }
}

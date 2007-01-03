using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Expression;
using NHibernate;
using SnCore.Data.Tests;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceTest : ManagedServiceTest
    {
        public ManagedPlaceTest()
        {

        }

        [Test]
        public void CreatePlace()
        {
            ManagedPlaceType type = new ManagedPlaceType(Session);

            ManagedCountry c = new ManagedCountry(Session);
            ManagedState t = new ManagedState(Session);
            ManagedCity s = new ManagedCity(Session);
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

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

                TransitPlaceType t_type = new TransitPlaceType();
                t_type.Name = Guid.NewGuid().ToString();
                type.CreateOrUpdate(t_type, AdminSecurityContext);

                TransitPlace t_place = new TransitPlace();
                t_place.Name = Guid.NewGuid().ToString();
                t_place.Type = t_type.Name;
                t_place.City = ts.Name;
                t_place.Country = tc.Name;
                t_place.State = tt.Name;
                t_place.AccountId = a.Id;
                t_place.Website = Guid.NewGuid().ToString();

                ManagedPlace m_place = new ManagedPlace(Session);
                m_place.CreateOrUpdate(t_place, a.GetSecurityContext());
            }
            finally
            {
                try
                {
                    a.Delete(AdminSecurityContext);
                    type.Delete(AdminSecurityContext);
                    s.Delete(AdminSecurityContext);
                    t.Delete(AdminSecurityContext);
                    c.Delete(AdminSecurityContext);
                }
                catch
                {

                }
            }
        }
    }
}

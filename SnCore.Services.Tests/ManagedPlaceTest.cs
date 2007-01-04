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
    public class ManagedPlaceTest : ManagedCRUDTest<Place, TransitPlace, ManagedPlace>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedCityTest _city = new ManagedCityTest();
        private ManagedPlaceTypeTest _type = new ManagedPlaceTypeTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _type.SetUp();
            _account.SetUp();
            _city.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _type.TearDown();
            _city.TearDown();
            _account.TearDown();
            base.TearDown();
        }

        public ManagedPlaceTest()
        {

        }

        public override TransitPlace GetTransitInstance()
        {
            TransitPlace t_instance = new TransitPlace();
            t_instance.AccountId = _account.Instance.Id;
            t_instance.City = _city.Instance.Name;
            t_instance.Country = _city.Instance.Instance.Country.Name;
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.State = _city.Instance.Instance.State.Name;
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.Website = Guid.NewGuid().ToString();
            return t_instance;
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

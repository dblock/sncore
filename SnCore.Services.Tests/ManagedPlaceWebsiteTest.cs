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
    public class ManagedPlaceWebsiteTest : ManagedServiceTest
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _place.TearDown();
        }

        public ManagedPlaceWebsiteTest()
        {

        }

        [Test]
        public void CreatePlaceWebsite()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitPlaceWebsite ta = new TransitPlaceWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "http://www.dblock.org";
                ta.PlaceId = _place.Instance.Id;

                ManagedPlaceWebsite m_a = new ManagedPlaceWebsite(Session);
                m_a.CreateOrUpdate(ta, a.GetSecurityContext());
                m_a.Delete(a.GetSecurityContext());
            }
            finally
            {
                a.Delete(a.GetSecurityContext());
            }
        }

        [Test]
        [ExpectedException(typeof(ManagedPlaceWebsite.InvalidUriException))]
        public void CreatePlaceWebsiteInvalid()
        {
            ManagedAccount a = new ManagedAccount(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitPlaceWebsite ta = new TransitPlaceWebsite();
                ta.Name = "My Website";
                ta.Description = "Lots of details.";
                ta.Url = "<script>attack!</script>";
                ta.PlaceId = _place.Instance.Id;

                ManagedPlaceWebsite m_w = new ManagedPlaceWebsite(Session);
                m_w.CreateOrUpdate(ta, a.GetSecurityContext());
            }
            finally
            {
                a.Delete(AdminSecurityContext);
            }
        }

    }
}

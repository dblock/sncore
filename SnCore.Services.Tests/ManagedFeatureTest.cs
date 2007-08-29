using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedFeatureTest : ManagedCRUDTest<Feature, TransitFeature, ManagedFeature>
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

        public ManagedFeatureTest()
        {

        }

        public override TransitFeature GetTransitInstance()
        {
            TransitFeature t_instance = new TransitFeature();
            t_instance.DataObjectName = typeof(Place).Name;
            t_instance.DataRowId = _place.Instance.Id;
            return t_instance;
        }

        [Test]
        public void GetDbInstanceTest()
        {
            TransitFeature t_instance = new TransitFeature();
            t_instance.DataObjectName = "Place";
            t_instance.DataRowId = _place.Instance.Id;
            ManagedFeature m_instance = new ManagedFeature(Session);
            t_instance.Id = m_instance.CreateOrUpdate(t_instance, AdminSecurityContext);
            Console.WriteLine("Created feature: {0}", t_instance.Id);
            IDbObject dbobject = m_instance.GetInstance();
            Console.WriteLine("Instance: {0} -> {1}", dbobject.GetType().FullName, dbobject.Id);
            Assert.AreEqual(dbobject.Id, _place.Instance.Id);
            Console.WriteLine("Instance: {0}", m_instance.GetInstance<Place>().Id);
            IList<Account> accounts = m_instance.GetInstanceAccounts();
            Console.WriteLine("Accounts: {0}", accounts.Count);
            m_instance.Delete(AdminSecurityContext);
        }
    }
}

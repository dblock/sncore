using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebPlaceServiceTests;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class FeatureTest : WebServiceTest<WebObjectService.TransitFeature, WebObjectServiceNoCache>
    {
        public PlaceTest _place = new PlaceTest();
        public int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
        }

        public FeatureTest()
            : base("Feature")
        {
        }


        public override WebObjectService.TransitFeature GetTransitInstance()
        {
            WebObjectService.TransitFeature t_instance = new WebObjectService.TransitFeature();
            t_instance.DataObjectName = "Place";
            t_instance.DataRowId = _place_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, "Place", options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, "Place" };
            return args;
        }

        [Test]
        public void GetLatestFeatureTest()
        {
            WebObjectService.TransitFeature t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebObjectService.TransitFeature t_feature = EndPoint.GetLatestFeature(GetAdminTicket(), t_instance.DataObjectName);
            Console.WriteLine("Feature: {0}", t_feature.Id);
            Delete(GetAdminTicket(), t_instance.Id);
        }

        [Test]
        public void DeleteAllFeaturesTest()
        {
            WebObjectService.TransitFeature t_instance = GetTransitInstance();
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebObjectService.TransitFeature t_feature = EndPoint.GetLatestFeature(GetAdminTicket(), t_instance.DataObjectName);
            Console.WriteLine("Feature: {0}", t_feature.Id);
            EndPoint.DeleteAllFeatures(GetAdminTicket(), t_instance);
            WebObjectService.TransitFeature t_feature_deleted = EndPoint.GetLatestFeature(GetAdminTicket(), t_instance.DataObjectName);
            if (t_feature_deleted != null)
            {
                Assert.AreNotEqual(t_feature.Id, t_feature_deleted.Id);
                Console.WriteLine("Feature: {0}", t_feature_deleted.Id);
            }
        }
    }
}

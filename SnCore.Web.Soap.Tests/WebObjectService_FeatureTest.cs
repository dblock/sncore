using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class FeatureTest : WebServiceTest<WebObjectService.TransitFeature, WebObjectServiceNoCache>
    {
        public DataObjectTest _dataobject = new DataObjectTest();
        public int _dataobject_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _dataobject.SetUp();
            _dataobject_id = _dataobject.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _dataobject.Delete(GetAdminTicket(), _dataobject_id);
            _dataobject.TearDown();
        }

        public FeatureTest()
            : base("Feature")
        {
        }


        public override WebObjectService.TransitFeature GetTransitInstance()
        {
            WebObjectService.TransitFeature t_instance = new WebObjectService.TransitFeature();
            t_instance.DataObjectName = (string) _dataobject.GetInstancePropertyById(GetAdminTicket(), _dataobject_id, "Name");
            t_instance.DataRowId = 1;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, (string)_dataobject.GetInstancePropertyById(GetAdminTicket(), _dataobject_id, "Name"), options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, (string)_dataobject.GetInstancePropertyById(GetAdminTicket(), _dataobject_id, "Name") };
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

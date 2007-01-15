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
        protected void GetLatestFeatureTest()
        {

        }
    }
}

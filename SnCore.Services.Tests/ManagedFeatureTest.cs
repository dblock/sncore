using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedFeatureTest : ManagedCRUDTest<Feature, TransitFeature, ManagedFeature>
    {
        private ManagedDataObjectTest _dataobject = new ManagedDataObjectTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _dataobject.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _dataobject.TearDown();
            base.TearDown();
        }

        public ManagedFeatureTest()
        {

        }

        public override TransitFeature GetTransitInstance()
        {
            TransitFeature t_instance = new TransitFeature();
            t_instance.DataObjectName = _dataobject.Instance.Name;
            t_instance.DataRowId = _dataobject.Instance.Id;
            return t_instance;
        }
    }
}

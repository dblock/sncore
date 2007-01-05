using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceAttributeTest : ManagedCRUDTest<PlaceAttribute, TransitPlaceAttribute, ManagedPlaceAttribute>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedAttributeTest _attribute = new ManagedAttributeTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _attribute.SetUp();
            _place.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _place.TearDown();
            _attribute.TearDown();
            base.TearDown();
        }

        public ManagedPlaceAttributeTest()
        {

        }

        public override TransitPlaceAttribute GetTransitInstance()
        {
            TransitPlaceAttribute t_instance = new TransitPlaceAttribute();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AttributeId = _attribute.Instance.Id;
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            t_instance.Value = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

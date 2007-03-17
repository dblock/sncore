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
            _attribute.SetUp();
            _place.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _place.TearDown();
            _attribute.TearDown();
        }

        public ManagedPlaceAttributeTest()
        {

        }

        public override TransitPlaceAttribute GetTransitInstance()
        {
            TransitPlaceAttribute t_instance = new TransitPlaceAttribute();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AttributeId = _attribute.Instance.Id;
            t_instance.Url = GetNewUri();
            t_instance.Value = GetNewString();
            return t_instance;
        }
    }
}

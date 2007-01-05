using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePropertyValueTest : ManagedCRUDTest<PlacePropertyValue, TransitPlacePropertyValue, ManagedPlacePropertyValue>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedPlacePropertyTest _property = new ManagedPlacePropertyTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _place.SetUp();
            _property.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _property.TearDown();
            _place.TearDown();
            base.TearDown();
        }

        public ManagedPlacePropertyValueTest()
        {

        }

        public override TransitPlacePropertyValue GetTransitInstance()
        {
            TransitPlacePropertyValue t_instance = new TransitPlacePropertyValue();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.PlaceProperty = new TransitPlaceProperty();
            t_instance.PlaceProperty.Id = _property.Instance.Id;
            t_instance.Value = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

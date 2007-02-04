using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlaceNameTest : ManagedCRUDTest<PlaceName, TransitPlaceName, ManagedPlaceName>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _place.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _place.TearDown();
            base.TearDown();
        }

        public ManagedPlaceNameTest()
        {

        }

        public override TransitPlaceName GetTransitInstance()
        {
            TransitPlaceName t_instance = new TransitPlaceName();
            t_instance.Name = GetNewString();
            t_instance.PlaceId = _place.Instance.Id;
            return t_instance;
        }
    }
}

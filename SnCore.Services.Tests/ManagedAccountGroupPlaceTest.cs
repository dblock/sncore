using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountGroupPlaceTest : ManagedCRUDTest<AccountGroupPlace, TransitAccountGroupPlace, ManagedAccountGroupPlace>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedAccountGroupTest _group = new ManagedAccountGroupTest();

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _group.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _group.TearDown();
            _place.TearDown();
        }

        public ManagedAccountGroupPlaceTest()
        {

        }

        public override TransitAccountGroupPlace GetTransitInstance()
        {
            TransitAccountGroupPlace t_instance = new TransitAccountGroupPlace();
            t_instance.AccountGroupId = _group.Instance.Id;
            t_instance.PlaceId = _place.Instance.Id;
            return t_instance;
        }
    }
}

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
            base.SetUp();
            _place.SetUp();
            _group.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _group.SetUp();
            _place.TearDown();
            base.TearDown();
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

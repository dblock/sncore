using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPlaceTest : ManagedCRUDTest<AccountPlace, TransitAccountPlace, ManagedAccountPlace>
    {
        private ManagedAccountPlaceTypeTest _type = new ManagedAccountPlaceTypeTest();
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _type.SetUp();
            _place.SetUp();
            _account.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            _type.TearDown();
            _place.TearDown();
            base.TearDown();
        }

        public ManagedAccountPlaceTest()
        {

        }

        public override TransitAccountPlace GetTransitInstance()
        {
            TransitAccountPlace t_instance = new TransitAccountPlace();
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

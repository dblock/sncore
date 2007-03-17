using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountPlaceRequestTest : ManagedCRUDTest<AccountPlaceRequest, TransitAccountPlaceRequest, ManagedAccountPlaceRequest>
    {
        private ManagedAccountTest _account = new ManagedAccountTest();
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedAccountPlaceTypeTest _type = new ManagedAccountPlaceTypeTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _place.SetUp();
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _type.TearDown();
            _place.TearDown();
            _account.TearDown();
        }

        public ManagedAccountPlaceRequestTest()
        {

        }

        public override TransitAccountPlaceRequest GetTransitInstance()
        {
            TransitAccountPlaceRequest t_instance = new TransitAccountPlaceRequest();
            t_instance.Message = GetNewString();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.Submitted = DateTime.UtcNow;
            t_instance.Type = _type.Instance.Instance.Name;
            return t_instance;
        }
    }
}

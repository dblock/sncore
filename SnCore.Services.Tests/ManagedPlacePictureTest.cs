using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPlacePictureTest : ManagedCRUDTest<PlacePicture, TransitPlacePicture, ManagedPlacePicture>
    {
        private ManagedPlaceTest _place = new ManagedPlaceTest();
        private ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            _account.SetUp();
            _place.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
            _place.TearDown();
        }

        public ManagedPlacePictureTest()
        {

        }

        public override TransitPlacePicture GetTransitInstance()
        {
            TransitPlacePicture t_instance = new TransitPlacePicture();
            t_instance.Bitmap = new byte[128];
            t_instance.Name = GetNewString();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

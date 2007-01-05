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
            base.SetUp();
            _account.SetUp();
            _place.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _account.TearDown();
            _place.TearDown();
            base.TearDown();
        }

        public ManagedPlacePictureTest()
        {

        }

        public override TransitPlacePicture GetTransitInstance()
        {
            TransitPlacePictureWithBitmap t_instance = new TransitPlacePictureWithBitmap();
            t_instance.Bitmap = new byte[128];
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.PlaceId = _place.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

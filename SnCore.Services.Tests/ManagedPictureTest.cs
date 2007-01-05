using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedPictureTest : ManagedCRUDTest<Picture, TransitPicture, ManagedPicture>
    {
        private ManagedPictureTypeTest _type = new ManagedPictureTypeTest();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _type.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _type.TearDown();
            base.TearDown();
        }

        public ManagedPictureTest()
        {

        }

        public override TransitPicture GetTransitInstance()
        {
            TransitPictureWithBitmap t_instance = new TransitPictureWithBitmap();
            t_instance.Bitmap = new byte[128];
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.Description = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}

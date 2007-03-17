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
            _type.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _type.TearDown();
        }

        public ManagedPictureTest()
        {

        }

        public override TransitPicture GetTransitInstance()
        {
            TransitPicture t_instance = new TransitPicture();
            t_instance.Bitmap = new byte[128];
            t_instance.Name = GetNewString();
            t_instance.Type = _type.Instance.Instance.Name;
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}

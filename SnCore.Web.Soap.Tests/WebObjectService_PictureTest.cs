using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class PictureTest : WebServiceTest<WebObjectService.TransitPicture, WebObjectServiceNoCache>
    {
        public PictureTypeTest _type = new PictureTypeTest();
        public int _type_id = 0;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _type.SetUp();
            _type_id = _type.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _type.Delete(GetAdminTicket(), _type_id);
            _type.TearDown();
            base.TearDown();
        }

        public PictureTest()
            : base("Picture")
        {
        }


        public override WebObjectService.TransitPicture GetTransitInstance()
        {
            WebObjectService.TransitPicture t_instance = new WebObjectService.TransitPicture();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.Bitmap = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 12, 240, 100);
            return t_instance;
        }

        [Test]
        protected void GetPictureIfModifiedSinceTest()
        {

        }

        [Test]
        protected void GetRandomPictureByTypeTest()
        {

        }
    }
}

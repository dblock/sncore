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
            t_instance.Description = GetNewString();
            t_instance.Name = GetNewString();
            t_instance.Type = (string) _type.GetInstancePropertyById(GetAdminTicket(), _type_id, "Name");
            t_instance.Bitmap = GetNewBitmap();
            return t_instance;
        }

        [Test]
        public void GetPictureIfModifiedSinceTest()
        {
            WebObjectService.TransitPicture t_picture = GetTransitInstance();
            t_picture.Id = Create(GetAdminTicket(), t_picture);
            Assert.IsNotNull(EndPoint.GetPictureIfModifiedSinceById(GetAdminTicket(), t_picture.Id, DateTime.UtcNow.AddHours(-1)));
            Assert.IsNull(EndPoint.GetPictureIfModifiedSinceById(GetAdminTicket(), t_picture.Id, DateTime.UtcNow));
            Delete(GetAdminTicket(), t_picture.Id);
        }

        [Test]
        public void GetRandomPictureByTypeTest()
        {
            WebObjectService.TransitPicture t_picture = GetTransitInstance();
            t_picture.Id = Create(GetAdminTicket(), t_picture);
            WebObjectService.TransitPicture t_instance = EndPoint.GetRandomPictureByType(GetAdminTicket(), t_picture.Type);
            Console.WriteLine("Random picture: {0}", t_instance.Id);
            Assert.IsNotNull(t_instance);
            Delete(GetAdminTicket(), t_picture.Id);
        }
    }
}

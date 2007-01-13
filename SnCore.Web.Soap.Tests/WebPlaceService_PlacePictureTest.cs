using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePictureTest : WebServiceTest<WebPlaceService.TransitPlacePicture, WebPlaceServiceNoCache>
    {
        private PlaceTest _place = new PlaceTest();
        private int _place_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _place.SetUp();
            _place_id = _place.Create(GetAdminTicket());
        }

        [TearDown]
        public override void TearDown()
        {
            _place.Delete(GetAdminTicket(), _place_id);
            _place.TearDown();
        }

        public PlacePictureTest()
            : base("PlacePicture")
        {
        }

        public override WebPlaceService.TransitPlacePicture GetTransitInstance()
        {
            WebPlaceService.TransitPlacePicture t_instance = new WebPlaceService.TransitPlacePicture();
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.PlaceId = _place_id;
            t_instance.Bitmap = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 12, 240, 100);
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _place_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _place_id };
            return args;
        }
    }
}

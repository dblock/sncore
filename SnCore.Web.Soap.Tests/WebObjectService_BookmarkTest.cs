using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Tools.Drawing;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class BookmarkTest : WebServiceTest<WebObjectService.TransitBookmark, WebObjectServiceNoCache>
    {
        public BookmarkTest()
            : base("Bookmark")
        {
        }


        public override WebObjectService.TransitBookmark GetTransitInstance()
        {
            WebObjectService.TransitBookmark t_instance = new WebObjectService.TransitBookmark();
            t_instance.Description = Guid.NewGuid().ToString();
            t_instance.FullBitmap = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 12, 240, 100);
            t_instance.LinkBitmap = ThumbnailBitmap.GetBitmapDataFromText(Guid.NewGuid().ToString(), 12, 240, 100);
            t_instance.Name = Guid.NewGuid().ToString();
            t_instance.Url = string.Format("http://uri/{0}", Guid.NewGuid());
            return t_instance;
        }

        [Test]
        protected void GetBookmarksWithOptionsTest()
        {

        }

        [Test]
        protected void GetBookmarksIfModifiedSinceByIdTest()
        {

        }

    }
}

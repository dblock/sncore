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
            t_instance.Description = GetNewString();
            t_instance.FullBitmap = GetNewBitmap();
            t_instance.LinkBitmap = GetNewBitmap();
            t_instance.Name = GetNewString();
            t_instance.Url = GetNewUri();
            return t_instance;
        }

        [Test]
        public void GetBookmarksWithOptionsTest()
        {
            WebObjectService.BookmarkQueryOptions options = new WebObjectService.BookmarkQueryOptions();
            options.WithFullBitmaps = true;
            options.WithLinkedBitmaps = false;
            WebObjectService.TransitBookmark[] bookmarks = EndPoint.GetBookmarksWithOptions(GetAdminTicket(), options, null);
            Console.WriteLine("Length: {0}", bookmarks.Length);
        }

        [Test]
        public void GetBookmarksIfModifiedSinceByIdTest()
        {
            WebObjectService.TransitBookmark t_bookmark = GetTransitInstance();
            t_bookmark.Id = Create(GetAdminTicket(), t_bookmark);
            Assert.IsNotNull(EndPoint.GetBookmarkIfModifiedSinceById(GetAdminTicket(), t_bookmark.Id, DateTime.UtcNow.AddHours(-1)));
            Assert.IsNull(EndPoint.GetBookmarkIfModifiedSinceById(GetAdminTicket(), t_bookmark.Id, DateTime.UtcNow));
            Delete(GetAdminTicket(), t_bookmark.Id);
        }

    }
}

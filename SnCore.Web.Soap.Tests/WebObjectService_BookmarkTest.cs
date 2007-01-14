using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

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
            return t_instance;
        }
    }
}

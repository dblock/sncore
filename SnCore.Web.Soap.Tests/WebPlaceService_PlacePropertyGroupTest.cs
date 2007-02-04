using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlacePropertyGroupTest : WebServiceTest<WebPlaceService.TransitPlacePropertyGroup, WebPlaceServiceNoCache>
    {
        public PlacePropertyGroupTest()
            : base("PlacePropertyGroup")
        {
        }

        public override WebPlaceService.TransitPlacePropertyGroup GetTransitInstance()
        {
            WebPlaceService.TransitPlacePropertyGroup t_instance = new WebPlaceService.TransitPlacePropertyGroup();
            t_instance.Name = GetNewString();
            t_instance.Description = GetNewString();
            return t_instance;
        }
    }
}

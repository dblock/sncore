using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebPlaceServiceTests
{
    [TestFixture]
    public class PlaceTypeTest : WebServiceTest<WebPlaceService.TransitPlaceType, WebPlaceServiceNoCache>
    {
        public PlaceTypeTest()
            : base("PlaceType")
        {
        }

        public override WebPlaceService.TransitPlaceType GetTransitInstance()
        {
            WebPlaceService.TransitPlaceType t_instance = new WebPlaceService.TransitPlaceType();
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

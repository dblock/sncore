using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class PictureTypeTest : WebServiceTest<WebObjectService.TransitPictureType, WebObjectServiceNoCache>
    {
        public PictureTypeTest()
            : base("PictureType")
        {
        }


        public override WebObjectService.TransitPictureType GetTransitInstance()
        {
            WebObjectService.TransitPictureType t_instance = new WebObjectService.TransitPictureType();
            return t_instance;
        }
    }
}

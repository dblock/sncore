using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class PictureTest : WebServiceTest<WebObjectService.TransitPicture, WebObjectServiceNoCache>
    {
        public PictureTest()
            : base("Picture")
        {
        }


        public override WebObjectService.TransitPicture GetTransitInstance()
        {
            WebObjectService.TransitPicture t_instance = new WebObjectService.TransitPicture();
            return t_instance;
        }
    }
}

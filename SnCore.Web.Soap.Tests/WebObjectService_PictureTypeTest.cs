using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

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
            t_instance.Name = GetNewString();
            return t_instance;
        }
    }
}

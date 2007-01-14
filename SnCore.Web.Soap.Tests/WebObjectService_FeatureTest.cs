using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class FeatureTest : WebServiceTest<WebObjectService.TransitFeature, WebObjectServiceNoCache>
    {
        public FeatureTest()
            : base("Feature")
        {
        }


        public override WebObjectService.TransitFeature GetTransitInstance()
        {
            WebObjectService.TransitFeature t_instance = new WebObjectService.TransitFeature();
            return t_instance;
        }
    }
}

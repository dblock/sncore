using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class SurveyTest : WebServiceTest<WebObjectService.TransitSurvey, WebObjectServiceNoCache>
    {
        public SurveyTest()
            : base("Survey")
        {
        }


        public override WebObjectService.TransitSurvey GetTransitInstance()
        {
            WebObjectService.TransitSurvey t_instance = new WebObjectService.TransitSurvey();
            return t_instance;
        }
    }
}

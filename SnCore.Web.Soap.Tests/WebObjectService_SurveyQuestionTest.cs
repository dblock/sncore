using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebObjectServiceTests
{
    [TestFixture]
    public class SurveyQuestionTest : WebServiceTest<WebObjectService.TransitSurveyQuestion, WebObjectServiceNoCache>
    {
        public SurveyQuestionTest()
            : base("SurveyQuestion")
        {
        }


        public override WebObjectService.TransitSurveyQuestion GetTransitInstance()
        {
            WebObjectService.TransitSurveyQuestion t_instance = new WebObjectService.TransitSurveyQuestion();
            return t_instance;
        }
    }
}

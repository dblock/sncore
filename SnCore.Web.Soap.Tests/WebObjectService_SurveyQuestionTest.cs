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
        public SurveyTest _survey = new SurveyTest();
        public int _survey_id = 0;

        [SetUp]
        public override void SetUp()
        {
            _survey.SetUp();
            _survey_id = _survey.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _survey.Delete(GetAdminTicket(), _survey_id);
        }

        public SurveyQuestionTest()
            : base("SurveyQuestion")
        {
        }


        public override WebObjectService.TransitSurveyQuestion GetTransitInstance()
        {
            WebObjectService.TransitSurveyQuestion t_instance = new WebObjectService.TransitSurveyQuestion();
            t_instance.Question = GetNewString();
            t_instance.SurveyId = _survey_id;
            t_instance.SurveyName = GetNewString();
            return t_instance;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _survey_id };
            return args;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _survey_id, options };
            return args;
        }
    }
}

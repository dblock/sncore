using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedSurveyQuestionTest : ManagedCRUDTest<SurveyQuestion, TransitSurveyQuestion, ManagedSurveyQuestion>
    {
        private ManagedSurveyTest _survey = new ManagedSurveyTest();

        [SetUp]
        public override void SetUp()
        {
            _survey.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _survey.TearDown();
        }

        public ManagedSurveyQuestionTest()
        {

        }

        public override TransitSurveyQuestion GetTransitInstance()
        {
            TransitSurveyQuestion t_instance = new TransitSurveyQuestion();
            t_instance.Question = GetNewString();
            t_instance.SurveyId = _survey.Instance.Id;
            return t_instance;
        }
    }
}

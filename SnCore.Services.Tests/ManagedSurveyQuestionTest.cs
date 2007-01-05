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
            base.SetUp();
            _survey.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            _survey.TearDown();
            base.TearDown();
        }
        public ManagedSurveyQuestionTest()
        {

        }

        public override TransitSurveyQuestion GetTransitInstance()
        {
            TransitSurveyQuestion t_instance = new TransitSurveyQuestion();
            t_instance.Question = Guid.NewGuid().ToString();
            t_instance.SurveyId = _survey.Instance.Id;
            return t_instance;
        }
    }
}

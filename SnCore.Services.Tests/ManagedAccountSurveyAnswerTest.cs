using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountSurveyAnswerTest : ManagedCRUDTest<AccountSurveyAnswer, TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer>
    {
        ManagedSurveyQuestionTest _surveyquestion = new ManagedSurveyQuestionTest();
        ManagedAccountTest _account = new ManagedAccountTest();

        [SetUp]
        public override void SetUp()
        {
            _surveyquestion.SetUp();
            _account.SetUp();
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _account.TearDown();
            _surveyquestion.TearDown();
        }

        public ManagedAccountSurveyAnswerTest()
        {

        }

        public override TransitAccountSurveyAnswer GetTransitInstance()
        {
            TransitAccountSurveyAnswer t_instance = new TransitAccountSurveyAnswer();
            t_instance.Answer = GetNewString();
            t_instance.SurveyQuestionId = _surveyquestion.Instance.Id;
            t_instance.AccountId = _account.Instance.Id;
            return t_instance;
        }
    }
}

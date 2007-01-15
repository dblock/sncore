using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;
using SnCore.Web.Soap.Tests.WebObjectServiceTests;

namespace SnCore.Web.Soap.Tests.WebAccountServiceTests
{
    [TestFixture]
    public class AccountSurveyAnswerTest : AccountBaseTest<WebAccountService.TransitAccountSurveyAnswer>
    {
        public SurveyQuestionTest _question = new SurveyQuestionTest();
        public int _question_id;

        [SetUp]
        public override void SetUp()
        {
            _question.SetUp();
            _question_id = _question.Create(GetAdminTicket());
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            _question.Delete(GetAdminTicket(), _question_id);
            _question.TearDown();
        }

        public AccountSurveyAnswerTest()
            : base("AccountSurveyAnswer")
        {

        }

        public override WebAccountService.TransitAccountSurveyAnswer GetTransitInstance()
        {
            WebAccountService.TransitAccountSurveyAnswer t_instance = new WebAccountService.TransitAccountSurveyAnswer();
            t_instance.AccountId = _account_id;
            t_instance.Answer = Guid.NewGuid().ToString();
            t_instance.SurveyQuestionId = _question_id;
            return t_instance;
        }

        public override object[] GetArgs(string ticket, object options)
        {
            object[] args = { ticket, _account_id, _question._survey_id, options };
            return args;
        }

        public override object[] GetCountArgs(string ticket)
        {
            object[] args = { ticket, _account_id, _question._survey_id };
            return args;
        }

        [Test]
        protected void GetAccountSurveyAnswersCountByQuestionIdTest()
        {

        }
    }
}

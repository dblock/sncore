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
            t_instance.Answer = GetNewString();
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
        public void GetAccountSurveyAnswersByQuestionIdTest()
        {
            WebAccountService.TransitAccountSurveyAnswer t_instance = GetTransitInstance();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            int count = EndPoint.GetAccountSurveyAnswersCountByQuestionId(GetAdminTicket(), _question_id);
            Assert.AreEqual(count, 1);
            Console.WriteLine("Count: {0}", count);
            WebAccountService.TransitAccountSurveyAnswer[] answers = EndPoint.GetAccountSurveyAnswersByQuestionId(GetAdminTicket(), _question_id, null);
            Console.WriteLine("Length: {0}", answers.Length);
            Assert.AreEqual(count, answers.Length);
            Delete(GetAdminTicket(), t_instance.Id);
        }


        [Test]
        public void GetAccountSurveysByIdTest()
        {
            WebAccountService.TransitAccountSurveyAnswer t_instance = GetTransitInstance();
            t_instance.AccountId = GetAdminAccount().Id;
            t_instance.Id = Create(GetAdminTicket(), t_instance);
            WebAccountService.TransitSurvey[] surveys = EndPoint.GetAccountSurveysById(GetAdminTicket(), t_instance.AccountId, null);
            Console.WriteLine("Length: {0}", surveys.Length);
            Assert.IsTrue(surveys.Length > 0);
            Delete(GetAdminTicket(), t_instance.Id);
        }
    }
}

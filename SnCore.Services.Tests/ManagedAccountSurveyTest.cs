using System;
using NUnit.Framework;
using SnCore.Data;
using NHibernate;
using SnCore.Data.Tests;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedAccountSurveyTest : ManagedServiceTest
    {
        public ManagedAccountSurveyTest()
        {

        }

        [Test]
        public void CreateAccountSurvey()
        {
            ManagedAccount a = new ManagedAccount(Session);
            ManagedSurvey m_survey = new ManagedSurvey(Session);
            ManagedSurveyQuestion m_question = new ManagedSurveyQuestion(Session);
            ManagedAccountSurveyAnswer m_answer = new ManagedAccountSurveyAnswer(Session);

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow, AdminSecurityContext);

                TransitSurvey t_survey = new TransitSurvey();
                t_survey.Name = GetNewString();
                m_survey.CreateOrUpdate(t_survey, AdminSecurityContext);

                TransitSurveyQuestion t_question = new TransitSurveyQuestion();
                t_question.SurveyId = m_survey.Id;
                t_question.Question = GetNewString();
                m_question.CreateOrUpdate(t_question, AdminSecurityContext);

                TransitAccountSurveyAnswer ta = new TransitAccountSurveyAnswer();
                ta.SurveyQuestionId = m_question.Id;
                ta.Answer = GetNewString();
                m_answer.CreateOrUpdate(ta, a.GetSecurityContext());
            }
            finally
            {
                m_answer.Delete(a.GetSecurityContext());
                a.Delete(AdminSecurityContext);
            }
        }
    }
}

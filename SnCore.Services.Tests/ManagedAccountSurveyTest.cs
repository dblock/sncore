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
            Survey s = new Survey();
            SurveyQuestion q = new SurveyQuestion();

            try
            {
                a.Create("Test User", "testpassword", "foo@localhost.com", DateTime.UtcNow);

                s.Name = Guid.NewGuid().ToString();
                
                q.Survey = s;
                q.Question = Guid.NewGuid().ToString();

                Session.Save(s);
                Session.Save(q);

                TransitAccountSurveyAnswer ta = new TransitAccountSurveyAnswer();
                ta.SurveyQuestion = q.Question;
                ta.SurveyQuestionId = q.Id;
                ta.Answer = Guid.NewGuid().ToString();

                a.CreateOrUpdate(ta);
            }
            finally
            {
                a.Delete();
                Session.Delete(q);
                Session.Delete(s);
                Session.Flush();
            }
        }
    }
}

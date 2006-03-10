using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace SnCore.Services
{
    public class TransitSurveyQuestion : TransitService
    {
        private int mSurveyId;

        public int SurveyId
        {
            get
            {

                return mSurveyId;
            }
            set
            {
                mSurveyId = value;
            }
        }

        private string mSurveyName;

        public string SurveyName
        {
            get
            {

                return mSurveyName;
            }
            set
            {
                mSurveyName = value;
            }
        }

        private string mQuestion;

        public string Question
        {
            get
            {

                return mQuestion;
            }
            set
            {
                mQuestion = value;
            }
        }

        public TransitSurveyQuestion()
        {

        }

        public TransitSurveyQuestion(SurveyQuestion p)
            : base(p.Id)
        {
            Question = p.Question;
            SurveyId = p.Survey.Id;
            SurveyName = p.Survey.Name;
        }

        public SurveyQuestion GetSurveyQuestion(ISession session)
        {
            SurveyQuestion p = (Id != 0) ? (SurveyQuestion)session.Load(typeof(SurveyQuestion), Id) : new SurveyQuestion();
            p.Question = this.Question;
            p.Survey = (Survey)session.Load(typeof(Survey), this.SurveyId);
            return p;
        }
    }

    public class ManagedSurveyQuestion : ManagedService
    {
        private SurveyQuestion mSurveyQuestion = null;

        public ManagedSurveyQuestion(ISession session)
            : base(session)
        {

        }

        public ManagedSurveyQuestion(ISession session, int id)
            : base(session)
        {
            mSurveyQuestion = (SurveyQuestion)session.Load(typeof(SurveyQuestion), id);
        }

        public ManagedSurveyQuestion(ISession session, SurveyQuestion value)
            : base(session)
        {
            mSurveyQuestion = value;
        }

        public int Id
        {
            get
            {
                return mSurveyQuestion.Id;
            }
        }

        public string Question
        {
            get
            {
                return mSurveyQuestion.Question;
            }
        }

        public TransitSurveyQuestion TransitSurveyQuestion
        {
            get
            {
                return new TransitSurveyQuestion(mSurveyQuestion);
            }
        }

        public void Delete()
        {
            Session.Delete(mSurveyQuestion);
        }

        public void Create(TransitSurveyQuestion q)
        {
            mSurveyQuestion = q.GetSurveyQuestion(Session);
            Session.Save(mSurveyQuestion);
        }
    }
}

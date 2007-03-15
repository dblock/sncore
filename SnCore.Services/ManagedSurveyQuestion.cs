using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace SnCore.Services
{
    public class TransitSurveyQuestion : TransitService<SurveyQuestion>
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

        public TransitSurveyQuestion(SurveyQuestion instance)
            : base(instance)
        {

        }

        public override void SetInstance(SurveyQuestion instance)
        {
            Question = instance.Question;
            SurveyId = instance.Survey.Id;
            SurveyName = instance.Survey.Name;
            base.SetInstance(instance);
        }

        public override SurveyQuestion GetInstance(ISession session, ManagedSecurityContext sec)
        {
            SurveyQuestion instance = base.GetInstance(session, sec);
            instance.Question = this.Question;
            if (Id == 0) instance.Survey = session.Load<Survey>(this.SurveyId);
            return instance;
        }
    }

    public class ManagedSurveyQuestion : ManagedService<SurveyQuestion, TransitSurveyQuestion>
    {
        public ManagedSurveyQuestion()
        {

        }

        public ManagedSurveyQuestion(ISession session)
            : base(session)
        {

        }

        public ManagedSurveyQuestion(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedSurveyQuestion(ISession session, SurveyQuestion value)
            : base(session, value)
        {

        }

        public string Question
        {
            get
            {
                return mInstance.Question;
            }
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

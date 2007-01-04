using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountSurveyAnswer : TransitService<AccountSurveyAnswer>
    {
        private int mAccountId;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private string mAccountName;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

        private int mAccountPictureId;

        public int AccountPictureId
        {
            get
            {

                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
            }
        }

        private int mSurveyQuestionId;

        public int SurveyQuestionId
        {
            get
            {

                return mSurveyQuestionId;
            }
            set
            {
                mSurveyQuestionId = value;
            }
        }

        private string mSurveyQuestion;

        public string SurveyQuestion
        {
            get
            {

                return mSurveyQuestion;
            }
            set
            {
                mSurveyQuestion = value;
            }
        }

        private string mAnswer;

        public string Answer
        {
            get
            {

                return mAnswer;
            }
            set
            {
                mAnswer = value;
            }
        }

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        public TransitAccountSurveyAnswer()
        {

        }

        public TransitAccountSurveyAnswer(AccountSurveyAnswer value)
            : base(value)
        {

        }

        public override void  SetInstance(AccountSurveyAnswer value)
        {
 	          SurveyQuestion = value.SurveyQuestion.Question;
            SurveyQuestionId = value.SurveyQuestion.Id;
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            Answer = value.Answer;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountSurveyAnswer GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountSurveyAnswer instance = base.GetInstance(session, sec);

            if (Id == 0) 
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.SurveyQuestion = (SurveyQuestion)session.Load(typeof(SurveyQuestion), this.SurveyQuestionId);
            }

            instance.Answer = this.Answer;
            return instance;
        }
    }

    public class ManagedAccountSurveyAnswer : ManagedService<AccountSurveyAnswer, TransitAccountSurveyAnswer>
    {
        public ManagedAccountSurveyAnswer()
        {

        }

        public ManagedAccountSurveyAnswer(ISession session)
            : base(session)
        {

        }

        public ManagedAccountSurveyAnswer(ISession session, int id)
            : base(session, id)
        {
            
        }

        public ManagedAccountSurveyAnswer(ISession session, AccountSurveyAnswer value)
            : base(session, value)
        {

        }

        public string Question
        {
            get
            {
                return mInstance.SurveyQuestion.Question;
            }
        }

        public string Answer
        {
            get
            {
                return mInstance.Answer;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountSurveyAnswer>.GetSafeCollection(mInstance.Account.AccountSurveyAnswers).Remove(mInstance);
            base.Delete(sec);
        }

        public void AddTagWordsTo(ManagedTagWordCollection tags)
        {
            tags.AddData(mInstance.Answer);
        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }


        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

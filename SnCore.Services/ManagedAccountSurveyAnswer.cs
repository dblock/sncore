using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace SnCore.Services
{
    public class TransitAccountSurveyAnswer : TransitService
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

        public TransitAccountSurveyAnswer(AccountSurveyAnswer p)
            : base(p.Id)
        {
            SurveyQuestion = p.SurveyQuestion.Question;
            SurveyQuestionId = p.SurveyQuestion.Id;
            AccountId = p.Account.Id;
            AccountName = p.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(p.Account);
            Answer = p.Answer;
            Created = p.Created;
            Modified = p.Modified;
        }

        public AccountSurveyAnswer GetAccountSurveyAnswer(ISession session)
        {
            AccountSurveyAnswer p = (Id != 0) ? (AccountSurveyAnswer)session.Load(typeof(AccountSurveyAnswer), Id) : new AccountSurveyAnswer();

            if (Id == 0)
            {
                // survey queston and account cannot be modified after creation
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), this.AccountId);
                if (SurveyQuestionId > 0) p.SurveyQuestion = (SurveyQuestion)session.Load(typeof(SurveyQuestion), this.SurveyQuestionId);
            }

            p.Answer = this.Answer;
            return p;
        }
    }

    public class ManagedAccountSurveyAnswer : ManagedService
    {
        private AccountSurveyAnswer mAccountSurveyAnswer = null;

        public ManagedAccountSurveyAnswer(ISession session)
            : base(session)
        {

        }

        public ManagedAccountSurveyAnswer(ISession session, int id)
            : base(session)
        {
            mAccountSurveyAnswer = (AccountSurveyAnswer)session.Load(typeof(AccountSurveyAnswer), id);
        }

        public ManagedAccountSurveyAnswer(ISession session, AccountSurveyAnswer value)
            : base(session)
        {
            mAccountSurveyAnswer = value;
        }

        public int Id
        {
            get
            {
                return mAccountSurveyAnswer.Id;
            }
        }

        public string Question
        {
            get
            {
                return mAccountSurveyAnswer.SurveyQuestion.Question;
            }
        }

        public string Answer
        {
            get
            {
                return mAccountSurveyAnswer.Answer;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountSurveyAnswer.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountSurveyAnswer.Modified;
            }
        }

        public TransitAccountSurveyAnswer TransitAccountSurveyAnswer
        {
            get
            {
                return new TransitAccountSurveyAnswer(mAccountSurveyAnswer);
            }
        }

        public void Delete()
        {
            mAccountSurveyAnswer.Account.AccountSurveyAnswers.Remove(mAccountSurveyAnswer);
            Session.Delete(mAccountSurveyAnswer);
        }

        public void AddTagWordsTo(ManagedTagWordCollection tags)
        {
            tags.AddData(mAccountSurveyAnswer.Answer);
        }

        public Account Account
        {
            get
            {
                return mAccountSurveyAnswer.Account;
            }
        }
    }
}

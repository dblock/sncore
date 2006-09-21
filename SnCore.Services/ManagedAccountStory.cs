using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace SnCore.Services
{
    public class AccountStoryQueryOptions
    {
        private bool mPublishedOnly = true;

        public bool PublishedOnly
        {
            get
            {
                return mPublishedOnly;
            }
            set
            {
                mPublishedOnly = value;
            }
        }

        public AccountStoryQueryOptions()
        {

        }
    }

    public class TransitAccountStory : TransitService
    {
        private string mName;

        public string Name
        {
            get
            {

                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mSummary;

        public string Summary
        {
            get
            {

                return mSummary;
            }
            set
            {
                mSummary = value;
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

        private int mAccountStoryPictureId;

        public int AccountStoryPictureId
        {
            get
            {
                return mAccountStoryPictureId;
            }
            set
            {
                mAccountStoryPictureId = value;
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

        private int mCommentCount;

        public int CommentCount
        {
            get
            {

                return mCommentCount;
            }
            set
            {
                mCommentCount = value;
            }
        }

        private bool mPublish;

        public bool Publish
        {
            get
            {
                return mPublish;
            }
            set
            {
                mPublish = value;
            }
        }

        public TransitAccountStory()
        {

        }

        public TransitAccountStory(ISession session, AccountStory s)
            : base(s.Id)
        {
            Name = s.Name;
            Summary = s.Summary;
            AccountId = s.Account.Id;
            AccountName = s.Account.Name;
            AccountPictureId = ManagedService.GetRandomElementId(s.Account.AccountPictures);
            AccountStoryPictureId = ManagedService.GetRandomElementId(s.AccountStoryPictures);
            Created = s.Created;
            Modified = s.Modified;
            Publish = s.Publish;
            CommentCount = ManagedDiscussion.GetDiscussionPostCount(session, s.Account.Id,
                ManagedDiscussion.AccountStoryDiscussion, s.Id);
        }

        public AccountStory GetAccountStory(ISession session)
        {
            AccountStory s = (Id != 0) ? (AccountStory)session.Load(typeof(AccountStory), Id) : new AccountStory();

            if (Id == 0)
            {
                if (AccountId > 0) s.Account = (Account)session.Load(typeof(Account), this.AccountId);
            }

            s.Name = this.Name;
            s.Summary = this.Summary;
            s.Publish = this.Publish;
            return s;
        }

    }

    public class ManagedAccountStory : ManagedService
    {
        private AccountStory mAccountStory = null;

        public ManagedAccountStory(ISession session)
            : base(session)
        {

        }

        public ManagedAccountStory(ISession session, int id)
            : base(session)
        {
            mAccountStory = (AccountStory)session.Load(typeof(AccountStory), id);
        }

        public ManagedAccountStory(ISession session, AccountStory value)
            : base(session)
        {
            mAccountStory = value;
        }

        public int Id
        {
            get
            {
                return mAccountStory.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountStory.Account.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAccountStory.Name;
            }
        }

        public string Summary
        {
            get
            {
                return mAccountStory.Summary;
            }
        }

        public TransitAccountStory TransitAccountStory
        {
            get
            {
                return new TransitAccountStory(Session, mAccountStory);
            }
        }

        public void Delete()
        {
            ManagedFeature.Delete(Session, "AccountStory", Id);
            mAccountStory.Account.AccountStories.Remove(mAccountStory);
            Session.Delete(mAccountStory);
        }

        public ManagedAccountStoryPicture AddAccountStoryPicture(TransitAccountStoryPicture Picture)
        {
            AccountStoryPicture p = Picture.GetAccountStoryPicture(Session);

            // check that editing story Picture of current story
            if (p.Id != 0 && p.AccountStory.Id != Id)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            p.AccountStory = mAccountStory;
            p.Modifed = DateTime.UtcNow;

            if (p.Id == 0)
            {
                p.Created = p.Modifed;
                if (mAccountStory.AccountStoryPictures == null) mAccountStory.AccountStoryPictures = new ArrayList();
                if (mAccountStory.AccountStoryPictures.Count >= ManagedAccount.MaxOfAnything)
                {
                    throw new ManagedAccount.QuotaExceededException();
                }
                mAccountStory.AccountStoryPictures.Add(p);
                p.Location = mAccountStory.AccountStoryPictures.Count;
            }

            Session.Save(p);

            return new ManagedAccountStoryPicture(Session, p);
        }

        public void AddTagWordsTo(ManagedTagWordCollection tags)
        {
            tags.AddData(mAccountStory.Summary);
            tags.AddData(mAccountStory.Name);
        }
    }
}

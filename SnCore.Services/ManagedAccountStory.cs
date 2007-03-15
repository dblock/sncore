using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Data.Hibernate;

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

    public class TransitAccountStory : TransitService<AccountStory>
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

        private int mPictureId;

        public int AccountStoryPictureId
        {
            get
            {
                return mPictureId;
            }
            set
            {
                mPictureId = value;
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

        public TransitAccountStory(AccountStory value)
            : base(value)
        {

        }

        public TransitAccountStory(ISession session, AccountStory value)
            : base(value)
        {
            CommentCount = ManagedDiscussion.GetDiscussionPostCount(session, value.Account.Id,
                typeof(AccountStory), value.Id);
        }

        public override void SetInstance(AccountStory value)
        {
            Name = value.Name;
            Summary = value.Summary;
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountStoryPictureId = ManagedService<AccountStoryPicture, TransitAccountStoryPicture>.GetRandomElementId(value.AccountStoryPictures);
            Created = value.Created;
            Modified = value.Modified;
            Publish = value.Publish;
            base.SetInstance(value);
        }

        public override AccountStory GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountStory instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Summary = this.Summary;
            instance.Publish = this.Publish;
            return instance;
        }
    }

    public class ManagedAccountStory : ManagedService<AccountStory, TransitAccountStory>
    {
        public ManagedAccountStory()
        {

        }

        public ManagedAccountStory(ISession session)
            : base(session)
        {

        }

        public ManagedAccountStory(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountStory(ISession session, AccountStory value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public string Summary
        {
            get
            {
                return mInstance.Summary;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {           
            base.Delete(sec);
            ManagedDiscussion.FindAndDelete(Session, mInstance.Account.Id, typeof(AccountStory), mInstance.Id, sec);
            ManagedFeature.Delete(Session, "AccountStory", Id);
            Collection<AccountStory>.GetSafeCollection(mInstance.Account.AccountStories).Remove(mInstance);
        }

        public void AddTagWordsTo(ManagedTagWordCollection tags)
        {
            tags.AddData(mInstance.Summary);
            tags.AddData(mInstance.Name);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountStory t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerifiedEmail();
        }
    }
}

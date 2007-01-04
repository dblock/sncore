using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitAccountFeedItem : TransitService<AccountFeedItem>
    {
        private int mAccountFeedId;

        public int AccountFeedId
        {
            get
            {

                return mAccountFeedId;
            }
            set
            {
                mAccountFeedId = value;
            }
        }

        private string mTitle;

        public string Title
        {
            get
            {

                return mTitle;
            }
            set
            {
                mTitle = value;
            }
        }

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
            }
        }

        private string mLink;

        public string Link
        {
            get
            {

                return mLink;
            }
            set
            {
                mLink = value;
            }
        }

        private string mGuid;

        public string Guid
        {
            get
            {

                return mGuid;
            }
            set
            {
                mGuid = value;
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

        private DateTime mUpdated;

        public DateTime Updated
        {
            get
            {

                return mUpdated;
            }
            set
            {
                mUpdated = value;
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

        private string mAccountFeedLinkUrl;

        public string AccountFeedLinkUrl
        {
            get
            {
                return mAccountFeedLinkUrl;
            }
            set
            {
                mAccountFeedLinkUrl = value;
            }
        }

        private string mAccountFeedName;

        public string AccountFeedName
        {
            get
            {

                return mAccountFeedName;
            }
            set
            {
                mAccountFeedName = value;
            }
        }

        private int mCommentCount = 0;

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

        public TransitAccountFeedItem()
        {

        }

        public TransitAccountFeedItem(AccountFeedItem value)
            : base(value)
        {
        }

        public TransitAccountFeedItem(ISession session, AccountFeedItem value)
            : base(value)
        {
            CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                session, value.AccountFeed.Account.Id, ManagedDiscussion.AccountFeedItemDiscussion, value.Id);
        }

        public override void SetInstance(AccountFeedItem value)
        {
            AccountFeedId = value.AccountFeed.Id;
            Title = value.Title;
            Description = value.Description;
            Link = value.Link;
            Guid = value.Guid;
            Created = value.Created;
            Updated = value.Updated;
            AccountId = value.AccountFeed.Account.Id;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.AccountFeed.Account);
            AccountName = value.AccountFeed.Account.Name;
            AccountFeedName = value.AccountFeed.Name;
            AccountFeedLinkUrl = value.AccountFeed.LinkUrl;
            base.SetInstance(value);
        }

        public override AccountFeedItem GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFeedItem instance = base.GetInstance(session, sec);
            instance.Title = this.Title;
            instance.Description = this.Description;
            instance.Link = this.Link;
            instance.Guid = this.Guid;
            if (this.AccountFeedId != 0) instance.AccountFeed = (AccountFeed)session.Load(typeof(AccountFeed), AccountFeedId);
            return instance;
        }
    }

    public class ManagedAccountFeedItem : ManagedService<AccountFeedItem, TransitAccountFeedItem>
    {
        public ManagedAccountFeedItem()
        {

        }

        public ManagedAccountFeedItem(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItem(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFeedItem(ISession session, AccountFeedItem value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountFeed.Account.Id, ManagedDiscussion.AccountFeedItemDiscussion, mInstance.Id);
            ManagedFeature.Delete(Session, "AccountFeedItem", Id);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Updated = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Updated;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountFeed.Account, DataOperation.All));
            return acl;
        }
    }
}
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
    public class TransitAccountFeedItem : TransitService
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

        public TransitAccountFeedItem(ISession session, AccountFeedItem o)
            : base(o.Id)
        {
            AccountFeedId = o.AccountFeed.Id;
            Title = o.Title;
            Description = o.Description;
            Link = o.Link;
            Guid = o.Guid;
            Created = o.Created;
            Updated = o.Updated;
            AccountId = o.AccountFeed.Account.Id;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.AccountFeed.Account);
            AccountName = o.AccountFeed.Account.Name;
            AccountFeedName = o.AccountFeed.Name;
            AccountFeedLinkUrl = o.AccountFeed.LinkUrl;
            CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                session, o.AccountFeed.Account.Id, ManagedDiscussion.AccountFeedItemDiscussion, o.Id);
        }

        public AccountFeedItem GetAccountFeedItem(ISession session)
        {
            AccountFeedItem p = (Id != 0) ? (AccountFeedItem)session.Load(typeof(AccountFeedItem), Id) : new AccountFeedItem();
            p.Title = this.Title;
            p.Description = this.Description;
            p.Link = this.Link;
            p.Guid = this.Guid;
            if (this.AccountFeedId != 0) p.AccountFeed = (AccountFeed)session.Load(typeof(AccountFeed), AccountFeedId);
            return p;
        }
    }

    public class ManagedAccountFeedItem : ManagedService<AccountFeedItem>
    {
        private AccountFeedItem mAccountFeedItem = null;

        public ManagedAccountFeedItem(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItem(ISession session, int id)
            : base(session)
        {
            mAccountFeedItem = (AccountFeedItem)session.Load(typeof(AccountFeedItem), id);
        }

        public ManagedAccountFeedItem(ISession session, AccountFeedItem value)
            : base(session)
        {
            mAccountFeedItem = value;
        }

        public ManagedAccountFeedItem(ISession session, TransitAccountFeedItem value)
            : base(session)
        {
            mAccountFeedItem = value.GetAccountFeedItem(session);
        }

        public int Id
        {
            get
            {
                return mAccountFeedItem.Id;
            }
        }

        public TransitAccountFeedItem TransitAccountFeedItem
        {
            get
            {
                return new TransitAccountFeedItem(Session, mAccountFeedItem);
            }
        }

        public void CreateOrUpdate(TransitAccountFeedItem o)
        {
            mAccountFeedItem = o.GetAccountFeedItem(Session);
            mAccountFeedItem.Updated = DateTime.UtcNow;
            if (Id == 0) mAccountFeedItem.Created = mAccountFeedItem.Updated;
            Session.Save(mAccountFeedItem);
        }

        public void Delete()
        {
            try
            {
                int DiscussionId = ManagedDiscussion.GetDiscussionId(
                    Session, mAccountFeedItem.AccountFeed.Account.Id, ManagedDiscussion.AccountFeedItemDiscussion,
                    mAccountFeedItem.Id, false);
                Discussion mDiscussion = (Discussion)Session.Load(typeof(Discussion), DiscussionId);
                Session.Delete(mDiscussion);
            }
            catch (ManagedDiscussion.DiscussionNotFoundException)
            {

            }

            ManagedFeature.Delete(Session, "AccountFeedItem", Id);
            Session.Delete(mAccountFeedItem);
        }
    }
}
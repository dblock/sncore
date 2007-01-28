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
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitAccountFeedItemMediaQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public bool VisibleOnly = true;
        public bool InterestingOnly = false;

        public TransitAccountFeedItemMediaQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (VisibleOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountFeedItemMedia.Visible = 1");
            }

            if (InterestingOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountFeedItemMedia.Interesting = 1");
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT AccountFeedItemMedia FROM AccountFeedItemMedia AccountFeedItemMedia");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }
            return b.ToString();
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountFeedItemMedia : TransitService<AccountFeedItemMedia>
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

        private string mAccountFeedItemTitle;

        public string AccountFeedItemTitle
        {
            get
            {
                return mAccountFeedItemTitle;
            }
            set
            {
                mAccountFeedItemTitle = value;
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

        private byte[] mThumbnail;

        public byte[] Thumbnail
        {
            get
            {
                return mThumbnail;
            }
            set
            {
                mThumbnail = value;
            }
        }

        private string mLastError;

        public string LastError
        {
            get
            {
                return mLastError;
            }
            set
            {
                mLastError = value;
            }
        }

        private int mAccountFeedItemId;

        public int AccountFeedItemId
        {
            get
            {

                return mAccountFeedItemId;
            }
            set
            {
                mAccountFeedItemId = value;
            }
        }

        private string mEmbeddedHtml;

        public string EmbeddedHtml
        {
            get
            {

                return mEmbeddedHtml;
            }
            set
            {
                mEmbeddedHtml = value;
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

        private bool mInteresting;

        public bool Interesting
        {
            get
            {
                return mInteresting;
            }
            set
            {
                mInteresting = value;
            }
        }

        private bool mVisible;

        public bool Visible
        {
            get
            {
                return mVisible;
            }
            set
            {
                mVisible = value;
            }
        }

        public TransitAccountFeedItemMedia()
        {

        }

        public TransitAccountFeedItemMedia(ISession session, AccountFeedItemMedia instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountFeedItemMedia instance)
        {
            AccountFeedItemId = instance.AccountFeedItem.Id;
            EmbeddedHtml = instance.EmbeddedHtml;
            Created = instance.Created;
            Modified = instance.Modified;
            Interesting = instance.Interesting;
            Visible = instance.Visible;
            LastError = instance.LastError;
            AccountFeedItemTitle = instance.AccountFeedItem.Title;
            AccountFeedName = instance.AccountFeedItem.AccountFeed.Name;
            AccountName = instance.AccountFeedItem.AccountFeed.Account.Name;
            AccountId = instance.AccountFeedItem.AccountFeed.Account.Id;
            AccountFeedId = instance.AccountFeedItem.AccountFeed.Id;
            base.SetInstance(instance);
        }

        public override AccountFeedItemMedia GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFeedItemMedia instance = base.GetInstance(session, sec);
            instance.EmbeddedHtml = this.EmbeddedHtml;
            instance.Interesting = this.Interesting;
            instance.Visible = this.Visible;
            instance.LastError = this.LastError;
            if (this.AccountFeedItemId != 0) instance.AccountFeedItem = session.Load<AccountFeedItem>(AccountFeedItemId);
            return instance;
        }
    }

    public class ManagedAccountFeedItemMedia : ManagedService<AccountFeedItemMedia, TransitAccountFeedItemMedia>
    {
        public ManagedAccountFeedItemMedia()
        {

        }

        public ManagedAccountFeedItemMedia(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItemMedia(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFeedItemMedia(ISession session, AccountFeedItemMedia value)
            : base(session, value)
        {

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
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountFeedItem.AccountFeed.Account, DataOperation.All));
            return acl;
        }
    }
}

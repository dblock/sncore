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
    public class TransitAccountFeedItemImgQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public bool VisibleOnly = true;
        public bool InterestingOnly = false;

        public TransitAccountFeedItemImgQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (VisibleOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountFeedItemImg.Visible = 1");
            }

            if (InterestingOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountFeedItemImg.Interesting = 1");
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
            b.Append("SELECT AccountFeedItemImg FROM AccountFeedItemImg AccountFeedItemImg");
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

    public class TransitAccountFeedItemImg : TransitService<AccountFeedItemImg>
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

        private string mUrl;

        public string Url
        {
            get
            {

                return mUrl;
            }
            set
            {
                mUrl = value;
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

        public TransitAccountFeedItemImg()
        {

        }

        public TransitAccountFeedItemImg(ISession session, AccountFeedItemImg instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountFeedItemImg instance)
        {
            AccountFeedItemId = instance.AccountFeedItem.Id;
            Url = instance.Url;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            Interesting = instance.Interesting;
            Visible = instance.Visible;
            LastError = instance.LastError;
            Thumbnail = instance.Thumbnail;
            AccountFeedItemTitle = instance.AccountFeedItem.Title;
            AccountFeedName = instance.AccountFeedItem.AccountFeed.Name;
            AccountName = instance.AccountFeedItem.AccountFeed.Account.Name;
            AccountId = instance.AccountFeedItem.AccountFeed.Account.Id;
            AccountFeedId = instance.AccountFeedItem.AccountFeed.Id;
            base.SetInstance(instance);
        }

        public override AccountFeedItemImg GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFeedItemImg instance = base.GetInstance(session, sec);
            instance.Url = this.Url;
            instance.Description = this.Description;
            instance.Interesting = this.Interesting;
            instance.Visible = this.Visible;
            instance.LastError = this.LastError;
            if (this.Thumbnail != null) instance.Thumbnail = this.Thumbnail;
            if (this.AccountFeedItemId != 0) instance.AccountFeedItem = (AccountFeedItem)session.Load(typeof(AccountFeedItem), AccountFeedItemId);
            return instance;
        }
    }

    public class ManagedAccountFeedItemImg : ManagedService<AccountFeedItemImg, TransitAccountFeedItemImg>
    {
        public ManagedAccountFeedItemImg()
        {

        }

        public ManagedAccountFeedItemImg(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItemImg(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFeedItemImg(ISession session, AccountFeedItemImg value)
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
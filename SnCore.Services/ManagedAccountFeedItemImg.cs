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
                b.Append("i.Visible = 1");
            }

            if (InterestingOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("i.Interesting = 1");
            }

            return b.ToString();
        }

        public IQuery CreateCountQuery(ISession session)
        {
            return session.CreateQuery("SELECT COUNT(*) FROM AccountFeedItemImg i " + CreateSubQuery());
        }

        public IQuery CreateQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT i FROM AccountFeedItemImg i");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return session.CreateQuery(b.ToString());
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountFeedItemImg : TransitService
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

        public TransitAccountFeedItemImg(ISession session, AccountFeedItemImg o)
            : base(o.Id)
        {
            AccountFeedItemId = o.AccountFeedItem.Id;
            Url = o.Url;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            Interesting = o.Interesting;
            Visible = o.Visible;
            LastError = o.LastError;
            Thumbnail = o.Thumbnail;
            AccountFeedItemTitle = o.AccountFeedItem.Title;
            AccountFeedName = o.AccountFeedItem.AccountFeed.Name;
            AccountName = o.AccountFeedItem.AccountFeed.Account.Name;
            AccountId = o.AccountFeedItem.AccountFeed.Account.Id;
            AccountFeedId = o.AccountFeedItem.AccountFeed.Id;
        }

        public AccountFeedItemImg GetAccountFeedItemImg(ISession session)
        {
            AccountFeedItemImg p = (Id != 0) ? (AccountFeedItemImg)session.Load(typeof(AccountFeedItemImg), Id) : new AccountFeedItemImg();
            p.Url = this.Url;
            p.Description = this.Description;
            p.Interesting = this.Interesting;
            p.Visible = this.Visible;
            p.LastError = this.LastError;
            if (this.Thumbnail != null) p.Thumbnail = this.Thumbnail;
            if (this.AccountFeedItemId != 0) p.AccountFeedItem = (AccountFeedItem)session.Load(typeof(AccountFeedItem), AccountFeedItemId);
            return p;
        }
    }

    public class ManagedAccountFeedItemImg : ManagedService<AccountFeedItemImg>
    {
        private AccountFeedItemImg mAccountFeedItemImg = null;

        public ManagedAccountFeedItemImg(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItemImg(ISession session, int id)
            : base(session)
        {
            mAccountFeedItemImg = (AccountFeedItemImg)session.Load(typeof(AccountFeedItemImg), id);
        }

        public ManagedAccountFeedItemImg(ISession session, AccountFeedItemImg value)
            : base(session)
        {
            mAccountFeedItemImg = value;
        }

        public ManagedAccountFeedItemImg(ISession session, TransitAccountFeedItemImg value)
            : base(session)
        {
            mAccountFeedItemImg = value.GetAccountFeedItemImg(session);
        }

        public int Id
        {
            get
            {
                return mAccountFeedItemImg.Id;
            }
        }

        public TransitAccountFeedItemImg TransitAccountFeedItemImg
        {
            get
            {
                return new TransitAccountFeedItemImg(Session, mAccountFeedItemImg);
            }
        }

        public void CreateOrUpdate(TransitAccountFeedItemImg o)
        {
            mAccountFeedItemImg = o.GetAccountFeedItemImg(Session);
            mAccountFeedItemImg.Modified = DateTime.UtcNow;
            if (Id == 0) mAccountFeedItemImg.Created = mAccountFeedItemImg.Modified;
            Session.Save(mAccountFeedItemImg);
        }

        public void Delete()
        {
            Session.Delete(mAccountFeedItemImg);
        }
    }
}
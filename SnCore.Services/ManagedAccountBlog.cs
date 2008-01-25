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
using System.Collections.Generic;
using System.Web;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using Rss;
using Atom.Core;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountBlog : TransitService<AccountBlog>
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

        private bool mEnableComments = true;

        public bool EnableComments
        {
            get
            {
                return mEnableComments;
            }
            set
            {
                mEnableComments = value;
            }
        }

        private int mDefaultViewRows = 5;

        public int DefaultViewRows
        {
            get
            {
                return mDefaultViewRows;
            }
            set
            {
                mDefaultViewRows = value;
            }
        }

        private int mPostCount = 0;

        public int PostCount
        {
            get
            {
                return mPostCount;
            }
            set
            {
                mPostCount = value;
            }
        }

        public TransitAccountBlog()
        {

        }

        public TransitAccountBlog(AccountBlog instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountBlog instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            Created = instance.Created;
            Updated = instance.Updated;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            EnableComments = instance.EnableComments;
            DefaultViewRows = instance.DefaultViewRows;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account);
            base.SetInstance(instance);
        }

        public override AccountBlog GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountBlog instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.EnableComments = this.EnableComments;
            instance.DefaultViewRows = this.DefaultViewRows;
            return instance;
        }
    }

    public class ManagedAccountBlog : ManagedAuditableService<AccountBlog, TransitAccountBlog>
    {
        public ManagedAccountBlog()
        {

        }

        public ManagedAccountBlog(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlog(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountBlog(ISession session, AccountBlog value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            // orphan groups
            foreach (AccountGroup group in Collection<AccountGroup>.GetSafeCollection(mInstance.AccountGroups))
            {
                group.AccountBlog = null;
                Session.Save(group);
            }
            // delete features
            ManagedFeature.Delete(Session, "AccountBlog", Id);
            // delete automatic feeds
            AccountFeed feed = GetSyndicatedFeed();
            if (feed != null) Session.Delete(feed);
            base.Delete(sec);
        }

        public bool CanDelete(int id)
        {
            if (mInstance.Account.Id == id)
                return true;

            if (mInstance.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mInstance.AccountBlogAuthors)
            {
                if (author.Account.Id == id && author.AllowDelete)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanEdit(int id)
        {
            if (mInstance.Account.Id == id)
                return true;

            if (mInstance.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mInstance.AccountBlogAuthors)
            {
                if (author.Account.Id == id && author.AllowEdit)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanPost(int id)
        {
            if (mInstance.Account.Id == id)
                return true;

            if (mInstance.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mInstance.AccountBlogAuthors)
            {
                if (author.Account.Id == id && author.AllowPost)
                {
                    return true;
                }
            }

            return false;
        }

        public string FeedUrl
        {
            get
            {
                string website = ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore");
                return string.Format("{0}/AccountBlogRss.aspx?id={1}", website, mInstance.Id);
            }
        }

        public string LinkUrl
        {
            get
            {
                string website = ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore");
                return string.Format("{0}/AccountBlogView.aspx?id={1}", website, mInstance.Id);
            }
        }

        private AccountFeed GetSyndicatedFeed()
        {
            return (AccountFeed)Session.CreateCriteria(typeof(AccountFeed))
                .Add(Expression.Eq("FeedUrl", FeedUrl))
                .SetMaxResults(1)
                .UniqueResult();
        }

        public int Syndicate(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Update);

            AccountFeed feed = GetSyndicatedFeed();

            if (feed == null)
            {
                feed = new AccountFeed();
                feed.Account = mInstance.Account;
                feed.Created = feed.Updated = DateTime.UtcNow;
                feed.UpdateFrequency = 12;
                feed.PublishImgs = true;
                feed.Publish = true;
                feed.FeedUrl = FeedUrl;
                feed.LinkUrl = LinkUrl;
                feed.FeedType = ManagedFeedType.GetDefaultFeedType(Session);
            }
            else
            {
                feed.Updated = DateTime.UtcNow;
            }

            feed.Name = mInstance.Name;
            feed.Description = mInstance.Description;
            Session.Save(feed);
            return feed.Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Updated = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Updated;
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

        protected override void Check(TransitAccountBlog t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                GetQuota(sec).Check<AccountBlog, ManagedAccount.QuotaExceededException>(
                    mInstance.Account.AccountBlogs);
            }
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.Account,
                        string.Format("[user:{0}] has started a new blog: [blog:{1}]", 
                        mInstance.Account.Id, mInstance.Id),
                        string.Format("AccountBlogView.aspx?id={0}", mInstance.Id)));
                    break;
            }
            return result;
        }

        public override TransitAccountBlog GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountBlog t_instance = base.GetTransitInstance(sec);
            t_instance.PostCount = GetPostCount(Session, t_instance.Id);
            return t_instance;
        }

        public static int GetPostCount(ISession session, int blogid)
        {
            return session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountBlogPost p WHERE p.AccountBlog.Id = {0}",
                    blogid)).UniqueResult<int>();
        }
    }
}
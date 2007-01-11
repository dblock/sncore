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
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account);
            base.SetInstance(instance);
        }

        public override AccountBlog GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountBlog instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedAccountBlog : ManagedService<AccountBlog, TransitAccountBlog>
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
            ManagedFeature.Delete(Session, "AccountBlog", Id);
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
                return string.Format("{0}/AccountBlog.aspx?id={1}", website, mInstance.Id);
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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}
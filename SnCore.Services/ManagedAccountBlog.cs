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
    public class TransitAccountBlog : TransitService
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

        public TransitAccountBlog(AccountBlog o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Created = o.Created;
            Updated = o.Updated;
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.Account);
        }

        public AccountBlog GetAccountBlog(ISession session)
        {
            AccountBlog p = (Id != 0) ? (AccountBlog)session.Load(typeof(AccountBlog), Id) : new AccountBlog();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            p.Name = this.Name;
            p.Description = this.Description;
            return p;
        }
    }

    public class ManagedAccountBlog : ManagedService
    {
        private AccountBlog mAccountBlog = null;

        public ManagedAccountBlog(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlog(ISession session, int id)
            : base(session)
        {
            mAccountBlog = (AccountBlog)session.Load(typeof(AccountBlog), id);
        }

        public ManagedAccountBlog(ISession session, AccountBlog value)
            : base(session)
        {
            mAccountBlog = value;
        }

        public ManagedAccountBlog(ISession session, TransitAccountBlog value)
            : base(session)
        {
            mAccountBlog = value.GetAccountBlog(session);
        }

        public int Id
        {
            get
            {
                return mAccountBlog.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountBlog.Account.Id;
            }
        }

        public TransitAccountBlog TransitAccountBlog
        {
            get
            {
                return new TransitAccountBlog(mAccountBlog);
            }
        }

        public void Delete()
        {
            ManagedFeature.Delete(Session, "AccountBlog", Id);
            AccountFeed feed = GetSyndicatedFeed();

            if (feed != null)
            {
                Session.Delete(feed);
            }

            Session.Delete(mAccountBlog);
        }

        public bool CanDelete(int id)
        {
            if (mAccountBlog.Account.Id == id)
                return true;

            if (mAccountBlog.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mAccountBlog.AccountBlogAuthors)
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
            if (mAccountBlog.Account.Id == id)
                return true;

            if (mAccountBlog.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mAccountBlog.AccountBlogAuthors)
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
            if (mAccountBlog.Account.Id == id)
                return true;

            if (mAccountBlog.AccountBlogAuthors == null)
                return false;

            foreach (AccountBlogAuthor author in mAccountBlog.AccountBlogAuthors)
            {
                if (author.Account.Id == id && author.AllowPost)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetPermissions(int accountid, TransitAccountBlogPost post)
        {
            post.CanEdit = ((post.AccountId == accountid) || CanEdit(accountid));
            post.CanDelete = ((post.AccountId == accountid) || CanDelete(accountid));
        }

        public string FeedUrl
        {
            get
            {
                string website = ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore");
                return string.Format("{0}/AccountBlogRss.aspx?id={1}", website, mAccountBlog.Id);
            }
        }

        public string LinkUrl
        {
            get
            {
                string website = ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore");
                return string.Format("{0}/AccountBlog.aspx?id={1}", website, mAccountBlog.Id);
            }
        }

        private AccountFeed GetSyndicatedFeed()
        {
            return (AccountFeed)Session.CreateCriteria(typeof(AccountFeed))
                .Add(Expression.Eq("FeedUrl", FeedUrl))
                .SetMaxResults(1)
                .UniqueResult();
        }

        public int Syndicate()
        {
            AccountFeed feed = GetSyndicatedFeed();

            if (feed == null)
            {
                feed = new AccountFeed();
                feed.Account = mAccountBlog.Account;
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

            feed.Name = mAccountBlog.Name;
            feed.Description = mAccountBlog.Description;
            Session.Save(feed);
            return feed.Id;
        }
    }
}
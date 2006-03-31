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
    public class TransitAccountFeed : TransitService
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

        private string mUsername;

        public string Username
        {
            get
            {

                return mUsername;
            }
            set
            {
                mUsername = value;
            }
        }

        private string mPassword;

        public string Password
        {
            get
            {

                return mPassword;
            }
            set
            {
                mPassword = value;
            }
        }

        private int mUpdateFrequency;

        public int UpdateFrequency
        {
            get
            {

                return mUpdateFrequency;
            }
            set
            {
                mUpdateFrequency = value;
            }
        }

        private string mFeedUrl;

        public string FeedUrl
        {
            get
            {

                return mFeedUrl;
            }
            set
            {
                mFeedUrl = value;
            }
        }

        private string mLinkUrl;

        public string LinkUrl
        {
            get
            {

                return mLinkUrl;
            }
            set
            {
                mLinkUrl = value;
            }
        }

        private string mFeedType;

        public string FeedType
        {
            get
            {

                return mFeedType;
            }
            set
            {
                mFeedType = value;
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

        public TransitAccountFeed()
        {

        }

        public TransitAccountFeed(AccountFeed o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Created = o.Created;
            Updated = o.Updated;
            LastError = o.LastError;
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            AccountPictureId = ManagedService.GetRandomElementId(o.Account.AccountPictures);
            Username = o.Username;
            Password = o.Password;
            UpdateFrequency = o.UpdateFrequency;
            FeedUrl = o.FeedUrl;
            LinkUrl = o.LinkUrl;
            FeedType = o.FeedType.Name;
            Publish = o.Publish;
        }

        public AccountFeed GetAccountFeed(ISession session)
        {
            AccountFeed p = (Id != 0) ? (AccountFeed)session.Load(typeof(AccountFeed), Id) : new AccountFeed();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            p.Name = this.Name;
            p.Description = this.Description;
            p.LastError = this.LastError;
            p.Username = this.Username;
            p.Password = this.Password;
            p.UpdateFrequency = this.UpdateFrequency;
            p.FeedUrl = this.FeedUrl;
            p.LinkUrl = this.LinkUrl;
            p.Publish = this.Publish;
            if (!string.IsNullOrEmpty(this.FeedType)) p.FeedType = ManagedFeedType.Find(session, this.FeedType);
            return p;
        }
    }

    public class ManagedAccountFeed : ManagedService
    {
        private AccountFeed mAccountFeed = null;

        public ManagedAccountFeed(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeed(ISession session, int id)
            : base(session)
        {
            mAccountFeed = (AccountFeed)session.Load(typeof(AccountFeed), id);
        }

        public ManagedAccountFeed(ISession session, AccountFeed value)
            : base(session)
        {
            mAccountFeed = value;
        }

        public ManagedAccountFeed(ISession session, TransitAccountFeed value)
            : base(session)
        {
            mAccountFeed = value.GetAccountFeed(session);
        }

        public int Id
        {
            get
            {
                return mAccountFeed.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountFeed.Account.Id;
            }
        }

        public TransitAccountFeed TransitAccountFeed
        {
            get
            {
                return new TransitAccountFeed(mAccountFeed);
            }
        }

        public void Delete()
        {
            Session.Delete(string.Format("from Feature f where f.DataObjectId = {0} AND f.DataRowId = {1}",
                ManagedDataObject.Find(Session, "AccountFeed"), Id));
            Session.Delete(mAccountFeed);
        }

        protected HttpWebRequest GetFeedHttpRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mAccountFeed.FeedUrl);
            System.Net.ServicePointManager.Expect100Continue = false;
            request.UserAgent = "SnCore/1.0";
            request.Timeout = 60 * 1000;
            request.KeepAlive = false;
            request.MaximumAutomaticRedirections = 5;

            if (!string.IsNullOrEmpty(mAccountFeed.Username))
            {
                request.Credentials = new NetworkCredential(
                                mAccountFeed.Username,
                                mAccountFeed.Password,
                                null);
            }

            return request;
        }

        protected Stream GetFeedStream()
        {
            return GetFeedHttpRequest().GetResponse().GetResponseStream();
        }

        protected XmlDocument GetFeed()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(new StreamReader(GetFeedStream()));
            return xml;
        }

        protected XmlDocument Transform(XmlDocument feed)
        {
            if (!string.IsNullOrEmpty(mAccountFeed.FeedType.Xsl))
            {
                StringBuilder ts = new StringBuilder();
                XslCompiledTransform fxsl = new XslCompiledTransform();
                fxsl.Load(new XmlTextReader(new StringReader(mAccountFeed.FeedType.Xsl)), null, null);
                XPathDocument nav = new XPathDocument(new StringReader(feed.OuterXml));
                StringWriter sw = new StringWriter(ts);
                XmlTextWriter tw = new XmlTextWriter(sw);
                fxsl.Transform(nav, tw);
                XmlDocument result = new XmlDocument();
                result.LoadXml(ts.ToString());
                return result;
            }
            else
            {
                return feed;
            }
        }

        public bool Update(AtomFeed feed, IList deleted, List<AccountFeedItem> updated)
        {
            if (feed.Entries.Count == 0)
                return false;

            foreach (AtomEntry atomitem in feed.Entries)
            {
                // fetch an existing item if any

                AccountFeedItem item = null;

                if (atomitem.Id != null && !string.IsNullOrEmpty(atomitem.Id.ToString()))
                {
                    item = (AccountFeedItem)Session.CreateCriteria(typeof(AccountFeedItem))
                        .Add(Expression.Eq("Guid", atomitem.Id.ToString()))
                        .Add(Expression.Eq("AccountFeed.Id", Id))
                        .UniqueResult();
                }

                if (item == null)
                {
                    item = new AccountFeedItem();
                    item.Created = item.Updated = DateTime.UtcNow;
                }
                else if (
                    atomitem.Modified != null
                    && atomitem.Modified.DateTime.Ticks > 0
                    && atomitem.Modified.DateTime.ToUniversalTime() <= item.Updated)
                {
                    // item has not been modified since last update
                    deleted.Remove(item);
                    continue;
                }
                else if (
                    atomitem.Modified == null
                    && atomitem.Created != null
                    && atomitem.Created.DateTime.Ticks > 0
                    && atomitem.Created.DateTime.ToUniversalTime() <= item.Updated)
                {
                    // item creation date has not been modified since last update and there's no modified date
                    deleted.Remove(item);
                    continue;
                }
                else
                {
                    // remove the item from obsolete/deleted items
                    item.Updated = DateTime.UtcNow;
                    deleted.Remove(item);
                }

                item.AccountFeed = mAccountFeed;
                item.Description = string.Empty;
                foreach (AtomContent content in atomitem.Contents)
                {
                    if (!string.IsNullOrEmpty(item.Description))
                        item.Description += "\n";

                    item.Description += content.Content;
                }

                if (string.IsNullOrEmpty(item.Description))
                {
                    item.Description = atomitem.Summary.Content;
                }

                item.Title = atomitem.Title.Content;
                if (atomitem.Links.Count > 0) item.Link = atomitem.Links[0].HRef.ToString();
                if (atomitem.Id != null && !string.IsNullOrEmpty(atomitem.Id.ToString())) item.Guid = atomitem.Id.ToString();
                if (atomitem.Created != null && atomitem.Created.DateTime.Ticks > 0) item.Created = atomitem.Created.DateTime.ToUniversalTime();

                updated.Add(item);
            }

            return true;
        }

        public bool Update(RssFeed feed, IList deleted, List<AccountFeedItem> updated)
        {
            if (feed.Channels.Count == 0)
                return false;

            foreach (RssChannel rsschannel in feed.Channels)
            {
                foreach (RssItem rssitem in rsschannel.Items)
                {
                    // fetch an existing item if any

                    AccountFeedItem item = null;

                    if (rssitem.Guid != null && !string.IsNullOrEmpty(rssitem.Guid.Name))
                    {
                        item = (AccountFeedItem)Session.CreateCriteria(typeof(AccountFeedItem))
                            .Add(Expression.Eq("Guid", rssitem.Guid.Name))
                            .Add(Expression.Eq("AccountFeed.Id", Id))
                            .UniqueResult();
                    }
                    else if (rssitem.Link != null && !string.IsNullOrEmpty(rssitem.Link.ToString()))
                    {
                        item = (AccountFeedItem)Session.CreateCriteria(typeof(AccountFeedItem))
                            .Add(Expression.Eq("Link", rssitem.Link.ToString()))
                            .Add(Expression.Eq("AccountFeed.Id", Id))
                            .UniqueResult();
                    }

                    if (item == null)
                    {
                        item = new AccountFeedItem();
                        item.Created = item.Updated = DateTime.UtcNow;
                    }
                    else if ((rssitem.PubDate.Ticks > 0) && (rssitem.PubDate.ToUniversalTime() <= item.Updated))
                    {
                        // item has not been modified since last update
                        deleted.Remove(item);
                        continue;
                    }
                    else
                    {
                        // remove the item from obsolete/deleted items
                        item.Updated = DateTime.UtcNow;
                        deleted.Remove(item);
                    }

                    item.AccountFeed = mAccountFeed;
                    item.Description = rssitem.Description;
                    item.Title = rssitem.Title;
                    if (rssitem.Link != null) item.Link = rssitem.Link.ToString();
                    if (rssitem.Guid != null) item.Guid = rssitem.Guid.Name;
                    if (rssitem.PubDate.Ticks > 0) item.Created = rssitem.PubDate.ToUniversalTime();
                    updated.Add(item);
                }
            }

            return true;
        }

        public int Update()
        {
            IList deleted = mAccountFeed.AccountFeedItems;
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            if (!Update(RssFeed.Read(GetFeedHttpRequest()), deleted, updated))
                if (!Update(AtomFeed.Load(GetFeedStream()), deleted, updated))
                    throw new Exception("Invalid or empty RSS or ATOM feed.");

            try
            {
                foreach (AccountFeedItem item in deleted)
                    Session.Delete(item);

                foreach (AccountFeedItem item in updated)
                    Session.SaveOrUpdate(item);

                mAccountFeed.AccountFeedItems = updated;
                mAccountFeed.LastError = string.Empty;
                Session.SaveOrUpdate(mAccountFeed);
            }
            catch (Exception ex)
            {
                mAccountFeed.Updated = DateTime.UtcNow;
                mAccountFeed.LastError = ex.Message;
                Session.Save(mAccountFeed);
                throw;
            }

            return updated.Count;
        }
    }
}
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
    public class TransitRssItem
    {
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

        private DateTime mPubDate;

        public DateTime PubDate
        {
            get
            {
                return mPubDate;
            }
            set
            {
                mPubDate = value;
            }
        }

        public TransitRssItem()
        {

        }

        public TransitRssItem(RssItem item)
        {
            mDescription = item.Description;
            mTitle = item.Title;
            mPubDate = item.PubDate;
        }
    }

    public class TransitAccountRssWatch : TransitService<AccountRssWatch>
    {
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

        private DateTime mSent;

        public DateTime Sent
        {
            get
            {

                return mSent;
            }
            set
            {
                mSent = value;
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

        private bool mEnabled;

        public bool Enabled
        {
            get
            {

                return mEnabled;
            }
            set
            {
                mEnabled = value;
            }
        }

        public TransitAccountRssWatch()
        {

        }

        public TransitAccountRssWatch(AccountRssWatch instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountRssWatch instance)
        {
            Url = instance.Url;
            Name = instance.Name;
            Created = instance.Created;
            Modified = instance.Modified;
            Sent = instance.Sent;
            AccountId = instance.Account.Id;
            UpdateFrequency = instance.UpdateFrequency;
            Enabled = instance.Enabled;
            LastError = instance.LastError;
            base.SetInstance(instance);
        }

        public override AccountRssWatch GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountRssWatch instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Url = this.Url;
            instance.Name = this.Name;
            instance.UpdateFrequency = this.UpdateFrequency;
            instance.Enabled = this.Enabled;
            instance.LastError = this.LastError;
            return instance;
        }
    }

    public class ManagedAccountRssWatch : ManagedService<AccountRssWatch, TransitAccountRssWatch>
    {
        public ManagedAccountRssWatch()
        {

        }

        public ManagedAccountRssWatch(ISession session)
            : base(session)
        {

        }

        public ManagedAccountRssWatch(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountRssWatch(ISession session, AccountRssWatch value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Sent = mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountRssWatch t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota().Check(mInstance.Account.AccountRssWatchs);
        }

        private List<RssItem> GetFeedItemsSince(DateTime last)
        {
            Stream s = new MemoryStream(ASCIIEncoding.Default.GetBytes(ManagedSiteConnector.GetHttpContentAsUser(
                Session, mInstance.Account.Id, mInstance.Url)));
            RssFeed feed = RssFeed.Read(s);
            if (feed.Channels.Count == 0) throw new SoapException(string.Format(
                "Missing RSS channel in '{0}'.", mInstance.Url), SoapException.ClientFaultCode);
            List<RssItem> newitems = new List<RssItem>();
            foreach (RssChannel channel in feed.Channels)
            {
                foreach (RssItem item in channel.Items)
                {
                    if (item.PubDate < last)
                        continue;

                    newitems.Add(item);
                }
            }
            return newitems;
        }

        private List<TransitRssItem> GetFeedItemsDataSince(DateTime last)
        {
            List<RssItem> newitems = GetFeedItemsSince(last);
            List<TransitRssItem> result = new List<TransitRssItem>(newitems.Count);
            foreach (RssItem newitem in newitems)
            {
                result.Add(new TransitRssItem(newitem));
            }
            return result;
        }

        public List<TransitRssItem> GetSubscriptionUpdates(ManagedSecurityContext sec)
        {
            // updating a subscription will update the sent date
            GetACL().Check(sec, DataOperation.Update);
            return GetFeedItemsDataSince(mInstance.Sent);
        }
    }
}
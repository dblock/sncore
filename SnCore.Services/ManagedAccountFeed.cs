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
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web.Html;
using SnCore.Tools.Drawing;

namespace SnCore.Services
{
    public class TransitAccountFeed : TransitService<AccountFeed>
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

        private bool mPublishImgs;

        public bool PublishImgs
        {
            get
            {

                return mPublishImgs;
            }
            set
            {
                mPublishImgs = value;
            }
        }

        private bool mPublishMedia;

        public bool PublishMedia
        {
            get
            {
                return mPublishMedia;
            }
            set
            {
                mPublishMedia = value;
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
            : base(o)
        {
        }

        public override void SetInstance(AccountFeed value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Updated = value.Updated;
            LastError = value.LastError;
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            Username = value.Username;
            // Password = value.Password;
            UpdateFrequency = value.UpdateFrequency;
            FeedUrl = value.FeedUrl;
            LinkUrl = value.LinkUrl;
            FeedType = value.FeedType.Name;
            Publish = value.Publish;
            PublishImgs = value.PublishImgs;
            PublishMedia = value.PublishMedia;
            base.SetInstance(value);
        }

        public override AccountFeed GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFeed instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.LastError = this.LastError;
            instance.Username = this.Username;
            instance.Password = this.Password;
            instance.UpdateFrequency = this.UpdateFrequency;
            instance.FeedUrl = this.FeedUrl;
            instance.LinkUrl = this.LinkUrl;
            instance.Publish = this.Publish;
            instance.PublishImgs = this.PublishImgs;
            instance.PublishMedia = this.PublishMedia;
            if (!string.IsNullOrEmpty(this.FeedType)) instance.FeedType = ManagedFeedType.Find(session, this.FeedType);
            return instance;
        }
    }

    public class ManagedAccountFeed : ManagedService<AccountFeed, TransitAccountFeed>
    {
        public ManagedAccountFeed()
        {

        }

        public ManagedAccountFeed(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeed(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFeed(ISession session, AccountFeed value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedFeature.Delete(Session, "AccountFeed", Id);
            base.Delete(sec);
        }

        public HttpWebRequest GetFeedHttpRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(mInstance.FeedUrl);
            System.Net.ServicePointManager.Expect100Continue = false;
            request.UserAgent = "SnCore/1.0";
            request.Timeout = 60 * 1000;
            request.KeepAlive = false;
            request.MaximumAutomaticRedirections = 5;

            if (!string.IsNullOrEmpty(mInstance.Username))
            {
                request.Credentials = new NetworkCredential(
                                mInstance.Username,
                                mInstance.Password,
                                null);
            }

            return request;
        }

        public Stream GetFeedStream()
        {
            Stream stream = GetFeedHttpRequest().GetResponse().GetResponseStream();

            if (mInstance.FeedType == null || string.IsNullOrEmpty(mInstance.FeedType.Xsl))
                return stream;

            return Transform(stream, mInstance.FeedType.Xsl);
        }

        public static Stream Transform(Stream input, string xsl)
        {
            XslCompiledTransform fxsl = new XslCompiledTransform();
            fxsl.Load(new XmlTextReader(new StringReader(xsl)), null, null);
            return Transform(input, fxsl);
        }

        public static Stream Transform(Stream input, XslCompiledTransform transform)
        {
            XPathDocument nav = new XPathDocument(input);
            Stream stream = new MemoryStream();
            transform.Transform(nav, null, stream);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream Transform(XmlDocument document, string xsl)
        {
            XslCompiledTransform fxsl = new XslCompiledTransform();
            fxsl.Load(new XmlTextReader(new StringReader(xsl)), null, null);
            return Transform(document, fxsl);
        }

        public static Stream Transform(XmlDocument document, XslCompiledTransform transform)
        {
            Stream stream = new MemoryStream();
            transform.Transform(document, null, stream);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public bool Update(AtomFeed feed, IList<AccountFeedItem> deleted, List<AccountFeedItem> updated)
        {
            if (feed.Entries.Count == 0)
                return false;

            if (string.IsNullOrEmpty(mInstance.Name) && feed.Title != null)
                mInstance.Name = feed.Title.Content;

            if (string.IsNullOrEmpty(mInstance.Description) && feed.SubTitle != null)
                mInstance.Description = feed.SubTitle.Content;

            if (string.IsNullOrEmpty(mInstance.Description) && (feed.Tagline != null))
                mInstance.Description = feed.Tagline.Content;

            if (string.IsNullOrEmpty(mInstance.LinkUrl) && feed.Links.Count > 0)
            {
                foreach (AtomLink link in feed.Links)
                {
                    if (link.Rel == Relationship.Alternate)
                    {
                        mInstance.LinkUrl = link.HRef.ToString();
                    }
                }
            }

            foreach (AtomEntry atomitem in feed.Entries)
            {
                // item is too far in the future, clearly bogus or purposeful in the future
                if (atomitem.Created != null
                    && atomitem.Created.DateTime.Ticks > 0
                    && DateTime.UtcNow.AddDays(1) < atomitem.Created.DateTime.ToUniversalTime())
                {
                    continue;
                }

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

                item.AccountFeed = mInstance;
                item.Description = string.Empty;
                foreach (AtomContent content in atomitem.Contents)
                {
                    if (!string.IsNullOrEmpty(item.Description))
                        item.Description += "\n";

                    switch (content.Type)
                    {
                        case MediaType.TextHtml:
                            item.Description += HttpUtility.HtmlDecode(content.Content);
                            break;
                        default:
                            item.Description += content.Content;
                            break;
                    }
                }

                if (string.IsNullOrEmpty(item.Description))
                {
                    item.Description = atomitem.Summary.Content;
                }

                item.Title = atomitem.Title.Content;

                if (atomitem.Links.Count > 0)
                {
                    foreach (AtomLink link in atomitem.Links)
                    {
                        if (link.Rel == Relationship.Alternate)
                        {
                            item.Link = link.HRef.ToString();
                        }
                    }
                }

                if (atomitem.Id != null && !string.IsNullOrEmpty(atomitem.Id.ToString())) item.Guid = atomitem.Id.ToString();
                if (atomitem.Created != null && atomitem.Created.DateTime.Ticks > 0) item.Created = atomitem.Created.DateTime.ToUniversalTime();

                updated.Add(item);
            }

            return true;
        }

        public bool Update(RssFeed feed, IList<AccountFeedItem> deleted, List<AccountFeedItem> updated)
        {
            if (feed.Channels.Count == 0)
                return false;

            foreach (RssChannel rsschannel in feed.Channels)
            {
                if (string.IsNullOrEmpty(mInstance.Name))
                    mInstance.Name = rsschannel.Title;

                if (string.IsNullOrEmpty(mInstance.Description))
                    mInstance.Description = rsschannel.Description;

                if (string.IsNullOrEmpty(mInstance.LinkUrl))
                    mInstance.LinkUrl = rsschannel.Link.ToString();

                foreach (RssItem rssitem in rsschannel.Items)
                {
                    // item is too far in the future, clearly bogus or purposeful in the future
                    if ((rssitem.PubDate.Ticks > 0) && (DateTime.UtcNow.AddDays(1) < rssitem.PubDate.ToUniversalTime()))
                        continue;

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

                    item.AccountFeed = mInstance;
                    item.Description = string.IsNullOrEmpty(rssitem.Content) ? rssitem.Description : rssitem.Content;
                    item.Title = rssitem.Title;
                    if (rssitem.Link != null) item.Link = rssitem.Link.ToString();
                    if (rssitem.Guid != null) item.Guid = rssitem.Guid.Name;
                    if (rssitem.PubDate.Ticks > 0) item.Created = rssitem.PubDate.ToUniversalTime();
                    updated.Add(item);
                }
            }

            return true;
        }

        public int Update(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Update);

            mInstance.Updated = DateTime.UtcNow;

            IList<AccountFeedItem> deleted = mInstance.AccountFeedItems;
            List<AccountFeedItem> updated = new List<AccountFeedItem>();

            bool fUpdated = Update(RssFeed.Read(GetFeedStream()), deleted, updated);

            if (!fUpdated) fUpdated = Update(AtomFeed.Load(GetFeedStream(),
              new Uri("http://www.w3.org/2005/Atom")), deleted, updated);

            if (!fUpdated) fUpdated = Update(AtomFeed.Load(GetFeedStream(),
               new Uri("http://purl.org/atom/ns#")), deleted, updated);

            if (!fUpdated)
            {
                throw new Exception("Invalid or empty RSS or ATOM feed.");
            }

            try
            {
                if (deleted != null)
                {
                    foreach (AccountFeedItem item in deleted)
                        Session.Delete(item);
                }

                foreach (AccountFeedItem item in updated)
                    Session.SaveOrUpdate(item);

                mInstance.AccountFeedItems = updated;
                mInstance.LastError = string.Empty;
                Session.Save(mInstance);
            }
            catch (Exception ex)
            {
                mInstance.LastError = ex.Message;
                Session.Save(mInstance);
                throw;
            }

            return updated.Count;
        }

        public int UpdateImages(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Update);

            int result = 0;
            Uri basehref = null;
            Uri.TryCreate(mInstance.LinkUrl, UriKind.Absolute, out basehref);

            IList items = Session.CreateCriteria(typeof(AccountFeedItem))
                .Add(Expression.Eq("AccountFeed.Id", mInstance.Id))
                .List();

            foreach (AccountFeedItem item in items)
            {
                IList<HtmlImage> images = null;

                try
                {
                    images = HtmlImageExtractor.Extract(item.Description, basehref);
                }
                catch
                {
                    continue;
                }

                TimeSpan tsDistribution = new TimeSpan(0, 30, 0);

                foreach (HtmlImage image in images)
                {
                    AccountFeedItemImg x_img = null;

                    // images may appear only once, repeating images don't get updated
                    // nor images linked from multiple feeds or feed items

                    x_img = (AccountFeedItemImg)Session.CreateCriteria(typeof(AccountFeedItemImg))
                            .Add(Expression.Eq("Url", image.Src)).UniqueResult();

                    if (x_img != null)
                    {
                        // image alRetreivey exists
                        continue;
                    }

                    x_img = new AccountFeedItemImg();
                    x_img.Created = item.Created.Subtract(tsDistribution); // shuffle images
                    tsDistribution = tsDistribution.Add(new TimeSpan(0, 30, 0));
                    x_img.Modified = DateTime.UtcNow;
                    x_img.AccountFeedItem = item;
                    x_img.Description = image.Alt;
                    x_img.Interesting = false;
                    x_img.Url = image.Src;

                    // fetch the image to get its size
                    try
                    {
                        WebClient client = new WebClient();
                        byte[] data = client.DownloadData(x_img.Url);
                        if (data == null) throw new Exception("Missing file data.");
                        ThumbnailBitmap bitmap = new ThumbnailBitmap(data);
                        x_img.Thumbnail = bitmap.Thumbnail;
                        x_img.Visible = mInstance.PublishImgs;

                        // hide images smaller than the thumbnail size
                        if (bitmap.Size.Height < ThumbnailBitmap.ThumbnailSize.Height
                            || bitmap.Size.Width < ThumbnailBitmap.ThumbnailSize.Width)
                        {
                            x_img.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        x_img.LastError = ex.Message;
                        x_img.Visible = false;
                    }

                    Session.Save(x_img);
                    result++;
                }
            }

            return result;
        }

        public int UpdateMedias(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Update);

            int result = 0;
            Uri basehref = null;
            Uri.TryCreate(mInstance.LinkUrl, UriKind.Absolute, out basehref);

            IList items = Session.CreateCriteria(typeof(AccountFeedItem))
                .Add(Expression.Eq("AccountFeed.Id", mInstance.Id))
                .List();

            TimeSpan tsDistribution = new TimeSpan(0, 30, 0);

            foreach (AccountFeedItem item in items)
            {
                List<HtmlGenericControl> embed = HtmlObjectExtractor.Extract(item.Description, basehref);
                foreach (HtmlGenericControl control in embed)
                {
                    string content = HtmlGenericCollector.GetHtml(control);

                    AccountFeedItemMedia x_media = null;

                    // media may appear only once, repeating media don't get updated
                    // TODO: expensive LIKE query

                    x_media = Session.CreateCriteria(typeof(AccountFeedItemMedia))
                            .Add(Expression.Like("EmbeddedHtml", content))
                            .UniqueResult<AccountFeedItemMedia>();

                    if (x_media != null)
                        continue;

                    try
                    {
                        x_media = new AccountFeedItemMedia();
                        x_media.AccountFeedItem = item;
                        x_media.Created = item.Created.Subtract(tsDistribution); // shuffle images
                        tsDistribution = tsDistribution.Add(new TimeSpan(0, 30, 0));
                        x_media.Modified = DateTime.UtcNow;
                        x_media.EmbeddedHtml = content;
                        x_media.Type = HtmlObjectExtractor.GetType(control);
                        x_media.Visible = mInstance.PublishMedia;
                        x_media.Interesting = false;
                    }
                    catch (Exception ex)
                    {
                        x_media.LastError = ex.Message;
                        x_media.Visible = false;
                    }

                    if (string.IsNullOrEmpty(x_media.Type))
                        continue;

                    Session.Save(x_media);
                    result++;
                }
            }

            return result;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Updated = DateTime.UtcNow;

            if (mInstance.Id == 0)
            {
                mInstance.Created = mInstance.Updated;
                mInstance.LastError = "Feed has not yet been updated since last save.";
            }

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

        protected override void Check(TransitAccountFeed t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.Account.AccountFeeds);
            }
        }
    }
}

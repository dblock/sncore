using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
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
    public class TransitAccountFeedItemQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public string Country;
        public string State;
        public string City;
        public string Search;
        public bool PublishedOnly = true;
        public int AccountId = 0;
        public string AccountFeedName;

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }

        public TransitAccountFeedItemQueryOptions()
        {

        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountFeedItem.AccountFeed.Account.City = '{0}'", Renderer.SqlEncode(City));
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountFeedItem.AccountFeed.Account.Country.Name = '{0}'", Renderer.SqlEncode(Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountFeedItem.AccountFeed.Account.State.Name = '{0}'", Renderer.SqlEncode(State));
            }

            if (!string.IsNullOrEmpty(Search))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("(AccountFeedItem.Title LIKE '%{0}%' OR AccountFeedItem.Description LIKE '%{0}%')", Renderer.SqlEncode(Search));
            }

            if (AccountId != 0)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountFeedItem.AccountFeed.Account.Id = {0}", AccountId);
            }

            if (PublishedOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountFeedItem.AccountFeed.Publish = 1");
            }

            if (!string.IsNullOrEmpty(AccountFeedName))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                if (AccountFeedName.Length == 1)
                {
                    b.AppendFormat("AccountFeedItem.AccountFeed.Name LIKE '{0}%'", Renderer.SqlEncode(AccountFeedName));
                }
                else
                {
                    b.AppendFormat("AccountFeedItem.AccountFeed.Name LIKE '%{0}%'", Renderer.SqlEncode(AccountFeedName));
                }
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
            b.Append("SELECT AccountFeedItem FROM AccountFeedItem AccountFeedItem");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY AccountFeedItem.{0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }
            return b.ToString();
        }
    };

    public class TransitAccountFeedItem : TransitService<AccountFeedItem>
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

        private bool mAccountFeedPublish;

        public bool AccountFeedPublish
        {
            get
            {
                return mAccountFeedPublish;
            }
            set
            {
                mAccountFeedPublish = value;
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

        public TransitAccountFeedItem(AccountFeedItem value)
            : base(value)
        {
        }

        public override void SetInstance(AccountFeedItem value)
        {
            AccountFeedId = value.AccountFeed.Id;
            Title = value.Title;
            Description = value.Description;
            Link = value.Link;
            Guid = value.Guid;
            Created = value.Created;
            Updated = value.Updated;
            AccountId = value.AccountFeed.Account.Id;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.AccountFeed.Account);
            AccountName = value.AccountFeed.Account.Name;
            AccountFeedName = value.AccountFeed.Name;
            AccountFeedLinkUrl = value.AccountFeed.LinkUrl;
            AccountFeedPublish = value.AccountFeed.Publish;
            base.SetInstance(value);
        }

        public override AccountFeedItem GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFeedItem instance = base.GetInstance(session, sec);
            instance.Title = this.Title;
            instance.Description = this.Description;
            instance.Link = this.Link;
            instance.Guid = this.Guid;
            if (this.AccountFeedId != 0) instance.AccountFeed = session.Load<AccountFeed>(AccountFeedId);
            return instance;
        }
    }

    public class ManagedAccountFeedItem : ManagedService<AccountFeedItem, TransitAccountFeedItem>, IAuditableService
    {
        public ManagedAccountFeedItem()
        {

        }

        public ManagedAccountFeedItem(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFeedItem(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFeedItem(ISession session, AccountFeedItem value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(
                Session, mInstance.AccountFeed.Account.Id, typeof(AccountFeedItem), mInstance.Id, sec);
            ManagedFeature.Delete(Session, "AccountFeedItem", Id);
            base.Delete(sec);
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
            acl.Add(new ACLAccount(mInstance.AccountFeed.Account, DataOperation.All));

            if (ManagedDiscussion.IsDiscussionType(type))
            {
                acl.Add(new ACLAuthenticatedAllowCreate());
            }

            return acl;
        }

        public override TransitAccountFeedItem GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountFeedItem t_instance = base.GetTransitInstance(sec);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.AccountFeed.Account.Id, typeof(AccountFeedItem), mInstance.Id);
            return t_instance;
        }

        public static IList<Feature> GetLatestFeaturesByAccountFeedId(ISession session, int id)
        {
            return session.GetNamedQuery("GetAccountFeedItemFeaturesByAccountFeedId")
                .SetInt32("AccountFeed_Id", id)
                .List<Feature>();
        }

        public static Feature GetLatestFeatureByAccountFeedId(ISession session, int id)
        {
            IList<Feature> features = session.GetNamedQuery("GetAccountFeedItemFeaturesByAccountFeedId")
                .SetInt32("AccountFeed_Id", id)
                .SetMaxResults(1)
                .List<Feature>();

            if (features == null || features.Count == 0) return null;
            return features[0];
        }

        public IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.AccountFeed.Account,
                        string.Format("[user:{0}] has posted [b]{1}[/b] in [feed:{2}]",
                        mInstance.AccountFeed.Account.Id, mInstance.Title, mInstance.AccountFeed.Id),
                        string.Format("AccountFeedItemView.aspx?id={0}", mInstance.Id)));
                    break;
            }
            return result;
        }
    }
}
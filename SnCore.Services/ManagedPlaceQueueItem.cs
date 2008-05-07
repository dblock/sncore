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

namespace SnCore.Services
{
    public class TransitFriendsPlaceQueueItem
    {
        private TransitPlace mPlace;

        public TransitPlace Place
        {
            get
            {
                return mPlace;
            }
            set
            {
                mPlace = value;
            }
        }

        private TransitAccount[] mAccounts;

        public TransitAccount[] Accounts
        {
            get
            {
                return mAccounts;
            }
            set
            {
                mAccounts = value;
            }
        }

        public TransitFriendsPlaceQueueItem()
        {

        }

        public TransitFriendsPlaceQueueItem(Place place, List<Account> accounts)
        {
            List<TransitAccount> ta = new List<TransitAccount>();
            foreach (Account a in accounts) ta.Add(new TransitAccount(a));
            mAccounts = ta.ToArray();

            mPlace = new TransitPlace(place);
        }
    }

    public class TransitPlaceQueueItem : TransitService<PlaceQueueItem>
    {
        private int mPlaceId;

        public int PlaceId
        {
            get
            {
                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
            }
        }

        private int mPlaceQueueId;

        public int PlaceQueueId
        {
            get
            {
                return mPlaceQueueId;
            }
            set
            {
                mPlaceQueueId = value;
            }
        }

        private TransitPlace mPlace;

        public TransitPlace Place
        {
            get
            {
                return mPlace;
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

        public TransitPlaceQueueItem()
        {

        }

        public TransitPlaceQueueItem(PlaceQueueItem instance)
            : base(instance)
        {

        }

        public override void SetInstance(PlaceQueueItem instance)
        {
            Created = instance.Created;
            Updated = instance.Updated;
            mPlace = new TransitPlace(instance.Place);
            base.SetInstance(instance);
        }

        public override PlaceQueueItem GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceQueueItem instance = null;

            if (Id != 0)
            {
                instance = session.Load<PlaceQueueItem>(Id);
            }
            else
            {
                instance = (PlaceQueueItem)session.CreateCriteria(typeof(PlaceQueueItem))
                   .Add(Expression.Eq("Place.Id", PlaceId))
                   .Add(Expression.Eq("PlaceQueue.Id", PlaceQueueId))
                   .UniqueResult();

                if (instance == null) instance = new PlaceQueueItem();
            }

            if (Id == 0)
            {
                if (PlaceQueueId > 0) instance.PlaceQueue = session.Load<PlaceQueue>(PlaceQueueId);
                if (PlaceId > 0) instance.Place = session.Load<Place>(PlaceId);
            }

            return instance;
        }
    }

    public class ManagedPlaceQueueItem : ManagedAuditableService<PlaceQueueItem, TransitPlaceQueueItem>
    {
        public ManagedPlaceQueueItem()
        {
        
        }

        public ManagedPlaceQueueItem(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceQueueItem(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceQueueItem(ISession session, PlaceQueueItem value)
            : base(session, value)
        {

        }

        public int PlaceQueueId
        {
            get
            {
                return mInstance.PlaceQueue.Id;
            }
        }

        public static void GetPlaces(Account acct, Dictionary<Place, List<Account>> list)
        {
            if (acct.PlaceQueues == null)
                return;

            foreach (PlaceQueue queue in acct.PlaceQueues)
            {
                if (queue.PlaceQueueItems == null)
                    continue;

                // permissions
                if (!queue.PublishFriends && !queue.PublishAll)
                    continue;

                foreach (PlaceQueueItem item in queue.PlaceQueueItems)
                {
                    List<Account> row = null;
                    if (list.TryGetValue(item.Place, out row))
                    {
                        if (!row.Contains(acct))
                        {
                            row.Add(acct);
                        }
                    }
                    else
                    {
                        row = new List<Account>();
                        row.Add(acct);
                        list.Add(item.Place, row);
                    }
                }
            }
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
            acl.Add(new ACLAccount(mInstance.PlaceQueue.Account, DataOperation.All));
            return acl;
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] added [place:{1}] to the queue",
                        mInstance.PlaceQueue.Account.Id, mInstance.Place.Id),
                        string.Format("PlaceView.aspx?id={0}", mInstance.Place.Id)));
                    break;
            }
            return result;
        }

    }
}
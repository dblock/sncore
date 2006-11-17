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
    public class TransitFriendsPlaceQueueItem : TransitService
    {
        private TransitPlace mPlace;

        public TransitPlace Place
        {
            get
            {
                return mPlace;
            }
        }

        private TransitAccount[] mAccounts;

        public TransitAccount[] Accounts
        {
            get
            {
                return mAccounts;
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

    public class TransitPlaceQueueItem : TransitService
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

        public TransitPlaceQueueItem(PlaceQueueItem o)
            : base(o.Id)
        {
            Created = o.Created;
            Updated = o.Updated;
            mPlace = new TransitPlace(o.Place);
        }

        public PlaceQueueItem GetPlaceQueueItem(ISession session)
        {
            PlaceQueueItem p = null;

            if (Id != 0)
            {
                p = (PlaceQueueItem)session.Load(typeof(PlaceQueueItem), Id);
            }
            else
            {
                p = (PlaceQueueItem)session.CreateCriteria(typeof(PlaceQueueItem))
                   .Add(Expression.Eq("Place.Id", PlaceId))
                   .Add(Expression.Eq("PlaceQueue.Id", PlaceQueueId))
                   .UniqueResult();

                if (p == null) p = new PlaceQueueItem();
            }

            if (Id == 0)
            {
                if (PlaceQueueId > 0) p.PlaceQueue = (PlaceQueue)session.Load(typeof(PlaceQueue), PlaceQueueId);
                if (PlaceId > 0) p.Place = (Place)session.Load(typeof(Place), PlaceId);
            }

            return p;
        }
    }

    public class ManagedPlaceQueueItem : ManagedService
    {
        private PlaceQueueItem mPlaceQueueItem = null;

        public ManagedPlaceQueueItem(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceQueueItem(ISession session, int id)
            : base(session)
        {
            mPlaceQueueItem = (PlaceQueueItem)session.Load(typeof(PlaceQueueItem), id);
        }

        public ManagedPlaceQueueItem(ISession session, PlaceQueueItem value)
            : base(session)
        {
            mPlaceQueueItem = value;
        }

        public ManagedPlaceQueueItem(ISession session, TransitPlaceQueueItem value)
            : base(session)
        {
            mPlaceQueueItem = value.GetPlaceQueueItem(session);
        }

        public int Id
        {
            get
            {
                return mPlaceQueueItem.Id;
            }
        }

        public int PlaceQueueId
        {
            get
            {
                return mPlaceQueueItem.PlaceQueue.Id;
            }
        }

        public TransitPlaceQueueItem TransitPlaceQueueItem
        {
            get
            {
                return new TransitPlaceQueueItem(mPlaceQueueItem);
            }
        }

        public void Delete()
        {
            Session.Delete(mPlaceQueueItem);
        }


        public void CreateOrUpdate(TransitPlaceQueueItem o)
        {
            mPlaceQueueItem = o.GetPlaceQueueItem(Session);
            mPlaceQueueItem.Updated = DateTime.UtcNow;
            if (mPlaceQueueItem.Id == 0) mPlaceQueueItem.Created = mPlaceQueueItem.Updated;
            Session.Save(mPlaceQueueItem);
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
                        if (! row.Contains(acct))
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
    }
}
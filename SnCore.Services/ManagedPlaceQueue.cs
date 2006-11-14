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
    public class TransitPlaceQueue : TransitService
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

        private bool mPublishAll;

        public bool PublishAll
        {
            get
            {
                return mPublishAll;
            }
            set
            {
                mPublishAll = value;
            }
        }

        private bool mPublishFriends;

        public bool PublishFriends
        {
            get
            {
                return mPublishFriends;
            }
            set
            {
                mPublishFriends = value;
            }
        }

        public TransitPlaceQueue()
        {

        }

        public TransitPlaceQueue(PlaceQueue o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            PublishAll = o.PublishAll;
            PublishFriends = o.PublishFriends;
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.Account);
        }

        public PlaceQueue GetPlaceQueue(ISession session)
        {
            PlaceQueue p = (Id != 0) ? (PlaceQueue)session.Load(typeof(PlaceQueue), Id) : new PlaceQueue();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            p.Name = this.Name;
            p.Description = this.Description;
            p.PublishAll = this.PublishAll;
            p.PublishFriends = this.PublishFriends;
            return p;
        }
    }

    public class ManagedPlaceQueue : ManagedService
    {
        private PlaceQueue mPlaceQueue = null;

        public ManagedPlaceQueue(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceQueue(ISession session, int id)
            : base(session)
        {
            mPlaceQueue = (PlaceQueue)session.Load(typeof(PlaceQueue), id);
        }

        public ManagedPlaceQueue(ISession session, PlaceQueue value)
            : base(session)
        {
            mPlaceQueue = value;
        }

        public ManagedPlaceQueue(ISession session, TransitPlaceQueue value)
            : base(session)
        {
            mPlaceQueue = value.GetPlaceQueue(session);
        }

        public int Id
        {
            get
            {
                return mPlaceQueue.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mPlaceQueue.Account.Id;
            }
        }

        public TransitPlaceQueue TransitPlaceQueue
        {
            get
            {
                return new TransitPlaceQueue(mPlaceQueue);
            }
        }

        public void Delete()
        {
            Session.Delete(mPlaceQueue);
        }

        public void CreateOrUpdate(TransitPlaceQueue o)
        {
            mPlaceQueue = o.GetPlaceQueue(Session);
            Session.Save(mPlaceQueue);
        }

        public bool CanWrite(int user_id)
        {
            // owner
            if (mPlaceQueue.Account.Id == user_id)
                return true;

            return false;
        }

        public bool CanDelete(int user_id)
        {
            // owner
            if (mPlaceQueue.Account.Id == user_id)
                return true;

            return false;
        }

        public bool CanRead(int user_id)
        {
            // published to everyone
            if (mPlaceQueue.PublishAll)
                return true;

            // owner
            if (mPlaceQueue.Account.Id == user_id)
                return true;

            // publish to friends
            if (user_id != 0 && mPlaceQueue.PublishFriends && mPlaceQueue.Account.AccountFriends != null)
            {
                foreach (AccountFriend friend in mPlaceQueue.Account.AccountFriends)
                {
                    if (friend.Id == user_id)
                        return true;
                }
            }

            return false;
        }
    }
}
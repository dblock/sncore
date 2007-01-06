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
    public class TransitPlaceQueue : TransitService<PlaceQueue>
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

        public TransitPlaceQueue()
        {

        }

        public TransitPlaceQueue(PlaceQueue value)
            : base(value)
        {

        }

        public override void SetInstance(PlaceQueue value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Modified = value.Modified;
            PublishAll = value.PublishAll;
            PublishFriends = value.PublishFriends;
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            base.SetInstance(value);
        }

        public override PlaceQueue GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceQueue instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.PublishAll = this.PublishAll;
            instance.PublishFriends = this.PublishFriends;
            return instance;
        }
    }

    public class ManagedPlaceQueue : ManagedService<PlaceQueue, TransitPlaceQueue>
    {
        public ManagedPlaceQueue()
        {

        }

        public ManagedPlaceQueue(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceQueue(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceQueue(ISession session, PlaceQueue value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return Object.Account.Id;
            }
        }

        public bool CanWrite(int user_id)
        {
            // owner
            if (Object.Account.Id == user_id)
                return true;

            return false;
        }

        public bool CanDelete(int user_id)
        {
            // owner
            if (Object.Account.Id == user_id)
                return true;

            return false;
        }

        public bool CanRetreive(int user_id)
        {
            // published to everyone
            if (Object.PublishAll)
                return true;

            // owner
            if (Object.Account.Id == user_id)
                return true;

            // publish to friends
            if (user_id != 0 && Object.PublishFriends && Object.Account.AccountFriends != null)
            {
                foreach (AccountFriend friend in Object.Account.AccountFriends)
                {
                    if (friend.Id == user_id)
                        return true;
                }
            }

            return false;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
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
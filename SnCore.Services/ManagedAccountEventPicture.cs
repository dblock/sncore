using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountEventPicture : TransitArrayElementService<AccountEventPicture>
    {
        private byte[] mPicture;

        public byte[] Picture
        {
            get
            {
                return mPicture;
            }
            set
            {
                mPicture = value;
            }
        }

        private byte[] mThumbnail;

        public byte[] Thumbnail
        {
            get
            {
                return mThumbnail;
            }
            set
            {
                mThumbnail = value;
            }
        }

        private int mCommentCount;

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

        private int mAccountEventId;

        public int AccountEventId
        {
            get
            {

                return mAccountEventId;
            }
            set
            {
                mAccountEventId = value;
            }
        }

        private bool mHasPicture = false;

        public bool HasPicture
        {
            get
            {

                return mHasPicture;
            }
            set
            {
                mHasPicture = value;
            }
        }

        private string mName = string.Empty;

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

        private string mDescription = string.Empty;

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

        private TransitCounter mCounter;

        public TransitCounter Counter
        {
            get
            {
                return mCounter;
            }
            set
            {
                mCounter = value;
            }
        }

        public TransitAccountEventPicture()
        {

        }

        public override void SetInstance(AccountEventPicture instance)
        {
            base.SetInstance(instance);
            AccountEventId = instance.AccountEvent.Id;
            HasPicture = (instance.Picture != null);
            Picture = instance.Picture;
            Thumbnail = new ThumbnailBitmap(instance.Picture).Thumbnail;
            Name = instance.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
        }

        public override AccountEventPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEventPicture t_instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                if (AccountEventId > 0) t_instance.AccountEvent = session.Load<AccountEvent>(this.AccountEventId);
            }

            t_instance.Name = this.Name;
            t_instance.Description = this.Description;
            if (this.Picture != null) t_instance.Picture = this.Picture;
            return t_instance;
        }
    }

    public class ManagedAccountEventPicture : ManagedService<AccountEventPicture, TransitAccountEventPicture>
    {
        public ManagedAccountEventPicture()
        {

        }

        public ManagedAccountEventPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEventPicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEventPicture(ISession session, AccountEventPicture value)
            : base(session, value)
        {

        }

        public override TransitAccountEventPicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountEventPicture t_instance = base.GetTransitInstance(sec);
            t_instance.SetWithinCollection(mInstance, mInstance.AccountEvent.AccountEventPictures);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.AccountEvent.Account.Id,
                typeof(AccountEventPicture), mInstance.Id);
            t_instance.Counter = ManagedStats.FindByUri(Session, "AccountEventPicture.aspx", mInstance.Id, sec);
            return t_instance;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountEvent.Account.Id, typeof(AccountEventPicture), mInstance.Id, sec);
            Collection<AccountEventPicture>.GetSafeCollection(mInstance.AccountEvent.AccountEventPictures).Remove(mInstance);
            base.Delete(sec);
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
            acl.Add(new ACLAccount(mInstance.AccountEvent.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountEventPicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.AccountEvent.AccountEventPictures);
            }
        }
    }
}

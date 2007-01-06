using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountEventPictureWithPicture : TransitAccountEventPicture
    {
        public byte[] Picture;

        public TransitAccountEventPictureWithPicture()
        {

        }

        public TransitAccountEventPictureWithPicture(AccountEventPicture p)
            : base(p)
        {
            Picture = p.Picture;
            HasPicture = true;
        }

        public override AccountEventPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEventPicture t_instance = base.GetInstance(session, sec);
            t_instance.Picture = Picture;
            return t_instance;
        }
    }

    public class TransitAccountEventPictureWithThumbnail : TransitAccountEventPicture
    {
        public byte[] Thumbnail;

        public TransitAccountEventPictureWithThumbnail()
        {

        }

        public TransitAccountEventPictureWithThumbnail(AccountEventPicture p)
            : base(p)
        {
            Thumbnail = new ThumbnailBitmap(p.Picture).Thumbnail;
        }
    }

    public class TransitAccountEventPicture : TransitArrayElementService<AccountEventPicture>
    {
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

        public TransitAccountEventPicture(AccountEventPicture p)
            : base(p, p.AccountEvent.AccountEventPictures)
        {

        }

        public override void SetInstance(AccountEventPicture instance)
        {
            base.SetInstance(instance);
            AccountEventId = instance.AccountEvent.Id;
            HasPicture = instance.Picture != null;
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
                if (AccountEventId > 0) t_instance.AccountEvent = (AccountEvent)session.Load(typeof(AccountEvent), this.AccountEventId);
            }

            t_instance.Name = this.Name;
            t_instance.Description = this.Description;
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
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.AccountEvent.Account.Id,
                ManagedDiscussion.AccountEventPictureDiscussion, mInstance.Id);
            t_instance.Counter = ManagedStats.GetCounter(Session, "AccountEventPicture.aspx", mInstance.Id);
            return t_instance;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
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
    }
}

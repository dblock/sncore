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

        private int mPosition = 0;

        public int Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
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
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            Created = instance.Created;
            Modified = instance.Modified;
            Position = instance.Position;
        }

        public override AccountEventPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEventPicture p = base.GetInstance(session, sec);

            if (Id == 0)
            {
                if (AccountEventId > 0) p.AccountEvent = session.Load<AccountEvent>(this.AccountEventId);
                p.Account = GetOwner(session, AccountId, sec);
                p.Position = Position;
            }

            p.Name = this.Name;
            p.Description = this.Description;
            if (this.Picture != null) p.Picture = this.Picture;
            return p;
        }
    }

    public class ManagedAccountEventPicture : ManagedAuditableService<AccountEventPicture, TransitAccountEventPicture>
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

        public void Move(ManagedSecurityContext sec, int disp)
        {
            GetACL().Check(sec, DataOperation.Update);
            ManagedPictureServiceImpl<AccountEventPicture>.Move(Session, mInstance, mInstance.AccountEvent.AccountEventPictures, disp);
            Session.Flush();
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountEvent.Account.Id, typeof(AccountEventPicture), mInstance.Id, sec);
            ManagedPictureServiceImpl<AccountEventPicture>.Delete(Session, mInstance, mInstance.AccountEvent.AccountEventPictures);
            Collection<AccountEventPicture>.Remove(mInstance.AccountEvent.AccountEventPictures, mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            ManagedPictureServiceImpl<AccountEventPicture>.Save(Session, mInstance, mInstance.AccountEvent.AccountEventPictures);
            if (mInstance.AccountEvent.AccountEventPictures == null) mInstance.AccountEvent.AccountEventPictures = new List<AccountEventPicture>();
            mInstance.AccountEvent.AccountEventPictures.Add(mInstance);
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.AllExceptUpdate));
            acl.Add(new ACLAccount(mInstance.AccountEvent.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountEventPicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota(sec).Check<AccountEventPicture, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountEventPicture instance WHERE instance.AccountEvent.Id = {0}", 
                        mInstance.AccountEvent.Id)).UniqueResult<int>());
            }
        }

        public void MigrateToAccount(Account newowner, ManagedSecurityContext sec)
        {
            // migrate review discussion
            Discussion d = ManagedDiscussion.Find(
                Session, mInstance.AccountEvent.Account.Id, typeof(AccountEventPicture), mInstance.Id, sec);

            if (d != null)
            {
                ManagedDiscussion md = new ManagedDiscussion(Session, d);
                md.MigrateToAccount(newowner, sec);
            }

            mInstance.Account = newowner;
            Session.Save(mInstance);
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.AccountEvent.Account,
                        string.Format("[user:{0}] added a picture to [event:{1}]",
                        mInstance.Account.Id, mInstance.AccountEvent.Id),
                        string.Format("AccountEventPictureView.aspx?id={0}", mInstance.Id)));
                    break;
            }
            return result;
        }
    }
}

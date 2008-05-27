using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SnCore.Tools.Drawing;
using NHibernate.Expression;
using SnCore.Data.Hibernate;
using SnCore.Tools;

namespace SnCore.Services
{
    public class AccountPicturesQueryOptions
    {
        public bool Hidden = true;

        public AccountPicturesQueryOptions()
        {

        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountPicture : TransitArrayElementService<AccountPicture>
    {
        private byte[] mBitmap;

        public byte[] Bitmap
        {
            get
            {
                return mBitmap;
            }
            set
            {
                mBitmap = value;
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

        private bool mHidden;

        public bool Hidden
        {
            get
            {
                return mHidden;
            }
            set
            {
                mHidden = value;
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

        public TransitAccountPicture()
        {

        }

        public override void SetInstance(AccountPicture instance)
        {
            base.SetInstance(instance);
            Name = instance.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountId = instance.Account.Id;
            Hidden = instance.Hidden;
            Bitmap = instance.Bitmap;
            Thumbnail = new ThumbnailBitmap(Bitmap).Thumbnail;
            Position = instance.Position;
        }

        public override AccountPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPicture instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Hidden = this.Hidden;
            instance.Modified = DateTime.UtcNow;
            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Created = instance.Modified;
                instance.Position = this.Position;
            }
            if (Bitmap != null) instance.Bitmap = Bitmap;
            return instance;
        }
    }

    public class ManagedAccountPicture : ManagedAuditableService<AccountPicture, TransitAccountPicture>
    {
        public ManagedAccountPicture()
        {

        }

        public ManagedAccountPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPicture(ISession session, AccountPicture value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public string Description
        {
            get
            {
                return mInstance.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mInstance.Bitmap;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
            }
        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public void Move(ManagedSecurityContext sec, int disp)
        {
            GetACL().Check(sec, DataOperation.Update);
            ManagedPictureServiceImpl<AccountPicture>.Move(Session, mInstance, mInstance.Account.AccountPictures, disp);
            Session.Flush();
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(
                Session, mInstance.Account.Id, typeof(AccountPicture), mInstance.Id, sec);
            ManagedPictureServiceImpl<AccountPicture>.Delete(Session, mInstance, mInstance.Account.AccountPictures);
            Collection<AccountPicture>.Remove(mInstance.Account.AccountPictures, mInstance);
            Session.Delete(mInstance);
        }

        public override TransitAccountPicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountPicture t_instance = base.GetTransitInstance(sec);
            List<AccountPicture> collection = new List<AccountPicture>();
            foreach (AccountPicture pic in Collection<AccountPicture>.GetSafeCollection(mInstance.Account.AccountPictures))
                if (!pic.Hidden)
                    collection.Add(pic);
            t_instance.SetWithinCollection(mInstance, collection);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.Account.Id,
                typeof(AccountPicture), mInstance.Id);
            t_instance.Counter = ManagedStats.FindByUri(Session, "AccountPicture.aspx", mInstance.Id, sec);
            return t_instance;
        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            ManagedPictureServiceImpl<AccountPicture>.Save(Session, mInstance, mInstance.Account.AccountPictures);
            if (mInstance.Account.AccountPictures == null) mInstance.Account.AccountPictures = new List<AccountPicture>();
            mInstance.Account.AccountPictures.Add(mInstance);
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

        protected override void Check(TransitAccountPicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountPicture, ManagedAccount.QuotaExceededException>(
                mInstance.Account.AccountPictures);
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            switch (op)
            {
                case DataOperation.Create:
                    List<AccountAuditEntry> result = new List<AccountAuditEntry>();
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.Account,
                        string.Format("[user:{0}] uploaded a new picture",
                            mInstance.Account.Id, mInstance.Id), string.Format("AccountPictureView.aspx?id={0}", mInstance.Id)));
                    return result;
                default:
                    return null;
            }
        }
    }
}

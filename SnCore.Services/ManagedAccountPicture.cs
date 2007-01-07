using System;
using NHibernate;
using System.Collections.Generic;
using System.IO;
using SnCore.Tools.Drawing;
using NHibernate.Expression;

namespace SnCore.Services
{
    public class AccountPicturesQueryOptions
    {
        public bool Hidden = true;

        public AccountPicturesQueryOptions()
        {

        }
    };

    public class TransitAccountPictureWithBitmap : TransitAccountPicture
    {
        public byte[] Bitmap;

        public TransitAccountPictureWithBitmap()
        {

        }

        public TransitAccountPictureWithBitmap(AccountPicture p)
            : base(p)
        {
            Bitmap = p.Bitmap;
        }

        public override AccountPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPicture instance = base.GetInstance(session, sec);
            if (this.Bitmap != null) instance.Bitmap = this.Bitmap;
            return instance;
        }
    }

    public class TransitAccountPictureWithThumbnail : TransitAccountPicture
    {
        public byte[] Thumbnail;

        public TransitAccountPictureWithThumbnail()
        {

        }

        public TransitAccountPictureWithThumbnail(AccountPicture p)
            : base(p)
        {
            Thumbnail = new ThumbnailBitmap(p.Bitmap).Thumbnail;
        }
    }


    public class TransitAccountPicture : TransitArrayElementService<AccountPicture>
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

        public TransitAccountPicture()
        {

        }

        public TransitAccountPicture(AccountPicture p, IList<AccountPicture> collection)
            : base(p, collection)
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
        }

        public TransitAccountPicture(AccountPicture p)
            : this(p, p.Account.AccountPictures)
        {

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
            }
            return instance;
        }
    }

    public class ManagedAccountPicture : ManagedService<AccountPicture, TransitAccountPicture>
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

        public TransitAccountPictureWithBitmap GetTransitAccountPictureWithBitmap()
        {
            TransitAccountPictureWithBitmap pic = new TransitAccountPictureWithBitmap(mInstance);
            return pic;
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
            ManagedDiscussion.FindAndDelete(Session, mInstance.Account.Id, ManagedDiscussion.AccountPictureDiscussion, mInstance.Id);
            mInstance.Account.AccountPictures.Remove(mInstance);
            Session.Delete(mInstance);
        }

        public override TransitAccountPicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountPicture t_instance = base.GetTransitInstance(sec);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.Account.Id,
                ManagedDiscussion.AccountPictureDiscussion, mInstance.Id);
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

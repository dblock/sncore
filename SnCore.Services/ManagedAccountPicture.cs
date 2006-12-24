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

        public new AccountPicture GetAccountPicture(ISession session)
        {
            AccountPicture p = base.GetAccountPicture(session);
            if (this.Bitmap != null) p.Bitmap = this.Bitmap;
            return p;
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
            : base(p.Id, p, collection)
        {
            Name = p.Name;
            Description = p.Description;
            Created = p.Created;
            Modified = p.Modified;
            AccountId = p.Account.Id;
            Hidden = p.Hidden;
        }

        public TransitAccountPicture(AccountPicture p)
            : this(p, p.Account.AccountPictures)
        {

        }

        public AccountPicture GetAccountPicture(ISession session)
        {
            AccountPicture p = (Id > 0) ? (AccountPicture)session.Load(typeof(AccountPicture), Id) : new AccountPicture();
            p.Name = this.Name;
            p.Description = this.Description;
            p.Hidden = this.Hidden;
            if (this.Id == 0) p.Created = p.Modified = DateTime.UtcNow;
            else p.Modified = DateTime.UtcNow;
            return p;
        }
    }

    /// <summary>
    /// Managed picture.
    /// </summary>
    public class ManagedAccountPicture : ManagedService<AccountPicture>
    {
        private AccountPicture mAccountPicture = null;

        public ManagedAccountPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPicture(ISession session, int id)
            : base(session)
        {
            mAccountPicture = (AccountPicture)session.Load(typeof(AccountPicture), id);
        }

        public ManagedAccountPicture(ISession session, AccountPicture value)
            : base(session)
        {
            mAccountPicture = value;
        }

        public int Id
        {
            get
            {
                return mAccountPicture.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAccountPicture.Name;
            }
        }

        public string Description
        {
            get
            {
                return mAccountPicture.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mAccountPicture.Bitmap;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountPicture.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountPicture.Modified;
            }
        }

        public TransitAccountPictureWithBitmap GetTransitAccountPictureWithBitmap()
        {
            TransitAccountPictureWithBitmap pic = new TransitAccountPictureWithBitmap(mAccountPicture);
            return pic;
        }

        public TransitAccountPictureWithThumbnail GetTransitAccountPictureWithThumbnail()
        {
            TransitAccountPictureWithThumbnail pic = new TransitAccountPictureWithThumbnail(mAccountPicture);
            return pic;
        }

        public TransitAccountPicture GetTransitAccountPicture(IList<AccountPicture> collection)
        {
            TransitAccountPicture pic = new TransitAccountPicture(mAccountPicture, collection);
            pic.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mAccountPicture.Account.Id,
                ManagedDiscussion.AccountPictureDiscussion, mAccountPicture.Id);
            pic.Counter = ManagedStats.GetCounter(Session, "AccountPicture.aspx", mAccountPicture.Id);
            return pic;
        }

        public TransitAccountPicture GetTransitAccountPicture()
        {
            return GetTransitAccountPicture(mAccountPicture.Account.AccountPictures);
        }

        public int AccountId
        {
            get
            {
                return mAccountPicture.Account.Id;
            }
        }

        public void Delete()
        {
            try
            {
                int DiscussionId = ManagedDiscussion.GetDiscussionId(
                    Session, mAccountPicture.Account.Id, ManagedDiscussion.AccountPictureDiscussion, mAccountPicture.Id, false);
                Discussion mDiscussion = (Discussion)Session.Load(typeof(Discussion), DiscussionId);
                Session.Delete(mDiscussion);
            }
            catch (ManagedDiscussion.DiscussionNotFoundException)
            {

            }

            mAccountPicture.Account.AccountPictures.Remove(mAccountPicture);
            Session.Delete(mAccountPicture);
        }

        public Account Account
        {
            get
            {
                return mAccountPicture.Account;
            }
        }
    }
}

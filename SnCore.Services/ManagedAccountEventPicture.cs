using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Drawing;

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

        public override AccountEventPicture GetAccountEventPicture(ISession session)
        {
            AccountEventPicture p = base.GetAccountEventPicture(session);
            p.Picture = Picture;
            return p;
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
            : base(p.Id, p, p.AccountEvent.AccountEventPictures)
        {
            AccountEventId = p.AccountEvent.Id;
            HasPicture = p.Picture != null;
            Name = p.Name;
            Description = p.Description;
            Created = p.Created;
            Modified = p.Modified;
        }

        public virtual AccountEventPicture GetAccountEventPicture(ISession session)
        {
            AccountEventPicture p = (Id != 0) ? (AccountEventPicture)session.Load(typeof(AccountEventPicture), Id) : new AccountEventPicture();

            if (Id == 0)
            {
                if (AccountEventId > 0) p.AccountEvent = (AccountEvent)session.Load(typeof(AccountEvent), this.AccountEventId);
            }

            p.Name = this.Name;
            p.Description = this.Description;
            return p;
        }

    }

    public class ManagedAccountEventPicture : ManagedService<AccountEventPicture, TransitAccountEventPicture>
    {
        private AccountEventPicture mAccountEventPicture = null;

        public ManagedAccountEventPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEventPicture(ISession session, int id)
            : base(session)
        {
            mAccountEventPicture = (AccountEventPicture)session.Load(typeof(AccountEventPicture), id);
        }

        public ManagedAccountEventPicture(ISession session, AccountEventPicture value)
            : base(session)
        {
            mAccountEventPicture = value;
        }

        public int AccountId
        {
            get
            {
                return mAccountEventPicture.AccountEvent.Account.Id;
            }
        }

        public TransitAccountEventPicture TransitAccountEventPicture
        {
            get
            {
                TransitAccountEventPicture pic = new TransitAccountEventPicture(mAccountEventPicture);
                pic.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                    Session, mAccountEventPicture.AccountEvent.Account.Id,
                    ManagedDiscussion.AccountEventPictureDiscussion, mAccountEventPicture.Id);
                pic.Counter = ManagedStats.GetCounter(Session, "AccountEventPicture.aspx", mAccountEventPicture.Id);
                return pic;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            mAccountEventPicture.AccountEvent.AccountEventPictures.Remove(mAccountEventPicture);
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
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountEvent.Account, DataOperation.All));
            return acl;
        }
    }
}

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
    public class TransitAccountStoryPicture : TransitArrayElementService<AccountStoryPicture>
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

        private int mAccountStoryId;

        public int AccountStoryId
        {
            get
            {
                return mAccountStoryId;
            }
            set
            {
                mAccountStoryId = value;
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

        public TransitAccountStoryPicture()
        {

        }

        public TransitAccountStoryPicture(AccountStoryPicture p)
            : base(p)
        {

        }

        public override void SetInstance(AccountStoryPicture instance)
        {
            base.SetInstance(instance);
            Position = instance.Position;
            AccountStoryId = instance.AccountStory.Id;
            Name = instance.Name;
            Created = instance.Created;
            Modified = instance.Modifed;
            Picture = instance.Picture;
            if (instance.Picture != null) mThumbnail = new ThumbnailBitmap(instance.Picture).Thumbnail;
        }

        public override AccountStoryPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountStoryPicture instance = base.GetInstance(session, sec);
            if (Id == 0)
            {
                instance.AccountStory = session.Load<AccountStory>(AccountStoryId);
                instance.Position = Position;
            }
            instance.Name = Name;
            if (Picture != null) instance.Picture = Picture;
            return instance;
        }
    }

    public class ManagedAccountStoryPicture : ManagedService<AccountStoryPicture, TransitAccountStoryPicture>
    {
        public ManagedAccountStoryPicture()
        {

        }

        public ManagedAccountStoryPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountStoryPicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountStoryPicture(ISession session, AccountStoryPicture value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.AccountStory.Account.Id;
            }
        }

        public void Move(ManagedSecurityContext sec, int disp)
        {
            GetACL().Check(sec, DataOperation.Update);
            ManagedPictureServiceImpl<AccountStoryPicture>.Move(Session, mInstance, mInstance.AccountStory.AccountStoryPictures, disp);
            Session.Flush();
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountStory.Account.Id, typeof(AccountStoryPicture), mInstance.Id, sec);
            ManagedPictureServiceImpl<AccountStoryPicture>.Delete(Session, mInstance, mInstance.AccountStory.AccountStoryPictures);
            Collection<AccountStoryPicture>.Remove(mInstance.AccountStory.AccountStoryPictures, mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modifed = DateTime.UtcNow;
            if (mInstance.Id == 0)
            {
                mInstance.Created = mInstance.Modifed;
            }
            ManagedPictureServiceImpl<AccountStoryPicture>.Save(Session, mInstance, mInstance.AccountStory.AccountStoryPictures);
            if (mInstance.AccountStory.AccountStoryPictures == null) mInstance.AccountStory.AccountStoryPictures = new List<AccountStoryPicture>();
            mInstance.AccountStory.AccountStoryPictures.Add(mInstance);
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.AccountStory.Account, DataOperation.All));
            return acl;
        }

        public override TransitAccountStoryPicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountStoryPicture t_instance = base.GetTransitInstance(sec);
            t_instance.Counter = ManagedStats.FindByUri(Session, "AccountStoryPictureView.aspx", mInstance.Id, sec); 
            t_instance.SetWithinCollection(mInstance, mInstance.AccountStory.AccountStoryPictures);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.AccountStory.Account.Id, typeof(AccountStoryPicture), mInstance.Id);
            return t_instance;
        }

        protected override void Check(TransitAccountStoryPicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerified();

                GetQuota(sec).Check<AccountStoryPicture, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountStoryPicture instance WHERE instance.AccountStory.Id = {0}",
                        mInstance.AccountStory.Id)).UniqueResult<int>());
            }
        }
    }
}

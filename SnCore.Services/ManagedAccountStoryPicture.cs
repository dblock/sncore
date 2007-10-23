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

        private int mLocation = 0;

        public int Location
        {
            get
            {

                return mLocation;
            }
            set
            {
                mLocation = value;
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
            Location = instance.Location;
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
            if (Id == 0) instance.AccountStory = session.Load<AccountStory>(AccountStoryId);
            if (Id == 0) instance.Location = Location;
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

        public int Location
        {
            get
            {
                return mInstance.Location;
            }
        }

        public int AccountId
        {
            get
            {
                return mInstance.AccountStory.Account.Id;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountStory.Account.Id, typeof(AccountStoryPicture), mInstance.Id, sec);

            // renumber the order of Pictures
            foreach (AccountStoryPicture p in Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures))
            {
                if (p.Location >= Object.Location)
                {
                    p.Location = p.Location - 1;
                    Session.Save(p);
                }
            }

            Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures).Remove(mInstance);
            base.Delete(sec);
        }

        public void Move(ManagedSecurityContext sec, int Disp)
        {
            GetACL().Check(sec, DataOperation.Update);

            int newLocation = mInstance.Location + Disp;

            if (newLocation < 1 || newLocation > mInstance.AccountStory.AccountStoryPictures.Count)
            {
                // throw new ArgumentOutOfRangeException();
                return;
            }

            foreach (AccountStoryPicture p in Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures))
            {
                if (p.Location == mInstance.Location)
                {
                    // this item
                }
                else if (p.Location < mInstance.Location && p.Location >= newLocation)
                {
                    // item was before me but switched sides
                    p.Location++;
                }
                else if (p.Location > mInstance.Location && p.Location <= newLocation)
                {
                    // item was after me, but switched sides
                    p.Location--;
                }

                Session.Save(p);
            }

            mInstance.Location = newLocation;
            Session.Save(mInstance);
            Session.Flush();
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modifed = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modifed;
            if (mInstance.Id == 0) mInstance.Location = Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures).Count + 1;
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
                sec.CheckVerifiedEmail();
                GetQuota(sec).Check<AccountStoryPicture, ManagedAccount.QuotaExceededException>(
                    mInstance.AccountStory.AccountStoryPictures);
            }
        }
    }
}

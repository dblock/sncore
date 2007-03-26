using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitPlacePicture : TransitArrayElementService<PlacePicture>
    {
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

        private int mPlaceId;

        public int PlaceId
        {
            get
            {

                return mPlaceId;
            }
            set
            {
                mPlaceId = value;
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

        public TransitPlacePicture()
        {

        }

        public TransitPlacePicture(PlacePicture p)
            : base(p)
        {

        }

        public override void SetInstance(PlacePicture instance)
        {
            base.SetInstance(instance);
            Name = instance.Name;
            Description = instance.Description;
            Created = instance.Created;
            Modified = instance.Modified;
            PlaceId = instance.Place.Id;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            Bitmap = instance.Bitmap;
            Thumbnail = new ThumbnailBitmap(Bitmap).Thumbnail;
        }

        public override PlacePicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePicture p = base.GetInstance(session, sec);
            p.Name = this.Name;
            p.Description = this.Description;
            if (Id == 0)
            {
                if (PlaceId > 0) p.Place = session.Load<Place>(PlaceId);
                p.Account = GetOwner(session, AccountId, sec);
            }
            if (Bitmap != null) p.Bitmap = Bitmap;
            return p;
        }
    }

    /// <summary>
    /// Managed place picture.
    /// </summary>
    public class ManagedPlacePicture : ManagedService<PlacePicture, TransitPlacePicture>
    {
        public ManagedPlacePicture()
        {

        }

        public ManagedPlacePicture(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlacePicture(ISession session, PlacePicture value)
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

        public Place Place
        {
            get
            {
                return mInstance.Place;
            }
        }

        public void MigrateToAccount(Account newowner, ManagedSecurityContext sec)
        {
            // migrate review discussion
            Discussion d = ManagedDiscussion.Find(
                Session, mInstance.Place.Account.Id, typeof(Place), mInstance.Id, sec);

            if (d != null)
            {
                ManagedDiscussion md = new ManagedDiscussion(Session, d);
                md.MigrateToAccount(newowner, sec);
            }

            mInstance.Account = newowner;
            Session.Save(mInstance);
        }

        public bool CanWrite(int user_id)
        {
            if (mInstance.Account.Id == user_id)
                return true;

            ManagedPlace m_place = new ManagedPlace(Session, mInstance.Place);

            if (m_place.CanWrite(user_id))
                return true;

            return false;
        }

        public override TransitPlacePicture GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitPlacePicture t_instance = base.GetTransitInstance(sec);
            t_instance.SetWithinCollection(mInstance, mInstance.Place.PlacePictures);
            t_instance.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.Place.Account.Id,
                typeof(PlacePicture), mInstance.Id);
            t_instance.Counter = ManagedStats.FindByUri(Session, "PlacePicture.aspx", mInstance.Id, sec);
            return t_instance;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Place.Account, DataOperation.All));
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.AllExceptUpdate));
            foreach (AccountPlace relationship in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
            {
                acl.Add(new ACLAccount(relationship.Account,
                    relationship.Type.CanWrite ? DataOperation.Update : DataOperation.Retreive));
            }
            return acl;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.Place.Account.Id, typeof(PlacePicture), mInstance.Id, sec);
            base.Delete(sec);
        }

        protected override void Check(TransitPlacePicture t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerifiedEmail();
        }
    }
}

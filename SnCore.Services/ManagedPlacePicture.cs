using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitPlacePictureWithBitmap : TransitPlacePicture
    {
        public byte[] Bitmap;

        public TransitPlacePictureWithBitmap()
        {

        }

        public TransitPlacePictureWithBitmap(PlacePicture p)
            : base(p)
        {
            Bitmap = p.Bitmap;
        }

        public override PlacePicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePicture instance = base.GetInstance(session, sec);
            instance.Bitmap = Bitmap;
            return instance;
        }
    }

    public class TransitPlacePictureWithThumbnail : TransitPlacePicture
    {
        public byte[] Thumbnail;

        public TransitPlacePictureWithThumbnail()
        {

        }

        public TransitPlacePictureWithThumbnail(PlacePicture p)
            : base(p)
        {
            Thumbnail = new ThumbnailBitmap(p.Bitmap).Thumbnail;
        }
    }


    public class TransitPlacePicture : TransitArrayElementService<PlacePicture>
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
            : base(p.Id, p, p.Place.PlacePictures)
        {
            Name = p.Name;
            Description = p.Description;
            Created = p.Created;
            Modified = p.Modified;
            PlaceId = p.Place.Id;
            AccountId = p.Account.Id;
            AccountName = p.Account.Name;
        }

        public override PlacePicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlacePicture p = base.GetInstance(session, sec);
            p.Name = this.Name;
            p.Description = this.Description;
            if (Id == 0)
            {
                if (PlaceId > 0) p.Place = (Place)session.Load(typeof(Place), PlaceId);
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }
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

        public void MigrateToAccount(Account newowner)
        {
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
            acl.Add(new ACLAccount(mInstance.Place.Account, DataOperation.All));
            foreach (AccountPlace relationship in Collection<AccountPlace>.GetSafeCollection(mInstance.Place.AccountPlaces))
            {
                acl.Add(new ACLAccount(relationship.Account,
                    relationship.Type.CanWrite ? DataOperation.Update : DataOperation.Retreive));
            }
            return acl;
        }
    }
}

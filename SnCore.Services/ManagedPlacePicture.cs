using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;

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

        public override PlacePicture GetPlacePicture(ISession session)
        {
            PlacePicture p = base.GetPlacePicture(session);
            if (this.Bitmap != null) p.Bitmap = this.Bitmap;
            return p;
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


    public class TransitPlacePicture : TransitService
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

        public TransitPlacePicture()
        {

        }

        public TransitPlacePicture(PlacePicture p)
            : base(p.Id)
        {
            Name = p.Name;
            Description = p.Description;
            Created = p.Created;
            Modified = p.Modified;
            PlaceId = p.Place.Id;
        }

        public virtual PlacePicture GetPlacePicture(ISession session)
        {
            PlacePicture p = (Id > 0) ? (PlacePicture)session.Load(typeof(PlacePicture), Id) : new PlacePicture();
            p.Name = this.Name;
            p.Description = this.Description;
            p.Place = (Place)session.Load(typeof(Place), PlaceId);
            return p;
        }
    }

    /// <summary>
    /// Managed place picture.
    /// </summary>
    public class ManagedPlacePicture : ManagedService
    {
        private PlacePicture mPlacePicture = null;

        public ManagedPlacePicture(ISession session)
            : base(session)
        {

        }

        public ManagedPlacePicture(ISession session, int id)
            : base(session)
        {
            mPlacePicture = (PlacePicture)session.Load(typeof(PlacePicture), id);
        }

        public ManagedPlacePicture(ISession session, PlacePicture value)
            : base(session)
        {
            mPlacePicture = value;
        }

        public int Id
        {
            get
            {
                return mPlacePicture.Id;
            }
        }

        public string Name
        {
            get
            {
                return mPlacePicture.Name;
            }
        }

        public string Description
        {
            get
            {
                return mPlacePicture.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mPlacePicture.Bitmap;
            }
        }

        public DateTime Created
        {
            get
            {
                return mPlacePicture.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mPlacePicture.Modified;
            }
        }

        public TransitPlacePictureWithBitmap TransitPlacePictureWithBitmap
        {
            get
            {
                TransitPlacePictureWithBitmap pic = new TransitPlacePictureWithBitmap(mPlacePicture);
                return pic;
            }
        }

        public TransitPlacePictureWithThumbnail TransitPlacePictureWithThumbnail
        {
            get
            {
                TransitPlacePictureWithThumbnail pic = new TransitPlacePictureWithThumbnail(mPlacePicture);
                return pic;
            }
        }

        public TransitPlacePicture TransitPlacePicture
        {
            get
            {
                TransitPlacePicture pic = new TransitPlacePicture(mPlacePicture);
                pic.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                    Session, mPlacePicture.Place.Account.Id,
                    ManagedDiscussion.PlacePictureDiscussion, mPlacePicture.Id);
                return pic;
            }
        }

        public void CreateOrUpdate(TransitPlacePicture o)
        {
            mPlacePicture = o.GetPlacePicture(Session);
            mPlacePicture.Modified = DateTime.UtcNow;
            if (Id == 0) mPlacePicture.Created = mPlacePicture.Modified;
            Session.Save(mPlacePicture);
        }

        public void Delete()
        {
            Session.Delete(mPlacePicture);
        }

        public Place Place
        {
            get
            {
                return mPlacePicture.Place;
            }
        }
    }
}

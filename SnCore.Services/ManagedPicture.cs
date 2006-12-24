using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;

namespace SnCore.Services
{
    public class TransitPictureWithBitmap : TransitPicture
    {
        public byte[] Bitmap;

        public TransitPictureWithBitmap()
        {

        }

        public TransitPictureWithBitmap(Picture p)
            : base(p)
        {
            Bitmap = p.Bitmap;
        }

        public override Picture GetPicture(ISession session)
        {
            Picture p = base.GetPicture(session);
            if (this.Bitmap != null) p.Bitmap = this.Bitmap;
            return p;
        }
    }

    public class TransitPictureWithThumbnail : TransitPicture
    {
        public byte[] Thumbnail;

        public TransitPictureWithThumbnail()
        {

        }

        public TransitPictureWithThumbnail(Picture p)
            : base(p)
        {
            Thumbnail = new ThumbnailBitmap(p.Bitmap).Thumbnail;
        }
    }


    public class TransitPicture : TransitService
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

        private string mType;

        public string Type
        {
            get
            {

                return mType;
            }
            set
            {
                mType = value;
            }
        }

        public TransitPicture()
        {

        }

        public TransitPicture(Picture p)
            : base(p.Id)
        {
            Name = p.Name;
            Description = p.Description;
            Created = p.Created;
            Modified = p.Modified;
            Type = p.Type.Name;
        }

        public virtual Picture GetPicture(ISession session)
        {
            Picture p = (Id > 0) ? (Picture)session.Load(typeof(Picture), Id) : new Picture();
            p.Name = this.Name;
            p.Description = this.Description;
            if (!string.IsNullOrEmpty(this.Type)) p.Type = ManagedPictureType.Find(session, this.Type);
            return p;
        }
    }

    /// <summary>
    /// Managed picture.
    /// </summary>
    public class ManagedPicture : ManagedService<Picture>
    {
        private Picture mPicture = null;

        public ManagedPicture(ISession session)
            : base(session)
        {

        }

        public ManagedPicture(ISession session, int id)
            : base(session)
        {
            mPicture = (Picture)session.Load(typeof(Picture), id);
        }

        public ManagedPicture(ISession session, Picture value)
            : base(session)
        {
            mPicture = value;
        }

        public int Id
        {
            get
            {
                return mPicture.Id;
            }
        }

        public string Name
        {
            get
            {
                return mPicture.Name;
            }
        }

        public string Description
        {
            get
            {
                return mPicture.Description;
            }
        }

        public byte[] Bitmap
        {
            get
            {
                return mPicture.Bitmap;
            }
        }

        public DateTime Created
        {
            get
            {
                return mPicture.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mPicture.Modified;
            }
        }

        public TransitPictureWithBitmap TransitPictureWithBitmap
        {
            get
            {
                TransitPictureWithBitmap pic = new TransitPictureWithBitmap(mPicture);
                return pic;
            }
        }

        public TransitPictureWithThumbnail TransitPictureWithThumbnail
        {
            get
            {
                TransitPictureWithThumbnail pic = new TransitPictureWithThumbnail(mPicture);
                return pic;
            }
        }

        public TransitPicture TransitPicture
        {
            get
            {
                return new TransitPicture(mPicture);
            }
        }

        public void CreateOrUpdate(TransitPicture o)
        {
            mPicture = o.GetPicture(Session);
            mPicture.Modified = DateTime.UtcNow;
            if (Id == 0) mPicture.Created = mPicture.Modified;
            Session.Save(mPicture);
        }

        public void Delete()
        {
            Session.Delete(mPicture);
        }
    }
}

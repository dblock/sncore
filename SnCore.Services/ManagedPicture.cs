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

        public TransitPictureWithBitmap(Picture value)
            : base(value)
        {
            Bitmap = value.Bitmap;
        }

        public override Picture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Picture instance = base.GetInstance(session, sec);
            if (this.Bitmap != null) instance.Bitmap = this.Bitmap;
            return instance;
        }
    }

    public class TransitPictureWithThumbnail : TransitPicture
    {
        public byte[] Thumbnail;

        public TransitPictureWithThumbnail()
        {

        }

        public TransitPictureWithThumbnail(Picture value)
            : base(value)
        {
            Thumbnail = new ThumbnailBitmap(value.Bitmap).Thumbnail;
        }
    }


    public class TransitPicture : TransitService<Picture>
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
            : base(p)
        {

        }

        public override void SetInstance(Picture value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Modified = value.Modified;
            Type = value.Type.Name;
            base.SetInstance(value);
        }

        public override Picture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Picture instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            if (!string.IsNullOrEmpty(this.Type)) instance.Type = ManagedPictureType.Find(session, this.Type);
            return instance;
        }
    }

    public class ManagedPicture : ManagedService<Picture, TransitPicture>
    {
        public ManagedPicture(ISession session)
            : base(session)
        {

        }

        public ManagedPicture(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPicture(ISession session, Picture value)
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

        public TransitPictureWithBitmap GetTransitPictureWithBitmap()
        {
            TransitPictureWithBitmap pic = new TransitPictureWithBitmap(mInstance);
            return pic;
        }

        public TransitPictureWithThumbnail GetTransitPictureWithThumbnail()
        {
            TransitPictureWithThumbnail pic = new TransitPictureWithThumbnail(mInstance);
            return pic;
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
            return acl;
        }    
    }
}

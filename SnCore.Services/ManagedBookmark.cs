using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools;

namespace SnCore.Services
{
    public class BookmarkQueryOptions
    {
        public bool WithFullBitmaps = false;
        public bool WithLinkedBitmaps = false;

        public BookmarkQueryOptions()
        {
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    }

    public class TransitBookmarkWithBitmaps : TransitBookmark
    {
        public byte[] FullBitmap;
        public byte[] LinkBitmap;

        public TransitBookmarkWithBitmaps()
        {

        }

        public TransitBookmarkWithBitmaps(Bookmark b)
            : base(b)
        {
            FullBitmap = b.FullBitmap;
            LinkBitmap = b.LinkBitmap;
        }

        public override Bookmark GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Bookmark instance = base.GetInstance(session, sec);
            if (this.LinkBitmap != null) instance.LinkBitmap = this.LinkBitmap;
            if (this.FullBitmap != null) instance.FullBitmap = this.FullBitmap;
            return instance;
        }
    }

    public class TransitBookmark : TransitService<Bookmark>
    {
        private bool mHasFullBitmap;

        public bool HasFullBitmap
        {
            get
            {
                return mHasFullBitmap;
            }
        }

        private bool mHasLinkBitmap;

        public bool HasLinkBitmap
        {
            get
            {
                return mHasLinkBitmap;
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

        private string mUrl;

        public string Url
        {
            get
            {
                return mUrl;
            }
            set
            {
                mUrl = value;
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

        public TransitBookmark()
        {

        }

        public TransitBookmark(Bookmark instance)
            : base(instance)
        {

        }

        public override void SetInstance(Bookmark instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            Url = instance.Url;
            Created = instance.Created;
            Modified = instance.Modified;
            mHasFullBitmap = (instance.FullBitmap != null);
            mHasLinkBitmap = (instance.LinkBitmap != null);
            base.SetInstance(instance);
        }

        public override Bookmark GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Bookmark instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Url = this.Url;
            return instance;
        }
    }

    public class ManagedBookmark : ManagedService<Bookmark, TransitBookmark>
    {
        public ManagedBookmark()
        {

        }

        public ManagedBookmark(ISession session)
            : base(session)
        {

        }

        public ManagedBookmark(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBookmark(ISession session, Bookmark value)
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

        public byte[] FullBitmap
        {
            get
            {
                return mInstance.FullBitmap;
            }
        }

        public byte[] LinkBitmap
        {
            get
            {
                return mInstance.LinkBitmap;
            }
        }

        public string Url
        {
            get
            {
                return mInstance.Url;
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

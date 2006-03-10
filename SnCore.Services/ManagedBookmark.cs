using System;
using NHibernate;
using System.Collections;
using System.IO;
using SnCore.Tools.Drawing;

namespace SnCore.Services
{
    public class BookmarkQueryOptions
    {
        public bool WithFullBitmaps = false;
        public bool WithLinkedBitmaps = false;

        public BookmarkQueryOptions()
        {
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

        public override Bookmark GetBookmark(ISession session)
        {
            Bookmark b = base.GetBookmark(session);
            if (this.LinkBitmap != null) b.LinkBitmap = this.LinkBitmap;
            if (this.FullBitmap != null) b.FullBitmap = this.FullBitmap;
            return b;
        }
    }

    public class TransitBookmark : TransitService
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

        public TransitBookmark(Bookmark b)
            : base(b.Id)
        {
            Name = b.Name;
            Description = b.Description;
            Url = b.Url;
            Created = b.Created;
            Modified = b.Modified;
        }

        public virtual Bookmark GetBookmark(ISession session)
        {
            Bookmark b = (Id > 0) ? (Bookmark)session.Load(typeof(Bookmark), Id) : new Bookmark();
            b.Name = this.Name;
            b.Description = this.Description;
            b.Url = this.Url;
            return b;
        }
    }

    /// <summary>
    /// Managed social bookmark.
    /// </summary>
    public class ManagedBookmark : ManagedService
    {
        private Bookmark mBookmark = null;

        public ManagedBookmark(ISession session)
            : base(session)
        {

        }

        public ManagedBookmark(ISession session, int id)
            : base(session)
        {
            mBookmark = (Bookmark)session.Load(typeof(Bookmark), id);
        }

        public ManagedBookmark(ISession session, Bookmark value)
            : base(session)
        {
            mBookmark = value;
        }

        public int Id
        {
            get
            {
                return mBookmark.Id;
            }
        }

        public string Name
        {
            get
            {
                return mBookmark.Name;
            }
        }

        public string Description
        {
            get
            {
                return mBookmark.Description;
            }
        }

        public byte[] FullBitmap
        {
            get
            {
                return mBookmark.FullBitmap;
            }
        }

        public byte[] LinkBitmap
        {
            get
            {
                return mBookmark.LinkBitmap;
            }
        }

        public string Url
        {
            get
            {
                return mBookmark.Url;
            }
        }

        public DateTime Created
        {
            get
            {
                return mBookmark.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mBookmark.Modified;
            }
        }

        public TransitBookmarkWithBitmaps TransitBookmarkWithBitmaps
        {
            get
            {
                TransitBookmarkWithBitmaps pic = new TransitBookmarkWithBitmaps(mBookmark);
                return pic;
            }
        }

        public TransitBookmark TransitBookmark
        {
            get
            {
                return new TransitBookmark(mBookmark);
            }
        }

        public void CreateOrUpdate(TransitBookmark o)
        {
            mBookmark = o.GetBookmark(Session);
            mBookmark.Modified = DateTime.UtcNow;
            if (Id == 0) mBookmark.Created = mBookmark.Modified;
            Session.Save(mBookmark);
        }

        public void Delete()
        {
            Session.Delete(mBookmark);
        }
    }
}

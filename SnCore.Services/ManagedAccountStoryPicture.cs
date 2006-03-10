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
    public class TransitAccountStoryPictureWithPicture : TransitAccountStoryPicture
    {
        public byte[] Picture;

        public TransitAccountStoryPictureWithPicture()
        {

        }

        public TransitAccountStoryPictureWithPicture(AccountStoryPicture p)
            : base(p)
        {
            Picture = p.Picture;
            HasPicture = true;
        }

        public override AccountStoryPicture GetAccountStoryPicture(ISession session)
        {
            AccountStoryPicture p = base.GetAccountStoryPicture(session);
            p.Picture = Picture;
            return p;
        }
    }

    public class TransitAccountStoryPictureWithThumbnail : TransitAccountStoryPicture
    {
        public byte[] Thumbnail;

        public TransitAccountStoryPictureWithThumbnail()
        {

        }

        public TransitAccountStoryPictureWithThumbnail(AccountStoryPicture p)
            : base(p)
        {
            Thumbnail = new ThumbnailBitmap(p.Picture).Thumbnail;
        }
    }

    public class TransitAccountStoryPicture : TransitService
    {
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

        public TransitAccountStoryPicture()
        {

        }

        public TransitAccountStoryPicture(AccountStoryPicture p)
            : base(p.Id)
        {
            Location = p.Location;
            AccountStoryId = p.AccountStory.Id;
            HasPicture = p.Picture != null;
            Name = p.Name;
            Created = p.Created;
            Modified = p.Modifed;
        }

        public virtual AccountStoryPicture GetAccountStoryPicture(ISession session)
        {
            AccountStoryPicture p = (Id != 0) ? (AccountStoryPicture)session.Load(typeof(AccountStoryPicture), Id) : new AccountStoryPicture();

            if (Id == 0)
            {
                if (AccountStoryId > 0) p.AccountStory = (AccountStory)session.Load(typeof(AccountStory), this.AccountStoryId);
            }

            p.Location = this.Location;
            p.Name = this.Name;
            return p;
        }

    }

    public class ManagedAccountStoryPicture : ManagedService
    {
        private AccountStoryPicture mAccountStoryPicture = null;

        public ManagedAccountStoryPicture(ISession session)
            : base(session)
        {

        }

        public ManagedAccountStoryPicture(ISession session, int id)
            : base(session)
        {
            mAccountStoryPicture = (AccountStoryPicture)session.Load(typeof(AccountStoryPicture), id);
        }

        public ManagedAccountStoryPicture(ISession session, AccountStoryPicture value)
            : base(session)
        {
            mAccountStoryPicture = value;
        }

        public int Id
        {
            get
            {
                return mAccountStoryPicture.Id;
            }
        }

        public int Location
        {
            get
            {
                return mAccountStoryPicture.Location;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountStoryPicture.AccountStory.Account.Id;
            }
        }

        public TransitAccountStoryPicture TransitAccountStoryPicture
        {
            get
            {
                return new TransitAccountStoryPicture(mAccountStoryPicture);
            }
        }

        public TransitAccountStoryPictureWithPicture TransitAccountStoryPictureWithPicture
        {
            get
            {
                return new TransitAccountStoryPictureWithPicture(mAccountStoryPicture);
            }
        }

        public TransitAccountStoryPictureWithThumbnail TransitAccountStoryPictureWithThumbnail
        {
            get
            {
                return new TransitAccountStoryPictureWithThumbnail(mAccountStoryPicture);
            }
        }

        public void Delete()
        {
            mAccountStoryPicture.AccountStory.AccountStoryPictures.Remove(mAccountStoryPicture);
            Session.Delete(mAccountStoryPicture);

            // renumber the order of Pictures
            foreach (AccountStoryPicture p in mAccountStoryPicture.AccountStory.AccountStoryPictures)
            {
                if (p.Location >= mAccountStoryPicture.Location)
                {
                    p.Location = p.Location - 1;
                    Session.Save(p);
                }
            }
        }

        public void Move(int Disp)
        {
            int newLocation = mAccountStoryPicture.Location + Disp;

            if (newLocation < 1 || newLocation > mAccountStoryPicture.AccountStory.AccountStoryPictures.Count)
            {
                // throw new ArgumentOutOfRangeException();
                return;
            }

            foreach (AccountStoryPicture p in mAccountStoryPicture.AccountStory.AccountStoryPictures)
            {
                if (p.Location == mAccountStoryPicture.Location)
                {
                    // this item
                }
                else if (p.Location < mAccountStoryPicture.Location && p.Location >= newLocation)
                {
                    // item was before me but switched sides
                    p.Location++;
                }
                else if (p.Location > mAccountStoryPicture.Location && p.Location <= newLocation)
                {
                    // item was after me, but switched sides
                    p.Location--;
                }

                Session.Save(p);
            }

            mAccountStoryPicture.Location = newLocation;
            Session.Save(mAccountStoryPicture);
            Session.Flush();
        }
    }
}

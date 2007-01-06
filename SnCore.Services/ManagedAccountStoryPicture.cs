using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Drawing;
using SnCore.Data.Hibernate;

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

        public override AccountStoryPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountStoryPicture instance = base.GetInstance(session, sec);
            instance.Picture = Picture;
            return instance;
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

    public class TransitAccountStoryPicture : TransitArrayElementService<AccountStoryPicture>
    {
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
            : base(p.Id, p, p.AccountStory.AccountStoryPictures)
        {
            Location = p.Location;
            AccountStoryId = p.AccountStory.Id;
            HasPicture = p.Picture != null;
            Name = p.Name;
            Created = p.Created;
            Modified = p.Modifed;
        }

        public override AccountStoryPicture GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountStoryPicture instance = base.GetInstance(session, sec);
            if (Id == 0) instance.AccountStory = (AccountStory) session.Load(typeof(AccountStory), AccountStoryId);
            instance.Location = Location;
            instance.Name = Name;
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
            Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures).Remove(mInstance);
            base.Delete(sec);

            // renumber the order of Pictures
            foreach (AccountStoryPicture p in Collection<AccountStoryPicture>.GetSafeCollection(mInstance.AccountStory.AccountStoryPictures))
            {
                if (p.Location >= Object.Location)
                {
                    p.Location = p.Location - 1;
                    Session.Save(p);
                }
            }
        }

        public void Move(int Disp)
        {
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
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.AccountStory.Account, DataOperation.All));
            return acl;
        }
    }
}

using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Data.Hibernate;
using SnCore.Tools.Drawing;

namespace SnCore.Services
{
    public class TransitPlaceWebsite : TransitService<PlaceWebsite>
    {
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

        public TransitPlaceWebsite()
        {

        }

        public TransitPlaceWebsite(PlaceWebsite instance)
            : base(instance)
        {

        }

        public override void SetInstance(PlaceWebsite instance)
        {
            Url = instance.Url;
            Name = instance.Name;
            Description = instance.Description;
            AccountId = instance.Account.Id;
            PlaceId = instance.Place.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            Bitmap = instance.Bitmap;
            if (Bitmap != null) Thumbnail = new ThumbnailBitmap(Bitmap).Thumbnail;
            base.SetInstance(instance);
        }

        public override PlaceWebsite GetInstance(ISession session, ManagedSecurityContext sec)
        {
            PlaceWebsite instance = base.GetInstance(session, sec);
            instance.Modified = DateTime.UtcNow;
            if (Id == 0)
            {
                instance.Account = base.GetOwner(session, AccountId, sec);
                instance.Place = session.Load<Place>(PlaceId);
                instance.Created = instance.Modified;
            }
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Url = this.Url;
            instance.Bitmap = Bitmap;
            return instance;
        }
    }

    public class ManagedPlaceWebsite : ManagedAuditableService<PlaceWebsite, TransitPlaceWebsite>
    {
        public class InvalidUriException : Exception
        {
            public InvalidUriException()
                : base("Invalid url format.\nPlease make sure it starts with http://.")
            {

            }
        }

        public ManagedPlaceWebsite()
        {

        }

        public ManagedPlaceWebsite(ISession session)
            : base(session)
        {

        }

        public ManagedPlaceWebsite(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedPlaceWebsite(ISession session, PlaceWebsite value)
            : base(session, value)
        {

        }

        public string Url
        {
            get
            {
                return mInstance.Url;
            }
        }

        public string Description
        {
            get
            {
                return mInstance.Description;
            }
        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<PlaceWebsite>.GetSafeCollection(mInstance.Account.PlaceWebsites).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (!Uri.IsWellFormedUriString(mInstance.Url, UriKind.Absolute))
                throw new ManagedPlaceWebsite.InvalidUriException();

            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            // website owner can delete/edit his own addition
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            // place owners have the same rights to linked websites
            acl.AddRange(new ManagedPlace(Session, mInstance.Place).GetACL(type).AccessControlList);
            return acl;
        }

        protected override void Check(TransitPlaceWebsite t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                GetQuota(sec).Check<PlaceWebsite, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM PlaceWebsite instance WHERE instance.Account.Id = {0}", 
                        mInstance.Account.Id)).UniqueResult<int>());
            }
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] linked {1} from [place:{2}]",
                        mInstance.Account.Id, mInstance.Url, mInstance.Place.Id),
                        string.Format("PlaceView.aspx?id={0}", mInstance.Place.Id)));
                    break;
            }
            return result;
        }

        public void UpdateThumbnail()
        {
            WebsiteBitmap bitmap = new WebsiteBitmap();
            mInstance.Bitmap = ThumbnailBitmap.GetBitmap(bitmap.GetBitmapFromWeb(mInstance.Url));
            mInstance.Modified = DateTime.UtcNow;
            Session.Save(mInstance);
        }

        public void MigrateToAccount(Account newowner, ManagedSecurityContext sec)
        {
            mInstance.Account = newowner;
            Session.Save(mInstance);
        }
    }
}

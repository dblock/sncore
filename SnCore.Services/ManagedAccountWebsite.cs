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
    public class TransitAccountWebsite : TransitService<AccountWebsite>
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

        public TransitAccountWebsite()
        {

        }

        public TransitAccountWebsite(AccountWebsite instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountWebsite instance)
        {
            Url = instance.Url;
            Name = instance.Name;
            Description = instance.Description;
            AccountId = instance.Account.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            Bitmap = instance.Bitmap;
            if (Bitmap != null) Thumbnail = new ThumbnailBitmap(Bitmap).Thumbnail;
            base.SetInstance(instance);
        }

        public override AccountWebsite GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountWebsite instance = base.GetInstance(session, sec);
            instance.Modified = DateTime.UtcNow;
            if (Id == 0)
            {
                instance.Account = base.GetOwner(session, AccountId, sec);
                instance.Created = instance.Modified;
            }
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Url = this.Url;
            instance.Bitmap = Bitmap;
            return instance;
        }
    }

    public class ManagedAccountWebsite : ManagedAuditableService<AccountWebsite, TransitAccountWebsite>
    {
        public class InvalidUriException : Exception
        {
            public InvalidUriException()
                : base("Invalid url format.\nPlease make sure it starts with http://.")
            {

            }
        }

        public ManagedAccountWebsite()
        {

        }

        public ManagedAccountWebsite(ISession session)
            : base(session)
        {

        }

        public ManagedAccountWebsite(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountWebsite(ISession session, AccountWebsite value)
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
            Collection<AccountWebsite>.GetSafeCollection(mInstance.Account.AccountWebsites).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (!Uri.IsWellFormedUriString(mInstance.Url, UriKind.Absolute))
                throw new ManagedAccountWebsite.InvalidUriException();

            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountWebsite t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountWebsite, ManagedAccount.QuotaExceededException>(
                mInstance.Account.AccountWebsites);
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] added {1} to websites",
                        mInstance.Account.Id, mInstance.Url),
                        string.Format("AccountView.aspx?id={0}", mInstance.Account.Id)));
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
    }
}

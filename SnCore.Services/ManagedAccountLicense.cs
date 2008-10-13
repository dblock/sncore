using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountLicense : TransitService<AccountLicense>
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

        private string mLicenseUrl;

        public string LicenseUrl
        {
            get
            {

                return mLicenseUrl;
            }
            set
            {
                mLicenseUrl = value;
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

        private string mImageUrl;

        public string ImageUrl
        {
            get
            {
                return mImageUrl;
            }
            set
            {
                mImageUrl = value;
            }
        }

        public TransitAccountLicense()
        {

        }

        public TransitAccountLicense(AccountLicense value)
            : base(value)
        {

        }

        public override void SetInstance(AccountLicense value)
        {
            Name = value.Name;
            LicenseUrl = value.LicenseUrl;
            ImageUrl = value.ImageUrl;
            AccountId = value.Account.Id;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountLicense GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountLicense instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.LicenseUrl = this.LicenseUrl;
            instance.ImageUrl = this.ImageUrl;
            return instance;
        }
    }

    public class ManagedAccountLicense : ManagedService<AccountLicense, TransitAccountLicense>
    {
        public ManagedAccountLicense()
        {

        }

        public ManagedAccountLicense(ISession session)
            : base(session)
        {

        }

        public ManagedAccountLicense(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountLicense(ISession session, AccountLicense value)
            : base(session, value)
        {

        }


        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public string LicenseUrl
        {
            get
            {
                return mInstance.LicenseUrl;
            }
        }

        public string ImageUrl
        {
            get
            {
                return mInstance.ImageUrl;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedFeature.Delete(Session, "AccountLicense", Id);
            Collection<AccountLicense>.GetSafeCollection(mInstance.Account.AccountLicenses).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
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

        protected override void Check(TransitAccountLicense t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountLicense, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountLicense instance WHERE instance.Account.Id = {0}",
                        mInstance.Account.Id)).UniqueResult<int>());
        }
    }
}

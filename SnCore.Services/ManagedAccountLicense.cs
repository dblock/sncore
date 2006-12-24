using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace SnCore.Services
{
    public class TransitAccountLicense : TransitService
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

        public TransitAccountLicense(AccountLicense s)
            : base(s.Id)
        {
            Name = s.Name;
            LicenseUrl = s.LicenseUrl;
            ImageUrl = s.ImageUrl;
            AccountId = s.Account.Id;
            Created = s.Created;
            Modified = s.Modified;
        }

        public AccountLicense GetAccountLicense(ISession session)
        {
            AccountLicense s = (Id != 0) ? (AccountLicense)session.Load(typeof(AccountLicense), Id) : new AccountLicense();

            if (Id == 0)
            {
                if (AccountId > 0) s.Account = (Account)session.Load(typeof(Account), this.AccountId);
            }

            s.Name = this.Name;
            s.LicenseUrl = this.LicenseUrl;
            s.ImageUrl = this.ImageUrl;
            return s;
        }

    }

    public class ManagedAccountLicense : ManagedService<AccountLicense>
    {
        private AccountLicense mAccountLicense = null;

        public ManagedAccountLicense(ISession session)
            : base(session)
        {

        }

        public ManagedAccountLicense(ISession session, int id)
            : base(session)
        {
            mAccountLicense = (AccountLicense)session.Load(typeof(AccountLicense), id);
        }

        public ManagedAccountLicense(ISession session, AccountLicense value)
            : base(session)
        {
            mAccountLicense = value;
        }

        public int Id
        {
            get
            {
                return mAccountLicense.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountLicense.Account.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAccountLicense.Name;
            }
        }

        public string LicenseUrl
        {
            get
            {
                return mAccountLicense.LicenseUrl;
            }
        }

        public string ImageUrl
        {
            get
            {
                return mAccountLicense.ImageUrl;
            }
        }

        public TransitAccountLicense TransitAccountLicense
        {
            get
            {
                return new TransitAccountLicense(mAccountLicense);
            }
        }

        public void Delete()
        {
            ManagedFeature.Delete(Session, "AccountLicense", Id);
            mAccountLicense.Account.AccountLicenses.Remove(mAccountLicense);
            Session.Delete(mAccountLicense);
        }
    }
}

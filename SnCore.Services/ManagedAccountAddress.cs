using System;
using NHibernate;
using System.Collections.Generic;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountAddress : TransitService<AccountAddress>
    {
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

        private string mApt;

        public string Apt
        {
            get
            {

                return mApt;
            }
            set
            {
                mApt = value;
            }
        }

        private string mCity;

        public string City
        {
            get
            {

                return mCity;
            }
            set
            {
                mCity = value;
            }
        }

        private string mCountry;

        public string Country
        {
            get
            {

                return mCountry;
            }
            set
            {
                mCountry = value;
            }
        }

        private string mState;

        public string State
        {
            get
            {

                return mState;
            }
            set
            {
                mState = value;
            }
        }

        private string mZip;

        public string Zip
        {
            get
            {

                return mZip;
            }
            set
            {
                mZip = value;
            }
        }

        private string mStreet;

        public string Street
        {
            get
            {

                return mStreet;
            }
            set
            {
                mStreet = value;
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

        public TransitAccountAddress()
        {

        }

        public TransitAccountAddress(AccountAddress instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountAddress instance)
        {
            AccountId = instance.Account.Id;
            Name = instance.Name;
            Street = instance.Street;
            Apt = instance.Apt;
            City = instance.City;
            Country = instance.Country.Name;
            State = (instance.State != null ? instance.State.Name : string.Empty);
            Zip = instance.Zip;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override AccountAddress GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountAddress instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = base.GetOwner(session, AccountId, sec);
            instance.Apt = this.Apt;
            instance.City = this.City;
            instance.Country = session.Load<Country>(ManagedCountry.GetCountryId(session, this.Country));
            instance.Name = this.Name;
            instance.State = session.Load<State>(ManagedState.GetStateId(session, this.State, this.Country));
            if (instance.State.Country.Id != instance.Country.Id) throw new ManagedCountry.InvalidCountryException();
            instance.Street = this.Street;
            instance.Zip = this.Zip;
            return instance;
        }
    }

    public class ManagedAccountAddress : ManagedService<AccountAddress, TransitAccountAddress>
    {
        public ManagedAccountAddress()
        {

        }

        public ManagedAccountAddress(ISession session)
            : base(session)
        {

        }

        public ManagedAccountAddress(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountAddress(ISession session, AccountAddress value)
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

        public string Apt
        {
            get
            {
                return mInstance.Apt;
            }
        }

        public string City
        {
            get
            {
                return mInstance.City;
            }
        }

        public string Country
        {
            get
            {
                return mInstance.Country.Name;
            }
        }

        public string State
        {
            get
            {
                return mInstance.State.Name;
            }
        }

        public string Zip
        {
            get
            {
                return mInstance.Zip;
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

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountAddress>.GetSafeCollection(mInstance.Account.AccountAddresses).Remove(mInstance);
            base.Delete(sec);
        }

        public Account Account
        {
            get
            {
                return mInstance.Account;
            }
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

        protected override void Check(TransitAccountAddress t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountAddress, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountAddress instance WHERE instance.Account.Id = {0}",
                        mInstance.Account.Id)).UniqueResult<int>());
        }
    }
}

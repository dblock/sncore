using System;
using NHibernate;
using System.Collections;

namespace SnCore.Services
{
    public class TransitAccountAddress : TransitService
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

        public TransitAccountAddress(AccountAddress e)
            : base(e.Id)
        {
            AccountId = e.Account.Id;
            Name = e.Name;
            Street = e.Street;
            Apt = e.Apt;
            City = e.City;
            Country = e.Country.Name;
            State = e.State.Name;
            Zip = e.Zip;
            Created = e.Created;
            Modified = e.Modified;
        }

        public AccountAddress GetAccountAddress(ISession session)
        {
            AccountAddress a = (Id != 0) ? (AccountAddress)session.Load(typeof(AccountAddress), Id) : new AccountAddress();

            if (Id == 0)
            {
                if (AccountId > 0) a.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            a.Apt = this.Apt;
            a.City = this.City;
            a.Country = (Country)session.Load(typeof(Country), ManagedCountry.GetCountryId(session, this.Country));
            a.Name = this.Name;
            a.State = (State)session.Load(typeof(State), ManagedState.GetStateId(session, this.State, this.Country));
            if (a.State.Country.Id != a.Country.Id)
            {
                throw new ManagedCountry.InvalidCountryException();
            }
            a.Street = this.Street;
            a.Zip = this.Zip;
            return a;
        }
    }

    /// <summary>
    /// Managed address.
    /// </summary>
    public class ManagedAccountAddress : ManagedService
    {
        private AccountAddress mAccountAddress = null;

        public ManagedAccountAddress(ISession session)
            : base(session)
        {

        }

        public ManagedAccountAddress(ISession session, int id)
            : base(session)
        {
            mAccountAddress = (AccountAddress)session.Load(typeof(AccountAddress), id);
        }

        public ManagedAccountAddress(ISession session, AccountAddress value)
            : base(session)
        {
            mAccountAddress = value;
        }

        public int AccountId
        {
            get
            {
                return mAccountAddress.Account.Id;
            }
        }

        public int Id
        {
            get
            {
                return mAccountAddress.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAccountAddress.Name;
            }
        }

        public string Apt
        {
            get
            {
                return mAccountAddress.Apt;
            }
        }

        public string City
        {
            get
            {
                return mAccountAddress.City;
            }
        }

        public string Country
        {
            get
            {
                return mAccountAddress.Country.Name;
            }
        }

        public string State
        {
            get
            {
                return mAccountAddress.State.Name;
            }
        }

        public string Zip
        {
            get
            {
                return mAccountAddress.Zip;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountAddress.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountAddress.Modified;
            }
        }

        public TransitAccountAddress TransitAccountAddress
        {
            get
            {
                return new TransitAccountAddress(mAccountAddress);
            }
        }

        public void Delete()
        {
            mAccountAddress.Account.AccountAddresses.Remove(mAccountAddress);
            Session.Delete(mAccountAddress);
        }

        public Account Account
        {
            get
            {
                return mAccountAddress.Account;
            }
        }
    }
}

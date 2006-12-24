using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;

namespace SnCore.Services
{
    public class TransitAccountWebsite : TransitService
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

        public TransitAccountWebsite()
        {

        }

        public TransitAccountWebsite(AccountWebsite w)
            : base(w.Id)
        {
            Url = w.Url;
            Name = w.Name;
            Description = w.Description;
            AccountId = w.Account.Id;
        }

        public AccountWebsite GetAccountWebsite(ISession session)
        {
            AccountWebsite w = (Id != 0) ? (AccountWebsite)session.Load(typeof(AccountWebsite), Id) : new AccountWebsite();

            if (Id == 0)
            {
                if (AccountId > 0) w.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            w.Name = this.Name;
            w.Description = this.Description;
            w.Url = this.Url;
            return w;
        }

    }

    public class ManagedAccountWebsite : ManagedService<AccountWebsite>
    {
        public class InvalidUriException : SoapException
        {
            public InvalidUriException()
                : base("Invalid url format.\nPlease make sure it starts with http://.", SoapException.ClientFaultCode)
            {

            }
        }

        private AccountWebsite mAccountWebsite = null;

        public ManagedAccountWebsite(ISession session)
            : base(session)
        {

        }

        public ManagedAccountWebsite(ISession session, int id)
            : base(session)
        {
            mAccountWebsite = (AccountWebsite)session.Load(typeof(AccountWebsite), id);
        }

        public ManagedAccountWebsite(ISession session, AccountWebsite value)
            : base(session)
        {
            mAccountWebsite = value;
        }

        public int Id
        {
            get
            {
                return mAccountWebsite.Id;
            }
        }

        public string Url
        {
            get
            {
                return mAccountWebsite.Url;
            }
        }

        public string Description
        {
            get
            {
                return mAccountWebsite.Description;
            }
        }

        public string Name
        {
            get
            {
                return mAccountWebsite.Name;
            }
        }

        public TransitAccountWebsite TransitAccountWebsite
        {
            get
            {
                return new TransitAccountWebsite(mAccountWebsite);
            }
        }

        public void Delete()
        {
            mAccountWebsite.Account.AccountWebsites.Remove(mAccountWebsite);
            Session.Delete(mAccountWebsite);
        }

        public Account Account
        {
            get
            {
                return mAccountWebsite.Account;
            }
        }
    }
}

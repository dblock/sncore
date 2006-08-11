using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;

namespace SnCore.Services
{
    public class TransitAccountPropertyValue : TransitService
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

        private TransitAccountProperty mAccountProperty;

        public TransitAccountProperty AccountProperty
        {
            get
            {
                return mAccountProperty;
            }
            set
            {
                mAccountProperty = value;
            }
        }

        private string mValue;

        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
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

        public TransitAccountPropertyValue()
        {

        }

        public TransitAccountPropertyValue(AccountPropertyValue o)
            : base(o.Id)
        {
            AccountId = o.Account.Id;
            AccountProperty = new TransitAccountProperty(o.AccountProperty);
            Created = o.Created;
            Modified = o.Modified;
            Value = o.Value;
        }

        public AccountPropertyValue GetAccountPropertyValue(ISession session)
        {
            AccountPropertyValue p = (Id != 0) ? (AccountPropertyValue)session.Load(typeof(AccountPropertyValue), Id) : new AccountPropertyValue();
            p.Account = (this.AccountId > 0) ? (Account)session.Load(typeof(Account), AccountId) : null;
            p.AccountProperty = (this.AccountProperty != null && this.AccountProperty.Id > 0) ? (AccountProperty)session.Load(typeof(AccountProperty), this.AccountProperty.Id) : null;
            p.Value = this.Value;
            return p;
        }
    }

    public class ManagedAccountPropertyValue : ManagedService
    {
        private AccountPropertyValue mAccountPropertyValue = null;

        public ManagedAccountPropertyValue(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPropertyValue(ISession session, int id)
            : base(session)
        {
            mAccountPropertyValue = (AccountPropertyValue)session.Load(typeof(AccountPropertyValue), id);
        }

        public ManagedAccountPropertyValue(ISession session, AccountPropertyValue value)
            : base(session)
        {
            mAccountPropertyValue = value;
        }

        public ManagedAccountPropertyValue(ISession session, TransitAccountPropertyValue value)
            : base(session)
        {
            mAccountPropertyValue = value.GetAccountPropertyValue(session);
        }

        public int Id
        {
            get
            {
                return mAccountPropertyValue.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountPropertyValue.Account.Id;
            }
        }

        public TransitAccountPropertyValue TransitAccountPropertyValue
        {
            get
            {
                return new TransitAccountPropertyValue(mAccountPropertyValue);
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountPropertyValue);
        }
    }
}

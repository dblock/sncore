using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;

namespace SnCore.Services
{
    public class TransitAccountAttribute : TransitService
    {
        private TransitAttribute mAttribute;

        public TransitAttribute Attribute
        {
            get
            {
                return mAttribute;
            }
            set
            {
                mAttribute = value;
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

        private int mAttributeId;

        public int AttributeId
        {
            get
            {

                return mAttributeId;
            }
            set
            {
                mAttributeId = value;
            }
        }

        public TransitAccountAttribute()
        {

        }

        public TransitAccountAttribute(AccountAttribute attribute)
            : base(attribute.Id)
        {
            AccountId = attribute.Account.Id;
            AttributeId = attribute.Attribute.Id;
            Value = attribute.Value;
            Url = attribute.Url;
            Created = attribute.Created;
            Attribute = new TransitAttribute(attribute.Attribute);
        }

        public AccountAttribute GetAccountAttribute(ISession session)
        {
            AccountAttribute result = (Id > 0) ? (AccountAttribute)session.Load(typeof(AccountAttribute), Id) : new AccountAttribute();
            result.Url = Url;
            result.Value = Value;

            if (Id == 0)
            {
                if (AccountId > 0) result.Account = (Account)session.Load(typeof(Account), AccountId);
                if (AttributeId > 0) result.Attribute = (Attribute)session.Load(typeof(Attribute), AttributeId);
            }
            else
            {
                if (AttributeId != result.Attribute.Id)
                {
                    throw new InvalidOperationException();
                }

                if (AccountId != result.Account.Id)
                {
                    throw new InvalidOperationException();
                }
            }

            return result;
        }
    }

    public class ManagedAccountAttribute : ManagedService
    {
        private AccountAttribute mAccountAttribute = null;

        public ManagedAccountAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedAccountAttribute(ISession session, int id)
            : base(session)
        {
            mAccountAttribute = (AccountAttribute)session.Load(typeof(AccountAttribute), id);
        }

        public ManagedAccountAttribute(ISession session, TransitAccountAttribute tae)
            : base(session)
        {
            mAccountAttribute = tae.GetAccountAttribute(session);
        }

        public ManagedAccountAttribute(ISession session, AccountAttribute value)
            : base(session)
        {
            mAccountAttribute = value;
        }

        public int Id
        {
            get
            {
                return mAccountAttribute.Id;
            }
        }

        public string Value
        {
            get
            {
                return mAccountAttribute.Value;
            }
        }

        public string Url
        {
            get
            {
                return mAccountAttribute.Url;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountAttribute.Created;
            }
        }

        public TransitAccountAttribute TransitAccountAttribute
        {
            get
            {
                return new TransitAccountAttribute(mAccountAttribute);
            }
        }

        public void Delete()
        {
            mAccountAttribute.Account.AccountAttributes.Remove(mAccountAttribute);
            Session.Delete(mAccountAttribute);
        }

        public ManagedAccount Account
        {
            get
            {
                return new ManagedAccount(Session, mAccountAttribute.Account);
            }
        }
    }
}

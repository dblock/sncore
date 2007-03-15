using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountAttribute : TransitService<AccountAttribute>
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

        public TransitAccountAttribute(AccountAttribute instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountAttribute instance)
        {
            AccountId = instance.Account.Id;
            AttributeId = instance.Attribute.Id;
            Value = instance.Value;
            Url = instance.Url;
            Created = instance.Created;
            Attribute = new TransitAttribute(instance.Attribute);
            base.SetInstance(instance);
        }

        public override AccountAttribute GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountAttribute instance = base.GetInstance(session, sec);
            instance.Url = Url;
            instance.Value = Value;

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Attribute = session.Load<Attribute>(AttributeId);
            }
            else
            {
                if (AttributeId != instance.Attribute.Id)
                {
                    throw new InvalidOperationException();
                }

                if (AccountId != instance.Account.Id)
                {
                    throw new InvalidOperationException();
                }
            }

            return instance;
        }
    }

    public class ManagedAccountAttribute : ManagedService<AccountAttribute, TransitAccountAttribute>
    {
        public ManagedAccountAttribute()
        {

        }

        public ManagedAccountAttribute(ISession session)
            : base(session)
        {

        }

        public ManagedAccountAttribute(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountAttribute(ISession session, AccountAttribute value)
            : base(session, value)
        {

        }

        public string Value
        {
            get
            {
                return mInstance.Value;
            }
        }

        public string Url
        {
            get
            {
                return mInstance.Url;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountAttribute>.GetSafeCollection(mInstance.Account.AccountAttributes).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

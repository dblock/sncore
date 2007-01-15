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
    public class TransitAccountPropertyValue : TransitService<AccountPropertyValue>
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

        private int mAccountPropertyId;

        public int AccountPropertyId
        {
            get
            {
                return mAccountPropertyId;
            }
            set
            {
                mAccountPropertyId = value;
            }
        }

        private string mAccountPropertyName;

        public string AccountPropertyName
        {
            get
            {
                return mAccountPropertyName;
            }
            set
            {
                mAccountPropertyName = value;
            }
        }

        private string mAccountPropertyTypeName;

        public string AccountPropertyTypeName
        {
            get
            {
                return mAccountPropertyTypeName;
            }
            set
            {
                mAccountPropertyTypeName = value;
            }
        }

        private int mAccountPropertyGroupId;

        public int AccountPropertyGroupId
        {
            get
            {
                return mAccountPropertyGroupId;
            }
            set
            {
                mAccountPropertyGroupId = value;
            }
        }

        private string mAccountPropertyGroupName;

        public string AccountPropertyGroupName
        {
            get
            {
                return mAccountPropertyGroupName;
            }
            set
            {
                mAccountPropertyGroupName = value;
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

        public TransitAccountPropertyValue(AccountPropertyValue instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountPropertyValue instance)
        {
            AccountId = instance.Account.Id;
            AccountPropertyId = instance.AccountProperty.Id;
            AccountPropertyName = instance.AccountProperty.Name;
            AccountPropertyTypeName = instance.AccountProperty.TypeName;
            AccountPropertyGroupName = instance.AccountProperty.AccountPropertyGroup.Name;
            AccountPropertyGroupId = instance.AccountProperty.AccountPropertyGroup.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            Value = instance.Value;
            base.SetInstance(instance);
        }

        public override AccountPropertyValue GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPropertyValue instance = base.GetInstance(session, sec);
            instance.Account = GetOwner(session, AccountId, sec);
            if (AccountPropertyId > 0) instance.AccountProperty = (AccountProperty)session.Load(typeof(AccountProperty), AccountPropertyId);
            instance.Value = this.Value;
            return instance;
        }
    }

    public class ManagedAccountPropertyValue : ManagedService<AccountPropertyValue, TransitAccountPropertyValue>
    {
        public ManagedAccountPropertyValue()
        {

        }

        public ManagedAccountPropertyValue(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPropertyValue(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPropertyValue(ISession session, AccountPropertyValue value)
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

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

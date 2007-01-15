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
    public class TransitAccountProperty : TransitService<AccountProperty>
    {
        private bool mPublish;

        public bool Publish
        {
            get
            {
                return mPublish;
            }
            set
            {
                mPublish = value;
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

        private string mType;

        public string Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        private string mDefaultValue;

        public string DefaultValue
        {
            get
            {
                return mDefaultValue;
            }
            set
            {
                mDefaultValue = value;
            }
        }

        private int mAccountPropertyGroupId = 0;

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

        public TransitAccountProperty()
        {

        }

        public TransitAccountProperty(AccountProperty value)
            : base(value)
        {

        }

        public override void SetInstance(AccountProperty value)
        {
            Name = value.Name;
            Description = value.Description;
            Type = value.TypeName;
            DefaultValue = value.DefaultValue;
            Publish = value.Publish;
            AccountPropertyGroupId = value.AccountPropertyGroup.Id;
            base.SetInstance(value);
        }

        public override AccountProperty GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountProperty instance = base.GetInstance(session, sec);
            instance.AccountPropertyGroup = (AccountPropertyGroupId > 0) ? (AccountPropertyGroup)session.Load(typeof(AccountPropertyGroup), AccountPropertyGroupId) : null;
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.DefaultValue = this.DefaultValue;
            instance.TypeName = this.Type;
            instance.Publish = this.Publish;
            return instance;
        }
    }

    public class ManagedAccountProperty : ManagedService<AccountProperty, TransitAccountProperty>
    {
        public ManagedAccountProperty()
        {

        }

        public ManagedAccountProperty(ISession session)
            : base(session)
        {

        }

        public ManagedAccountProperty(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountProperty(ISession session, AccountProperty value)
            : base(session, value)
        {

        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

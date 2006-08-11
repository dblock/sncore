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
    public class TransitAccountProperty : TransitService
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

        private Type mType;

        public Type Type
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

        public TransitAccountProperty(AccountProperty o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Type = Type.GetType(o.TypeName);
            DefaultValue = o.DefaultValue;
            Publish = o.Publish;
            AccountPropertyGroupId = o.AccountPropertyGroup.Id;
        }

        public AccountProperty GetAccountProperty(ISession session)
        {
            AccountProperty p = (Id != 0) ? (AccountProperty)session.Load(typeof(AccountProperty), Id) : new AccountProperty();
            p.AccountPropertyGroup = (AccountPropertyGroupId > 0) ? (AccountPropertyGroup)session.Load(typeof(AccountPropertyGroup), AccountPropertyGroupId) : null;
            p.Name = this.Name;
            p.Description = this.Description;
            p.DefaultValue = this.DefaultValue;
            p.TypeName = this.Type.ToString();
            p.Publish = this.Publish;
            return p;
        }
    }

    public class ManagedAccountProperty : ManagedService
    {
        private AccountProperty mAccountProperty = null;

        public ManagedAccountProperty(ISession session)
            : base(session)
        {

        }

        public ManagedAccountProperty(ISession session, int id)
            : base(session)
        {
            mAccountProperty = (AccountProperty)session.Load(typeof(AccountProperty), id);
        }

        public ManagedAccountProperty(ISession session, AccountProperty value)
            : base(session)
        {
            mAccountProperty = value;
        }

        public ManagedAccountProperty(ISession session, TransitAccountProperty value)
            : base(session)
        {
            mAccountProperty = value.GetAccountProperty(session);
        }

        public int Id
        {
            get
            {
                return mAccountProperty.Id;
            }
        }

        public TransitAccountProperty TransitAccountProperty
        {
            get
            {
                return new TransitAccountProperty(mAccountProperty);
            }
        }

        public void CreateOrUpdate(TransitAccountProperty o)
        {
            mAccountProperty = o.GetAccountProperty(Session);
            Session.Save(mAccountProperty);
        }

        public void Delete()
        {
            Session.Delete(mAccountProperty);
        }
    }
}

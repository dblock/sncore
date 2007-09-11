using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;

namespace SnCore.Services
{
    public class TransitAccountEventType : TransitService<AccountEventType>
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

        private bool mDefaultType = false;

        public bool DefaultType
        {
            get
            {
                return mDefaultType;
            }
            set
            {
                mDefaultType = value;
            }
        }

        public TransitAccountEventType()
        {

        }

        public TransitAccountEventType(AccountEventType instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountEventType instance)
        {
            Name = instance.Name;
            DefaultType = instance.DefaultType;
            base.SetInstance(instance);
        }

        public override AccountEventType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEventType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.DefaultType = this.DefaultType;
            return instance;
        }
    }

    public class ManagedAccountEventType : ManagedService<AccountEventType, TransitAccountEventType>
    {
        public ManagedAccountEventType()
        {

        }

        public ManagedAccountEventType(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEventType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEventType(ISession session, AccountEventType value)
            : base(session, value)
        {

        }

        public static AccountEventType Find(ISession session, string name)
        {
            return (AccountEventType)session.CreateCriteria(typeof(AccountEventType))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            base.Save(sec);

            if (mInstance.DefaultType)
            {
                IList<AccountEventType> instances = Session.CreateCriteria(typeof(AccountEventType))
                    .List<AccountEventType>();

                foreach (AccountEventType instance in instances)
                {
                    if (instance != mInstance && mInstance.DefaultType)
                    {
                        instance.DefaultType = false;
                        Session.Save(instance);
                    }
                }
            }
        }
    }
}

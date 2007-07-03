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
    public class TransitAccountFlagType : TransitService<AccountFlagType>
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

        public TransitAccountFlagType()
        {

        }

        public TransitAccountFlagType(AccountFlagType instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountFlagType instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override AccountFlagType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFlagType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedAccountFlagType : ManagedService<AccountFlagType, TransitAccountFlagType>
    {
        public ManagedAccountFlagType()
        {

        }

        public ManagedAccountFlagType(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFlagType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFlagType(ISession session, AccountFlagType value)
            : base(session, value)
        {

        }

        public static AccountFlagType Find(ISession session, string name)
        {
            return (AccountFlagType)session.CreateCriteria(typeof(AccountFlagType))
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
    }
}

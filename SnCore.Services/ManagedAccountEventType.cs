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
            base.SetInstance(instance);
        }

        public override AccountEventType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEventType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedAccountEventType : ManagedService<AccountEventType, TransitAccountEventType>
    {
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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

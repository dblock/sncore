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
    public class TransitAccountPropertyGroup : TransitService<AccountPropertyGroup>
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

        public TransitAccountPropertyGroup()
        {

        }

        public TransitAccountPropertyGroup(AccountPropertyGroup value)
            : base(value)
        {

        }

        public override void SetInstance(AccountPropertyGroup value)
        {
            Name = value.Name;
            Description = value.Description;
            base.SetInstance(value);
        }

        public override AccountPropertyGroup GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountPropertyGroup instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedAccountPropertyGroup : ManagedService<AccountPropertyGroup, TransitAccountPropertyGroup>
    {
        public ManagedAccountPropertyGroup()
        {

        }

        public ManagedAccountPropertyGroup(ISession session)
            : base(session)
        {

        }

        public ManagedAccountPropertyGroup(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountPropertyGroup(ISession session, AccountPropertyGroup value)
            : base(session, value)
        {

        }

        public static AccountPropertyGroup Find(ISession session, string name)
        {
            return (AccountPropertyGroup)session.CreateCriteria(typeof(AccountPropertyGroup))
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

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
    public class TransitBugType : TransitService<BugType>
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

        public TransitBugType()
        {

        }

        public TransitBugType(BugType value)
            : base(value)
        {

        }

        public override void SetInstance(BugType instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override BugType GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugType instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedBugType : ManagedService<BugType, TransitBugType>
    {
        public ManagedBugType()
        {

        }

        public ManagedBugType(ISession session)
            : base(session)
        {

        }

        public ManagedBugType(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugType(ISession session, BugType value)
            : base(session, value)
        {

        }

        public static BugType Find(ISession session, string name)
        {
            return (BugType)session.CreateCriteria(typeof(BugType))
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

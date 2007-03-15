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
    public class TransitBugSeverity : TransitService<BugSeverity>
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

        public TransitBugSeverity()
        {

        }

        public TransitBugSeverity(BugSeverity value)
            : base(value)
        {

        }

        public override void SetInstance(BugSeverity value)
        {
            Name = value.Name;
            base.SetInstance(value);
        }

        public override BugSeverity GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugSeverity instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedBugSeverity : ManagedService<BugSeverity, TransitBugSeverity>
    {
        public ManagedBugSeverity()
        {

        }

        public ManagedBugSeverity(ISession session)
            : base(session)
        {

        }

        public ManagedBugSeverity(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugSeverity(ISession session, BugSeverity value)
            : base(session, value)
        {

        }

        public static BugSeverity Find(ISession session, string name)
        {
            return (BugSeverity)session.CreateCriteria(typeof(BugSeverity))
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

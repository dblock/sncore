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
    public class TransitBugPriority : TransitService<BugPriority>
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

        public TransitBugPriority()
        {

        }

        public TransitBugPriority(BugPriority value)
            : base(value)
        {

        }

        public override void SetInstance(BugPriority value)
        {
            Name = value.Name;
            base.SetInstance(value);
        }

        public override BugPriority GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugPriority instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedBugPriority : ManagedService<BugPriority, TransitBugPriority>
    {
        public ManagedBugPriority()
        {

        }

        public ManagedBugPriority(ISession session)
            : base(session)
        {

        }

        public ManagedBugPriority(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugPriority(ISession session, BugPriority value)
            : base(session, value)
        {

        }

        public static BugPriority Find(ISession session, string name)
        {
            return (BugPriority)session.CreateCriteria(typeof(BugPriority))
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

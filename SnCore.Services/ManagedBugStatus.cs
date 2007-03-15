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
    public class TransitBugStatus : TransitService<BugStatu>
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

        public TransitBugStatus()
        {

        }

        public TransitBugStatus(BugStatu value)
            : base(value)
        {

        }

        public override void SetInstance(BugStatu instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override BugStatu GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugStatu instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedBugStatus : ManagedService<BugStatu, TransitBugStatus>
    {
        public ManagedBugStatus()
        {

        }

        public ManagedBugStatus(ISession session)
            : base(session)
        {

        }

        public ManagedBugStatus(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugStatus(ISession session, BugStatu value)
            : base(session, value)
        {

        }

        public static BugStatu Find(ISession session, string name)
        {
            return (BugStatu)session.CreateCriteria(typeof(BugStatu))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            BugStatu status = Find(session, name);
            return status == null ? 0 : status.Id;
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

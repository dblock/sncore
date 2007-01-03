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
    public class TransitBugResolution : TransitService<BugResolution>
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

        public TransitBugResolution()
        {

        }

        public TransitBugResolution(BugResolution instance)
            : base(instance)
        {

        }

        public override void SetInstance(BugResolution instance)
        {
            Name = instance.Name;
            base.SetInstance(instance);
        }

        public override BugResolution GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugResolution instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            return instance;
        }
    }

    public class ManagedBugResolution : ManagedService<BugResolution, TransitBugResolution>
    {
        public ManagedBugResolution(ISession session)
            : base(session)
        {

        }

        public ManagedBugResolution(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugResolution(ISession session, BugResolution instance)
            : base(session, instance)
        {

        }

        public static BugResolution Find(ISession session, string name)
        {
            return (BugResolution)session.CreateCriteria(typeof(BugResolution))
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

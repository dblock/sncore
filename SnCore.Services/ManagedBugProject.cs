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
    public class TransitBugProject : TransitService<BugProject>
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

        private DateTime mCreated;

        public DateTime Created
        {
            get
            {

                return mCreated;
            }
            set
            {
                mCreated = value;
            }
        }

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        public TransitBugProject()
        {

        }

        public TransitBugProject(BugProject value)
            : base(value)
        {

        }

        public override void SetInstance(BugProject value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override BugProject GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugProject instance = base.GetInstance(session, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedBugProject : ManagedService<BugProject, TransitBugProject>
    {
        public ManagedBugProject()
        {

        }

        public ManagedBugProject(ISession session)
            : base(session)
        {

        }

        public ManagedBugProject(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugProject(ISession session, BugProject value)
            : base(session, value)
        {

        }

        public static BugProject Find(ISession session, string name)
        {
            return (BugProject)session.CreateCriteria(typeof(BugProject))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

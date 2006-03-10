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
    public class TransitBugProject : TransitService
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

        public TransitBugProject(BugProject o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
        }

        public BugProject GetBugProject(ISession session)
        {
            BugProject p = (Id != 0) ? (BugProject)session.Load(typeof(BugProject), Id) : new BugProject();
            p.Name = this.Name;
            p.Description = this.Description;
            return p;
        }
    }

    /// <summary>
    /// Managed bug project.
    /// </summary>
    public class ManagedBugProject : ManagedService
    {
        private BugProject mBugProject = null;

        public ManagedBugProject(ISession session)
            : base(session)
        {

        }

        public ManagedBugProject(ISession session, int id)
            : base(session)
        {
            mBugProject = (BugProject)session.Load(typeof(BugProject), id);
        }

        public ManagedBugProject(ISession session, BugProject value)
            : base(session)
        {
            mBugProject = value;
        }

        public ManagedBugProject(ISession session, TransitBugProject value)
            : base(session)
        {
            mBugProject = value.GetBugProject(session);
        }

        public int Id
        {
            get
            {
                return mBugProject.Id;
            }
        }

        public TransitBugProject TransitBugProject
        {
            get
            {
                return new TransitBugProject(mBugProject);
            }
        }

        public void CreateOrUpdate(TransitBugProject o)
        {
            mBugProject = o.GetBugProject(Session);
            mBugProject.Modified = DateTime.UtcNow;
            if (Id == 0) mBugProject.Created = mBugProject.Modified;
            Session.Save(mBugProject);
        }

        public void Delete()
        {
            Session.Delete(mBugProject);
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

    }
}

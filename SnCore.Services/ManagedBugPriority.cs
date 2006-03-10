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
    public class TransitBugPriority : TransitService
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

        public TransitBugPriority(BugPriority o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public BugPriority GetBugPriority(ISession session)
        {
            BugPriority p = (Id != 0) ? (BugPriority)session.Load(typeof(BugPriority), Id) : new BugPriority();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Priority.
    /// </summary>
    public class ManagedBugPriority : ManagedService
    {
        private BugPriority mBugPriority = null;

        public ManagedBugPriority(ISession session)
            : base(session)
        {

        }

        public ManagedBugPriority(ISession session, int id)
            : base(session)
        {
            mBugPriority = (BugPriority)session.Load(typeof(BugPriority), id);
        }

        public ManagedBugPriority(ISession session, BugPriority value)
            : base(session)
        {
            mBugPriority = value;
        }

        public ManagedBugPriority(ISession session, TransitBugPriority value)
            : base(session)
        {
            mBugPriority = value.GetBugPriority(session);
        }

        public int Id
        {
            get
            {
                return mBugPriority.Id;
            }
        }

        public TransitBugPriority TransitBugPriority
        {
            get
            {
                return new TransitBugPriority(mBugPriority);
            }
        }

        public void CreateOrUpdate(TransitBugPriority o)
        {
            mBugPriority = o.GetBugPriority(Session);
            Session.Save(mBugPriority);
        }

        public void Delete()
        {
            Session.Delete(mBugPriority);
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
    }
}

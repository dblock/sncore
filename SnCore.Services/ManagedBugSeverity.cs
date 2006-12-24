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
    public class TransitBugSeverity : TransitService
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

        public TransitBugSeverity(BugSeverity o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public BugSeverity GetBugSeverity(ISession session)
        {
            BugSeverity p = (Id != 0) ? (BugSeverity)session.Load(typeof(BugSeverity), Id) : new BugSeverity();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Severity.
    /// </summary>
    public class ManagedBugSeverity : ManagedService<BugSeverity>
    {
        private BugSeverity mBugSeverity = null;

        public ManagedBugSeverity(ISession session)
            : base(session)
        {

        }

        public ManagedBugSeverity(ISession session, int id)
            : base(session)
        {
            mBugSeverity = (BugSeverity)session.Load(typeof(BugSeverity), id);
        }

        public ManagedBugSeverity(ISession session, BugSeverity value)
            : base(session)
        {
            mBugSeverity = value;
        }

        public ManagedBugSeverity(ISession session, TransitBugSeverity value)
            : base(session)
        {
            mBugSeverity = value.GetBugSeverity(session);
        }

        public int Id
        {
            get
            {
                return mBugSeverity.Id;
            }
        }

        public TransitBugSeverity TransitBugSeverity
        {
            get
            {
                return new TransitBugSeverity(mBugSeverity);
            }
        }

        public void CreateOrUpdate(TransitBugSeverity o)
        {
            mBugSeverity = o.GetBugSeverity(Session);
            Session.Save(mBugSeverity);
        }

        public void Delete()
        {
            Session.Delete(mBugSeverity);
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

    }
}

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
    public class TransitBugResolution : TransitService
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

        public TransitBugResolution(BugResolution o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public BugResolution GetBugResolution(ISession session)
        {
            BugResolution p = (Id != 0) ? (BugResolution)session.Load(typeof(BugResolution), Id) : new BugResolution();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Resolution.
    /// </summary>
    public class ManagedBugResolution : ManagedService
    {
        private BugResolution mBugResolution = null;

        public ManagedBugResolution(ISession session)
            : base(session)
        {

        }

        public ManagedBugResolution(ISession session, int id)
            : base(session)
        {
            mBugResolution = (BugResolution)session.Load(typeof(BugResolution), id);
        }

        public ManagedBugResolution(ISession session, BugResolution value)
            : base(session)
        {
            mBugResolution = value;
        }

        public ManagedBugResolution(ISession session, TransitBugResolution value)
            : base(session)
        {
            mBugResolution = value.GetBugResolution(session);
        }

        public int Id
        {
            get
            {
                return mBugResolution.Id;
            }
        }

        public TransitBugResolution TransitBugResolution
        {
            get
            {
                return new TransitBugResolution(mBugResolution);
            }
        }

        public void CreateOrUpdate(TransitBugResolution o)
        {
            mBugResolution = o.GetBugResolution(Session);
            Session.Save(mBugResolution);
        }

        public void Delete()
        {
            Session.Delete(mBugResolution);
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

    }
}

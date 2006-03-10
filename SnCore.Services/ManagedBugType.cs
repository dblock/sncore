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
    public class TransitBugType : TransitService
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

        public TransitBugType(BugType o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public BugType GetBugType(ISession session)
        {
            BugType p = (Id != 0) ? (BugType)session.Load(typeof(BugType), Id) : new BugType();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Type.
    /// </summary>
    public class ManagedBugType : ManagedService
    {
        private BugType mBugType = null;

        public ManagedBugType(ISession session)
            : base(session)
        {

        }

        public ManagedBugType(ISession session, int id)
            : base(session)
        {
            mBugType = (BugType)session.Load(typeof(BugType), id);
        }

        public ManagedBugType(ISession session, BugType value)
            : base(session)
        {
            mBugType = value;
        }

        public ManagedBugType(ISession session, TransitBugType value)
            : base(session)
        {
            mBugType = value.GetBugType(session);
        }

        public int Id
        {
            get
            {
                return mBugType.Id;
            }
        }

        public TransitBugType TransitBugType
        {
            get
            {
                return new TransitBugType(mBugType);
            }
        }

        public void CreateOrUpdate(TransitBugType o)
        {
            mBugType = o.GetBugType(Session);
            Session.Save(mBugType);
        }

        public void Delete()
        {
            Session.Delete(mBugType);
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
    }
}

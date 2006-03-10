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
    public class TransitBugStatus : TransitService
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

        public TransitBugStatus(BugStatu o)
            : base(o.Id)
        {
            Name = o.Name;
        }

        public BugStatu GetBugStatus(ISession session)
        {
            BugStatu p = (Id != 0) ? (BugStatu)session.Load(typeof(BugStatu), Id) : new BugStatu();
            p.Name = this.Name;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Status.
    /// </summary>
    public class ManagedBugStatus : ManagedService
    {
        private BugStatu mBugStatus = null;

        public ManagedBugStatus(ISession session)
            : base(session)
        {

        }

        public ManagedBugStatus(ISession session, int id)
            : base(session)
        {
            mBugStatus = (BugStatu)session.Load(typeof(BugStatu), id);
        }

        public ManagedBugStatus(ISession session, BugStatu value)
            : base(session)
        {
            mBugStatus = value;
        }

        public ManagedBugStatus(ISession session, TransitBugStatus value)
            : base(session)
        {
            mBugStatus = value.GetBugStatus(session);
        }

        public int Id
        {
            get
            {
                return mBugStatus.Id;
            }
        }

        public TransitBugStatus TransitBugStatus
        {
            get
            {
                return new TransitBugStatus(mBugStatus);
            }
        }

        public void CreateOrUpdate(TransitBugStatus o)
        {
            mBugStatus = o.GetBugStatus(Session);
            Session.Save(mBugStatus);
        }

        public void Delete()
        {
            Session.Delete(mBugStatus);
        }

        public static BugStatu Find(ISession session, string name)
        {
            return (BugStatu)session.CreateCriteria(typeof(BugStatu))
                .Add(Expression.Eq("Name", name))
                .UniqueResult();
        }

        public static int FindId(ISession session, string name)
        {
            return Find(session, name).Id;
        }

    }
}

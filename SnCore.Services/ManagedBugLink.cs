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
using System.Collections.Generic;

namespace SnCore.Services
{
    public class TransitBugLink : TransitService
    {
        private int mBugId;

        public int BugId
        {
            get
            {

                return mBugId;
            }
            set
            {
                mBugId = value;
            }
        }

        private int mRelatedBugId;

        public int RelatedBugId
        {
            get
            {

                return mRelatedBugId;
            }
            set
            {
                mRelatedBugId = value;
            }
        }

        public TransitBugLink()
        {

        }

        public TransitBugLink(BugLink o)
            : base(o.Id)
        {
            BugId = o.Bug.Id;
            RelatedBugId = o.RelatedBug.Id;
        }

        public BugLink GetBugLink(ISession session)
        {
            BugLink p = (Id != 0) ? (BugLink)session.Load(typeof(BugLink), Id) : new BugLink();
            if (Id == 0)
            {
                p.Bug = (Bug)session.Load(typeof(Bug), this.BugId);
                p.RelatedBug = (Bug)session.Load(typeof(Bug), this.RelatedBugId);
            }
            return p;
        }
    }

    /// <summary>
    /// Managed bug Link.
    /// </summary>
    public class ManagedBugLink : ManagedService
    {
        private BugLink mBugLink = null;

        public ManagedBugLink(ISession session)
            : base(session)
        {

        }

        public ManagedBugLink(ISession session, int id)
            : base(session)
        {
            mBugLink = (BugLink)session.Load(typeof(BugLink), id);
        }

        public ManagedBugLink(ISession session, BugLink value)
            : base(session)
        {
            mBugLink = value;
        }

        public ManagedBugLink(ISession session, TransitBugLink value)
            : base(session)
        {
            mBugLink = value.GetBugLink(session);
        }

        public int Id
        {
            get
            {
                return mBugLink.Id;
            }
        }

        public TransitBugLink TransitBugLink
        {
            get
            {
                return new TransitBugLink(mBugLink);
            }
        }

        public void CreateOrUpdate(TransitBugLink o)
        {
            mBugLink = o.GetBugLink(Session);
            Session.Save(mBugLink);

            if (mBugLink.Bug.BugLinks == null)
                mBugLink.Bug.BugLinks = new List<BugLink>();
            mBugLink.Bug.BugLinks.Add(mBugLink);
        }

        public void Delete()
        {
            Session.Delete(mBugLink);
        }
    }
}

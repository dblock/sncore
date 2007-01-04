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
    public class TransitBugLink : TransitService<BugLink>
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

        public TransitBugLink(BugLink value)
            : base(value)
        {
        }

        public override void SetInstance(BugLink value)
        {
            BugId = value.Bug.Id;
            RelatedBugId = value.RelatedBug.Id;
            base.SetInstance(value);
        }

        public override BugLink GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugLink instance = base.GetInstance(session, sec);
            if (Id == 0)
            {
                instance.Bug = (Bug)session.Load(typeof(Bug), this.BugId);
                instance.RelatedBug = (Bug)session.Load(typeof(Bug), this.RelatedBugId);
            }
            return instance;
        }
    }

    public class ManagedBugLink : ManagedService<BugLink, TransitBugLink>
    {
        public ManagedBugLink()
        {

        }

        public ManagedBugLink(ISession session)
            : base(session)
        {

        }

        public ManagedBugLink(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugLink(ISession session, BugLink value)
            : base(session, value)
        {

        }

        public override int CreateOrUpdate(TransitBugLink instance, ManagedSecurityContext sec)
        {
            base.CreateOrUpdate(instance, sec);

            if (mInstance.Bug.BugLinks == null)
            {
                mInstance.Bug.BugLinks = new List<BugLink>();
            }

            mInstance.Bug.BugLinks.Add(mInstance);
            return mInstance.Id;
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            return acl;
        }
    }
}

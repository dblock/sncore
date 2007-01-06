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
    public class TransitBugNote : TransitService<BugNote>
    {
        private string mDetails;

        public string Details
        {
            get
            {

                return mDetails;
            }
            set
            {
                mDetails = value;
            }
        }

        private int mAccountId;

        public int AccountId
        {
            get
            {

                return mAccountId;
            }
            set
            {
                mAccountId = value;
            }
        }

        private string mAccountName;

        public string AccountName
        {
            get
            {

                return mAccountName;
            }
            set
            {
                mAccountName = value;
            }
        }

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

        public TransitBugNote()
        {

        }

        public TransitBugNote(BugNote instance)
            : base(instance)
        {

        }

        public TransitBugNote(ISession session, BugNote instance)
            : base(instance)
        {
            try
            {
                Account account = (Account)session.Load(typeof(Account), instance.AccountId);
                AccountName = (account != null) ? account.Name : string.Empty;
            }
            catch (NHibernate.ObjectNotFoundException)
            {
                AccountName = "Unknown";
                AccountId = -1;
            }
        }

        public override void SetInstance(BugNote instance)
        {
            Details = instance.Details;
            AccountId = instance.AccountId;
            BugId = instance.Bug.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override BugNote GetInstance(ISession session, ManagedSecurityContext sec)
        {
            BugNote instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Bug = (Id == 0 && BugId != 0) ? (Bug)session.Load(typeof(Bug), this.BugId) : null;
                instance.AccountId = this.AccountId;
            }

            instance.Details = this.Details;
            return instance;
        }
    }

    public class ManagedBugNote : ManagedService<BugNote, TransitBugNote>
    {
        public ManagedBugNote()
        {

        }

        public ManagedBugNote(ISession session)
            : base(session)
        {

        }

        public ManagedBugNote(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBugNote(ISession session, BugNote value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            return acl;
        }
    }
}

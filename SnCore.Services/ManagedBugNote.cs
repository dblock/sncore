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
    public class TransitBugNote : TransitService
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

        public TransitBugNote(ISession session, BugNote o)
            : base(o.Id)
        {
            Details = o.Details;
            AccountId = o.AccountId;
            try
            {
                Account account = (Account)session.Load(typeof(Account), o.AccountId);
                AccountName = (account != null) ? account.Name : string.Empty;
            }
            catch (NHibernate.ObjectNotFoundException)
            {
                AccountName = "Unknown";
            }
            BugId = o.Bug.Id;
            Created = o.Created;
            Modified = o.Modified;
        }

        public BugNote GetBugNote(ISession session)
        {
            BugNote p = (Id != 0) ? (BugNote)session.Load(typeof(BugNote), Id) : new BugNote();

            if (Id == 0)
            {
                p.Bug = (Id == 0 && BugId != 0) ? (Bug)session.Load(typeof(Bug), this.BugId) : null;
                p.AccountId = this.AccountId;
            }

            p.Details = this.Details;
            return p;
        }
    }

    /// <summary>
    /// Managed bug Note.
    /// </summary>
    public class ManagedBugNote : ManagedService
    {
        private BugNote mBugNote = null;

        public ManagedBugNote(ISession session)
            : base(session)
        {

        }

        public ManagedBugNote(ISession session, int id)
            : base(session)
        {
            mBugNote = (BugNote)session.Load(typeof(BugNote), id);
        }

        public ManagedBugNote(ISession session, BugNote value)
            : base(session)
        {
            mBugNote = value;
        }

        public ManagedBugNote(ISession session, TransitBugNote value)
            : base(session)
        {
            mBugNote = value.GetBugNote(session);
        }

        public int Id
        {
            get
            {
                return mBugNote.Id;
            }
        }

        public TransitBugNote TransitBugNote
        {
            get
            {
                return new TransitBugNote(Session, mBugNote);
            }
        }

        public void CreateOrUpdate(TransitBugNote o)
        {
            mBugNote = o.GetBugNote(Session);
            mBugNote.Modified = DateTime.UtcNow;
            if (Id == 0) mBugNote.Created = mBugNote.Modified;
            Session.Save(mBugNote);
        }

        public void Delete()
        {
            Session.Delete(mBugNote);
        }
    }
}

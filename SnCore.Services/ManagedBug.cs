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
using SnCore.Tools.Web;
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitBugQueryOptions
    {
        public string SortExpression = "Created";
        public TransitSortDirection SortDirection = TransitSortDirection.Ascending;
        
        public bool Resolved = false;
        public bool Closed = false;
        public bool Open = true;

        public TransitBugQueryOptions()
        {
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitBug : TransitService
    {
        private string mSubject;

        public string Subject
        {
            get
            {

                return mSubject;
            }
            set
            {
                mSubject = value;
            }
        }

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

        private DateTime mUpdated;

        public DateTime Updated
        {
            get
            {

                return mUpdated;
            }
            set
            {
                mUpdated = value;
            }
        }

        private string mStatus;

        public string Status
        {
            get
            {

                return mStatus;
            }
            set
            {
                mStatus = value;
            }
        }

        private string mType;

        public string Type
        {
            get
            {

                return mType;
            }
            set
            {
                mType = value;
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

        private string mPriority;

        public string Priority
        {
            get
            {

                return mPriority;
            }
            set
            {
                mPriority = value;
            }
        }

        private string mSeverity;

        public string Severity
        {
            get
            {

                return mSeverity;
            }
            set
            {
                mSeverity = value;
            }
        }

        private string mResolution;

        public string Resolution
        {
            get
            {

                return mResolution;
            }
            set
            {
                mResolution = value;
            }
        }

        private int mProjectId;

        public int ProjectId
        {
            get
            {

                return mProjectId;
            }
            set
            {
                mProjectId = value;
            }
        }

        public TransitBug()
        {

        }

        public TransitBug(Bug o)
            : base(o.Id)
        {
            Subject = o.Subject;
            Details = o.Details;
            Created = o.Created;
            Updated = o.Updated;
            Status = o.Status.Name;
            Type = o.Type.Name;
            AccountId = o.AccountId;
            Priority = o.Priority.Name;
            Severity = o.Severity.Name;
            Resolution = o.Resolution.Name;
            ProjectId = o.Project.Id;
        }

        public Bug GetBug(ISession session)
        {
            Bug p = (Id != 0) ? (Bug)session.Load(typeof(Bug), Id) : new Bug();

            if (Id == 0)
            {
                p.AccountId = this.AccountId;
                p.Project = (BugProject)session.Load(typeof(BugProject), this.ProjectId);
            }

            p.Subject = this.Subject;
            p.Details = this.Details;
            if (!string.IsNullOrEmpty(this.Status)) p.Status = ManagedBugStatus.Find(session, this.Status);
            if (p.Status == null) p.Status = (BugStatu)session.Load(typeof(BugStatu), ManagedBugStatus.FindId(session, "Open"));
            if (!string.IsNullOrEmpty(this.Type)) p.Type = ManagedBugType.Find(session, this.Type);
            if (!string.IsNullOrEmpty(this.Priority)) p.Priority = ManagedBugPriority.Find(session, this.Priority);
            if (!string.IsNullOrEmpty(this.Severity)) p.Severity = ManagedBugSeverity.Find(session, this.Severity);
            if (!string.IsNullOrEmpty(this.Resolution)) p.Resolution = ManagedBugResolution.Find(session, this.Resolution);
            if (p.Resolution == null) p.Resolution = (BugResolution)session.Load(typeof(BugResolution), ManagedBugResolution.FindId(session, "None"));
            return p;
        }
    }

    /// <summary>
    /// Managed bug.
    /// </summary>
    public class ManagedBug : ManagedService
    {
        private Bug mBug = null;

        public ManagedBug(ISession session)
            : base(session)
        {

        }

        public ManagedBug(ISession session, int id)
            : base(session)
        {
            mBug = (Bug)session.Load(typeof(Bug), id);
        }

        public ManagedBug(ISession session, Bug value)
            : base(session)
        {
            mBug = value;
        }

        public ManagedBug(ISession session, TransitBug value)
            : base(session)
        {
            mBug = value.GetBug(session);
        }

        public int Id
        {
            get
            {
                return mBug.Id;
            }
        }

        public TransitBug TransitBug
        {
            get
            {
                TransitBug bug = new TransitBug(mBug);

                try
                {
                    Account account = (Account)Session.Load(typeof(Account), bug.AccountId);
                    bug.AccountName = (account != null) ? account.Name : string.Empty;
                }
                catch (NHibernate.ObjectNotFoundException)
                {
                    bug.AccountName = "Unknown";
                }

                return bug;
            }
        }

        public void CreateOrUpdate(TransitBug o)
        {
            mBug = o.GetBug(Session);
            mBug.Updated = DateTime.UtcNow;
            if (Id == 0) mBug.Created = mBug.Updated;
            Session.Save(mBug);
        }

        public void Resolve(string resolution)
        {
            switch (mBug.Status.Name)
            {
                case "Resolved":
                case "Closed":
                    throw new InvalidOperationException();
            }

            mBug.Status = ManagedBugStatus.Find(Session, "Resolved");
            mBug.Resolution = ManagedBugResolution.Find(Session, resolution);
            mBug.Updated = DateTime.UtcNow;
            Session.Save(mBug);
        }

        public void Close()
        {
            switch (mBug.Status.Name)
            {
                case "Resolved":
                    break;
                default:
                    throw new InvalidOperationException();
            }

            mBug.Status = ManagedBugStatus.Find(Session, "Closed");
            mBug.Updated = DateTime.UtcNow;
            Session.Save(mBug);

            // notify that the bug has been closed

            try
            {
                ManagedAccount acct = new ManagedAccount(Session, mBug.AccountId);

                string url = string.Format(
                    "{0}/BugView.aspx?id={1}",
                    ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                    mBug.Id);

                string messagebody =
                    "<html>" +
                    "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                    "<body>" +
                    "Dear " + Renderer.Render(acct.Name) + ",<br>" +
                    "<br>The " + mBug.Type.Name.ToLower() + " #" + mBug.Id.ToString() + " that you have filed was closed." +
                    "<br>Thank you for submitting this " + mBug.Type.Name.ToLower() + "." +
                    "<br>Your feedback is greatly appreciated!" +
                    "<br><br>" +
                    "<blockquote>" +
                    "<a href=\"" + url + "\">View</a> this " + mBug.Type.Name.ToLower() + "." +
                    "</blockquote>" +
                    "</body>" +
                    "</html>";

                acct.SendAccountMailMessage(
                    ManagedConfiguration.GetValue(Session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                    acct.ActiveEmailAddress,
                    string.Format("{0}: {1} #{2} has been closed.",
                        ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                        mBug.Type.Name, mBug.Id),
                    messagebody,
                    true);
            }
            catch
            {
            }
        }

        public void Reopen()
        {
            switch (mBug.Status.Name)
            {
                case "Resolved":
                case "Closed":
                    break;
                default:
                    throw new InvalidOperationException();
            }

            mBug.Status = ManagedBugStatus.Find(Session, "Reopened");
            mBug.Updated = DateTime.UtcNow;
            Session.Save(mBug);
        }

        public void Delete()
        {
            Session.Delete(
                string.Format("FROM BugLink b WHERE b.Bug.Id = {0} OR b.RelatedBug.Id = {0}",
                    Id));

            mBug.BugLinks = null;
            Session.Delete(mBug);
        }

        public void LinkTo(ManagedBug bug)
        {
            TransitBugLink t_link = new TransitBugLink();
            t_link.BugId = Id;
            t_link.RelatedBugId = bug.Id;

            ManagedBugLink link = new ManagedBugLink(Session);
            link.CreateOrUpdate(t_link);
        }
    }
}

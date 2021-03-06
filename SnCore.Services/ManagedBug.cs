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

        private bool mResolved = false;

        public bool Resolved
        {
            get
            {
                return mResolved;
            }
            set
            {
                mResolved = value;
            }
        }

        private bool mClosed = false;

        public bool Closed
        {
            get
            {
                return mClosed;
            }
            set
            {
                mClosed = value;
            }
        }

        private bool mOpen = true;

        public bool Open
        {
            get
            {
                return mOpen;
            }
            set
            {
                mOpen = value;
            }
        }

        private string mSearchQuery;

        public string SearchQuery
        {
            get
            {
                return mSearchQuery;
            }
            set
            {
                mSearchQuery = value;
            }
        }

        public TransitBugQueryOptions()
        {

        }

        public string GetQuery(ISession session)
        {
            StringBuilder s_query = new StringBuilder();
            s_query.AppendLine("SELECT {Bug.*} FROM Bug {Bug}");

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                int maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
                s_query.AppendFormat("\nINNER JOIN FREETEXTTABLE(Bug, ([Subject], [Details]), " +
                    "'{0}', {1}) AS ft ON Bug.Bug_Id = ft.[KEY] ",
                    Renderer.SqlEncode(SearchQuery), maxsearchresults);
            }

            s_query.AppendFormat(" WHERE Bug.Project_Id = {0}", ProjectId);

            if (!Resolved)
            {
                s_query.AppendFormat(" AND NOT Bug.Status_Id = {0}", ManagedBugStatus.FindId(session, "Resolved"));
            }

            if (!Closed)
            {
                s_query.AppendFormat(" AND NOT Bug.Status_Id = {0}", ManagedBugStatus.FindId(session, "Closed"));
            }

            if (!Open)
            {
                s_query.AppendFormat(" AND NOT Bug.Status_Id = {0}", ManagedBugStatus.FindId(session, "Open"));
                s_query.AppendFormat(" AND NOT Bug.Status_Id = {0}", ManagedBugStatus.FindId(session, "Reopened"));
            }

            if (string.IsNullOrEmpty(SearchQuery) && !string.IsNullOrEmpty(SortExpression))
            {
                s_query.AppendFormat(" ORDER BY Bug.{0} {1}",
                    Renderer.SqlEncode(SortExpression),
                    (SortDirection == TransitSortDirection.Ascending) ? "ASC" : "DESC");
            }

            return s_query.ToString();
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitBug : TransitService<Bug>
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

        private string mProjectName;

        public string ProjectName
        {
            get
            {

                return mProjectName;
            }
            set
            {
                mProjectName = value;
            }
        }

        public TransitBug()
        {

        }

        public TransitBug(Bug instance)
            : base(instance)
        {

        }

        public override void SetInstance(Bug instance)
        {
            Subject = instance.Subject;
            Details = instance.Details;
            Created = instance.Created;
            Updated = instance.Updated;
            Status = instance.Status.Name;
            Type = instance.Type.Name;
            AccountId = instance.AccountId;
            Priority = instance.Priority.Name;
            Severity = instance.Severity.Name;
            Resolution = instance.Resolution.Name;
            ProjectId = instance.Project.Id;
            ProjectName = instance.Project.Name;
            base.SetInstance(instance);
        }

        public override Bug GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Bug instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.AccountId = GetOwner(session, AccountId, sec).Id;
                instance.Project = session.Load<BugProject>(this.ProjectId);
            }

            instance.Subject = this.Subject;
            instance.Details = this.Details;
            if (!string.IsNullOrEmpty(this.Status)) instance.Status = ManagedBugStatus.Find(session, this.Status);
            if (instance.Status == null) instance.Status = session.Load<BugStatu>(ManagedBugStatus.FindId(session, "Open"));
            if (!string.IsNullOrEmpty(this.Type)) instance.Type = ManagedBugType.Find(session, this.Type);
            if (!string.IsNullOrEmpty(this.Priority)) instance.Priority = ManagedBugPriority.Find(session, this.Priority);
            if (!string.IsNullOrEmpty(this.Severity)) instance.Severity = ManagedBugSeverity.Find(session, this.Severity);
            if (!string.IsNullOrEmpty(this.Resolution)) instance.Resolution = ManagedBugResolution.Find(session, this.Resolution);
            if (instance.Resolution == null) instance.Resolution = session.Load<BugResolution>(ManagedBugResolution.FindId(session, "None"));
            return instance;
        }
    }

    public class ManagedBug : ManagedService<Bug, TransitBug>
    {
        public ManagedBug()
        {

        }

        public ManagedBug(ISession session)
            : base(session)
        {

        }

        public ManagedBug(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedBug(ISession session, Bug value)
            : base(session, value)
        {

        }

        public void Resolve(string resolution, ManagedSecurityContext sec)
        {
            switch (mInstance.Status.Name)
            {
                case "Resolved":
                case "Closed":
                    throw new InvalidOperationException();
            }

            mInstance.Status = ManagedBugStatus.Find(Session, "Resolved");
            mInstance.Resolution = ManagedBugResolution.Find(Session, resolution);
            Save(sec);
        }

        public void Close(ManagedSecurityContext sec)
        {
            switch (mInstance.Status.Name)
            {
                case "Resolved":
                    break;
                default:
                    throw new InvalidOperationException();
            }

            mInstance.Status = ManagedBugStatus.Find(Session, "Closed");
            Save(sec);

            // notify that the bug has been closed, account may not exist any more

            Session.Flush();

            ManagedAccount acct = new ManagedAccount(Session, mInstance.AccountId);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, acct,
                string.Format("EmailBugClosed.aspx?id={0}", mInstance.Id));
        }

        public void Reopen(ManagedSecurityContext sec)
        {
            switch (mInstance.Status.Name)
            {
                case "Resolved":
                case "Closed":
                    break;
                default:
                    throw new InvalidOperationException();
            }

            mInstance.Status = ManagedBugStatus.Find(Session, "Reopened");
            Save(sec);
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Session.Delete(string.Format("FROM BugLink b WHERE b.Bug.Id = {0} OR b.RelatedBug.Id = {0}", Id));
            mInstance.BugLinks = null;
            base.Delete(sec);
        }

        public void LinkTo(ManagedBug bug, ManagedSecurityContext sec)
        {
            TransitBugLink t_link = new TransitBugLink();
            t_link.BugId = Id;
            t_link.RelatedBugId = bug.Id;

            ManagedBugLink link = new ManagedBugLink(Session);
            link.CreateOrUpdate(t_link, sec);
        }

        public override int CreateOrUpdate(TransitBug t_instance, ManagedSecurityContext sec)
        {
            base.CreateOrUpdate(t_instance, sec);

            if (t_instance.Id == 0)
            {
                ManagedAccount admin = new ManagedAccount(Session, ManagedAccount.GetAdminAccount(Session));

                if (admin.Id != mInstance.AccountId)
                {
                    Session.Flush();

                    ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                        Session, admin,
                        string.Format("EmailBugCreated.aspx?id={0}", mInstance.Id));
                }
            }

            return mInstance.Id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Updated = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Updated;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            return acl;
        }

        public override TransitBug GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitBug t_instance = base.GetTransitInstance(sec);
            t_instance.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, t_instance.AccountId);
            return t_instance;
        }
    }
}

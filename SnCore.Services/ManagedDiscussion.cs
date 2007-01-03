using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class DiscussionQueryOptions
    {
        public int AccountId;
        public bool TopOfThRetreiveOnly = true;

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }

        public string CountQuery
        {
            get
            {
                return "SELECT COUNT(*) FROM DiscussionPost tp, DiscussionThread t, Discussion d" +
                       " WHERE tp.AccountId = " + AccountId.ToString() +
                       " AND t.Discussion.Id = instance.Id" +
                       " AND instance.Personal = 0" +
                       " AND t.Id = tp.DiscussionThread.Id" +
                       (TopOfThRetreiveOnly ? " AND tp.DiscussionPostParent IS NULL" : string.Empty);
            }
        }

        public string SelectQuery
        {
            get
            {
                return "SELECT tp FROM DiscussionPost tp, DiscussionThread t, Discussion d" +
                       " WHERE tp.AccountId = " + AccountId.ToString() +
                       " AND t.Discussion.Id = instance.Id" +
                       " AND instance.Personal = 0" +
                       " AND t.Id = tp.DiscussionThread.Id" +
                       (TopOfThRetreiveOnly ? " AND tp.DiscussionPostParent IS NULL" : string.Empty) +
                       " ORDER BY t.Created DESC";
            }
        }
    }

    public class TransitDiscussion : TransitService<Discussion>
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

        private string mDescription;

        public string Description
        {
            get
            {

                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private int mObjectId;

        public int ObjectId
        {
            get
            {

                return mObjectId;
            }
            set
            {
                mObjectId = value;
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

        private bool mPersonal;

        public bool Personal
        {
            get
            {

                return mPersonal;
            }
            set
            {
                mPersonal = value;
            }
        }

        private int mPostCount;

        public int PostCount
        {
            get
            {

                return mPostCount;
            }
            set
            {
                mPostCount = value;
            }
        }

        private long mThRetreiveCount;

        public long ThRetreiveCount
        {
            get
            {

                return mThRetreiveCount;
            }
            set
            {
                mThRetreiveCount = value;
            }
        }

        public TransitDiscussion()
        {

        }

        public TransitDiscussion(Discussion instance)
            : base(instance)
        {

        }

        public TransitDiscussion(ISession session, Discussion instance)
            : base(instance)
        {
            PostCount = ManagedDiscussion.GetDiscussionPostCount(session, instance.Id);
            ThRetreiveCount = ManagedDiscussion.GetDiscussionThreadCount(session, instance.Id);
        }

        public override void SetInstance(Discussion instance)
        {
            Name = instance.Name;
            Description = instance.Description;
            AccountId = instance.Account.Id;
            Created = instance.Created;
            Modified = instance.Modified;
            Personal = instance.Personal;
            ObjectId = instance.ObjectId;
            base.SetInstance(instance);
        }

        public override Discussion GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Discussion instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Personal = this.Personal;
                instance.ObjectId = this.ObjectId;
            }

            instance.Name = this.Name;
            instance.Description = this.Description;
            return instance;
        }
    }

    public class ManagedDiscussion : ManagedService<Discussion, TransitDiscussion>
    {
        public const string AccountPictureDiscussion = "Picture Comments";
        public const string AccountStoryDiscussion = "Story Comments";
        public const string AccountStoryPictureDiscussion = "Story Picture Comments";
        public const string AccountTagsDiscussion = "Testimonials";
        public const string AccountFeedItemDiscussion = "Feed Entry Comments";
        public const string PlaceDiscussion = "Place Comments";
        public const string PlacePictureDiscussion = "Place Picture Comments";
        public const string AccountBlogPostDiscussion = "Blog Post Comments";
        public const string AccountEventDiscussion = "Event Comments";
        public const string AccountEventPictureDiscussion = "Event Picture Comments";

        public class DiscussionNotFoundException : SoapException
        {
            public DiscussionNotFoundException()
                : base("Discussion not found", SoapException.ClientFaultCode)
            {

            }
        }

        public ManagedDiscussion(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussion(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedDiscussion(ISession session, Discussion value)
            : base(session, value)
        {

        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public string Description
        {
            get
            {
                return mInstance.Description;
            }
        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public bool Personal
        {
            get
            {
                return mInstance.Personal;
            }
        }

        public void Create(string name, string description, bool personal, ManagedSecurityContext sec)
        {
            TransitDiscussion t = new TransitDiscussion();
            t.Name = name;
            t.Description = description;
            t.AccountId = sec.Account.Id;
            t.Personal = personal;
            t.Created = t.Modified = DateTime.UtcNow;
            CreateOrUpdate(t, sec);
        }

        public int CreatePost(
            int accountid,
            int parentid,
            string subject,
            string body,
            ManagedSecurityContext sec)
        {
            DiscussionThread thRetreive = null;
            DiscussionPost parent = null;

            if (parentid != 0)
            {
                parent = (DiscussionPost)Session.Load(typeof(DiscussionPost), parentid);
                if (parent == null)
                {
                    throw new ArgumentException();
                }

                thRetreive = parent.DiscussionThread;
                if (thRetreive.Discussion.Id != Id)
                {
                    throw new ArgumentException();
                }

                thRetreive.Modified = DateTime.UtcNow;
                Session.Save(thRetreive);
            }
            else
            {
                thRetreive = new DiscussionThread();
                thRetreive.Created = thRetreive.Modified = DateTime.UtcNow;
                thRetreive.Discussion = mInstance;
                Session.Save(thRetreive);

                if (mInstance.DiscussionThreads == null) mInstance.DiscussionThreads = new List<DiscussionThread>();
                mInstance.DiscussionThreads.Add(thRetreive);
            }

            DiscussionPost result = new DiscussionPost();
            result.AccountId = accountid;
            result.Created = result.Modified = thRetreive.Modified;
            result.DiscussionPostParent = parent;
            result.DiscussionThread = thRetreive;
            result.Subject = subject;
            result.Body = body;
            Session.Save(result);

            mInstance.Modified = DateTime.UtcNow;
            Session.Save(mInstance);

            Session.Flush();

            if (thRetreive.DiscussionPosts == null) thRetreive.DiscussionPosts = new List<DiscussionPost>();
            thRetreive.DiscussionPosts.Add(result);

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, accountid);
                ManagedAccount ma = new ManagedAccount(Session, parent != null ? parent.AccountId : thRetreive.Discussion.Account.Id);

                if (ra.Id != ma.Id)
                {
                    string replyTo = ma.ActiveEmailAddress;
                    if (! string.IsNullOrEmpty(replyTo))
                    {
                        ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                            Session,
                            new MailAddress(replyTo, ma.Name).ToString(),
                            string.Format("EmailDiscussionPost.aspx?id={0}", result.Id));
                    }
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }

            return result.Id;
        }

        public static int GetDiscussionThreadCount(ISession session, int accountid, string name, int objectid)
        {
            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", (objectid == 0) ? null : (object)objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion == null)
                return 0;

            return GetDiscussionThreadCount(session, existingtagdiscussion.Id);
        }

        public static int GetDiscussionPostCount(ISession session, int accountid, string name, int objectid)
        {
            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", (objectid == 0) ? null : (object)objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion == null)
                return 0;

            return GetDiscussionPostCount(session, existingtagdiscussion.Id);
        }

        public static int GetDiscussionPostCount(ISession session, int discussionid)
        {
            return (int) session.CreateQuery(
                string.Format(
                    "SELECT COUNT(*)" +
                    " FROM DiscussionPost p, DiscussionThread t" +
                    " WHERE p.DiscussionThread.Id = t.Id" +
                    " AND t.Discussion.Id = {0}",
                    discussionid)).UniqueResult();
        }

        public static int GetDiscussionThreadCount(ISession session, int discussionid)
        {
            return (int) session.CreateQuery(
                string.Format(
                    "SELECT COUNT(*)" +
                    " FROM DiscussionThread t " +
                    "WHERE t.Discussion.Id = {0}",
                    discussionid)).UniqueResult();
        }

        public static bool FindAndDelete(
            ISession session,
            int accountid,
            string name,
            int objectid)
        {
            Discussion discussion = Find(session, accountid, name, objectid);
            if (discussion != null)
            {
                session.Delete(discussion);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Discussion Find(
            ISession session,
            int accountid,
            string name,
            int objectid)
        {
            return (Discussion)
                session.CreateCriteria(typeof(Discussion))
                    .Add(Expression.Eq("Name", name))
                    .Add(Expression.Eq("Account.Id", accountid))
                    .Add(Expression.Eq("ObjectId", objectid))
                    .Add(Expression.Eq("Personal", true))
                    .UniqueResult();
        }

        public static int GetDiscussionId(
            ISession session, 
            int accountid, 
            string name, 
            int objectid)
        {
            Discussion d = Find(session, accountid, name, objectid);

            if (d == null)
            {
                throw new ManagedDiscussion.DiscussionNotFoundException();
            }

            return d.Id;
        }

        public static int GetOrCreateDiscussionId(
            ISession session, 
            int accountid,
            string name, 
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion d = Find(session, accountid, name, objectid);

            if (d != null)
            {
                return d.Id;
            }

            TransitDiscussion td = new TransitDiscussion();
            td.AccountId = accountid;
            td.Name = name;
            td.Personal = true;
            td.Description = string.Empty;
            td.Created = td.Modified = DateTime.UtcNow;
            td.ObjectId = objectid;

            // creating a discussion that belongs to a different user (commenting on someone's items)
            ManagedDiscussion m_d = new ManagedDiscussion(session);
            ManagedSecurityContext o_sec = new ManagedSecurityContext(session, accountid);
            return m_d.CreateOrUpdate(td, o_sec);
        }

        public void MigrateToAccount(Account newowner)
        {
            mInstance.Account = newowner;
            Session.Save(mInstance);
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
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

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
                return ", DiscussionThread t, Discussion d" +
                       " WHERE DiscussionPost.AccountId = " + AccountId.ToString() +
                       " AND t.Discussion.Id = instance.Id" +
                       " AND instance.Personal = 0" +
                       " AND t.Id = DiscussionPost.DiscussionThread.Id" +
                       (TopOfThRetreiveOnly ? " AND DiscussionPost.DiscussionPostParent IS NULL" : string.Empty);
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

        private long mThreadCount;

        public long ThreadCount
        {
            get
            {

                return mThreadCount;
            }
            set
            {
                mThreadCount = value;
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
            ThreadCount = ManagedDiscussion.GetDiscussionThreadCount(session, instance.Id);
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

        public ManagedDiscussion()
        {

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
            return (int)session.CreateQuery(
                string.Format(
                    "SELECT COUNT(*)" +
                    " FROM DiscussionPost p, DiscussionThread t" +
                    " WHERE p.DiscussionThread.Id = t.Id" +
                    " AND t.Discussion.Id = {0}",
                    discussionid)).UniqueResult();
        }

        public static int GetDiscussionThreadCount(ISession session, int discussionid)
        {
            return (int)session.CreateQuery(
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
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion discussion = Find(session, accountid, name, objectid, sec);
            if (discussion == null)
                return false;
            ManagedDiscussion m_instance = new ManagedDiscussion(session, discussion);
            m_instance.Delete(sec);
            return true;
        }

        public static Discussion Find(
            ISession session,
            int accountid,
            string name,
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion instance = (Discussion)
                session.CreateCriteria(typeof(Discussion))
                    .Add(Expression.Eq("Name", name))
                    .Add(Expression.Eq("Account.Id", accountid))
                    .Add(Expression.Eq("ObjectId", objectid))
                    .Add(Expression.Eq("Personal", true))
                    .UniqueResult();

            if (instance == null)
                return null;

            ManagedDiscussion m_instance = new ManagedDiscussion(session, instance);
            m_instance.GetACL().Check(sec, DataOperation.Retreive);
            return instance;
        }

        public static int GetDiscussionId(
            ISession session,
            int accountid,
            string name,
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion d = Find(session, accountid, name, objectid, sec);

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
            Discussion d = Find(session, accountid, name, objectid, sec);

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

        public List<TransitDiscussionPost> GetDiscussionPosts(ManagedSecurityContext sec)
        {
            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>();
            if (mInstance.DiscussionThreads != null)
            {
                foreach (DiscussionThread thread in mInstance.DiscussionThreads)
                {
                    ManagedDiscussionThread m_thread = new ManagedDiscussionThread(Session, thread);
                    result.AddRange(m_thread.GetDiscussionPosts(sec));
                }
            }
            return result;
        }
    }
}

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
using System.Reflection;

namespace SnCore.Services
{
    public class DiscussionQueryOptions
    {
        public int AccountId;
        public bool TopOfThreadOnly = true;

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
                       " AND t.Discussion.Id = d.Id" +
                       " AND d.Personal = 0" +
                       " AND t.Id = DiscussionPost.DiscussionThread.Id" +
                       (TopOfThreadOnly ? " AND DiscussionPost.DiscussionPostParent IS NULL" : string.Empty);
            }
        }

        public string SelectQuery
        {
            get
            {
                return "SELECT tp FROM DiscussionPost tp, DiscussionThread t, Discussion d" +
                       " WHERE tp.AccountId = " + AccountId.ToString() +
                       " AND t.Discussion.Id = d.Id" +
                       " AND d.Personal = 0" +
                       " AND t.Id = tp.DiscussionThread.Id" +
                       (TopOfThreadOnly ? " AND tp.DiscussionPostParent IS NULL" : string.Empty) +
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

        private int mThreadCount;

        public int ThreadCount
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

        private string mParentObjectName;

        public string ParentObjectName
        {
            get
            {
                return mParentObjectName;
            }
            set
            {
                mParentObjectName = value;
            }
        }

        private string mParentObjectUri;

        public string ParentObjectUri
        {
            get
            {
                return mParentObjectUri;
            }
            set
            {
                mParentObjectUri = value;
            }
        }

        private string mParentObjectType;

        public string ParentObjectType
        {
            get
            {
                return mParentObjectType;
            }
            set
            {
                mParentObjectType = value;
            }
        }

        private string mDefaultView;

        public string DefaultView
        {
            get
            {
                return mDefaultView;
            }
            set
            {
                mDefaultView = value;
            }
        }

        private bool mCanUpdate = false;

        public bool CanUpdate
        {
            get
            {
                return mCanUpdate;
            }
            set
            {
                mCanUpdate = value;
            }
        }

        public TransitDiscussion()
        {

        }

        public TransitDiscussion(Discussion instance)
            : base(instance)
        {

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
            DefaultView = instance.DefaultView;
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
            instance.DefaultView = this.DefaultView;
            return instance;
        }
    }

    public class ManagedDiscussion : ManagedService<Discussion, TransitDiscussion>
    {
        public class DiscussionNotFoundException : Exception
        {
            public DiscussionNotFoundException()
                : base("Discussion not found")
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

        public static int GetDiscussionThreadCount(ISession session, int accountid, Type type, int objectid)
        {
            ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(type);

            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", mapentry.Name))
                .Add(Expression.Eq("Account.Id", accountid))
                .Add(Expression.Eq("ObjectId", (objectid == 0) ? null : (object)objectid))
                .Add(Expression.Eq("Personal", true))
                .UniqueResult();

            if (existingtagdiscussion == null)
                return 0;

            return GetDiscussionThreadCount(session, existingtagdiscussion.Id);
        }

        public static int GetDiscussionPostCount(ISession session, int accountid, Type type, int objectid)
        {
            ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(type);

            Discussion existingtagdiscussion = (Discussion)session.CreateCriteria(typeof(Discussion))
                .Add(Expression.Eq("Name", mapentry.Name))
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
            return session.CreateQuery(
                string.Format(
                    "SELECT COUNT(*)" +
                    " FROM DiscussionPost p, DiscussionThread t" +
                    " WHERE p.DiscussionThread.Id = t.Id" +
                    " AND t.Discussion.Id = {0}",
                    discussionid)).UniqueResult<int>();
        }

        public static int GetDiscussionThreadCount(ISession session, int discussionid)
        {
            return session.CreateQuery(
                string.Format(
                    "SELECT COUNT(*)" +
                    " FROM DiscussionThread t " +
                    "WHERE t.Discussion.Id = {0}",
                    discussionid)).UniqueResult<int>();
        }

        public static bool FindAndDelete(
            ISession session,
            int accountid,
            Type type,
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion discussion = Find(session, accountid, type, objectid, sec);
            if (discussion == null)
                return false;
            ManagedDiscussion m_instance = new ManagedDiscussion(session, discussion);
            m_instance.Delete(sec);
            return true;
        }

        public static Discussion Find(
            ISession session,
            int accountid,
            Type type,
            int objectid,
            ManagedSecurityContext sec)
        {
            ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(type);

            Discussion instance = (Discussion)
                session.CreateCriteria(typeof(Discussion))
                    .Add(Expression.Eq("Name", mapentry.Name))
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
            Type type,
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion d = Find(session, accountid, type, objectid, sec);

            if (d == null)
            {
                throw new ManagedDiscussion.DiscussionNotFoundException();
            }

            return d.Id;
        }

        public static int GetOrCreateDiscussionId(
            ISession session,
            string typename,
            int objectid,
            ManagedSecurityContext sec)
        {
            ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(
                Assembly.GetAssembly(typeof(Account)).GetType(typename));
            // find the owner's account id
            int account_id = mapentry.GetAccountId(session, objectid);
            return GetOrCreateDiscussionId(session, account_id, mapentry.Type, objectid, sec);
        }

        private static int GetOrCreateDiscussionId(
            ISession session,
            int accountid,
            Type type,
            int objectid,
            ManagedSecurityContext sec)
        {
            Discussion d = Find(session, accountid, type, objectid, sec);

            if (d != null)
            {
                return d.Id;
            }

            ManagedDiscussionMapEntry mapentry = ManagedDiscussionMap.Find(type);

            TransitDiscussion td = new TransitDiscussion();
            td.AccountId = accountid;
            td.Name = mapentry.Name;
            td.Personal = true;
            td.Description = string.Empty;
            td.Created = td.Modified = DateTime.UtcNow;
            td.ObjectId = objectid;

            // creating a discussion that belongs to a different user (commenting on someone's items)
            ManagedDiscussion m_d = new ManagedDiscussion(session);
            ManagedSecurityContext o_sec = new ManagedSecurityContext(session, accountid);
            return m_d.CreateOrUpdate(td, o_sec);
        }

        public void MigrateToAccount(Account newowner, ManagedSecurityContext sec)
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

        public override ACL GetACL(Type type)
        {
            ManagedDiscussionMapEntry mapentry = null;
            ACL acl = null;

            if (mInstance.Personal && ManagedDiscussionMap.TryFind(mInstance.Name, out mapentry))
            {
                acl = mapentry.GetACL(Session, mInstance.ObjectId, typeof(Discussion));
            }
            else
            {
                acl = base.GetACL(type);
                acl.Add(new ACLEveryoneAllowRetrieve());
            }

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
                    List<TransitDiscussionPost> t_posts = m_thread.GetDiscussionPosts(sec);
                    if (t_posts.Count > 0 && t_posts[0].Sticky)
                    {
                        result.InsertRange(0, t_posts);
                    }
                    else
                    {
                        result.AddRange(t_posts);
                    }
                }
            }
            return result;
        }

        public override TransitDiscussion GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitDiscussion t_instance = base.GetTransitInstance(sec);
            t_instance.PostCount = GetDiscussionPostCount(Session, Id);
            t_instance.ThreadCount = GetDiscussionThreadCount(Session, Id);
            t_instance.CanUpdate = GetACL().TryCheck(sec, DataOperation.Update);

            if (t_instance.Personal && t_instance.ObjectId > 0)
            {
                ManagedDiscussionMapEntry mapentry;

                if (ManagedDiscussionMap.TryFind(Name, out mapentry))
                {
                    t_instance.ParentObjectName = mapentry.GetObjectName(Session, t_instance.ObjectId);
                    t_instance.ParentObjectType = mapentry.Type.Name;
                    t_instance.ParentObjectUri = string.Format(mapentry.DiscussionUriFormat, t_instance.ObjectId);
                }
            }

            return t_instance;
        }

        public static bool IsDiscussionType(Type type)
        {
            return 
                (type == typeof(DiscussionThread) 
                    || type == typeof(DiscussionPost) 
                    || type == typeof(Discussion));
        }
    }
}

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
using SnCore.Data.Hibernate;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitDiscussionPost : TransitService<DiscussionPost>
    {
        private bool mCanEdit = false;

        public bool CanEdit
        {
            get
            {

                return mCanEdit;
            }
            set
            {
                mCanEdit = value;
            }
        }

        private bool mCanDelete = false;

        public bool CanDelete
        {
            get
            {

                return mCanDelete;
            }
            set
            {
                mCanDelete = value;
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

        private int mDiscussionId = 0;

        public int DiscussionId
        {
            get
            {

                return mDiscussionId;
            }
            set
            {
                mDiscussionId = value;
            }
        }

        private string mDiscussionName;

        public string DiscussionName
        {
            get
            {

                return mDiscussionName;
            }
            set
            {
                mDiscussionName = value;
            }
        }

        private int mDiscussionThreadId = 0;

        public int DiscussionThreadId
        {
            get
            {

                return mDiscussionThreadId;
            }
            set
            {
                mDiscussionThreadId = value;
            }
        }

        private int mInstanceParentId = 0;

        public int DiscussionPostParentId
        {
            get
            {

                return mInstanceParentId;
            }
            set
            {
                mInstanceParentId = value;
            }
        }

        private int mAccountId = 0;

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

        private string mAccountName = string.Empty;

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

        private int mAccountPictureId = 0;

        public int AccountPictureId
        {
            get
            {

                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
            }
        }

        private string mBody = string.Empty;

        public string Body
        {
            get
            {

                return mBody;
            }
            set
            {
                mBody = value;
            }
        }

        private string mSubject = string.Empty;

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

        private int mLevel = 0;

        public int Level
        {
            get
            {

                return mLevel;
            }
            set
            {
                mLevel = value;
            }
        }

        private DateTime mDiscussionThreadModified;

        public DateTime DiscussionThreadModified
        {
            get
            {

                return mDiscussionThreadModified;
            }
            set
            {
                mDiscussionThreadModified = value;
            }
        }

        private int mDiscussionThreadCount = 0;

        public int DiscussionThreadCount
        {
            get
            {

                return mDiscussionThreadCount;
            }
            set
            {
                mDiscussionThreadCount = value;
            }
        }

        private int mRepliesCount = 0;

        public int RepliesCount
        {
            get
            {

                return mRepliesCount;
            }
            set
            {
                mRepliesCount = value;
            }
        }

        private bool mSticky = false;

        public bool Sticky
        {
            get
            {
                return mSticky;
            }
            set
            {
                mSticky = value;
            }
        }

        public TransitDiscussionPost()
        {

        }

        public override DiscussionPost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DiscussionPost instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                if (DiscussionPostParentId > 0)
                {
                    instance.DiscussionPostParent = session.Load<DiscussionPost>(this.DiscussionPostParentId);
                    instance.DiscussionThread = instance.DiscussionPostParent.DiscussionThread;
                }
                else if (DiscussionThreadId > 0)
                {
                    instance.DiscussionThread = session.Load<DiscussionThread>(this.DiscussionThreadId);
                }
                else
                {
                    instance.DiscussionThread = new DiscussionThread();
                    instance.DiscussionThread.Discussion = session.Load<Discussion>(this.DiscussionId);
                    instance.DiscussionThread.Created = instance.DiscussionThread.Modified = DateTime.UtcNow;
                }

                if (DiscussionThreadId > 0 && instance.DiscussionThread.Id != DiscussionThreadId)
                    throw new ArgumentException("Invalid Thread Id");

                if (DiscussionId > 0 && instance.DiscussionThread.Discussion.Id != DiscussionId)
                    throw new ArgumentException("Invalid Discussion Id");

                instance.AccountId = GetOwner(session, AccountId, sec).Id;
            }

            instance.Body = this.Body;
            instance.Subject = this.Subject;
            instance.Sticky = this.Sticky;
            return instance;
        }

        public override void SetInstance(DiscussionPost instance)
        {
            DiscussionName = instance.DiscussionThread.Discussion.Name;
            DiscussionId = instance.DiscussionThread.Discussion.Id;
            DiscussionThreadId = instance.DiscussionThread.Id;
            DiscussionThreadModified = instance.DiscussionThread.Modified;
            DiscussionThreadCount =
                (instance.DiscussionThread.DiscussionPosts != null) ?
                    instance.DiscussionThread.DiscussionPosts.Count :
                    0;
            RepliesCount =
                (instance.DiscussionPosts != null) ?
                    instance.DiscussionPosts.Count :
                    0;
            DiscussionPostParentId = (instance.DiscussionPostParent == null) ? 0 : instance.DiscussionPostParent.Id;
            AccountId = instance.AccountId;
            Body = instance.Body;
            Subject = instance.Subject;
            Created = instance.Created;
            Modified = instance.Modified;
            Sticky = instance.Sticky;
            Level = 0;
            DiscussionPost parent = instance.DiscussionPostParent;
            while (parent != null && parent != instance)
            {
                Level++;
                parent = parent.DiscussionPostParent;
            }

            base.SetInstance(instance);
        }
    }

    public class ManagedDiscussionPost : ManagedService<DiscussionPost, TransitDiscussionPost>, IAuditableService
    {
        public ManagedDiscussionPost()
        {

        }

        public ManagedDiscussionPost(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussionPost(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedDiscussionPost(ISession session, DiscussionPost value)
            : base(session, value)
        {

        }

        public int DiscussionId
        {
            get
            {
                return mInstance.DiscussionThread.Discussion.Id;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                new ManagedDiscussionPost(Session, post).Delete(sec);
            }

            mInstance.DiscussionPosts = null;
            Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionThread.DiscussionPosts).Remove(mInstance);

            base.Delete(sec);
        }

        public override TransitDiscussionPost GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitDiscussionPost post = base.GetTransitInstance(sec);
            post.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, post.AccountId);
            post.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, post.AccountId);
            post.CanEdit = (GetACL().Apply(sec, DataOperation.Update) == ACLVerdict.Allowed);
            post.CanDelete = (GetACL().Apply(sec, DataOperation.Delete) == ACLVerdict.Allowed);
            return post;
        }

        public List<TransitDiscussionPost> GetPosts(ManagedSecurityContext sec)
        {
            if (mInstance.DiscussionPosts == null)
                return new List<TransitDiscussionPost>();

            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(mInstance.DiscussionPosts.Count);
            List<TransitDiscussionPost> sticky = new List<TransitDiscussionPost>();
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                if (post.Sticky)
                {
                    sticky.Insert(0, m_post.GetTransitInstance(sec));
                    sticky.InsertRange(1, m_post.GetPosts(sec));
                }
                else
                {
                    result.Insert(0, m_post.GetTransitInstance(sec));
                    result.InsertRange(1, m_post.GetPosts(sec));
                }
            }

            result.InsertRange(0, sticky);
            return result;
        }

        public override int CreateOrUpdate(TransitDiscussionPost t_instance, ManagedSecurityContext sec)
        {
            Nullable<DateTime> lastModified = new Nullable<DateTime>();
            if (mInstance != null) lastModified = mInstance.Modified;

            // discussion admin can update stickyness
            bool fStickyModified = false;
            if (mInstance != null && mInstance.Sticky != t_instance.Sticky) fStickyModified = true;
            if (mInstance == null && t_instance.Sticky) fStickyModified = true;
            if (fStickyModified)
            {
                ManagedDiscussion m_discussion = new ManagedDiscussion(Session, mInstance != null 
                    ? mInstance.DiscussionThread.Discussion.Id 
                    : t_instance.DiscussionId);
                m_discussion.GetACL().Check(sec, DataOperation.Update);
            }

            int id = base.CreateOrUpdate(t_instance, sec);

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, mInstance.AccountId);
                ManagedAccount ma = new ManagedAccount(Session, mInstance.DiscussionPostParent != null
                    ? mInstance.DiscussionPostParent.AccountId
                    : mInstance.DiscussionThread.Discussion.Account.Id);

                // if the author is editing the post, don't notify within 30 minute periods
                if (ra.Id != ma.Id && (t_instance.Id == 0 || 
                    (lastModified.HasValue && lastModified.Value.AddMinutes(30) > DateTime.UtcNow)))
                {
                    Session.Flush();

                    ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                        Session, ma, string.Format("EmailDiscussionPost.aspx?id={0}", mInstance.Id));
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }

            return id;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;

            // message cannot span discussions
            if (mInstance.DiscussionPostParent != null &&
                mInstance.DiscussionPostParent.DiscussionThread.Discussion.Id != mInstance.DiscussionThread.Discussion.Id)
            {
                throw new ArgumentException("Invalid Discussion Id");
            }

            if (mInstance.Id == 0)
            {
                // both discussion thread and discussion board are updated to simplify queries
                mInstance.Created = mInstance.Modified;
                mInstance.DiscussionThread.Modified = mInstance.Modified;
                mInstance.DiscussionThread.Discussion.Modified = mInstance.Modified;
                Session.Save(mInstance.DiscussionThread);
            }

            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ManagedDiscussionMapEntry mapentry = null;
            ACL acl = null;

            if (mInstance.DiscussionThread.Discussion.Personal &&
                ManagedDiscussionMap.TryFind(mInstance.DiscussionThread.Discussion.DataObject, out mapentry))
            {
                acl = mapentry.GetACL(Session, mInstance.DiscussionThread.Discussion.ObjectId, typeof(DiscussionPost));
                if (mInstance.Id > 0) acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            }
            else
            {
                acl = base.GetACL(type);
                acl.Add(new ACLEveryoneAllowRetrieve());
                acl.Add(new ACLAuthenticatedAllowCreate());
                acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            }

            return acl;
        }

        public const int DefaultHourlyLimit = 30; // TODO: export into configuration settings

        // messages sent by this user to those who aren't this user's friends
        public static IList<DiscussionPost> GetDiscussionPosts(ISession session, int account_id, DateTime limit)
        {
            return session.CreateQuery("FROM DiscussionPost p" +
                string.Format(" WHERE p.AccountId = {0}", account_id) +
                string.Format(" AND p.Created >= '{0}'", limit)
                ).List<DiscussionPost>();
        }

        protected override void Check(TransitDiscussionPost t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);

            if (t_instance.Id != 0)
                return;

            sec.CheckVerifiedEmail();

            int account_id = t_instance.GetOwner(Session, t_instance.AccountId, sec).Id;

            // how many posts within the last hour?
            new ManagedQuota(DefaultHourlyLimit).Check<DiscussionPost, ManagedAccount.QuotaExceededException>(
                GetDiscussionPosts(Session, account_id, DateTime.UtcNow.AddHours(-1)));
            // how many messages within the last 24 hours?
            ManagedQuota.GetDefaultEnabledQuota().Check<DiscussionPost, ManagedAccount.QuotaExceededException>(
                GetDiscussionPosts(Session, account_id, DateTime.UtcNow.AddDays(-1)));

            // check whether the sender was flagged
            new ManagedQuota(ManagedAccountFlag.DefaultAccountFlagThreshold).Check<AccountFlag, ManagedAccountFlag.AccountFlaggedException>(
                ManagedAccountFlag.GetAccountFlagsByFlaggedAccountId(Session, sec.Account.Id));
        }

        public IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    string url = string.Format("DiscussionThreadView.aspx?id={0}&did={1}", mInstance.DiscussionThread.Id, mInstance.DiscussionThread.Discussion.Id);
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, sec.Account,
                        string.Format("[user:{0}] has posted <a href=\"{1}\">{2}</a> in [discussion:{3}]",
                        mInstance.AccountId, url, Renderer.Render(mInstance.Subject), mInstance.DiscussionThread.Discussion.Id),
                        url));
                    break;
            }
            return result;
        }

        private void AttachToThread(DiscussionPost post, DiscussionThread thread)
        {
            post.DiscussionThread = thread;
            if (post.DiscussionPosts == null) return;
            foreach (DiscussionPost child in post.DiscussionPosts)
            {
                AttachToThread(child, thread);
            }
        }

        public void Move(ManagedSecurityContext sec, int targetid)
        {
            GetACL().Check(sec, DataOperation.Delete | DataOperation.Create);

            Discussion target_discussion = Session.Load<Discussion>(targetid);

            // detach the post from the source thread and reset its parent
            mInstance.DiscussionPostParent = null;
            mInstance.DiscussionThread.DiscussionPosts.Remove(mInstance);
            if (mInstance.DiscussionThread.DiscussionPosts.Count == 0) Session.Delete(mInstance.DiscussionThread);

            // create the target thread
            DiscussionThread target_thread = new DiscussionThread();
            target_thread.Created = mInstance.Created;
            target_thread.Modified = DateTime.UtcNow;
            target_thread.Discussion = target_discussion;
            Session.Save(target_thread);
 
            // attach the post and all child posts to the target thread
            AttachToThread(mInstance, target_thread);

            Save(sec);
        }

        public int MoveToAccountBlog(ManagedSecurityContext sec, int targetid)
        {
            GetACL().Check(sec, DataOperation.Delete);

            ManagedAccountBlog blog = new ManagedAccountBlog(Session, targetid);
            blog.GetACL().Check(sec, DataOperation.Update);

            // copy the post to a blog post
            AccountBlogPost t_post = new AccountBlogPost();
            t_post.AccountBlog = Session.Load<AccountBlog>(targetid);
            t_post.AccountId = mInstance.AccountId;
            t_post.EnableComments = true;
            t_post.Created = mInstance.Created;
            t_post.Modified = mInstance.Modified;
            t_post.Title = mInstance.Subject;
            t_post.Body = mInstance.Body;
            t_post.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, mInstance.AccountId);
            Session.Save(t_post);

            // move the comments thread to the blog comments
            int discussion_id = ManagedDiscussion.GetOrCreateDiscussionId(Session, "AccountBlogPost", t_post.Id, sec);
            Discussion target_discussion = Session.Load<Discussion>(discussion_id);

            // create the target thread
            DiscussionThread target_thread = new DiscussionThread();
            target_thread.Created = mInstance.Created;
            target_thread.Modified = mInstance.DiscussionThread.Modified;
            target_thread.Discussion = target_discussion;
            Session.Save(target_thread);

            // attach the post and all child posts to the target thread
            AttachToThread(mInstance, target_thread);
            
            // nullify each child's parent
            foreach (DiscussionPost post in mInstance.DiscussionPosts)
            {
                post.DiscussionPostParent = null;
            }

            // delete the current post that became a blog entry
            Session.Delete(mInstance);
            return t_post.Id;
        }
    }
}

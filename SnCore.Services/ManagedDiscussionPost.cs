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

        public TransitDiscussionPost()
        {

        }

        public override DiscussionPost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            DiscussionPost instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                if (DiscussionThreadId > 0) instance.DiscussionThread = (DiscussionThread)session.Load(typeof(DiscussionThread), this.DiscussionThreadId);
                if (DiscussionPostParentId > 0) instance.DiscussionPostParent = (DiscussionPost)session.Load(typeof(DiscussionPost), this.DiscussionPostParentId);
                if (AccountId > 0) instance.AccountId = this.AccountId;
            }

            instance.Body = this.Body;
            instance.Subject = this.Subject;
            return instance;
        }

        public void SetPermissions(Discussion d, ManagedAccount a)
        {
            if (a == null)
                return;

            CanEdit =
                (AccountId == a.Id) || // can edit your own posts
                a.IsAdministrator();

            if (d == null)
                return;

            CanDelete =
                (CanEdit && RepliesCount == 0) || // can delete your own posts that have no replies
                (a.Id == d.Account.Id) || // can delete any post in your own discussion
                a.IsAdministrator();
        }
    }

    public class ManagedDiscussionPost : ManagedService<DiscussionPost, TransitDiscussionPost>
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
            TransitDiscussionPost t_instance = base.GetTransitInstance(sec);

            try
            {
                Account acct = (Account)Session.Load(typeof(Account), mInstance.AccountId);
                t_instance.AccountName = (acct != null) ? acct.Name : "Unknown User";
                t_instance.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(acct);
            }
            catch (ObjectNotFoundException)
            {

            }

            t_instance.DiscussionName = mInstance.DiscussionThread.Discussion.Name;
            t_instance.DiscussionId = mInstance.DiscussionThread.Discussion.Id;
            t_instance.DiscussionThreadId = mInstance.DiscussionThread.Id;
            t_instance.DiscussionThreadModified = mInstance.DiscussionThread.Modified;
            t_instance.DiscussionThreadCount =
                (mInstance.DiscussionThread.DiscussionPosts != null) ?
                    mInstance.DiscussionThread.DiscussionPosts.Count :
                    0;
            t_instance.RepliesCount =
                (mInstance.DiscussionPosts != null) ?
                    mInstance.DiscussionPosts.Count :
                    0;
            t_instance.DiscussionPostParentId = (mInstance.DiscussionPostParent == null) ? 0 : mInstance.DiscussionPostParent.Id;
            t_instance.AccountId = mInstance.AccountId;
            t_instance.Body = mInstance.Body;
            t_instance.Subject = mInstance.Subject;
            t_instance.Created = mInstance.Created;
            t_instance.Modified = mInstance.Modified;
            t_instance.Level = 0;
            DiscussionPost parent = mInstance.DiscussionPostParent;
            while (parent != null && parent != mInstance)
            {
                t_instance.Level++;
                parent = parent.DiscussionPostParent;
            }
            return t_instance;
        }

        public List<TransitDiscussionPost> GetPosts(ManagedSecurityContext sec)
        {
            if (mInstance.DiscussionPosts == null)
                return new List<TransitDiscussionPost>();

            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(mInstance.DiscussionPosts.Count);
            foreach (DiscussionPost post in Collection<DiscussionPost>.GetSafeCollection(mInstance.DiscussionPosts))
            {
                ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                result.Insert(0, m_post.GetTransitInstance(sec));
                result.InsertRange(1, m_post.GetPosts(sec));
            }

            return result;
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
            try
            {
                acl.Add(new ACLAccount((Account)Session.Load(typeof(Account), mInstance.AccountId), DataOperation.All));
            }
            catch (ObjectNotFoundException)
            {

            }
            return acl;
        }
    }
}

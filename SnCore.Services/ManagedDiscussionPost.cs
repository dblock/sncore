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
                if (DiscussionPostParentId > 0)
                {
                    instance.DiscussionPostParent = (DiscussionPost)session.Load(typeof(DiscussionPost), this.DiscussionPostParentId);
                    instance.DiscussionThread = instance.DiscussionPostParent.DiscussionThread;
                }
                else if (DiscussionThreadId > 0)
                {
                    instance.DiscussionThread = (DiscussionThread)session.Load(typeof(DiscussionThread), this.DiscussionThreadId);
                }
                else
                {
                    instance.DiscussionThread = new DiscussionThread();
                    instance.DiscussionThread.Discussion = (Discussion)session.Load(typeof(Discussion), this.DiscussionId);
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
            TransitDiscussionPost post = base.GetTransitInstance(sec);
            post.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, post.AccountId);
            post.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, post.AccountId);
            return post;
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

            // message cannot span discussions
            if (mInstance.DiscussionPostParent != null &&
                mInstance.DiscussionPostParent.DiscussionThread.Discussion.Id != mInstance.DiscussionThread.Discussion.Id)
            {
                throw new ArgumentException("Invalid Discussion Id");
            }

            if (mInstance.Id == 0)
            {
                mInstance.Created = mInstance.Modified;
                mInstance.DiscussionThread.Modified = mInstance.Modified;
                Session.Save(mInstance.DiscussionThread);
            }
            base.Save(sec);
            Session.Flush();

            try
            {
                ManagedAccount ra = new ManagedAccount(Session, mInstance.AccountId);
                ManagedAccount ma = new ManagedAccount(Session, mInstance.DiscussionPostParent != null
                    ? mInstance.DiscussionPostParent.AccountId
                    : mInstance.DiscussionThread.Discussion.Account.Id);

                if (ra.Id != ma.Id)
                {
                    string replyTo = ma.GetActiveEmailAddress();
                    if (!string.IsNullOrEmpty(replyTo))
                    {
                        ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                            Session,
                            new MailAddress(replyTo, ma.Name).ToString(),
                            string.Format("EmailDiscussionPost.aspx?id={0}", mInstance.Id));
                    }
                }
            }
            catch (ObjectNotFoundException)
            {
                // replying to an account that does not exist
            }
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccountId(mInstance.AccountId, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitDiscussionPost t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) sec.CheckVerifiedEmail();
        }
    }
}

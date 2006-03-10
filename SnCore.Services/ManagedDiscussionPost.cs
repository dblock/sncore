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

namespace SnCore.Services
{
    public class TransitDiscussionPost : TransitService
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

        private int mDiscussionPostParentId = 0;

        public int DiscussionPostParentId
        {
            get
            {

                return mDiscussionPostParentId;
            }
            set
            {
                mDiscussionPostParentId = value;
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

        public DiscussionPost GetDiscussionPost(ISession session)
        {
            DiscussionPost d = (Id != 0) ? (DiscussionPost)session.Load(typeof(DiscussionPost), Id) : new DiscussionPost();

            if (Id == 0)
            {
                d.DiscussionThread = (DiscussionThread)session.Load(typeof(DiscussionThread), this.DiscussionThreadId);
                d.DiscussionPostParent = (DiscussionPost)session.Load(typeof(DiscussionPost), this.DiscussionPostParentId);
                d.AccountId = this.AccountId;
            }

            d.Body = this.Body;
            d.Subject = this.Subject;
            return d;
        }

        public void SetPermissions(Discussion d, ManagedAccount a)
        {
            if (a == null)
                return;

            CanEdit =
                (AccountId == a.Id) ||
                a.IsAdministrator();

            if (d == null)
                return;

            CanDelete =
                (CanEdit && RepliesCount == 0) ||
                (a.Id == d.Account.Id && d.Personal) ||
                a.IsAdministrator();
        }
    }

    /// <summary>
    /// Managed discussion post.
    /// </summary>
    public class ManagedDiscussionPost : ManagedService
    {
        private DiscussionPost mDiscussionPost = null;

        public ManagedDiscussionPost(ISession session)
            : base(session)
        {

        }

        public ManagedDiscussionPost(ISession session, int id)
            : base(session)
        {
            mDiscussionPost = (DiscussionPost)session.Load(typeof(DiscussionPost), id);
        }

        public ManagedDiscussionPost(ISession session, DiscussionPost value)
            : base(session)
        {
            mDiscussionPost = value;
        }

        public int DiscussionId
        {
            get
            {
                return mDiscussionPost.DiscussionThread.Discussion.Id;
            }
        }

        public int Id
        {
            get
            {
                return mDiscussionPost.Id;
            }
        }

        public void Create(TransitDiscussionPost c)
        {
            mDiscussionPost = c.GetDiscussionPost(Session);
            mDiscussionPost.Modified = DateTime.UtcNow;
            if (mDiscussionPost.Id == 0) mDiscussionPost.Created = mDiscussionPost.Modified;
            Session.Save(mDiscussionPost);
        }

        public void Delete()
        {
            if (mDiscussionPost.DiscussionPosts != null)
            {
                foreach (DiscussionPost post in mDiscussionPost.DiscussionPosts)
                {
                    new ManagedDiscussionPost(Session, post).Delete();
                }

                mDiscussionPost.DiscussionPosts = null;
            }

            Session.Delete(mDiscussionPost);
        }

        public TransitDiscussionPost GetTransitDiscussionPost()
        {
            TransitDiscussionPost result = new TransitDiscussionPost();
            result.Id = Id;

            try
            {
                Account acct = (Account)Session.Load(typeof(Account), mDiscussionPost.AccountId);
                result.AccountName = (acct != null) ? acct.Name : "Unknown User";
                result.AccountPictureId = (acct != null) ? ManagedService.GetRandomElementId(acct.AccountPictures) : 0;
            }
            catch (ObjectNotFoundException)
            {

            }

            result.DiscussionName = mDiscussionPost.DiscussionThread.Discussion.Name;
            result.DiscussionId = mDiscussionPost.DiscussionThread.Discussion.Id;
            result.DiscussionThreadId = mDiscussionPost.DiscussionThread.Id;
            result.DiscussionThreadModified = mDiscussionPost.DiscussionThread.Modified;
            result.DiscussionThreadCount =
                (mDiscussionPost.DiscussionThread.DiscussionPosts != null) ?
                    mDiscussionPost.DiscussionThread.DiscussionPosts.Count :
                    0;
            result.RepliesCount =
                (mDiscussionPost.DiscussionPosts != null) ?
                    mDiscussionPost.DiscussionPosts.Count :
                    0;
            result.DiscussionPostParentId = (mDiscussionPost.DiscussionPostParent == null) ? 0 : mDiscussionPost.DiscussionPostParent.Id;
            result.AccountId = mDiscussionPost.AccountId;
            result.Body = mDiscussionPost.Body;
            result.Subject = mDiscussionPost.Subject;
            result.Created = mDiscussionPost.Created;
            result.Modified = mDiscussionPost.Modified;
            result.Level = 0;
            DiscussionPost parent = mDiscussionPost.DiscussionPostParent;
            while (parent != null && parent != mDiscussionPost)
            {
                result.Level++;
                parent = parent.DiscussionPostParent;
            }
            return result;
        }

        public List<TransitDiscussionPost> GetPosts()
        {
            List<TransitDiscussionPost> result = new List<TransitDiscussionPost>(mDiscussionPost.DiscussionPosts.Count);
            foreach (DiscussionPost post in mDiscussionPost.DiscussionPosts)
            {
                ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                result.Insert(0, m_post.GetTransitDiscussionPost());
                result.InsertRange(1, m_post.GetPosts());
            }
            return result;
        }
    }
}

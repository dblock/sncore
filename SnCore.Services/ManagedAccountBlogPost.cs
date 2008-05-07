using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using SnCore.Tools.Web;
using SnCore.Data.Hibernate;
using SnCore.Tools;

namespace SnCore.Services
{
    public class TransitAccountBlogPostQueryOptions
    {
        public int BlogId = 0;
        public bool PublishedOnly = true;

        public TransitAccountBlogPostQueryOptions()
        {

        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (BlogId > 0)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("AccountBlogPost.AccountBlog.Id = {0}", BlogId);
            }

            if (PublishedOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountBlogPost.Publish = 1");
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT AccountBlogPost FROM AccountBlogPost AccountBlogPost");
            b.Append(CreateSubQuery());
            b.Append(" ORDER BY AccountBlogPost.Sticky DESC, AccountBlogPost.Created DESC");
            return b.ToString();
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountBlogPost : TransitService<AccountBlogPost>
    {
        private int mAccountBlogId;

        public int AccountBlogId
        {
            get
            {

                return mAccountBlogId;
            }
            set
            {
                mAccountBlogId = value;
            }
        }

        private string mTitle;

        public string Title
        {
            get
            {

                return mTitle;
            }
            set
            {
                mTitle = value;
            }
        }

        private string mBody;

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

        private int mAccountPictureId;

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

        private string mAccountBlogName;

        public string AccountBlogName
        {
            get
            {

                return mAccountBlogName;
            }
            set
            {
                mAccountBlogName = value;
            }
        }

        private int mCommentCount = 0;

        public int CommentCount
        {
            get
            {
                return mCommentCount;
            }
            set
            {
                mCommentCount = value;
            }
        }

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

        private bool mEnableComments = true;

        public bool EnableComments
        {
            get
            {
                return mEnableComments;
            }
            set
            {
                mEnableComments = value;
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

        private bool mPublish = true;

        public bool Publish
        {
            get
            {
                return mPublish;
            }
            set
            {
                mPublish = value;
            }
        }

        public TransitAccountBlogPost()
        {

        }

        public TransitAccountBlogPost(AccountBlogPost instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountBlogPost instance)
        {
            AccountBlogId = instance.AccountBlog.Id;
            Title = instance.Title;
            Body = instance.Body;
            Created = instance.Created;
            Modified = instance.Modified;
            AccountId = instance.AccountId;
            EnableComments = instance.EnableComments && instance.AccountBlog.EnableComments;
            Sticky = instance.Sticky;
            Publish = instance.Publish;

            AccountName = instance.AccountName;
            AccountBlogName = instance.AccountBlog.Name;

            base.SetInstance(instance);
        }

        public override AccountBlogPost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountBlogPost instance = base.GetInstance(session, sec);
            instance.Title = this.Title;
            instance.Body = this.Body;
            instance.EnableComments = this.EnableComments;
            instance.Sticky = this.Sticky;
            instance.Publish = this.Publish;

            if (Id == 0)
            {
                Account owner = GetOwner(session, AccountId, sec);
                instance.AccountName = owner.Name;
                instance.AccountId = owner.Id;
                instance.AccountBlog = session.Load<AccountBlog>(AccountBlogId);
            }

            return instance;
        }
    }

    public class ManagedAccountBlogPost : ManagedAuditableService<AccountBlogPost, TransitAccountBlogPost>
    {
        public ManagedAccountBlogPost()
        {

        }

        public ManagedAccountBlogPost(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlogPost(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountBlogPost(ISession session, AccountBlogPost value)
            : base(session, value)
        {

        }

        public override TransitAccountBlogPost GetTransitInstance(ManagedSecurityContext sec)
        {
            ManagedAccountBlog m_blog = new ManagedAccountBlog(Session, mInstance.AccountBlog);
            TransitAccountBlogPost t_post = base.GetTransitInstance(sec);
            t_post.CanEdit = (GetACL().Apply(sec, DataOperation.Update) == ACLVerdict.Allowed);
            t_post.CanDelete = (GetACL().Apply(sec, DataOperation.Delete) == ACLVerdict.Allowed);
            t_post.AccountName = ManagedAccount.GetAccountNameWithDefault(Session, mInstance.AccountId);
            t_post.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, mInstance.AccountId);
            t_post.CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                Session, mInstance.AccountBlog.Account.Id, typeof(AccountBlogPost), mInstance.Id);
            return t_post;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(
                Session, mInstance.AccountBlog.Account.Id, typeof(AccountBlogPost), mInstance.Id, sec);
            ManagedFeature.Delete(Session, "AccountBlogPost", Id);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0)
            {
                mInstance.Created = mInstance.Modified;
                mInstance.AccountBlog.Updated = mInstance.Modified;
            }
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountBlog.Account, DataOperation.All));

            if (ManagedDiscussion.IsDiscussionType(type))
            {
                if (mInstance.EnableComments && mInstance.AccountBlog.EnableComments)
                {
                    acl.Add(new ACLAuthenticatedAllowCreate());
                }
            }

            foreach (AccountBlogAuthor author in Collection<AccountBlogAuthor>.GetSafeCollection(mInstance.AccountBlog.AccountBlogAuthors))
            {
                int op = (int)DataOperation.None;
                if (author.AllowDelete) op |= (int)DataOperation.Delete;
                if (author.AllowEdit) op |= (int)DataOperation.Update;
                if (author.AllowPost) op |= (int)DataOperation.Create;
                acl.Add(new ACLAccount(author.Account, op));
            }
            return acl;
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    string url = string.Format("AccountBlogPostView.aspx?id={0}", mInstance.Id);
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.AccountBlog.Account,
                        string.Format("[user:{0}] posted <a href=\"{1}\">{2}</a> in [blog:{3}]",
                        mInstance.AccountBlog.Account.Id, url, Renderer.Render(mInstance.Title), mInstance.AccountBlog.Id),
                        url));
                    break;
            }
            return result;
        }

        public void Move(ManagedSecurityContext sec, int targetid)
        {
            GetACL().Check(sec, DataOperation.Delete);
            AccountBlog target_blog = Session.Load<AccountBlog>(targetid);
            mInstance.AccountBlog = target_blog;
            Save(sec);
        }


        public int MoveToDiscussion(ManagedSecurityContext sec, int targetid)
        {
            GetACL().Check(sec, DataOperation.Delete);

            ManagedDiscussion target_discussion = new ManagedDiscussion(Session, targetid);
            target_discussion.GetACL().Check(sec, DataOperation.Create);

            // create the target thread
            DiscussionThread target_thread = new DiscussionThread();
            target_thread.Discussion = target_discussion.Instance;
            target_thread.Modified = mInstance.Modified;
            target_thread.Created = mInstance.Created;
            Session.Save(target_thread);
            if (target_discussion.Instance.Modified < mInstance.Modified)
            {
                target_discussion.Instance.Modified = mInstance.Modified;
                Session.Save(target_discussion.Instance);
            }
            // copy the post to a discusison post
            DiscussionPost target_post = new DiscussionPost();
            target_post.AccountId = mInstance.AccountId;
            target_post.Body = mInstance.Body;
            target_post.Created = mInstance.Created;
            target_post.Modified = mInstance.Modified;
            target_post.Sticky = false;
            target_post.Subject = mInstance.Title;
            target_post.DiscussionThread = target_thread;
            Session.Save(target_post);
            // set the new post as parent of all replies
            int comments_discussion_id = ManagedDiscussion.GetOrCreateDiscussionId(
                Session, typeof(AccountBlogPost).Name, mInstance.Id, sec);
            Discussion comments_discussion = Session.Load<Discussion>(comments_discussion_id);
            foreach (DiscussionThread thread in Collection<DiscussionThread>.GetSafeCollection(comments_discussion.DiscussionThreads))
            {
                foreach (DiscussionPost post in thread.DiscussionPosts)
                {
                    post.DiscussionThread = target_thread;
                    if (post.DiscussionPostParent == null)
                    {
                        post.DiscussionPostParent = target_post;
                    }
                }
                Session.Delete(thread);
            }
            // delete the current post that became a discussion post
            Session.Delete(mInstance);
            return target_post.Id;
        }
    }
}
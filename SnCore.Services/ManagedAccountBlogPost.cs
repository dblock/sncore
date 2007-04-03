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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
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

            AccountName = instance.AccountName;
            AccountBlogName = instance.AccountBlog.Name;

            base.SetInstance(instance);
        }

        public override AccountBlogPost GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountBlogPost instance = base.GetInstance(session, sec);
            instance.Title = this.Title;
            instance.Body = this.Body;

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

    public class ManagedAccountBlogPost : ManagedService<AccountBlogPost, TransitAccountBlogPost>
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
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountBlog.Account, DataOperation.All));

            if (ManagedDiscussion.IsDiscussionType(type))
            {
                acl.Add(new ACLAuthenticatedAllowCreate());
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
    }
}
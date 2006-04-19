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

namespace SnCore.Services
{
    public class TransitAccountBlogPost : TransitService
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

        public TransitAccountBlogPost()
        {

        }

        public TransitAccountBlogPost(ISession session, AccountBlogPost o)
            : base(o.Id)
        {
            AccountBlogId = o.AccountBlog.Id;
            Title = o.Title;
            Body = o.Body;
            Created = o.Created;
            Modified = o.Modified;
            AccountId = o.AccountId;

            try
            {
                Account acct = (Account) session.Load(typeof(Account), o.AccountId);
                AccountPictureId = ManagedService.GetRandomElementId(acct.AccountPictures);
            }
            catch (NHibernate.ObjectNotFoundException)
            {
                AccountId = 0;
                AccountPictureId = 0;
            }

            AccountName = o.AccountName;
            AccountBlogName = o.AccountBlog.Name;
            
            CommentCount = ManagedDiscussion.GetDiscussionPostCount(
                session, o.AccountBlog.Account.Id, ManagedDiscussion.AccountBlogPostDiscussion, o.Id);
        }

        public AccountBlogPost GetAccountBlogPost(ISession session)
        {
            AccountBlogPost p = (Id != 0) ? (AccountBlogPost)session.Load(typeof(AccountBlogPost), Id) : new AccountBlogPost();
            p.Title = this.Title;
            p.Body = this.Body;
            
            if (Id == 0)
            {
                p.AccountName = this.AccountName;
                p.AccountId = this.AccountId;
                p.AccountBlog = (AccountBlog)session.Load(typeof(AccountBlog), AccountBlogId);
            }

            return p;
        }
    }

    public class ManagedAccountBlogPost : ManagedService
    {
        private AccountBlogPost mAccountBlogPost = null;

        public ManagedAccountBlogPost(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlogPost(ISession session, int id)
            : base(session)
        {
            mAccountBlogPost = (AccountBlogPost)session.Load(typeof(AccountBlogPost), id);
        }

        public ManagedAccountBlogPost(ISession session, AccountBlogPost value)
            : base(session)
        {
            mAccountBlogPost = value;
        }

        public ManagedAccountBlogPost(ISession session, TransitAccountBlogPost value)
            : base(session)
        {
            mAccountBlogPost = value.GetAccountBlogPost(session);
        }

        public int Id
        {
            get
            {
                return mAccountBlogPost.Id;
            }
        }

        public TransitAccountBlogPost TransitAccountBlogPost
        {
            get
            {
                return new TransitAccountBlogPost(Session, mAccountBlogPost);
            }
        }

        public void CreateOrUpdate(TransitAccountBlogPost o)
        {
            mAccountBlogPost = o.GetAccountBlogPost(Session);
            mAccountBlogPost.Modified = DateTime.UtcNow;
            if (Id == 0) mAccountBlogPost.Created = mAccountBlogPost.Modified;
            Session.Save(mAccountBlogPost);
        }

        public void Delete()
        {
            try
            {
                int DiscussionId = ManagedDiscussion.GetDiscussionId(
                    Session, mAccountBlogPost.AccountBlog.Account.Id, ManagedDiscussion.AccountBlogPostDiscussion, 
                    mAccountBlogPost.Id, false);
                Discussion mDiscussion = (Discussion)Session.Load(typeof(Discussion), DiscussionId);
                Session.Delete(mDiscussion);
            }
            catch (ManagedDiscussion.DiscussionNotFoundException)
            {

            }

            ManagedFeature.Delete(Session, "AccountBlogPost", Id);

            Session.Delete(mAccountBlogPost);
        }

        public int AccountBlogId
        {
            get
            {
                return mAccountBlogPost.AccountBlog.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountBlogPost.AccountId;
            }
        }
    }
}
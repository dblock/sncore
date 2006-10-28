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
using System.Collections.Generic;
using System.Web;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using Rss;
using Atom.Core;

namespace SnCore.Services
{
    public class TransitAccountBlogAuthor : TransitService
    {
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

        private bool mAllowPost = false;

        public bool AllowPost
        {
            get
            {
                return mAllowPost;
            }
            set
            {
                mAllowPost = value;
            }
        }

        private bool mAllowDelete = false;

        public bool AllowDelete
        {
            get
            {
                return mAllowDelete;
            }
            set
            {
                mAllowDelete = value;
            }
        }

        private bool mAllowEdit = false;

        public bool AllowEdit
        {
            get
            {
                return mAllowEdit;
            }
            set
            {
                mAllowEdit = value;
            }
        }

        public TransitAccountBlogAuthor()
        {

        }

        public TransitAccountBlogAuthor(AccountBlogAuthor o)
            : base(o.Id)
        {
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(o.Account);
            AccountBlogId = o.AccountBlog.Id;
            AccountBlogName = o.AccountBlog.Name;
            AllowDelete = o.AllowDelete;
            AllowEdit = o.AllowEdit;
            AllowPost = o.AllowPost;
        }

        public AccountBlogAuthor GetAccountBlogAuthor(ISession session)
        {
            AccountBlogAuthor p = (Id != 0) ? (AccountBlogAuthor)session.Load(typeof(AccountBlogAuthor), Id) : new AccountBlogAuthor();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
                if (AccountBlogId > 0) p.AccountBlog = (AccountBlog)session.Load(typeof(AccountBlog), AccountBlogId);
            }

            p.AllowPost = AllowPost;
            p.AllowEdit = AllowEdit;
            p.AllowDelete = AllowDelete;

            return p;
        }
    }

    public class ManagedAccountBlogAuthor : ManagedService
    {
        private AccountBlogAuthor mAccountBlogAuthor = null;

        public ManagedAccountBlogAuthor(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlogAuthor(ISession session, int id)
            : base(session)
        {
            mAccountBlogAuthor = (AccountBlogAuthor)session.Load(typeof(AccountBlogAuthor), id);
        }

        public ManagedAccountBlogAuthor(ISession session, AccountBlogAuthor value)
            : base(session)
        {
            mAccountBlogAuthor = value;
        }

        public ManagedAccountBlogAuthor(ISession session, TransitAccountBlogAuthor value)
            : base(session)
        {
            mAccountBlogAuthor = value.GetAccountBlogAuthor(session);
        }

        public int Id
        {
            get
            {
                return mAccountBlogAuthor.Id;
            }
        }

        public int BlogAccountId
        {
            get
            {
                return mAccountBlogAuthor.AccountBlog.Account.Id;
            }
        }

        public TransitAccountBlogAuthor TransitAccountBlogAuthor
        {
            get
            {
                return new TransitAccountBlogAuthor(mAccountBlogAuthor);
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountBlogAuthor);
        }
    }
}
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
    public class TransitAccountBlogAuthor : TransitService<AccountBlogAuthor>
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

        public TransitAccountBlogAuthor(AccountBlogAuthor value)
            : base(value)
        {

        }

        public override void SetInstance(AccountBlogAuthor value)
        {
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountBlogId = value.AccountBlog.Id;
            AccountBlogName = value.AccountBlog.Name;
            AllowDelete = value.AllowDelete;
            AllowEdit = value.AllowEdit;
            AllowPost = value.AllowPost;
            base.SetInstance(value);
        }

        public override AccountBlogAuthor GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountBlogAuthor instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.AccountBlog = session.Load<AccountBlog>(AccountBlogId);
            }

            instance.AllowPost = AllowPost;
            instance.AllowEdit = AllowEdit;
            instance.AllowDelete = AllowDelete;

            return instance;
        }
    }

    public class ManagedAccountBlogAuthor : ManagedService<AccountBlogAuthor, TransitAccountBlogAuthor>
    {
        public ManagedAccountBlogAuthor()
        {

        }

        public ManagedAccountBlogAuthor(ISession session)
            : base(session)
        {

        }

        public ManagedAccountBlogAuthor(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountBlogAuthor(ISession session, AccountBlogAuthor value)
            : base(session, value)
        {

        }

        public int BlogAccountId
        {
            get
            {
                return mInstance.AccountBlog.Account.Id;
            }
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance.AccountBlog.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountBlogAuthor t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountBlogAuthor, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountBlogAuthor instance WHERE instance.AccountBlog.Id = {0}",
                        mInstance.AccountBlog.Id)).UniqueResult<int>());
        }
    }
}
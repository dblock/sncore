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
    public class TransitAccountContentGroup : TransitService
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

        public TransitAccountContentGroup()
        {

        }

        public TransitAccountContentGroup(AccountContentGroup o)
            : base(o.Id)
        {
            Name = o.Name;
            Description = o.Description;
            Created = o.Created;
            Modified = o.Modified;
            AccountId = o.Account.Id;
            AccountName = o.Account.Name;
            Trusted = o.Trusted;
            Login = o.Login;
        }

        private bool mTrusted;

        public bool Trusted
        {
            get
            {
                return mTrusted;
            }
            set
            {
                mTrusted = value;
            }
        }

        private bool mLogin;

        public bool Login
        {
            get
            {
                return mLogin;
            }
            set
            {
                mLogin = value;
            }
        }


        public AccountContentGroup GetAccountContentGroup(ISession session)
        {
            AccountContentGroup p = (Id != 0) ? (AccountContentGroup)session.Load(typeof(AccountContentGroup), Id) : new AccountContentGroup();

            if (Id == 0)
            {
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            p.Name = this.Name;
            p.Description = this.Description;
            p.Trusted = this.Trusted;
            p.Login = this.Login;
            return p;
        }
    }

    public class ManagedAccountContentGroup : ManagedService<AccountContentGroup>
    {
        private AccountContentGroup mAccountContentGroup = null;

        public ManagedAccountContentGroup(ISession session)
            : base(session)
        {

        }

        public ManagedAccountContentGroup(ISession session, int id)
            : base(session)
        {
            mAccountContentGroup = (AccountContentGroup)session.Load(typeof(AccountContentGroup), id);
        }

        public ManagedAccountContentGroup(ISession session, AccountContentGroup value)
            : base(session)
        {
            mAccountContentGroup = value;
        }

        public ManagedAccountContentGroup(ISession session, TransitAccountContentGroup value)
            : base(session)
        {
            mAccountContentGroup = value.GetAccountContentGroup(session);
        }

        public int Id
        {
            get
            {
                return mAccountContentGroup.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountContentGroup.Account.Id;
            }
        }

        public TransitAccountContentGroup TransitAccountContentGroup
        {
            get
            {
                return new TransitAccountContentGroup(mAccountContentGroup);
            }
        }

        public void Delete()
        {
            Session.Delete(mAccountContentGroup);
        }

        public void CheckPermissions(string ticket)
        {
            int user_id = ManagedAccount.GetAccountId(ticket, 0);
            CheckPermissions(user_id);
        }

        public void CheckPermissions(int user_id)
        {
            if (mAccountContentGroup.Login && user_id <= 0)
            {
                throw new ManagedAccount.AccessDeniedException();
            }
        }
    }
}
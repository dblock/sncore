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
    public class TransitAccountContentGroup : TransitService<AccountContentGroup>
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

        public TransitAccountContentGroup()
        {

        }

        public TransitAccountContentGroup(AccountContentGroup value)
            : base(value)
        {
        }

        public override void SetInstance(AccountContentGroup value)
        {
            Name = value.Name;
            Description = value.Description;
            Created = value.Created;
            Modified = value.Modified;
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            Trusted = value.Trusted;
            Login = value.Login;
            base.SetInstance(value);
        }

        public override AccountContentGroup GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountContentGroup instance = base.GetInstance(session, sec);
            if (Id == 0) instance.Account = GetOwner(session, AccountId, sec);
            instance.Name = this.Name;
            instance.Description = this.Description;
            instance.Trusted = this.Trusted;
            instance.Login = this.Login;
            return instance;
        }
    }

    public class ManagedAccountContentGroup : ManagedService<AccountContentGroup, TransitAccountContentGroup>
    {
        public ManagedAccountContentGroup()
        {

        }

        public ManagedAccountContentGroup(ISession session)
            : base(session)
        {

        }

        public ManagedAccountContentGroup(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountContentGroup(ISession session, AccountContentGroup value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
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
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            if (mInstance.Login)
            {
                acl.Add(new ACLAuthenticatedAllowRetrieve());
            }
            else
            {
                acl.Add(new ACLEveryoneAllowRetrieve());
            }
            return acl;
        }
    }
}
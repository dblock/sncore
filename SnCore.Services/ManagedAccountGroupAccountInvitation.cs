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
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountGroupAccountInvitation : TransitService<AccountGroupAccountInvitation>
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

        private int mRequesterId;

        public int RequesterId
        {
            get
            {

                return mRequesterId;
            }
            set
            {
                mRequesterId = value;
            }
        }

        private int mRequesterPictureId;

        public int RequesterPictureId
        {
            get
            {

                return mRequesterPictureId;
            }
            set
            {
                mRequesterPictureId = value;
            }
        }

        private string mRequesterName;

        public string RequesterName
        {
            get
            {

                return mRequesterName;
            }
            set
            {
                mRequesterName = value;
            }
        }

        private int mAccountGroupId;

        public int AccountGroupId
        {
            get
            {
                return mAccountGroupId;
            }
            set
            {
                mAccountGroupId = value;
            }
        }

        private string mAccountGroupName;

        public string AccountGroupName
        {
            get
            {
                return mAccountGroupName;
            }
            set
            {
                mAccountGroupName = value;
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

        private string mMessage;

        public string Message
        {
            get
            {
                return mMessage;
            }
            set
            {
                mMessage = value;
            }
        }

        public TransitAccountGroupAccountInvitation()
        {

        }

        public TransitAccountGroupAccountInvitation(AccountGroupAccountInvitation value)
            : base(value)
        {

        }

        public override void SetInstance(AccountGroupAccountInvitation value)
        {
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            RequesterId = value.Requester.Id;
            RequesterName = value.Requester.Name;
            RequesterPictureId = ManagedAccount.GetRandomAccountPictureId(value.Requester);
            AccountGroupId = value.AccountGroup.Id;
            AccountGroupName = value.AccountGroup.Name;
            Message = value.Message;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountGroupAccountInvitation GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroupAccountInvitation instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.Requester = GetOwner(session, RequesterId, sec);
                instance.AccountGroup = session.Load<AccountGroup>(AccountGroupId);
                instance.Message = Message;
            }

            return instance;
        }
    }

    public class ManagedAccountGroupAccountInvitation : ManagedService<AccountGroupAccountInvitation, TransitAccountGroupAccountInvitation>
    {
        public ManagedAccountGroupAccountInvitation()
        {

        }

        public ManagedAccountGroupAccountInvitation(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroupAccountInvitation(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroupAccountInvitation(ISession session, AccountGroupAccountInvitation value)
            : base(session, value)
        {

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
            // members can create invitations or approve / deny them depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Create));
            }
            return acl;
        }
    }
}
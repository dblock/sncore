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
    public class TransitAccountGroupAccountRequest : TransitService<AccountGroupAccountRequest>
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

        private DateTime mSubmitted;

        public DateTime Submitted
        {
            get
            {
                return mSubmitted;
            }
            set
            {
                mSubmitted = value;
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

        public TransitAccountGroupAccountRequest()
        {

        }

        public TransitAccountGroupAccountRequest(AccountGroupAccountRequest value)
            : base(value)
        {

        }

        public override void SetInstance(AccountGroupAccountRequest value)
        {
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountGroupId = value.AccountGroup.Id;
            AccountGroupName = value.AccountGroup.Name;
            Message = value.Message;
            Submitted = value.Submitted;
            base.SetInstance(value);
        }

        public override AccountGroupAccountRequest GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroupAccountRequest instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.AccountGroup = session.Load<AccountGroup>(AccountGroupId);
                instance.Submitted = DateTime.UtcNow;
                instance.Message = Message;
            }

            return instance;
        }
    }

    public class ManagedAccountGroupAccountRequest : ManagedService<AccountGroupAccountRequest, TransitAccountGroupAccountRequest>
    {
        public ManagedAccountGroupAccountRequest()
        {

        }

        public ManagedAccountGroupAccountRequest(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroupAccountRequest(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroupAccountRequest(ISession session, AccountGroupAccountRequest value)
            : base(session, value)
        {

        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            // everyone is able to request membership in a group 
            // TODO: invitation-only groups
            // if (!mInstance.AccountGroup.IsInviteOnly) 
            acl.Add(new ACLAuthenticatedAllowCreate());
            // requester can cancel and fetch his own request
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.Delete | DataOperation.Retreive));
            // members can delete (approve or deny) this request based on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                if (account.IsAdministrator)
                {
                    acl.Add(new ACLAccount(account.Account, DataOperation.AllExceptUpdate));
                }
            }
            return acl;
        }
    }
}
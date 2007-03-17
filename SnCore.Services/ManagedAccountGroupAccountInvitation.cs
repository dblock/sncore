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

        private bool mRequesterIsAdministrator = false;

        public bool RequesterIsAdministrator
        {
            get
            {

                return mRequesterIsAdministrator;
            }
            set
            {
                mRequesterIsAdministrator = value;
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

        private int mAccountGroupPictureId;

        public int AccountGroupPictureId
        {
            get
            {
                return mAccountGroupPictureId;
            }
            set
            {
                mAccountGroupPictureId = value;
            }
        }

        private bool mAccountGroupIsPrivate;

        public bool AccountGroupIsPrivate
        {
            get
            {
                return mAccountGroupIsPrivate;
            }
            set
            {
                mAccountGroupIsPrivate = value;
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

            RequesterIsAdministrator = false;
            foreach (AccountGroupAccount instance in value.AccountGroup.AccountGroupAccounts)
            {
                if (instance.Account == value.Account && instance.IsAdministrator)
                {
                    RequesterIsAdministrator = true;
                    break;
                }
            }

            RequesterName = value.Requester.Name;
            RequesterPictureId = ManagedAccount.GetRandomAccountPictureId(value.Requester);
            AccountGroupId = value.AccountGroup.Id;
            AccountGroupName = value.AccountGroup.Name;
            if (! value.AccountGroup.IsPrivate) AccountGroupPictureId = ManagedAccountGroup.GetRandomAccountGroupPictureId(value.AccountGroup);
            AccountGroupIsPrivate = value.AccountGroup.IsPrivate;
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
                instance.Account = session.Load<Account>(AccountId);
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

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            // members can create invitations or approve / deny them depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Create));
            }
            // the person who the invitation is for can retreive and delete it
            if (Id > 0) acl.Add(new ACLAccount(mInstance.Account, DataOperation.AllExceptUpdate));
            return acl;
        }

        public override int CreateOrUpdate(TransitAccountGroupAccountInvitation t_instance, ManagedSecurityContext sec)
        {
            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, t_instance.AccountGroupId);
            ManagedAccount m_account = new ManagedAccount(Session, t_instance.AccountId);

            if (m_group.HasAccountInvitation(t_instance.AccountId) || m_group.HasAccountRequest(t_instance.AccountId))
            {
                throw new Exception(string.Format("An invitation for {0} to join \"{1}\" is already pending.",
                    m_account.Instance.Name, m_group.Instance.Name));
            }

            if (m_group.HasAccount(t_instance.AccountId))
            {
                throw new Exception(string.Format("{0} is already a member of \"{1}\".",
                    m_account.Name, m_group.Instance.Name));
            }

            int id = base.CreateOrUpdate(t_instance, sec);

            Session.Flush();

            ManagedAccount recepient = new ManagedAccount(Session, t_instance.AccountId);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                Session, recepient, string.Format("EmailAccountGroupAccountInvitation.aspx?id={0}", id));

            return id;
        }

        public void Reject(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.AllExceptUpdate);

            ManagedAccount recepient = new ManagedAccount(Session, mInstance.Requester);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, recepient,
                string.Format("EmailAccountGroupAccountInvitationReject.aspx?id={0}&aid={1}&message={2}",
                this.Id, sec.Account.Id, Renderer.UrlEncode(message)));

            Session.Delete(mInstance);
        }

        public void Accept(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.AllExceptUpdate);

            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, mInstance.AccountGroup);
            if (!m_group.HasAccount(mInstance.Requester.Id))
            {
                Session.Delete(mInstance);

                throw new Exception(string.Format("Sorry, {0} is no longer member of \"{2}\".",
                    mInstance.Requester.Name));
            }

            if (m_group.Instance.IsPrivate && ! m_group.HasAdministratorAccount(mInstance.Requester.Id))
            {
                TransitAccountGroupAccountRequest t_request = new TransitAccountGroupAccountRequest();
                t_request.AccountGroupId = mInstance.AccountGroup.Id;
                t_request.AccountId = mInstance.Account.Id;
                t_request.Message = string.Format("{0} invited {1} to \"{2}\". " +
                    "The invitation was accepted and needs to be approved by the group administrator.\n{3}",
                    mInstance.Requester.Name, mInstance.Account.Name, mInstance.AccountGroup.Name, message);                
                t_request.Submitted = DateTime.UtcNow;

                ManagedAccountGroupAccountRequest m_request = new ManagedAccountGroupAccountRequest(Session);
                m_request.CreateOrUpdate(t_request, sec);
            }
            else
            {
                AccountGroupAccount account = new AccountGroupAccount();
                account.Account = mInstance.Account;
                account.AccountGroup = mInstance.AccountGroup;
                account.Created = account.Modified = DateTime.UtcNow;
                Session.Save(account);
                Session.Flush();

                ManagedAccount account_recepient = new ManagedAccount(Session, mInstance.Account);
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(Session, account_recepient,
                    string.Format("EmailAccountGroupAccount.aspx?id={0}", account.Id));
            }

            ManagedAccount recepient = new ManagedAccount(Session, mInstance.Requester);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                Session,
                recepient,
                string.Format("EmailAccountGroupAccountInvitationAccept.aspx?id={0}&aid={1}&message={2}",
                this.Id, sec.Account.Id, Renderer.UrlEncode(message)));

            Session.Delete(mInstance);
        }

    }
}
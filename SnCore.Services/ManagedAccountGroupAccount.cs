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
    public class TransitAccountGroupAccount : TransitService<AccountGroupAccount>
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

        private bool mIsAdministrator = false;

        public bool IsAdministrator
        {
            get
            {
                return mIsAdministrator;
            }
            set
            {
                mIsAdministrator = value;
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

        public TransitAccountGroupAccount()
        {

        }

        public TransitAccountGroupAccount(AccountGroupAccount value)
            : base(value)
        {

        }

        public override void SetInstance(AccountGroupAccount value)
        {
            AccountId = value.Account.Id;
            AccountName = value.Account.Name;
            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountGroupId = value.AccountGroup.Id;
            AccountGroupName = value.AccountGroup.Name;
            IsAdministrator = value.IsAdministrator;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountGroupAccount GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountGroupAccount instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Account = GetOwner(session, AccountId, sec);
                instance.AccountGroup = session.Load<AccountGroup>(AccountGroupId);
            }

            instance.IsAdministrator = IsAdministrator;
            return instance;
        }
    }

    public class ManagedAccountGroupAccount : ManagedService<AccountGroupAccount, TransitAccountGroupAccount>
    {
        public ManagedAccountGroupAccount()
        {

        }

        public ManagedAccountGroupAccount(ISession session)
            : base(session)
        {

        }

        public ManagedAccountGroupAccount(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountGroupAccount(ISession session, AccountGroupAccount value)
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
            // everyone is able to see this membership if the group is public
            if (!mInstance.AccountGroup.IsPrivate) acl.Add(new ACLEveryoneAllowRetrieve());
            // everyone is able to join the group if the group is public
            if (!mInstance.AccountGroup.IsPrivate) acl.Add(new ACLAuthenticatedAllowCreate());
            // member can remove and read himself from the group
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.Delete | DataOperation.Retreive));
            // members can edit or see the membership depending on their permissions
            foreach (AccountGroupAccount account in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroup.AccountGroupAccounts))
            {
                acl.Add(new ACLAccount(account.Account, account.IsAdministrator
                    ? DataOperation.All
                    : DataOperation.Retreive));
            }
            return acl;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, mInstance.AccountGroup);
            m_group.Leave(mInstance.Account.Id, sec);
            base.Delete(sec);
        }

        public override int CreateOrUpdate(TransitAccountGroupAccount t_instance, ManagedSecurityContext sec)
        {
            ManagedAccountGroup m_group = new ManagedAccountGroup(Session, t_instance.AccountGroupId);
            if (m_group.HasAccount(t_instance.AccountId))
            {
                throw new SoapException(string.Format(
                    "You are already a member of \"{0}\".", m_group.Instance.Name),
                    SoapException.ClientFaultCode);
            }

            return base.CreateOrUpdate(t_instance, sec);
        }
    }
}
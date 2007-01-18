using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitAccountInvitation : TransitService<AccountInvitation>
    {
        private string mEmail;

        public string Email
        {
            get
            {

                return mEmail;
            }
            set
            {
                mEmail = value;
            }
        }

        private string mCode;

        public string Code
        {
            get
            {

                return mCode;
            }
            set
            {
                mCode = value;
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

        public TransitAccountInvitation()
        {

        }

        public TransitAccountInvitation(AccountInvitation instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountInvitation instance)
        {
            Email = instance.Email;
            // Code = instance.Code;
            AccountId = instance.Account.Id;
            AccountName = instance.Account.Name;
            Message = instance.Message;
            Created = instance.Created;
            Modified = instance.Modified;
            base.SetInstance(instance);
        }

        public override AccountInvitation GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountInvitation instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                // invitations cannot be modified post-send
                instance.Email = this.Email;
                // admin can force a particular code (for unit testing)
                instance.Code = (sec.IsAdministrator() && !string.IsNullOrEmpty(this.Code)) ? this.Code : Guid.NewGuid().ToString();
                instance.Message = this.Message;
                instance.Account = GetOwner(session, AccountId, sec);
            }

            return instance;
        }
    }

    public class ManagedAccountInvitation : ManagedService<AccountInvitation, TransitAccountInvitation>
    {
        public ManagedAccountInvitation()
        {

        }

        public ManagedAccountInvitation(ISession session)
            : base(session)
        {

        }

        public ManagedAccountInvitation(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountInvitation(ISession session, AccountInvitation value)
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

        public string Name
        {
            get
            {
                return mInstance.Email;
            }
        }

        public string Code
        {
            get
            {
                return mInstance.Code;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            mInstance.Account.AccountInvitations.Remove(mInstance);
            base.Delete(sec);
        }

        public void Send()
        {
            ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                Session,
                mInstance.Email,
                string.Format("EmailAccountInvitation.aspx?id={0}", mInstance.Id));
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
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountInvitation t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0)
            {
                sec.CheckVerifiedEmail();
                GetQuota().Check(mInstance.Account.AccountInvitations);
            }
        }
    }
}

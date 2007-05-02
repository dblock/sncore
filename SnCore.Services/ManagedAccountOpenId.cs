using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountOpenId : TransitService<AccountOpenId>
    {
        private string mIdentityUrl;

        public string IdentityUrl
        {
            get
            {

                return mIdentityUrl;
            }
            set
            {
                mIdentityUrl = value;
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

        public TransitAccountOpenId()
        {

        }

        public TransitAccountOpenId(AccountOpenId value)
            : base(value)
        {

        }

        public override void SetInstance(AccountOpenId value)
        {
            AccountId = value.Account.Id;
            IdentityUrl = value.IdentityUrl;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountOpenId GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountOpenId instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.IdentityUrl = IdentityUrl.Trim();
                instance.Account = GetOwner(session, AccountId, sec);
            }

            return instance;
        }
    }

    public class ManagedAccountOpenId : ManagedService<AccountOpenId, TransitAccountOpenId>
    {
        public ManagedAccountOpenId()
        {

        }

        public ManagedAccountOpenId(ISession session)
            : base(session)
        {

        }

        public ManagedAccountOpenId(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountOpenId(ISession session, AccountOpenId value)
            : base(session, value)
        {

        }

        public string IdentityUrl
        {
            get
            {
                return mInstance.IdentityUrl;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            bool canDelete = false;

            // can delete if more than one openid exists
            if (mInstance.Account.AccountOpenIds != null && mInstance.Account.AccountOpenIds.Count > 1)
                canDelete = true;

            // can delete if there're e-mails that allow login
            if (mInstance.Account.AccountEmails != null && mInstance.Account.AccountEmails.Count > 0)
                canDelete = true;

            if (!canDelete)
            {
                throw new Exception("You cannot delete the last open id.");
            }

            Collection<AccountOpenId>.GetSafeCollection(mInstance.Account.AccountOpenIds).Remove(mInstance);
            base.Delete(sec);
        }

        public ManagedAccount Account
        {
            get
            {
                return new ManagedAccount(Session, mInstance.Account);
            }
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
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountOpenId t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota().Check(mInstance.Account.AccountOpenIds);
        }
    }
}

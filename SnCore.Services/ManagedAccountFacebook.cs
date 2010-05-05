using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountFacebook : TransitService<AccountFacebook>
    {
        private long mFacebookAccountId;

        public long FacebookAccountId
        {
            get
            {
                return mFacebookAccountId;
            }
            set
            {
                mFacebookAccountId = value;
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

        public TransitAccountFacebook()
        {

        }

        public TransitAccountFacebook(AccountFacebook value)
            : base(value)
        {

        }

        public override void SetInstance(AccountFacebook value)
        {
            AccountId = value.Account.Id;
            FacebookAccountId = value.FacebookAccountId;
            Created = value.Created;
            Modified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountFacebook GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFacebook instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.FacebookAccountId = FacebookAccountId;
                instance.Account = GetOwner(session, AccountId, sec);
            }

            return instance;
        }
    }

    public class ManagedAccountFacebook : ManagedService<AccountFacebook, TransitAccountFacebook>
    {
        public ManagedAccountFacebook()
        {

        }

        public ManagedAccountFacebook(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFacebook(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFacebook(ISession session, AccountFacebook value)
            : base(session, value)
        {

        }

        public long FacebookAccountId
        {
            get
            {
                return mInstance.FacebookAccountId;
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

            // can delete if more than one facebook id exists
            if (mInstance.Account.AccountFacebooks != null && mInstance.Account.AccountFacebooks.Count > 1)
                canDelete = true;

            // can delete if an open id login exists
            if (mInstance.Account.AccountOpenIds != null && mInstance.Account.AccountOpenIds.Count > 0)
                canDelete = true;

            // can delete if there're e-mails that allow login
            if (mInstance.Account.AccountEmails != null && mInstance.Account.AccountEmails.Count > 0)
                canDelete = true;

            if (!canDelete)
            {
                throw new Exception("You cannot delete the last Facebook id.");
            }

            Collection<AccountFacebook>.GetSafeCollection(mInstance.Account.AccountFacebooks).Remove(mInstance);
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
            AccountFacebook existing_facebook_account = Session.CreateCriteria(typeof(AccountFacebook))
                .Add(Expression.Eq("FacebookAccountId", mInstance.FacebookAccountId))
                .UniqueResult<AccountFacebook>();

            if (existing_facebook_account != null)
            {
                if (existing_facebook_account.Account.Id == mInstance.Account.Id)
                {
                    mInstance = existing_facebook_account;
                }
                else
                {
                    // hijack, since there can only be one
                    Session.Delete(existing_facebook_account);
                }
            }

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

        protected override void Check(TransitAccountFacebook t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountFacebook, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountFacebook instance WHERE instance.Account.Id = {0}",
                        mInstance.Account.Id)).UniqueResult<int>());
        }
    }
}

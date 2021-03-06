using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountEmail : TransitService<AccountEmail>
    {
        private string mAddress;

        public string Address
        {
            get
            {
                return mAddress;
            }
            set
            {
                mAddress = value;
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

        private bool mVerified;

        public bool Verified
        {
            get
            {

                return mVerified;
            }
            set
            {
                mVerified = value;
            }
        }

        private bool mPrincipal;

        public bool Principal
        {
            get
            {

                return mPrincipal;
            }
            set
            {
                mPrincipal = value;
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

        private bool mFailed = false;

        public bool Failed
        {
            get
            {
                return mFailed;
            }
            set
            {
                mFailed = value;
            }
        }

        private string mLastError;

        public string LastError
        {
            get
            {
                return mLastError;
            }
            set
            {
                mLastError = value;
            }
        }

        public TransitAccountEmail()
        {

        }

        public TransitAccountEmail(AccountEmail value)
            : base(value)
        {

        }

        public override void SetInstance(AccountEmail instance)
        {
            AccountId = instance.Account.Id;
            Address = instance.Address;
            Verified = instance.Verified;
            Principal = instance.Principal;
            Created = instance.Created;
            Modified = instance.Modified;
            Failed = instance.Failed;
            LastError = instance.LastError;
            base.SetInstance(instance);
        }

        public override AccountEmail GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEmail instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                instance.Address = Address.Trim();
                instance.Verified = Verified;
                instance.Account = GetOwner(session, AccountId, sec);
            }

            instance.Principal = Principal && instance.Verified;
            instance.Failed = Failed;
            instance.LastError = LastError;
            return instance;
        }
    }

    public class ManagedAccountEmail : ManagedService<AccountEmail, TransitAccountEmail>
    {
        public ManagedAccountEmail()
        {

        }

        public ManagedAccountEmail(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmail(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEmail(ISession session, AccountEmail instance)
            : base(session, instance)
        {

        }

        public string Address
        {
            get
            {
                return mInstance.Address;
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

        public List<ManagedAccountEmailConfirmation> GetManagedAccountEmailConfirmations()
        {
            List<ManagedAccountEmailConfirmation> list = new List<ManagedAccountEmailConfirmation>();
            foreach (AccountEmailConfirmation aec in Collection<AccountEmailConfirmation>.GetSafeCollection(mInstance.AccountEmailConfirmations))
            {
                list.Add(new ManagedAccountEmailConfirmation(Session, aec));
            }
            return list;
        }

        public bool Verified
        {
            get
            {
                return mInstance.Verified;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            bool canDelete = false;

            foreach (AccountEmail email in Collection<AccountEmail>.GetSafeCollection(mInstance.Account.AccountEmails))
            {
                if (email.Id == Id)
                    continue;

                if (!Verified)
                {
                    // you can delete an unverified e-mail when you have at least one other e-mail
                    canDelete = true;
                    break;
                }
                else if (email.Verified)
                {
                    // you can delete a verified e-mail when you have at least one other verified e-mail
                    canDelete = true;
                    break;
                }
            }

            if (!canDelete)
            {
                throw new Exception(
                    "You cannot delete the last verified e-mail.");
            }

            Collection<AccountEmail>.GetSafeCollection(mInstance.Account.AccountEmails).Remove(mInstance);
            mInstance.AccountEmailConfirmations = null;
            base.Delete(sec);

        }

        public ManagedAccount Account
        {
            get
            {
                return new ManagedAccount(Session, mInstance.Account);
            }
        }

        public ManagedAccountEmailConfirmation Confirm(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.All);

            if (this.Verified && ! this.mInstance.Failed)
                return null;

            // find and existing pending confirmation
            foreach (AccountEmailConfirmation c in Collection<AccountEmailConfirmation>.GetSafeCollection(mInstance.AccountEmailConfirmations))
            {
                if (c.AccountEmail.Id == Id)
                {
                    ManagedAccountEmailConfirmation existingac = new ManagedAccountEmailConfirmation(Session, c);
                    existingac.Send();
                    return existingac;
                }
            }

            AccountEmailConfirmation ac = new AccountEmailConfirmation();
            ac.AccountEmail = mInstance;
            ac.Code = Guid.NewGuid().ToString();
            ac.Modified = ac.Created = DateTime.UtcNow;
            Session.Save(ac);

            if (mInstance.AccountEmailConfirmations == null)
            {
                mInstance.AccountEmailConfirmations = new List<AccountEmailConfirmation>();
            }

            mInstance.AccountEmailConfirmations.Add(ac);

            ManagedAccountEmailConfirmation mac = new ManagedAccountEmailConfirmation(Session, ac);
            mac.Send();
            return mac;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;

            if (mInstance.Principal && !mInstance.Verified)
            {
                // an unverified e-mail cannot be set to principal
                throw new ManagedAccount.AccessDeniedException();
            }

            if (mInstance.Principal)
            {
                // clear principal flags if this e-mail becomes principal
                foreach (AccountEmail email in Collection<AccountEmail>.GetSafeCollection(mInstance.Account.AccountEmails))
                {
                    if (email.Principal && email.Id != mInstance.Id)
                    {
                        email.Principal = false;
                        Session.Save(email);
                    }
                }
            }

            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        protected override void Check(TransitAccountEmail t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);
            if (t_instance.Id == 0) GetQuota(sec).Check<AccountEmail, ManagedAccount.QuotaExceededException>(
                    Session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountEmail instance WHERE instance.Account.Id = {0}",
                        mInstance.Account.Id)).UniqueResult<int>());
        }
    }
}

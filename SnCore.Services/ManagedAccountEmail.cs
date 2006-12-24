using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using System.Web.Services.Protocols;

namespace SnCore.Services
{
    public class TransitAccountEmail : TransitService
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

        public TransitAccountEmail()
        {

        }

        public TransitAccountEmail(AccountEmail e)
            : base(e.Id)
        {
            AccountId = e.Account.Id;
            Address = e.Address;
            Verified = e.Verified;
            Principal = e.Principal;
            Created = e.Created;
            Modified = e.Modified;
        }

        public AccountEmail GetAccountEmail(ISession session)
        {
            AccountEmail result = (Id > 0) ? (AccountEmail)session.Load(typeof(AccountEmail), Id) : new AccountEmail();

            if (Id == 0)
            {
                result.Address = Address.Trim();
                result.Verified = Verified;
                if (AccountId > 0) result.Account = (Account)session.Load(typeof(Account), AccountId);
            }

            result.Principal = Principal;
            return result;
        }
    }

    /// <summary>
    /// Managed e-mail.
    /// </summary>
    public class ManagedAccountEmail : ManagedService<AccountEmail>
    {
        private AccountEmail mAccountEmail = null;

        public ManagedAccountEmail(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmail(ISession session, int id)
            : base(session)
        {
            mAccountEmail = (AccountEmail)session.Load(typeof(AccountEmail), id);
        }

        public ManagedAccountEmail(ISession session, TransitAccountEmail tae)
            : base(session)
        {
            mAccountEmail = tae.GetAccountEmail(session);
        }

        public ManagedAccountEmail(ISession session, AccountEmail value)
            : base(session)
        {
            mAccountEmail = value;
        }

        public int Id
        {
            get
            {
                return mAccountEmail.Id;
            }
        }

        public string Address
        {
            get
            {
                return mAccountEmail.Address;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountEmail.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountEmail.Modified;
            }
        }

        public List<ManagedAccountEmailConfirmation> ManagedAccountEmailConfirmations
        {
            get
            {
                List<ManagedAccountEmailConfirmation> list = new List<ManagedAccountEmailConfirmation>();
                foreach (AccountEmailConfirmation aec in mAccountEmail.AccountEmailConfirmations)
                {
                    list.Add(new ManagedAccountEmailConfirmation(Session, aec));
                }
                return list;
            }
        }

        public bool Verified
        {
            get
            {
                return mAccountEmail.Verified;
            }
        }

        public TransitAccountEmail TransitAccountEmail
        {
            get
            {
                return new TransitAccountEmail(mAccountEmail);
            }
        }

        public void Delete()
        {
            bool canDelete = false;

            foreach (AccountEmail email in mAccountEmail.Account.AccountEmails)
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
                throw new SoapException(
                    "You cannot delete the last verified e-mail.",
                    SoapException.ClientFaultCode);
            }

            mAccountEmail.Account.AccountEmails.Remove(mAccountEmail);
            mAccountEmail.AccountEmailConfirmations = null;
            Session.Delete(mAccountEmail);
        }

        public ManagedAccount Account
        {
            get
            {
                return new ManagedAccount(Session, mAccountEmail.Account);
            }
        }

        public ManagedAccountEmailConfirmation Confirm()
        {
            if (this.Verified)
                return null;

            if (mAccountEmail.AccountEmailConfirmations != null)
            {
                // find and existing pending confirmation
                foreach (AccountEmailConfirmation c in mAccountEmail.AccountEmailConfirmations)
                {
                    if (c.AccountEmail.Id == Id)
                    {
                        ManagedAccountEmailConfirmation existingac = new ManagedAccountEmailConfirmation(Session, c);
                        existingac.Send();
                        return existingac;
                    }
                }
            }

            AccountEmailConfirmation ac = new AccountEmailConfirmation();
            ac.AccountEmail = mAccountEmail;
            ac.Code = Guid.NewGuid().ToString();
            Session.Save(ac);

            if (mAccountEmail.AccountEmailConfirmations == null)
            {
                mAccountEmail.AccountEmailConfirmations = new List<AccountEmailConfirmation>();
            }

            mAccountEmail.AccountEmailConfirmations.Add(ac);

            ManagedAccountEmailConfirmation mac = new ManagedAccountEmailConfirmation(Session, ac);
            mac.Send();
            return mac;
        }
    }
}

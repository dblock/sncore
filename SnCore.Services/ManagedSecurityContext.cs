using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class ManagedSecurityContext
    {
        private Account mAccount;

        public Account Account
        {
            get
            {
                return mAccount;
            }
            set
            {
                mAccount = value;
            }
        }

        public ManagedSecurityContext(ISession session)
        {
            mAccount = null;
        }

        public ManagedSecurityContext(ISession session, int id)
        {
            mAccount = session.Load<Account>(id);
        }

        public ManagedSecurityContext(Account value)
        {
            mAccount = value;
        }

        public ManagedSecurityContext(ISession session, string ticket)
        {
            mAccount = null;
            
            try
            {
                if (!string.IsNullOrEmpty(ticket))
                {
                    int id = ManagedAccount.GetAccountIdFromTicket(ticket);
                    mAccount = (id > 0 ? session.Load<Account>(id) : null);
                }
            }
            catch (ManagedAccount.AccessDeniedException)
            {
            }
        }

        public bool IsAdministrator()
        {
            return mAccount != null && mAccount.IsAdministrator;
        }

        private bool HasPicture()
        {
            return Collection<AccountPicture>.GetSafeCollection(mAccount.AccountPictures).Count > 0;
        }

        private bool HasVerifiedEmail()
        {
            foreach (AccountEmail email in Collection<AccountEmail>.GetSafeCollection(mAccount.AccountEmails))
            {
                if (email.Verified)
                {
                    return true;
                }
            }

            return false;
        }

        public void CheckVerified()
        {
            if (IsAdministrator())
                return;

            bool hasVerifiedEmail = HasVerifiedEmail();
            bool hasPicture = HasPicture();

            if (! hasVerifiedEmail && ! hasPicture)
                throw new ManagedAccount.NoVerifiedException();

            if (! hasVerifiedEmail)
                throw new ManagedAccount.NoVerifiedEmailException();

            if (! hasPicture)
                throw new ManagedAccount.NoAccountPictureException();
        }
    }
}

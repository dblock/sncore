using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

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

        public ManagedSecurityContext(ISession session, int id)
        {
            mAccount = (Account) session.Load(typeof(Account), id);
        }

        public ManagedSecurityContext(Account value)
        {
            mAccount = value;
        }

        public ManagedSecurityContext(ISession session, string ticket)
        {
            int id = ManagedAccount.GetAccountId(ticket, 0);
            mAccount = (id > 0 ? (Account) session.Load(typeof(Account), id) : null);
        }

        public bool IsAdministrator()
        {
            return mAccount != null && mAccount.IsAdministrator;
        }
    }
}

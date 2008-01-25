using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using System.Web.Hosting;

namespace SnCore.Services
{
    public abstract class ManagedAuditableService<DatabaseType, TransitType>
        : ManagedService<DatabaseType, TransitType>, IAuditableService
        where DatabaseType : IDbObject, new()
        where TransitType : ITransitService, new()
    {
        private bool mSuppressAccountAudit = false;

        public bool SuppressAccountAudit
        {
            get
            {
                return mSuppressAccountAudit;
            }
            set
            {
                mSuppressAccountAudit = value;
            }
        }

        public ManagedAuditableService()
        {

        }

        public ManagedAuditableService(ISession session)
            : base(session)
        {

        }

        public ManagedAuditableService(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAuditableService(ISession session, DatabaseType instance)
            : base(session, instance)
        {

        }

        public ManagedAuditableService(ISession session, TransitType transitobject, ManagedSecurityContext sec)
            : base(session, transitobject, sec)
        {

        }

        public bool TryAudit(ISession session, DataOperation op, ManagedSecurityContext sec)
        {
            if (SuppressAccountAudit)
                return false;

            IList<AccountAuditEntry> audit_entries = CreateAccountAuditEntries(session, sec, op);

            if (audit_entries == null)
                return false;

            foreach (AccountAuditEntry audit_entry in audit_entries)
                session.Save(audit_entry);

            return true;
        }

        public abstract IList<AccountAuditEntry> CreateAccountAuditEntries(
            ISession session, ManagedSecurityContext sec, DataOperation op);
    }
}

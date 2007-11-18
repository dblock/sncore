using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using NHibernate;

namespace SnCore.Services
{
    public interface IAuditableService
    {
        IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op);
    }
}
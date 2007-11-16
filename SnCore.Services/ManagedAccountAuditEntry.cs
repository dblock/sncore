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
using Rss;
using Atom.Core;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountAuditEntry : TransitService<AccountAuditEntry>
    {
        private string mDescription;

        public string Description
        {
            get
            {
                return mDescription;
            }
            set
            {
                mDescription = value;
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

        private bool mIsSystem;

        public bool IsSystem
        {
            get
            {
                return mIsSystem;
            }
            set
            {
                mIsSystem = value;
            }
        }

        private bool mIsPrivate;

        public bool IsPrivate
        {
            get
            {
                return mIsPrivate;
            }
            set
            {
                mIsPrivate = value;
            }
        }

        public TransitAccountAuditEntry()
        {

        }

        public TransitAccountAuditEntry(AccountAuditEntry instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountAuditEntry instance)
        {
            Description = instance.Description;
            Created = instance.Created;
            AccountId = instance.AccountId;
            IsPrivate = instance.IsPrivate;
            IsSystem = instance.IsSystem;
            base.SetInstance(instance);
        }

        public override AccountAuditEntry GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountAuditEntry instance = base.GetInstance(session, sec);
            instance.Description = this.Description;
            instance.IsPrivate = this.IsPrivate;
            instance.IsSystem = this.IsSystem;
            return instance;
        }
    }

    public class ManagedAccountAuditEntry : ManagedService<AccountAuditEntry, TransitAccountAuditEntry>
    {
        public ManagedAccountAuditEntry()
        {

        }

        public ManagedAccountAuditEntry(ISession session)
            : base(session)
        {

        }

        public ManagedAccountAuditEntry(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountAuditEntry(ISession session, AccountAuditEntry value)
            : base(session, value)
        {

        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());

            if (!mInstance.IsPrivate && !mInstance.IsSystem)
            {
                // friends can retrieve public audit entries
                try
                {
                    Account acct = Session.Load<Account>(mInstance.AccountId);
                    if (acct.AccountFriends != null)
                    {
                        foreach (AccountFriend friend in acct.AccountFriends)
                        {
                            acl.Add(new ACLAccount(friend.Account, DataOperation.Retreive));
                        }
                    }
                }
                catch (ObjectNotFoundException)
                {

                }
            }

            return acl;
        }

        private static AccountAuditEntry CreateOrRetreiveAccountAuditEntry(ISession session, 
            Account account, string descr, bool is_system, bool is_private)
        {
            AccountAuditEntry audit_entry = session.CreateCriteria(typeof(AccountAuditEntry))
                .Add(Expression.Eq("AccountId", account.Id))
                .Add(Expression.Eq("IsSystem", is_system))
                .Add(Expression.Eq("IsPrivate", is_private))
                .Add(Expression.Eq("Description", descr))
                .UniqueResult<AccountAuditEntry>();

            if (audit_entry == null)
            {
                audit_entry = new AccountAuditEntry();
                audit_entry.AccountId = account.Id;
                audit_entry.IsSystem = is_system;
                audit_entry.IsPrivate = is_private;
                audit_entry.Description = descr;
                audit_entry.Created = audit_entry.Updated = DateTime.UtcNow;
                audit_entry.Count = 1;
            }
            else 
            {
                audit_entry.Count++;
                audit_entry.Updated = DateTime.UtcNow;
            }

            return audit_entry;
        }

        public static AccountAuditEntry CreateSystemAccountAuditEntry(ISession session, Account account, string descr)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, true, false);
        }

        public static AccountAuditEntry CreatePublicAccountAuditEntry(ISession session, Account account, string descr)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, false, false);
        }

        public static AccountAuditEntry CreatePrivateAccountAuditEntry(ISession session, Account account, string descr)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, false, true);
        }
    }
}
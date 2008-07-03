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
    public class TransitAccountAuditEntryQueryOptions
    {
        public string SortOrder = "Updated";
        public bool SortAscending = false;
        public bool System = false;
        public bool Private = false;
        public bool Broadcast = false;
        public int AccountId = 0;
        public bool Friends = true; // show friends' activity vs. my own

        public TransitAccountAuditEntryQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (AccountId != 0)
            {
                if (Friends)
                {
                    b.Append(b.Length > 0 ? " AND " : " WHERE ");
                    b.AppendFormat("( AccountFriend.Account_Id = {0} OR AccountFriend.Keen_Id = {0} )", AccountId);
                    b.AppendFormat(" AND AccountAuditEntry.Account_Id <> {0}", AccountId);
                }
                else
                {
                    b.Append(b.Length > 0 ? " AND " : " WHERE ");
                    b.AppendFormat(" AccountAuditEntry.Account_Id = {0}", AccountId);
                }
            }

            if (!System)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountAuditEntry.IsSystem <> 1");
            }

            if (!Private)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountAuditEntry.IsPrivate <> 1");
            }

            if (!Broadcast)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountAuditEntry.IsBroadcast <> 1");
            }
            else
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("AccountAuditEntry.IsBroadcast = 1");
            }

            if (Friends)
            {
                b.Insert(0, "INNER JOIN AccountFriend AccountFriend ON ( " +
                    "AccountAuditEntry.Account_Id = AccountFriend.Account_Id " +
                    "OR AccountAuditEntry.Account_Id = AccountFriend.Keen_Id )");
            }

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT {AccountAuditEntry.*} FROM AccountAuditEntry {AccountAuditEntry} ");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY AccountAuditEntry.{0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return b.ToString();
        }
    }

    public class TransitAccountAuditEntry : TransitService<AccountAuditEntry>
    {
        private int mAccountPictureId;

        public int AccountPictureId
        {
            get
            {
                return mAccountPictureId;
            }
            set
            {
                mAccountPictureId = value;
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

        private string mUrl;

        public string Url
        {
            get
            {
                return mUrl;
            }
            set
            {
                mUrl = value;
            }
        }

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

        private DateTime mUpdated;

        public DateTime Updated
        {
            get
            {
                return mUpdated;
            }
            set
            {
                mUpdated = value;
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

        private bool mIsBroadcast;

        public bool IsBroadcast
        {
            get
            {
                return mIsBroadcast;
            }
            set
            {
                mIsBroadcast = value;
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
            Updated = instance.Updated;
            AccountId = instance.AccountId;
            IsPrivate = instance.IsPrivate;
            IsSystem = instance.IsSystem;
            IsBroadcast = instance.IsBroadcast;
            Url = instance.Url;
            base.SetInstance(instance);
        }

        public override AccountAuditEntry GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountAuditEntry instance = base.GetInstance(session, sec);
            instance.Description = this.Description;
            instance.IsPrivate = this.IsPrivate;
            instance.IsSystem = this.IsSystem;
            instance.IsBroadcast = this.IsBroadcast;
            instance.Url = this.Url;
            instance.AccountId = GetOwner(session, AccountId, sec).Id;
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
            mInstance.Updated = DateTime.UtcNow;
            mInstance.Md5 = GetHash(mInstance.Description);
            if (mInstance.Id == 0) mInstance.Created = mInstance.Updated;
            base.Save(sec);
        }

        public override int CreateOrUpdate(TransitAccountAuditEntry t_instance, ManagedSecurityContext sec)
        {
            string hash = GetHash(t_instance.Description);
            AccountAuditEntry audit_entry = Session.CreateCriteria(typeof(AccountAuditEntry))
                .Add(Expression.Eq("AccountId", t_instance.AccountId))
                .Add(Expression.Eq("IsSystem", t_instance.IsSystem))
                .Add(Expression.Eq("IsPrivate", t_instance.IsPrivate))
                .Add(Expression.Eq("Md5", hash))
                .UniqueResult<AccountAuditEntry>();

            if (audit_entry != null)
            {
                audit_entry.Count++;
                audit_entry.Updated = DateTime.UtcNow;
                mInstance = audit_entry;
                return audit_entry.Id;
            }
            else
            {
                return base.CreateOrUpdate(t_instance, sec);
            }
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());

            // user can view his own activity and delete broadcasts
            try
            {
                Account acct = Session.Load<Account>(mInstance.AccountId);
                acl.Add(new ACLAccount(acct, mInstance.IsBroadcast ? DataOperation.AllExceptUpdate : DataOperation.Retreive));
            }
            catch (ObjectNotFoundException)
            {

            }

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
                            acl.Add(new ACLAccount(friend.Keen, DataOperation.Retreive));
                        }
                    }

                    if (acct.KeenAccountFriends != null)
                    {
                        foreach (AccountFriend friend in acct.KeenAccountFriends)
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

        public static string GetHash(string description)
        {
            return Encoding.Default.GetString(GetHashBytes(description));
        }

        public static byte[] GetHashBytes(string description)
        {
            if (description == null) description = string.Empty;
            return new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(description));
        }

        private static AccountAuditEntry CreateOrRetreiveAccountAuditEntry(ISession session, TransitAccountAuditEntry t_instance)
        {
            string hash = GetHash(t_instance.Description);
            AccountAuditEntry audit_entry = session.CreateCriteria(typeof(AccountAuditEntry))
                .Add(Expression.Eq("AccountId", t_instance.AccountId))
                .Add(Expression.Eq("IsSystem", t_instance.IsSystem))
                .Add(Expression.Eq("IsPrivate", t_instance.IsPrivate))
                .Add(Expression.Eq("Md5", hash))
                .UniqueResult<AccountAuditEntry>();

            if (audit_entry == null)
            {
                audit_entry = new AccountAuditEntry();
                audit_entry.AccountId = t_instance.AccountId;
                audit_entry.IsSystem = t_instance.IsSystem;
                audit_entry.IsPrivate = t_instance.IsPrivate;
                audit_entry.IsBroadcast = t_instance.IsBroadcast;
                audit_entry.Description = t_instance.Description;
                audit_entry.Url = t_instance.Url;
                audit_entry.Md5 = hash;
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

        private static AccountAuditEntry CreateOrRetreiveAccountAuditEntry(ISession session,
            Account account, string descr, string url, bool is_system, bool is_private)
        {
            TransitAccountAuditEntry t_instance = new TransitAccountAuditEntry();
            t_instance.AccountId = account.Id;
            t_instance.Description = descr;
            t_instance.Url = url;
            t_instance.IsSystem = is_system;
            t_instance.IsPrivate = is_private;
            t_instance.IsBroadcast = false;
            return CreateOrRetreiveAccountAuditEntry(session, t_instance);
        }

        public static AccountAuditEntry CreateSystemAccountAuditEntry(ISession session, Account account, string descr, string url)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, url, true, false);
        }

        public static AccountAuditEntry CreatePublicAccountAuditEntry(ISession session, Account account, string descr, string url)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, url, false, false);
        }

        public static AccountAuditEntry CreatePrivateAccountAuditEntry(ISession session, Account account, string descr, string url)
        {
            return CreateOrRetreiveAccountAuditEntry(
                session, account, descr, url, false, true);
        }

        public override TransitAccountAuditEntry GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountAuditEntry t_instance = base.GetTransitInstance(sec);

            try
            {
                Account acct = Session.Load<Account>(mInstance.AccountId);
                t_instance.AccountName = acct.Name;
                t_instance.AccountPictureId = ManagedAccount.GetRandomAccountPictureId(acct);
            }
            catch (ObjectNotFoundException)
            {

            }

            return t_instance;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            ManagedDiscussion.FindAndDelete(Session, mInstance.AccountId, typeof(AccountAuditEntry), mInstance.Id, sec);
            base.Delete(sec);
        }
    }
}
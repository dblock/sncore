using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Expression;

namespace SnCore.Services
{
    public enum DataOperation
    {
        None = 0,
        Create = 1,
        Retreive = 2,
        Update = 4,
        Delete = 8,
        All = Create | Retreive | Update | Delete,
        AllExceptUpdate = Create | Retreive | Delete
    };

    public enum DataOperationPermission
    {
        Deny,
        Allow,
    };

    public enum ACLVerdict
    {
        None,
        Denied,
        Allowed
    };

    public interface IACLEntry
    {
        ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op);
    };

    public abstract class ACLBaseEntry : IACLEntry
    {
        protected int mOperation = 0;
        protected DataOperationPermission mPermission = DataOperationPermission.Deny;

        public ACLBaseEntry(DataOperation op)
            : this((int)op, DataOperationPermission.Allow)
        {

        }

        public ACLBaseEntry(DataOperation op, DataOperationPermission perm)
            : this((int)op, perm)
        {

        }

        public ACLBaseEntry(int op, DataOperationPermission perm)
        {
            mOperation = op;
            mPermission = perm;
        }

        public abstract ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op);
    }

    public class ACLEveryoneAllowRetrieve : ACLBaseEntry
    {
        public ACLEveryoneAllowRetrieve()
            : base(DataOperation.Retreive, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            return (op == DataOperation.Retreive) ? ACLVerdict.Allowed : ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowRetrieve : ACLBaseEntry
    {
        public ACLAuthenticatedAllowRetrieve()
            : base(DataOperation.Retreive, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if (op == DataOperation.Retreive && sec.Account != null)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowCreateAndDelete : ACLBaseEntry
    {
        public ACLAuthenticatedAllowCreateAndDelete()
            : base(DataOperation.Create | DataOperation.Delete, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if ((op == DataOperation.Create || op == DataOperation.Delete) && sec.Account != null)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAuthenticatedAllowCreate : ACLBaseEntry
    {
        public ACLAuthenticatedAllowCreate()
            : base(DataOperation.Create, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if (op == DataOperation.Create && sec.Account != null)
            {
                return ACLVerdict.Allowed;
            }

            return ACLVerdict.None;
        }
    }

    public class ACLAccount : ACLBaseEntry
    {
        private Account mAccount = null;

        public ACLAccount(Account value, int op)
            : this(value, op, DataOperationPermission.Allow)
        {

        }

        public ACLAccount(Account value, DataOperation op)
            : this(value, (int)op, DataOperationPermission.Allow)
        {

        }

        public ACLAccount(Account value, int op, DataOperationPermission perm)
            : base(op, perm)
        {
            mAccount = value;
        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if (sec.Account == null || mAccount == null)
                return ACLVerdict.None;

            if (sec.Account.Id != mAccount.Id)
                return ACLVerdict.None;

            if ((mOperation & (int)op) == 0)
                return ACLVerdict.None;

            return mPermission == DataOperationPermission.Allow
                ? ACLVerdict.Allowed
                : ACLVerdict.Denied;
        }

        public static IList<IACLEntry> GetACLEntries(IList<Account> accounts, int op, DataOperationPermission perm)
        {
            List<IACLEntry> result = new List<IACLEntry>();
            foreach (Account account in accounts)
            {
                result.Add(new ACLAccount(account, op, perm));
            }
            return result;
        }
    }

    public class ACLAccountId : ACLBaseEntry
    {
        private int mAccountId = 0;

        public ACLAccountId(int value, int op)
            : this(value, op, DataOperationPermission.Allow)
        {

        }

        public ACLAccountId(int value, DataOperation op)
            : this(value, (int)op, DataOperationPermission.Allow)
        {

        }

        public ACLAccountId(int value, int op, DataOperationPermission perm)
            : base(op, perm)
        {
            mAccountId = value;
        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if (sec.Account == null)
                return ACLVerdict.None;

            if (sec.Account.Id != mAccountId)
                return ACLVerdict.None;

            if ((mOperation & (int)op) == 0)
                return ACLVerdict.None;

            return mPermission == DataOperationPermission.Allow
                ? ACLVerdict.Allowed
                : ACLVerdict.Denied;
        }

        public static IList<IACLEntry> GetACLEntries(IList<Account> accounts, int op, DataOperationPermission perm)
        {
            List<IACLEntry> result = new List<IACLEntry>();
            foreach (Account account in accounts)
            {
                result.Add(new ACLAccountId(account.Id, op, perm));
            }
            return result;
        }
    }

    public class ACL
    {
        private static ACL s_AdminACL = new ACL();

        private List<IACLEntry> mAccessControlList = new List<IACLEntry>();

        public ACL()
        {

        }

        public ACL(ACL value)
        {
            mAccessControlList.AddRange(value.mAccessControlList);
        }

        public ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            ACLVerdict current = ACLVerdict.Denied;

            foreach (IACLEntry entry in mAccessControlList)
            {
                ACLVerdict result = entry.Apply(sec, op);
                switch (result)
                {
                    case ACLVerdict.Denied:
                        return ACLVerdict.Denied;
                    case ACLVerdict.Allowed:
                        current = ACLVerdict.Allowed;
                        break;
                }
            }

            return current;
        }

        public void Check(ManagedSecurityContext sec, DataOperation op)
        {
            ACLVerdict result = Apply(sec, op);
            switch (result)
            {
                case ACLVerdict.Denied:
                case ACLVerdict.None:
                    throw new ManagedAccount.AccessDeniedException();
            }
        }

        public void Add(IACLEntry value)
        {
            mAccessControlList.Add(value);
        }

        public void AddRange(IList<IACLEntry> collection)
        {
            mAccessControlList.AddRange(collection);
        }

        public int Count
        {
            get
            {
                return mAccessControlList.Count;
            }
        }

        /// <summary>
        /// all-admin ACL
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static ACL GetAdministrativeACL(ISession session)
        {
            lock (s_AdminACL)
            {
                if (s_AdminACL.Count == 0)
                {
                    IList<Account> admins = session.CreateCriteria(typeof(Account)).Add(Expression.Eq("IsAdministrator", true)).List<Account>();
                    s_AdminACL.AddRange(ACLAccountId.GetACLEntries(admins, (int)DataOperation.All, DataOperationPermission.Allow));
                }
            }

            ACL acl = new ACL(s_AdminACL);
            return acl;
        }
    }
}

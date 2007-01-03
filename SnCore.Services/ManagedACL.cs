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
        All = Create | Retreive | Update | Delete
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
            : this((int) op, DataOperationPermission.Allow)
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

    public class ACLEveryoneAllowCreate : ACLBaseEntry
    {
        public ACLEveryoneAllowCreate()
            : base(DataOperation.Create, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            return (op == DataOperation.Create) ? ACLVerdict.Allowed : ACLVerdict.None;
        }
    }

    public class ACLEveryoneAllowCreateAndDelete : ACLBaseEntry
    {
        public ACLEveryoneAllowCreateAndDelete()
            : base(DataOperation.Create | DataOperation.Delete, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            return (op == DataOperation.Create || op == DataOperation.Delete) ? ACLVerdict.Allowed : ACLVerdict.None;
        }
    }

    public class ACLEveryoneAllowCreateAndRetrieve : ACLBaseEntry
    {
        public ACLEveryoneAllowCreateAndRetrieve()
            : base(DataOperation.Retreive | DataOperation.Create, DataOperationPermission.Allow)
        {

        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            return (op == DataOperation.Retreive || op == DataOperation.Create) ? ACLVerdict.Allowed : ACLVerdict.None;
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
            : this(value, (int) op, DataOperationPermission.Allow)
        {

        }

        public ACLAccount(Account value, int op, DataOperationPermission perm)
            : base(op, perm)
        {
            mAccount = value;
        }

        public override ACLVerdict Apply(ManagedSecurityContext sec, DataOperation op)
        {
            if (sec.Account != mAccount)
                return ACLVerdict.None;

            if ((mOperation & (int) op) == 0)
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

    public class ACL
    {
        private List<IACLEntry> mAccessControlList = new List<IACLEntry>();

        public ACL()
        {

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

        /// <summary>
        /// all-admin ACL
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static ACL GetAdministrativeACL(ISession session)
        {            
            // TODO: cache the all-admin ACL
            ACL m_acl = new ACL();
            IList<Account> admins = session.CreateCriteria(typeof(Account)).Add(Expression.Eq("IsAdministrator", true)).List<Account>();
            m_acl.AddRange(ACLAccount.GetACLEntries(admins, (int) DataOperation.All, DataOperationPermission.Allow));
            return m_acl;
        }
    }
}

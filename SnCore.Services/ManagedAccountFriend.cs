using System;
using NHibernate;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Expression;
using SnCore.Data.Hibernate;
using SnCore.Tools;
using SnCore.Tools.Web;
using System.Text;

namespace SnCore.Services
{
    public class TransitAccountFriendQueryOptions
    {
        public string SortOrder = "Created";
        public bool SortAscending = false;
        public string Name;
        public int AccountId = 0;

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }

        public TransitAccountFriendQueryOptions()
        {
        }

        public string CreateSubQuery()
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                
                string like = (Name.Length == 1) 
                    ? string.Format("{0}%", Renderer.SqlEncode(Name)) 
                    : string.Format("%{0}%", Renderer.SqlEncode(Name));

                b.AppendFormat("((AccountFriend.Account.Id != {0} AND AccountFriend.Account.Name LIKE '{1}%') OR (AccountFriend.Keen.Id != {0} AND AccountFriend.Keen.Name LIKE '{1}%'))", 
                    AccountId,
                    like);
            }

            b.Append(b.Length > 0 ? " AND " : " WHERE ");
            b.AppendFormat("(AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0})", AccountId);

            return b.ToString();
        }

        public string CreateCountQuery()
        {
            return CreateSubQuery();
        }

        public string CreateQuery()
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT AccountFriend FROM AccountFriend AccountFriend");
            b.Append(CreateSubQuery());
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY AccountFriend.{0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }
            return b.ToString();
        }
    };

    public class TransitAccountFriend : TransitService<AccountFriend>
    {
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

        private int mFriendId;

        public int FriendId
        {
            get
            {

                return mFriendId;
            }
            set
            {
                mFriendId = value;
            }
        }

        private int mFriendPictureId;

        public int FriendPictureId
        {
            get
            {

                return mFriendPictureId;
            }
            set
            {
                mFriendPictureId = value;
            }
        }

        private string mFriendName;

        public string FriendName
        {
            get
            {

                return mFriendName;
            }
            set
            {
                mFriendName = value;
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

        public TransitAccountFriend()
        {

        }

        public TransitAccountFriend(AccountFriend instance)
            : base(instance)
        {

        }

        public override void  SetInstance(AccountFriend instance)
        {
            FriendId = instance.Keen.Id;
            FriendPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Keen);
            FriendName = instance.Keen.Name;
            AccountId = instance.Account.Id;
            Created = instance.Created;
            base.SetInstance(instance);
        }

        public override AccountFriend GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFriend instance = base.GetInstance(session, sec);
            if (instance.Id == 0)
            {
                if (FriendId > 0) instance.Keen = session.Load<Account>(FriendId);
                if (AccountId > 0) instance.Account = session.Load<Account>(AccountId);
            }
            return instance;
        }
    }

    public class ManagedAccountFriend : ManagedAuditableService<AccountFriend, TransitAccountFriend>
    {
        public ManagedAccountFriend()
        {

        }

        public ManagedAccountFriend(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFriend(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFriend(ISession session, AccountFriend value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountFriend>.GetSafeCollection(mInstance.Account.AccountFriends).Remove(mInstance);

            // delete the friend in the other direction
            foreach (AccountFriend friend in Collection<AccountFriend>.GetSafeCollection(mInstance.Keen.AccountFriends))
            {
                if (friend.Keen.Id == mInstance.Account.Id)
                {
                    Session.Delete(friend);
                    mInstance.Keen.AccountFriends.Remove(friend);
                    break;
                }
            }

            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow; 
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            acl.Add(new ACLAccount(mInstance.Keen, DataOperation.All));
            return acl;
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();
            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.Account,
                        string.Format("[user:{0}] and [user:{1}] are now friends", mInstance.Keen.Id, mInstance.Account.Id),
                        string.Format("AccountView.aspx?id={0}", mInstance.Keen.Id)));
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance.Keen,
                        string.Format("[user:{0}] and [user:{1}] are now friends", mInstance.Account.Id, mInstance.Keen.Id),
                        string.Format("AccountView.aspx?id={0}", mInstance.Keen.Id)));
                    break;
                case DataOperation.Delete:
                    result.Add(ManagedAccountAuditEntry.CreateSystemAccountAuditEntry(session, mInstance.Account,
                        string.Format("[user:{0}] and [user:{1}] are no longer friends", mInstance.Keen.Id, mInstance.Account.Id),
                        string.Format("AccountView.aspx?id={0}", mInstance.Keen.Id)));
                    result.Add(ManagedAccountAuditEntry.CreateSystemAccountAuditEntry(session, mInstance.Keen,
                        string.Format("[user:{0}] and [user:{1}] are no longer friends", mInstance.Account.Id, mInstance.Keen.Id),
                        string.Format("AccountView.aspx?id={0}", mInstance.Keen.Id)));
                    break;
            }
            return result;
        }
    }
}

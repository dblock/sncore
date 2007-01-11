using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountFriend : TransitService<AccountFriend>
    {
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
            : this(instance, false)
        {

        }

        public TransitAccountFriend(AccountFriend instance, bool invert)
            : base(instance)
        {
            if (invert)
            {
                FriendId = instance.Account.Id;
                FriendPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Account);
                FriendName = instance.Account.Name;
            }
        }

        public override void  SetInstance(AccountFriend instance)
        {
            FriendId = instance.Keen.Id;
            FriendPictureId = ManagedAccount.GetRandomAccountPictureId(instance.Keen);
            FriendName = instance.Keen.Name;
            Created = instance.Created;
            base.SetInstance(instance);
        }

        public override AccountFriend GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFriend instance = base.GetInstance(session, sec);
            if (FriendId > 0) instance.Keen = (Account) session.Load(typeof(Account), FriendId);
            return instance;
        }
    }

    public class ManagedAccountFriend : ManagedService<AccountFriend, TransitAccountFriend>
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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            acl.Add(new ACLAccount(mInstance.Keen, DataOperation.All));
            return acl;
        }
    }
}

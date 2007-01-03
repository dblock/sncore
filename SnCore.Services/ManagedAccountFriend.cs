using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;

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
        private AccountFriend mAccountFriend = null;

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

        public int AccountId
        {
            get
            {
                return mAccountFriend.Account.Id;
            }
        }

        public int KeenId
        {
            get
            {
                return mAccountFriend.Keen.Id;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            mAccountFriend.Account.AccountFriends.Remove(mAccountFriend);

            // delete the friend in the other direction
            foreach (AccountFriend friend in mAccountFriend.Keen.AccountFriends)
            {
                if (friend.Keen.Id == mAccountFriend.Account.Id)
                {
                    Session.Delete(friend);
                    mAccountFriend.Keen.AccountFriends.Remove(friend);
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
            acl.Add(new ACLEveryoneAllowCreateAndRetrieve());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

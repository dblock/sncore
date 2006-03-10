using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;

namespace SnCore.Services
{
    public class TransitAccountFriend : TransitService
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

        public TransitAccountFriend(AccountFriend e)
            : this(e, false)
        {

        }

        public TransitAccountFriend(AccountFriend e, bool invert)
            : base(e.Id)
        {
            FriendId = invert ? e.Account.Id : e.Keen.Id;
            FriendPictureId = ManagedService.GetRandomElementId(invert ? e.Account.AccountPictures : e.Keen.AccountPictures);
            FriendName = invert ? e.Account.Name : e.Keen.Name;
            Created = e.Created;
        }
    }

    /// <summary>
    /// Managed friend.
    /// </summary>
    public class ManagedAccountFriend : ManagedService
    {
        private AccountFriend mAccountFriend = null;

        public ManagedAccountFriend(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFriend(ISession session, int id)
            : base(session)
        {
            mAccountFriend = (AccountFriend)session.Load(typeof(AccountFriend), id);
        }

        public ManagedAccountFriend(ISession session, AccountFriend value)
            : base(session)
        {
            mAccountFriend = value;
        }

        public int Id
        {
            get
            {
                return mAccountFriend.Id;
            }
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

        public TransitAccountFriend TransitAccountFriend
        {
            get
            {
                return new TransitAccountFriend(mAccountFriend);
            }
        }

        public void Delete()
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

            Session.Delete(mAccountFriend);
        }
    }
}

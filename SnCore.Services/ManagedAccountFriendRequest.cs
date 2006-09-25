using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using SnCore.Tools.Web;
using System.Net.Mail;

namespace SnCore.Services
{
    public class TransitAccountFriendRequest : TransitService
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

        private int mKeenId;

        public int KeenId
        {
            get
            {

                return mKeenId;
            }
            set
            {
                mKeenId = value;
            }
        }

        private int mKeenPictureId;

        public int KeenPictureId
        {
            get
            {

                return mKeenPictureId;
            }
            set
            {
                mKeenPictureId = value;
            }
        }

        private string mKeenName;

        public string KeenName
        {
            get
            {

                return mKeenName;
            }
            set
            {
                mKeenName = value;
            }
        }

        private string mMessage;

        public string Message
        {
            get
            {

                return mMessage;
            }
            set
            {
                mMessage = value;
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

        public TransitAccountFriendRequest()
        {

        }

        public TransitAccountFriendRequest(AccountFriendRequest e)
            : base(e.Id)
        {
            KeenPictureId = ManagedService.GetRandomElementId(e.Keen.AccountPictures);
            KeenName = e.Keen.Name;
            KeenId = e.Keen.Id;

            AccountPictureId = ManagedService.GetRandomElementId(e.Account.AccountPictures);
            AccountName = e.Account.Name;
            AccountId = e.Account.Id;

            Message = e.Message;
            Created = e.Created;
        }
    }

    /// <summary>
    /// Managed FriendRequest.
    /// </summary>
    public class ManagedAccountFriendRequest : ManagedService
    {
        private AccountFriendRequest mAccountFriendRequest = null;

        public ManagedAccountFriendRequest(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFriendRequest(ISession session, int id)
            : base(session)
        {
            mAccountFriendRequest = (AccountFriendRequest)session.Load(typeof(AccountFriendRequest), id);
        }

        public ManagedAccountFriendRequest(ISession session, AccountFriendRequest value)
            : base(session)
        {
            mAccountFriendRequest = value;
        }

        public int Id
        {
            get
            {
                return mAccountFriendRequest.Id;
            }
        }

        public TransitAccountFriendRequest TransitAccountFriendRequest
        {
            get
            {
                return new TransitAccountFriendRequest(mAccountFriendRequest);
            }
        }

        public void Delete()
        {
            mAccountFriendRequest.Account.AccountFriendRequests.Remove(mAccountFriendRequest);
            Session.Delete(mAccountFriendRequest);
        }

        public void Reject(string message)
        {
            Account requester = mAccountFriendRequest.Account;
            Account approver = mAccountFriendRequest.Keen;

            if (message != null && message.Length > 0)
            {
                ManagedAccount recepient = new ManagedAccount(Session, requester);
                string sentto = recepient.ActiveEmailAddress;
                if (sentto != null)
                {
                    ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                        Session,
                        new MailAddress(sentto, recepient.Name).ToString(),
                        string.Format("EmailAccountFriendRequestReject.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
                }

                // delete the request when user notified
                if (mAccountFriendRequest.Account.AccountFriendRequests != null)
                    mAccountFriendRequest.Account.AccountFriendRequests.Remove(mAccountFriendRequest);

                Session.Delete(mAccountFriendRequest);
            }
            else
            {
                // silently reject the request
                mAccountFriendRequest.Rejected = true;
                Session.Save(mAccountFriendRequest);
            }
        }

        public void Accept(string message)
        {
            AccountFriend friend = new AccountFriend();
            friend.Account = mAccountFriendRequest.Account;
            friend.Keen = mAccountFriendRequest.Keen;
            friend.Created = DateTime.UtcNow;
            Session.Save(friend);

            ManagedAccount recepient = new ManagedAccount(Session, friend.Account);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountFriendRequestAccept.aspx?id={0}&message={1}", this.Id, Renderer.UrlEncode(message)));
            }

            Session.Delete(mAccountFriendRequest);

            // delete a reciproque request if any
            AccountFriendRequest rr = (AccountFriendRequest)Session.CreateCriteria(typeof(AccountFriendRequest))
                .Add(Expression.Eq("Account.Id", mAccountFriendRequest.Keen.Id))
                .Add(Expression.Eq("Keen.Id", mAccountFriendRequest.Account.Id))
                .UniqueResult();

            if (rr != null)
            {
                Session.Delete(rr);
            }
                
            if (mAccountFriendRequest.Account.AccountFriendRequests != null)
                mAccountFriendRequest.Account.AccountFriendRequests.Remove(mAccountFriendRequest);

            if (friend.Account.AccountFriends == null) friend.Account.AccountFriends = new ArrayList();
            friend.Account.AccountFriends.Add(friend);
        }

        public int AccountId
        {
            get
            {
                return mAccountFriendRequest.Account.Id;
            }
        }

        public int KeenId
        {
            get
            {
                return mAccountFriendRequest.Keen.Id;
            }
        }
    }
}

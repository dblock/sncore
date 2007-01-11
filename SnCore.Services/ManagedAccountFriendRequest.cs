using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Expression;
using SnCore.Tools.Web;
using System.Net.Mail;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountFriendRequest : TransitService<AccountFriendRequest>
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

        public TransitAccountFriendRequest(AccountFriendRequest value)
            : base(value)
        {

        }

        public override void SetInstance(AccountFriendRequest value)
        {
            KeenPictureId = ManagedAccount.GetRandomAccountPictureId(value.Keen);
            KeenName = value.Keen.Name;
            KeenId = value.Keen.Id;

            AccountPictureId = ManagedAccount.GetRandomAccountPictureId(value.Account);
            AccountName = value.Account.Name;
            AccountId = value.Account.Id;

            Message = value.Message;
            Created = value.Created;
            base.SetInstance(value);
        }

        public override AccountFriendRequest GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountFriendRequest instance = base.GetInstance(session, sec);
            if (KeenId > 0) instance.Keen = (Account) session.Load(typeof(Account), KeenId);
            if (AccountId > 0) instance.Account = (Account) session.Load(typeof(Account), AccountId);
            instance.Message = Message;
            return instance;
        }
    }

    public class ManagedAccountFriendRequest : ManagedService<AccountFriendRequest, TransitAccountFriendRequest>
    {
        public ManagedAccountFriendRequest()
        {

        }

        public ManagedAccountFriendRequest(ISession session)
            : base(session)
        {

        }

        public ManagedAccountFriendRequest(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountFriendRequest(ISession session, AccountFriendRequest value)
            : base(session, value)
        {

        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountFriendRequest>.GetSafeCollection(mInstance.Account.AccountFriendRequests).Remove(mInstance);
            base.Delete(sec);
        }

        public void Reject(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.Update);

            Account requester = mInstance.Account;
            Account approver = mInstance.Keen;

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
                Collection<AccountFriendRequest>.GetSafeCollection(mInstance.Account.AccountFriendRequests).Remove(mInstance);

                Session.Delete(mInstance);
            }
            else
            {
                // silently reject the request
                mInstance.Rejected = true;
                Session.Save(mInstance);
            }
        }

        public void Accept(ManagedSecurityContext sec, string message)
        {
            GetACL().Check(sec, DataOperation.Update);

            AccountFriend friend = new AccountFriend();
            friend.Account = mInstance.Account;
            friend.Keen = mInstance.Keen;
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

            Session.Delete(mInstance);

            // delete a reciproque request if any
            AccountFriendRequest rr = (AccountFriendRequest)Session.CreateCriteria(typeof(AccountFriendRequest))
                .Add(Expression.Eq("Account.Id", mInstance.Keen.Id))
                .Add(Expression.Eq("Keen.Id", mInstance.Account.Id))
                .UniqueResult();

            if (rr != null)
            {
                Session.Delete(rr);
            }

            Collection<AccountFriendRequest>.GetSafeCollection(mInstance.Account.AccountFriendRequests).Remove(mInstance);

            if (friend.Account.AccountFriends == null) friend.Account.AccountFriends = new List<AccountFriend>();
            friend.Account.AccountFriends.Add(friend);
        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public int KeenId
        {
            get
            {
                return mInstance.Keen.Id;
            }
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            if (mInstance.Id == 0) mInstance.Created = DateTime.UtcNow;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            acl.Add(new ACLAccount(mInstance.Keen, DataOperation.Update | DataOperation.Retreive | DataOperation.Delete));
            return acl;
        }
    }
}

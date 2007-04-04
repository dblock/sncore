using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Collections.Generic;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountMessage : TransitService<AccountMessage>
    {
        private string mSubject;

        public string Subject
        {
            get
            {

                return mSubject;
            }
            set
            {
                mSubject = value;
            }
        }

        private string mBody;

        public string Body
        {
            get
            {

                return mBody;
            }
            set
            {
                mBody = value;
            }
        }

        private DateTime mSent;

        public DateTime Sent
        {
            get
            {

                return mSent;
            }
            set
            {
                mSent = value;
            }
        }

        private bool mUnRead;

        public bool UnRead
        {
            get
            {

                return mUnRead;
            }
            set
            {
                mUnRead = value;
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

        private int mInstanceFolderId;

        public int AccountMessageFolderId
        {
            get
            {

                return mInstanceFolderId;
            }
            set
            {
                mInstanceFolderId = value;
            }
        }

        private int mSenderAccountId = 0;

        public int SenderAccountId
        {
            get
            {

                return mSenderAccountId;
            }
            set
            {
                mSenderAccountId = value;
            }
        }

        private int mSenderAccountPictureId = 0;

        public int SenderAccountPictureId
        {
            get
            {

                return mSenderAccountPictureId;
            }
            set
            {
                mSenderAccountPictureId = value;
            }
        }

        private string mSenderAccountName;

        public string SenderAccountName
        {
            get
            {

                return mSenderAccountName;
            }
            set
            {
                mSenderAccountName = value;
            }
        }

        private int mRecepientAccountId = 0;

        public int RecepientAccountId
        {
            get
            {

                return mRecepientAccountId;
            }
            set
            {
                mRecepientAccountId = value;
            }
        }

        private int mRecepientAccountPictureId = 0;

        public int RecepientAccountPictureId
        {
            get
            {

                return mRecepientAccountPictureId;
            }
            set
            {
                mRecepientAccountPictureId = value;
            }
        }

        private string mRecepientAccountName;

        public string RecepientAccountName
        {
            get
            {

                return mRecepientAccountName;
            }
            set
            {
                mRecepientAccountName = value;
            }
        }

        public TransitAccountMessage()
        {

        }

        public TransitAccountMessage(AccountMessage instance)
            : base(instance)
        {

        }

        public override void SetInstance(AccountMessage instance)
        {
            Subject = instance.Subject;
            Body = instance.Body;
            Sent = instance.Sent;
            UnRead = instance.Unread;
            AccountMessageFolderId = instance.AccountMessageFolder.Id;
            AccountId = instance.Account.Id;
            RecepientAccountId = instance.RecepientAccountId;
            SenderAccountId = instance.SenderAccountId;
            base.SetInstance(instance);
        }

        public override AccountMessage GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountMessage instance = base.GetInstance(session, sec);

            if (Id == 0)
            {
                // sent messages cannot be modified
                instance.Subject = this.Subject;
                instance.Body = this.Body;
                instance.Sent = this.Sent;
                instance.SenderAccountId = GetOwner(session, SenderAccountId, sec).Id;
                instance.RecepientAccountId = this.RecepientAccountId;
                // the oner is the recepient
                instance.Account = session.Load<Account>(RecepientAccountId);
                instance.Unread = true;
            }
            else
            {
                instance.Unread = this.UnRead;
            }

            if (AccountMessageFolderId == 0)
            {
                instance.AccountMessageFolder = ManagedAccountMessageFolder.FindRootFolder(
                    session, instance.Account.Id, "inbox");
            }
            else
            {
                instance.AccountMessageFolder = (AccountMessageFolder)session.Load(
                    typeof(AccountMessageFolder), AccountMessageFolderId);
            }

            return instance;
        }
    }

    public class ManagedAccountMessage : ManagedService<AccountMessage, TransitAccountMessage>
    {
        public ManagedAccountMessage()
        {

        }

        public ManagedAccountMessage(ISession session)
            : base(session)
        {

        }

        public ManagedAccountMessage(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountMessage(ISession session, AccountMessage value)
            : base(session, value)
        {

        }

        public int AccountId
        {
            get
            {
                return mInstance.Account.Id;
            }
        }

        public override TransitAccountMessage GetTransitInstance(ManagedSecurityContext sec)
        {
            TransitAccountMessage message = base.GetTransitInstance(sec);
            message.SenderAccountName = ManagedAccount.GetAccountNameWithDefault(Session, mInstance.SenderAccountId);
            message.SenderAccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, mInstance.SenderAccountId);
            message.RecepientAccountName = ManagedAccount.GetAccountNameWithDefault(Session, mInstance.RecepientAccountId);
            message.RecepientAccountPictureId = ManagedAccount.GetRandomAccountPictureId(Session, mInstance.RecepientAccountId);
            return message;
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountMessage>.GetSafeCollection(mInstance.Account.AccountMessages).Remove(mInstance);
            base.Delete(sec);
        }

        public void MarkMessageAsRetreiveUnRead(bool value)
        {
            mInstance.Unread = value;
            Session.SaveOrUpdate(mInstance);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            bool fNew = (mInstance.Id == 0);

            if (mInstance.Id == 0)
            {
                mInstance.Sent = DateTime.UtcNow;
            }

            base.Save(sec);

            Session.Flush();

            if (fNew)
            {
                ManagedAccount recepient = new ManagedAccount(Session, mInstance.RecepientAccountId);
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, recepient, string.Format("EmailAccountMessage.aspx?id={0}", mInstance.Id));

                // save a copy in Sent items
                AccountMessage copy = new AccountMessage();
                copy.Account = Session.Load<Account>(mInstance.SenderAccountId);
                copy.AccountMessageFolder = ManagedAccountMessageFolder.FindRootFolder(Session, mInstance.SenderAccountId, "sent");
                copy.Body = mInstance.Body;
                copy.RecepientAccountId = mInstance.RecepientAccountId;
                copy.SenderAccountId = mInstance.SenderAccountId;
                copy.Sent = mInstance.Sent;
                copy.Subject = mInstance.Subject;
                copy.Unread = true;
                Session.Save(copy);
            }
        }

        public void MoveTo(ManagedSecurityContext sec, int folderid)
        {
            GetACL().Check(sec, DataOperation.Update);

            if (mInstance.AccountMessageFolder.Id == folderid)
                return;

            AccountMessageFolder folder = Session.Load<AccountMessageFolder>(folderid);
            if (folder.Account.Id != mInstance.Account.Id)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            mInstance.AccountMessageFolder = folder;
            Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }

        public const int DefaultHourlyLimit = 10; // TODO: export into configuration settings

        // messages sent by this user to those who aren't this user's friends
        public static IList<AccountMessage> GetAcountMessages(ISession session, int sender_id, DateTime limit)
        {
            return session.CreateQuery("FROM AccountMessage m" +
                string.Format(" WHERE m.SenderAccountId = {0}", sender_id) +
                " AND m.Account.Id <> m.SenderAccountId" +
                string.Format(" AND m.Sent >= '{0}'", limit) +
                " AND NOT EXISTS ( " +
                "  SELECT f FROM AccountFriend f WHERE" +
                "  ( f.Account.Id = m.SenderAccountId AND f.Keen.Id = m.RecepientAccountId ) OR" +
                "  ( f.Account.Id = m.RecepientAccountId AND f.Keen.Id = m.SenderAccountId )" +
                ")").List<AccountMessage>();
        }

        protected override void Check(TransitAccountMessage t_instance, ManagedSecurityContext sec)
        {
            base.Check(t_instance, sec);

            // existing instance doesn't need to be rechecked
            if (t_instance.Id != 0)
                return;

            // is the sender a friend of the receiver?
            int sender_id = t_instance.GetOwner(Session, t_instance.SenderAccountId, sec).Id;
            ManagedAccount sender = new ManagedAccount(Session, sender_id);
            if (sender.HasFriend(t_instance.RecepientAccountId))
                return;

            try
            {
                // how many messages within the last hour?
                new ManagedQuota(DefaultHourlyLimit).Check(
                    GetAcountMessages(Session, sender_id, DateTime.UtcNow.AddHours(-1)));
                // how many messages within the last 24 hours?
                ManagedQuota.GetDefaultEnabledQuota().Check(
                    GetAcountMessages(Session, sender_id, DateTime.UtcNow.AddDays(-1)));
            }
            catch (ManagedAccount.QuotaExceededException)
            {
                ManagedAccount admin = new ManagedAccount(Session, ManagedAccount.GetAdminAccount(Session));
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    Session, admin,
                    string.Format("EmailAccountQuotaExceeded.aspx?id={0}", sender_id));
                throw;
            }
        }
    }
}

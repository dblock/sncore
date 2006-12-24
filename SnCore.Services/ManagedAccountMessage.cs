using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace SnCore.Services
{
    public class TransitAccountMessage : TransitService
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

        private bool mUnread;

        public bool Unread
        {
            get
            {

                return mUnread;
            }
            set
            {
                mUnread = value;
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

        private int mAccountMessageFolderId;

        public int AccountMessageFolderId
        {
            get
            {

                return mAccountMessageFolderId;
            }
            set
            {
                mAccountMessageFolderId = value;
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

        public TransitAccountMessage(AccountMessage p)
            : base(p.Id)
        {
            Subject = p.Subject;
            Body = p.Body;
            Sent = p.Sent;
            Unread = p.Unread;
            AccountMessageFolderId = p.AccountMessageFolder.Id;
            AccountId = p.Account.Id;
            RecepientAccountId = p.RecepientAccountId;
            SenderAccountId = p.SenderAccountId;
        }

        public AccountMessage GetAccountMessage(ISession session)
        {
            AccountMessage p = (Id != 0) ? (AccountMessage)session.Load(typeof(AccountMessage), Id) : new AccountMessage();

            if (Id == 0)
            {
                // sent messages cannot be modified
                p.Subject = this.Subject;
                p.Body = this.Body;
                p.Sent = this.Sent;
                p.Unread = this.Unread;
                p.SenderAccountId = this.SenderAccountId;
                p.RecepientAccountId = this.RecepientAccountId;
                if (AccountId > 0) p.Account = (Account)session.Load(typeof(Account), this.AccountId);
            }

            if (AccountMessageFolderId == 0)
            {
                p.AccountMessageFolder = (AccountMessageFolder)session.CreateCriteria(typeof(AccountMessageFolder))
                    .Add(Expression.Eq("Name", "inbox"))
                    .Add(Expression.Eq("Account.Id", this.AccountId))
                    .UniqueResult();
            }
            else
            {
                p.AccountMessageFolder = (AccountMessageFolder)session.Load(
                    typeof(AccountMessageFolder), AccountMessageFolderId);
            }

            return p;
        }
    }

    /// <summary>
    /// Managed account message.
    /// </summary>
    public class ManagedAccountMessage : ManagedService<AccountMessage>
    {
        private AccountMessage mAccountMessage = null;

        public ManagedAccountMessage(ISession session)
            : base(session)
        {

        }

        public ManagedAccountMessage(ISession session, int id)
            : base(session)
        {
            mAccountMessage = (AccountMessage)session.Load(typeof(AccountMessage), id);
        }

        public ManagedAccountMessage(ISession session, AccountMessage value)
            : base(session)
        {
            mAccountMessage = value;
        }

        public int Id
        {
            get
            {
                return mAccountMessage.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountMessage.Account.Id;
            }
        }

        public TransitAccountMessage TransitAccountMessage
        {
            get
            {
                TransitAccountMessage tam = new TransitAccountMessage(mAccountMessage);

                Account sender = (Account)Session.CreateCriteria(typeof(Account))
                        .Add(Expression.Eq("Id", tam.SenderAccountId))
                        .UniqueResult();

                if (sender != null)
                {
                    tam.SenderAccountPictureId = ManagedAccount.GetRandomAccountPictureId(sender);
                    tam.SenderAccountName = sender.Name;
                }

                Account recepient = (Account)Session.CreateCriteria(typeof(Account))
                        .Add(Expression.Eq("Id", tam.RecepientAccountId))
                        .UniqueResult();

                if (recepient != null)
                {
                    tam.RecepientAccountPictureId = ManagedAccount.GetRandomAccountPictureId(recepient);
                    tam.RecepientAccountName = recepient.Name;
                }

                return tam;
            }
        }

        public void Delete()
        {
            mAccountMessage.Account.AccountMessages.Remove(mAccountMessage);
            Session.Delete(mAccountMessage);
        }

        public void MarkMessageAsReadUnread(bool value)
        {
            mAccountMessage.Unread = value;
            Session.SaveOrUpdate(mAccountMessage);
        }
    }
}

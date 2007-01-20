using System;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
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
                instance.Unread = this.UnRead;
                instance.SenderAccountId = GetOwner(session, SenderAccountId, sec).Id;
                instance.RecepientAccountId = this.RecepientAccountId;
                // the oner is the recepient
                instance.Account = (Account) session.Load(typeof(Account), RecepientAccountId);
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
            message.SenderAccountName = "Unknown User";
            message.RecepientAccountName = "Unknown User";

            try
            {
                Account sender = (Account) Session.CreateCriteria(typeof(Account))
                        .Add(Expression.Eq("Id", mInstance.SenderAccountId))
                        .UniqueResult();

                if (sender != null)
                {
                    message.SenderAccountPictureId = ManagedAccount.GetRandomAccountPictureId(sender);
                    message.SenderAccountName = sender.Name;
                }
            }
            catch (ObjectNotFoundException)
            {

            }

            try
            {
                Account recepient = (Account) Session.CreateCriteria(typeof(Account))
                        .Add(Expression.Eq("Id", mInstance.RecepientAccountId))
                        .UniqueResult();

                if (recepient != null)
                {
                    message.RecepientAccountPictureId = ManagedAccount.GetRandomAccountPictureId(recepient);
                    message.RecepientAccountName = recepient.Name;
                }
            }
            catch (ObjectNotFoundException)
            {

            }

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
            if (mInstance.Id == 0) mInstance.Sent = DateTime.UtcNow;
            base.Save(sec);
        }

        public void MoveTo(ManagedSecurityContext sec, int folderid)
        {
            GetACL().Check(sec, DataOperation.Update);

            if (mInstance.AccountMessageFolder.Id == folderid)
                return;

            AccountMessageFolder folder = (AccountMessageFolder) Session.Load(typeof(AccountMessageFolder), folderid);
            if (folder.Account.Id != mInstance.Account.Id)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            mInstance.AccountMessageFolder = folder;
            Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLAuthenticatedAllowCreate());
            acl.Add(new ACLAccount(mInstance.Account, DataOperation.All));
            return acl;
        }
    }
}

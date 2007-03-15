using System;
using NHibernate;
using System.Collections;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class TransitAccountEmailMessage : TransitService<AccountEmailMessage>
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

        private bool mDeleteSent;

        public bool DeleteSent
        {
            get
            {

                return mDeleteSent;
            }
            set
            {
                mDeleteSent = value;
            }
        }

        private bool mSent;

        public bool Sent
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

        private string mSendError;

        public string SendError
        {
            get
            {

                return mSendError;
            }
            set
            {
                mSendError = value;
            }
        }

        private string mMailTo;

        public string MailTo
        {
            get
            {

                return mMailTo;
            }
            set
            {
                mMailTo = value;
            }
        }

        private string mMailFrom;

        public string MailFrom
        {
            get
            {

                return mMailFrom;
            }
            set
            {
                mMailFrom = value;
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

        private DateTime mModified;

        public DateTime Modified
        {
            get
            {

                return mModified;
            }
            set
            {
                mModified = value;
            }
        }

        public TransitAccountEmailMessage()
        {

        }

        public TransitAccountEmailMessage(AccountEmailMessage value)
            : base(value)
        {

        }

        public override void SetInstance(AccountEmailMessage value)
        {
            Subject = value.Subject;
            Body = value.Body;
            DeleteSent = value.DeleteSent;
            Sent = value.Sent;
            SendError = value.SendError;
            MailTo = value.MailTo;
            MailFrom = value.MailFrom;
            Created = value.Created;
            Modified = value.Modified;
            AccountId = value.Account.Id;
            base.SetInstance(value);
        }

        public override AccountEmailMessage GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEmailMessage instance = base.GetInstance(session, sec);
            instance.Account = GetOwner(session, AccountId, sec);
            instance.Subject = this.Subject;
            instance.Body = this.Body;
            instance.DeleteSent = this.DeleteSent;
            instance.Sent = this.Sent;
            instance.SendError = this.SendError;
            instance.MailTo = this.MailTo;
            instance.MailFrom = this.MailFrom;
            return instance;
        }
    }

    public class ManagedAccountEmailMessage : ManagedService<AccountEmailMessage, TransitAccountEmailMessage>
    {
        public ManagedAccountEmailMessage()
        {

        }

        public ManagedAccountEmailMessage(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmailMessage(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEmailMessage(ISession session, AccountEmailMessage value)
            : base(session, value)
        {

        }

        public string Body
        {
            get
            {
                return mInstance.Body; ;
            }
        }

        public DateTime Created
        {
            get
            {
                return mInstance.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mInstance.Modified;
            }
        }

        public string Subject
        {
            get
            {
                return mInstance.Subject;
            }
        }

        public bool DeleteSent
        {
            get
            {
                return mInstance.DeleteSent;
            }
        }

        public string SendError
        {
            get
            {
                return mInstance.SendError;
            }
        }

        public string MailTo
        {
            get
            {
                return mInstance.MailTo;
            }
        }

        public string MailFrom
        {
            get
            {
                return mInstance.MailFrom;
            }
        }

        public bool Sent
        {
            get
            {
                return mInstance.Sent; ;
            }
        }

        public override void Delete(ManagedSecurityContext sec)
        {
            Collection<AccountEmailMessage>.GetSafeCollection(mInstance.Account.AccountEmailMessages).Remove(mInstance);
            base.Delete(sec);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            // administrators can only send e-mail messages
            return acl;
        }
    }
}

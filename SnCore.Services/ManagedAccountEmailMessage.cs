using System;
using NHibernate;
using System.Collections;

namespace SnCore.Services
{
    public class TransitAccountEmailMessage : TransitService
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

        public TransitAccountEmailMessage(AccountEmailMessage e)
            : base(e.Id)
        {
            Subject = e.Subject;
            Body = e.Body;
            DeleteSent = e.DeleteSent;
            Sent = e.Sent;
            SendError = e.SendError;
            MailTo = e.MailTo;
            MailFrom = e.MailFrom;
            Created = e.Created;
            Modified = e.Modified;
        }
    }

    /// <summary>
    /// Managed e-mail message.
    /// </summary>
    public class ManagedAccountEmailMessage : ManagedService
    {
        private AccountEmailMessage mAccountEmailMessage = null;

        public ManagedAccountEmailMessage(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmailMessage(ISession session, int id)
            : base(session)
        {
            mAccountEmailMessage = (AccountEmailMessage)session.Load(typeof(AccountEmailMessage), id);
        }

        public ManagedAccountEmailMessage(ISession session, AccountEmailMessage value)
            : base(session)
        {
            mAccountEmailMessage = value;
        }

        public int Id
        {
            get
            {
                return mAccountEmailMessage.Id;
            }
        }

        public string Body
        {
            get
            {
                return mAccountEmailMessage.Body; ;
            }
        }

        public DateTime Created
        {
            get
            {
                return mAccountEmailMessage.Created;
            }
        }

        public DateTime Modified
        {
            get
            {
                return mAccountEmailMessage.Modified;
            }
        }

        public string Subject
        {
            get
            {
                return mAccountEmailMessage.Subject;
            }
        }

        public bool DeleteSent
        {
            get
            {
                return mAccountEmailMessage.DeleteSent;
            }
        }

        public string SendError
        {
            get
            {
                return mAccountEmailMessage.SendError;
            }
        }

        public string MailTo
        {
            get
            {
                return mAccountEmailMessage.MailTo;
            }
        }

        public string MailFrom
        {
            get
            {
                return mAccountEmailMessage.MailFrom;
            }
        }

        public bool Sent
        {
            get
            {
                return mAccountEmailMessage.Sent; ;
            }
        }

        public TransitAccountEmailMessage TransitAccountEmailMessage
        {
            get
            {
                return new TransitAccountEmailMessage(mAccountEmailMessage);
            }
        }

        public void Delete()
        {
            mAccountEmailMessage.Account.AccountEmailMessages.Remove(mAccountEmailMessage);
            Session.Delete(mAccountEmailMessage);
        }
    }
}

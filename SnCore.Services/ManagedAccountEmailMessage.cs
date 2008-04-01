using System;
using NHibernate;
using System.Collections;
using SnCore.Data.Hibernate;
using System.Net.Mail;
using System.Text;
using System.Net;

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
            acl.Add(new ACLAuthenticatedAllowCreate());
            return acl;
        }

        public override int CreateOrUpdate(TransitAccountEmailMessage t_instance, ManagedSecurityContext sec)
        {
            t_instance.Modified = DateTime.UtcNow;
            
            if (t_instance.Id == 0)
            {
                t_instance.Created = t_instance.Modified;
                t_instance.Sent = false;
                t_instance.SendError = string.Empty;
            }

            return base.CreateOrUpdate(t_instance, sec);
        }

        protected override void Check(TransitAccountEmailMessage t_instance, ManagedSecurityContext sec)
        {
            if (!string.IsNullOrEmpty(t_instance.MailFrom) && sec.IsAdministrator())
            {
                // administrator can force a MailFrom address to whatever
                mInstance.MailFrom = t_instance.MailFrom;
            }
            else
            {
                Account user = t_instance.GetOwner(Session, t_instance.AccountId, sec);
                ManagedAccount m_user = new ManagedAccount(Session, user);
                string mailfrom = string.Empty;

                // a user is required to have a valid e-mail address
                if (!m_user.TryGetVerifiedEmailAddress(out mailfrom, sec))
                    throw new ManagedAccount.NoVerifiedEmailException();

                // if the user didn't specify the address, user his verified e-mail
                if (string.IsNullOrEmpty(t_instance.MailFrom))
                {
                    mInstance.MailFrom = new MailAddress(mailfrom, m_user.Name).ToString();;
                }
                // if the user specified a different e-mail address, don't let him send
                else if (t_instance.MailFrom != mailfrom)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            base.Check(t_instance, sec);
        }

        public void Send(ManagedSecurityContext sec, TransitAccountEmailMessage t_instance)
        {
            if (!sec.IsAdministrator())
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            SmtpClient smtp = GetSmtpClientInstance(Session);
            MailMessage message = GetMessageInstance(Session, t_instance.GetInstance(Session, sec));
            smtp.Send(message);
        }

        public static MailMessage GetMessageInstance(ISession session, AccountEmailMessage instance)
        {
            MailMessage message = new MailMessage();
            message.Headers.Add("x-mimeole", string.Format("Produced By {0} {1}",
                ManagedSystem.Title, ManagedSystem.ProductVersion));
            message.Headers.Add("Content-class", "urn:content-classes:message");
            message.Headers.Add("Content-Type", "text/html; charset=\"ISO-8859-1\"");
            message.IsBodyHtml = true;
            Encoding iso8859 = Encoding.GetEncoding(28591);
            message.BodyEncoding = iso8859;
            message.Body = instance.Body;
            message.ReplyTo = new MailAddress(instance.MailFrom);
            message.From = new MailAddress(
                ManagedConfiguration.GetValue(session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                ManagedConfiguration.GetValue(session, "SnCore.Admin.Name", "Admin")
                );
            message.To.Add(new MailAddress(instance.MailTo));
            message.Subject = instance.Subject;
            return message;
        }

        public static SmtpClient GetSmtpClientInstance(ISession session)
        {
            SmtpClient smtp = new SmtpClient(
                ManagedConfiguration.GetValue(session, "SnCore.Mail.Server", "localhost"),
                int.Parse(ManagedConfiguration.GetValue(session, "SnCore.Mail.Port", "25")));
            smtp.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod),
                ManagedConfiguration.GetValue(session, "SnCore.Mail.Delivery", "Network"));
            smtp.PickupDirectoryLocation = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.PickupDirectoryLocation", string.Empty);

            string smtpusername = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.Username", string.Empty);

            string smtppassword = ManagedConfiguration.GetValue(
                session, "SnCore.Mail.Password", string.Empty);

            if (!string.IsNullOrEmpty(smtpusername))
            {
                smtp.Credentials = new NetworkCredential(smtpusername, smtppassword);
            }
            else
            {
                smtp.UseDefaultCredentials = true;
            }

            return smtp;
        }
    }
}

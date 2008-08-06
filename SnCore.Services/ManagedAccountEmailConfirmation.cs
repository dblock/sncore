using System;
using System.Collections;
using NHibernate;
using NHibernate.Expression;
using System.Net.Mail;

namespace SnCore.Services
{
    public class TransitAccountEmailConfirmation : TransitService<AccountEmailConfirmation>
    {
        private TransitAccountEmail mAccountEmail;

        public TransitAccountEmail AccountEmail
        {
            get
            {
                return mAccountEmail;
            }
            set
            {
                mAccountEmail = value;
            }
        }

        private string mCode;

        public string Code
        {
            get
            {
                return mCode;
            }
            set
            {
                mCode = value;
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

        public TransitAccountEmailConfirmation()
        {

        }

        public TransitAccountEmailConfirmation(AccountEmailConfirmation value)
            : base(value)
        {
        }

        public override void SetInstance(AccountEmailConfirmation value)
        {
            mAccountEmail = new TransitAccountEmail(value.AccountEmail);
            mCode = value.Code;
            mCreated = value.Created;
            mModified = value.Modified;
            base.SetInstance(value);
        }

        public override AccountEmailConfirmation GetInstance(ISession session, ManagedSecurityContext sec)
        {
            AccountEmailConfirmation instance = base.GetInstance(session, sec);
            return instance;
        }
    }

    public class ManagedAccountEmailConfirmation : ManagedService<AccountEmailConfirmation, TransitAccountEmailConfirmation>
    {
        public class InvalidCodeException : Exception
        {
            private string mCode = null;

            public string Code
            {
                get
                {
                    return mCode;
                }
                set
                {
                    mCode = value;
                }
            }

            public InvalidCodeException(string code)
                : base(string.Format("The verification code '{0}' is invalid", code))
            {
                Code = code;
            }

        }

        public ManagedAccountEmailConfirmation()
        {

        }

        public ManagedAccountEmailConfirmation(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmailConfirmation(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccountEmailConfirmation(ISession session, AccountEmailConfirmation value)
            : base(session, value)
        {

        }

        public string Verify(string code)
        {
            if (mInstance.Code != code)
            {
                throw new InvalidCodeException(code);
            }

            mInstance.AccountEmail.Verified = true;
            mInstance.AccountEmail.Modified = DateTime.UtcNow;
            mInstance.AccountEmail.Failed = false;
            mInstance.AccountEmail.LastError = string.Empty;
            mInstance.Modified = DateTime.UtcNow;
            Session.Save(mInstance.AccountEmail);
            Session.Save(mInstance);

            // reset any other AccountEmail with the same value to unverified (reclaim)
            IList reclaimlist = Session.CreateCriteria(typeof(AccountEmail))
                 .Add(Expression.Eq("Address", mInstance.AccountEmail.Address))
                 .Add(Expression.Eq("Verified", true))
                 .Add(Expression.Not(Expression.Eq("Id", mInstance.AccountEmail.Id))).List();

            foreach (AccountEmail ae in reclaimlist)
            {
                ae.Verified = false;
                Session.Save(ae);
            }

            return mInstance.AccountEmail.Address;
        }

        public ManagedAccountEmail ManagedAccountEmail
        {
            get
            {
                return new ManagedAccountEmail(Session, mInstance.AccountEmail);
            }
        }

        public void Send()
        {
            ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                Session,
                new MailAddress(mInstance.AccountEmail.Address, mInstance.AccountEmail.Account.Name).ToString(),
                string.Format("EmailAccountEmailVerify.aspx?id={0}", mInstance.Id));
        }

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            // user himself cannot see the e-mail confirmations, shouldn't be able to retrieve the code
            // acl.Add(new ACLAccount(mInstance.AccountEmail.Account, DataOperation.All));
            return acl;
        }

        public static string Verify(ISession session, int id, string code)
        {
            AccountEmailConfirmation confirmation = session.CreateCriteria(typeof(AccountEmailConfirmation))
                .Add(Expression.Eq("Id", id))
                .UniqueResult<AccountEmailConfirmation>();

            if (confirmation == null)
            {
                throw new Exception(string.Format("Error locating confirmation number \"{0}\".", id));
            }

            ManagedAccountEmailConfirmation c = new ManagedAccountEmailConfirmation(session, id);
            string emailaddress = c.Verify(code);
            SnCore.Data.Hibernate.Session.Flush();
            return emailaddress;
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (mInstance.Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }
    }
}

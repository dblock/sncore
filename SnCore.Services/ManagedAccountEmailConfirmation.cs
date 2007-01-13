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
                : base("Invalid Code")
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

        public string Verify(string password, string code)
        {
            if (mInstance.AccountEmail.Account.Password != ManagedAccount.GetPasswordHash(password))
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            return Verify(code);
        }

        public string Verify(string code)
        {
            if (mInstance.Code != code)
            {
                throw new InvalidCodeException(code);
            }

            mInstance.AccountEmail.Verified = true;
            mInstance.AccountEmail.Modified = DateTime.UtcNow;
            Session.Save(mInstance.AccountEmail);
            mInstance.AccountEmail.AccountEmailConfirmations = null;
            Session.Delete(mInstance);

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

            string email = mInstance.AccountEmail.Address;

            mInstance = null;
            return email;
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

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            // user himself cannot see the e-mail confirmations, shouldn't be able to retrieve the code
            // acl.Add(new ACLAccount(mInstance.AccountEmail.Account, DataOperation.All));
            return acl;
        }
    }
}

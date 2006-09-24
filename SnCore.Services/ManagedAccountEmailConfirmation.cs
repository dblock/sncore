using System;
using System.Collections;
using NHibernate;
using NHibernate.Expression;

namespace SnCore.Services
{
    public class TransitAccountEmailConfirmation : TransitService
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

        public TransitAccountEmailConfirmation(AccountEmailConfirmation c)
            : base(c.Id)
        {
            mAccountEmail = new TransitAccountEmail(c.AccountEmail);
            mCode = c.Code;
        }
    }

    /// <summary>
    /// Managed e-mail confirmation;
    /// </summary>
    public class ManagedAccountEmailConfirmation : ManagedService
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

        private AccountEmailConfirmation mAccountEmailConfirmation = null;

        public ManagedAccountEmailConfirmation(ISession session)
            : base(session)
        {

        }

        public ManagedAccountEmailConfirmation(ISession session, int id)
            : base(session)
        {
            mAccountEmailConfirmation = (AccountEmailConfirmation)session.Load(typeof(AccountEmailConfirmation), id);
        }

        public ManagedAccountEmailConfirmation(ISession session, AccountEmailConfirmation value)
            : base(session)
        {
            mAccountEmailConfirmation = value;
        }

        public int Id
        {
            get
            {
                return mAccountEmailConfirmation.Id;
            }
        }

        public string Verify(string password, string code)
        {
            if (mAccountEmailConfirmation.AccountEmail.Account.Password != ManagedAccount.GetPasswordHash(password))
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            return Verify(code);
        }

        public string Verify(string code)
        {
            if (mAccountEmailConfirmation.Code != code)
            {
                throw new InvalidCodeException(code);
            }

            mAccountEmailConfirmation.AccountEmail.Verified = true;
            mAccountEmailConfirmation.AccountEmail.Modified = DateTime.UtcNow;
            Session.Update(mAccountEmailConfirmation.AccountEmail);
            mAccountEmailConfirmation.AccountEmail.AccountEmailConfirmations = null;
            Session.Delete(mAccountEmailConfirmation);

            // reset any other AccountEmail with the same value to unverified (reclaim)
            IList reclaimlist = Session.CreateCriteria(typeof(AccountEmail))
             .Add(Expression.Eq("Address", mAccountEmailConfirmation.AccountEmail.Address))
             .Add(Expression.Eq("Verified", true))
             .Add(Expression.Not(Expression.Eq("Id", mAccountEmailConfirmation.AccountEmail.Id))).List();

            foreach (AccountEmail ae in reclaimlist)
            {
                ae.Verified = false;
                Session.Update(ae);
            }

            string email = mAccountEmailConfirmation.AccountEmail.Address;

            mAccountEmailConfirmation = null;
            return email;
        }

        public ManagedAccountEmail ManagedAccountEmail
        {
            get
            {
                return new ManagedAccountEmail(Session, mAccountEmailConfirmation.AccountEmail);
            }
        }

        public TransitAccountEmailConfirmation TransitAccountEmailConfirmation
        {
            get
            {
                return new TransitAccountEmailConfirmation(mAccountEmailConfirmation);
            }
        }

        public void Send()
        {
            string url = string.Format(
                "{0}/AccountEmailVerify.aspx?id={1}&code={2}",
                ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                mAccountEmailConfirmation.Id,
                mAccountEmailConfirmation.Code);

            string subject = string.Format("{0}: Please confirm your e-mail address.",
                ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"));

            ManagedAccountEmail.Account.SendAccountMailMessage(
                ManagedConfiguration.GetValue(Session, "SnCore.Admin.EmailAddress", "admin@localhost.com"),
                mAccountEmailConfirmation.AccountEmail.Address,
                subject,
                "<html>" +
                "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                "<body>" +
                string.Format("Dear {0},<br><br>" +
                    "The e-mail address {1} has been added to your {3} account. " +
                    "You must confirm it. " +
                    "Please copy-paste the url below to a browser or click it.<br>" +
                    "<blockquote><a href='{2}'>{2}</a></blockquote>" +
                    "Thank you,<br>" +
                    "{3}" +
                    "</body>" +
                    "</html>",
                    mAccountEmailConfirmation.AccountEmail.Account.Name,
                    mAccountEmailConfirmation.AccountEmail.Address.ToLower(),
                    url,
                    ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore")),
                true);
        }
    }
}

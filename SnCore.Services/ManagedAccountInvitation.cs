using System;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using SnCore.Tools.Web;

namespace SnCore.Services
{
    public class TransitAccountInvitation : TransitService
    {
        private string mEmail;

        public string Email
        {
            get
            {

                return mEmail;
            }
            set
            {
                mEmail = value;
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

        public TransitAccountInvitation()
        {

        }

        public TransitAccountInvitation(AccountInvitation s)
            : base(s.Id)
        {
            Email = s.Email;
            Code = s.Code;
            AccountId = s.Account.Id;
            AccountName = s.Account.Name;
            Message = s.Message;
            Created = s.Created;
            Modified = s.Modified;
        }

        public AccountInvitation GetAccountInvitation(ISession session)
        {
            AccountInvitation s = (Id != 0) ? (AccountInvitation)session.Load(typeof(AccountInvitation), Id) : new AccountInvitation();

            if (Id == 0)
            {
                // invitations cannot be modified post-send
                s.Email = this.Email;
                s.Code = this.Code;
                s.Message = this.Message;
                if (AccountId > 0) s.Account = (Account)session.Load(typeof(Account), this.AccountId);
            }

            return s;
        }

    }

    public class ManagedAccountInvitation : ManagedService
    {
        private AccountInvitation mAccountInvitation = null;

        public ManagedAccountInvitation(ISession session)
            : base(session)
        {

        }

        public ManagedAccountInvitation(ISession session, int id)
            : base(session)
        {
            mAccountInvitation = (AccountInvitation)session.Load(typeof(AccountInvitation), id);
        }

        public ManagedAccountInvitation(ISession session, AccountInvitation value)
            : base(session)
        {
            mAccountInvitation = value;
        }

        public int Id
        {
            get
            {
                return mAccountInvitation.Id;
            }
        }

        public int AccountId
        {
            get
            {
                return mAccountInvitation.Account.Id;
            }
        }

        public string Name
        {
            get
            {
                return mAccountInvitation.Email;
            }
        }

        public string Code
        {
            get
            {
                return mAccountInvitation.Code;
            }
        }

        public TransitAccountInvitation TransitAccountInvitation
        {
            get
            {
                return new TransitAccountInvitation(mAccountInvitation);
            }
        }

        public void Delete()
        {
            mAccountInvitation.Account.AccountStories.Remove(mAccountInvitation);
            Session.Delete(mAccountInvitation);
        }

        public void Send()
        {
            string url = string.Format(
                "{0}/AccountCreateInvitation.aspx?id={1}&code={2}",
                ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                mAccountInvitation.Id,
                mAccountInvitation.Code);

            ManagedAccount a = new ManagedAccount(Session, mAccountInvitation.Account);
            a.SendAccountMailMessage(
                string.Format("{0} <{1}>", a.Name, a.ActiveEmailAddress),
                mAccountInvitation.Email,
                string.Format("{0}: {1} has invited you to {0}!",
                    ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                    mAccountInvitation.Account.Name),
                "<html>" +
                "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                "<body>" +
                string.Format("Hello,<br><br>" +
                    "Your friend {0} wants you to join {1}. " +
                    (string.IsNullOrEmpty(mAccountInvitation.Message) ? "" : "<br><br>" + mAccountInvitation.Message + "<br><br>") +
                    "Please copy-paste the url below to a browser or click it.<br>" +
                    "<blockquote><a href='{2}'>{2}</a></blockquote>" +
                    "Thank you,<br>" +
                    "{1}" +
                    "</body>" +
                    "</html>",
                    mAccountInvitation.Account.Name,
                    ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                    url),
                true);
        }
    }
}

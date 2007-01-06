using System;
using NHibernate;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NHibernate.Expression;
using System.Web.Services.Protocols;
using System.Xml;
using System.Resources;
using System.Net.Mail;
using System.IO;
using System.Web.Configuration;
using System.Web.Security;
using System.Reflection;
using SnCore.Tools.Web;
using Janrain.OpenId.Store.Net;
using Janrain.OpenId.Store;
using Janrain.OpenId;
using Janrain.OpenId.Consumer;
using SnCore.Tools;
using SnCore.Data.Hibernate;

namespace SnCore.Services
{
    public class AccountActivityQueryOptions
    {
        public string SortOrder = "LastLogin";
        public bool SortAscending = false;
        public bool PicturesOnly = false;
        public bool BloggersOnly = false;
        public string Country;
        public string State;
        public string City;
        public string Name;
        public string Email;

        public AccountActivityQueryOptions()
        {

        }

        public string CreateSubQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();

            if (!string.IsNullOrEmpty(City))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("a.City LIKE '{0}'", City);
            }

            if (!string.IsNullOrEmpty(Country))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("a.Country.Id = {0}", ManagedCountry.GetCountryId(session, Country));
            }

            if (!string.IsNullOrEmpty(State))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("a.State.Id = {0}", ManagedState.GetStateId(session, State, Country));
            }

            if (PicturesOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("EXISTS ( FROM AccountPicture ap WHERE ap.Account = a AND ap.Hidden = 0 )");
            }

            if (BloggersOnly)
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.Append("(EXISTS ( FROM AccountFeed af WHERE af.Account = a ) OR EXISTS ( FROM AccountBlog ab WHERE ab.Account = a ))");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("(EXISTS ( FROM AccountEmail ae WHERE ae.Account = a AND ae.Address = '{0}'))", Renderer.SqlEncode(Email));
            }

            // delay accounts, prevent bots from pushing accounts on top
            // and avoid privacy violation of showing someone online
            if (b.Length == 0 && SortOrder == "LastLogin")
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("LastLogin < '{0}'", DateTime.UtcNow.AddMinutes(-15));
            }

            return b.ToString();
        }

        public IQuery CreateCountQuery(ISession session)
        {
            return session.CreateQuery("SELECT COUNT(*) FROM Account a " + CreateSubQuery(session));
        }

        public IQuery CreateQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT a FROM Account a ");
            b.Append(CreateSubQuery(session));
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return session.CreateQuery(b.ToString());
        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountActivity : TransitAccount
    {
        private int mNewPictures = 0;

        public int NewPictures
        {
            get
            {
                return mNewPictures;
            }
            set
            {
                mNewPictures = value;
            }
        }

        private int mNewDiscussionPosts = 0;

        public int NewDiscussionPosts
        {
            get
            {
                return mNewDiscussionPosts;
            }
            set
            {
                mNewDiscussionPosts = value;
            }
        }

        private int mNewSyndicatedContent = 0;

        public int NewSyndicatedContent
        {
            get
            {
                return mNewSyndicatedContent;
            }
            set
            {
                mNewSyndicatedContent = value;
            }
        }

        public TransitAccountActivity()
        {
        }

        public TransitAccountActivity(ISession session, Account a)
            : base(a)
        {
            DateTime limit = DateTime.UtcNow.AddDays(-14);

            // new photos (count one week of photos)

            NewPictures = (int)session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountPicture p " +
                "WHERE p.Account.Id = {0} AND p.Modified > '{1}' AND p.Hidden = 0",
                a.Id, limit))
                .UniqueResult();

            NewDiscussionPosts = (int)session.CreateQuery(string.Format("SELECT COUNT(*) FROM DiscussionPost p, Discussion d, DiscussionThread t " +
                "WHERE p.AccountId = {0} AND p.Modified > '{1}' AND p.DiscussionThread.Id = t.Id and t.Discussion.Id = d.Id AND d.Personal = 0",
                a.Id, limit))
                .UniqueResult();

            NewSyndicatedContent = (int)session.CreateQuery(string.Format("SELECT COUNT(*) FROM AccountFeed f " +
                "WHERE f.Account.Id = {0} AND f.Created > '{1}'",
                a.Id, limit))
                .UniqueResult();

        }

        protected long Count
        {
            get
            {
                return NewPictures + NewDiscussionPosts + NewSyndicatedContent;
            }
        }

        public static int CompareByLastActivity(TransitAccountActivity left, TransitAccountActivity right)
        {
            return right.Count.CompareTo(left.Count);
        }

        public static int CompareByLastLogin(TransitAccountActivity left, TransitAccountActivity right)
        {
            return TransitAccount.CompareByLastLogin(left, right);
        }
    }

    public class TransitOpenIdRedirect
    {
        private string mUrl;
        private string mToken;

        public string Url
        {
            get
            {
                return mUrl;
            }
            set
            {
                mUrl = value;
            }
        }

        public string Token
        {
            get
            {
                return mToken;
            }
            set
            {
                mToken = value;
            }
        }

        public TransitOpenIdRedirect()
        {

        }

        public TransitOpenIdRedirect(string token, string url)
        {
            mUrl = url;
            mToken = token;
        }
    }

    public class TransitAccount : TransitService<Account>
    {
        public TransitAccount()
        {

        }

        public TransitAccount(Account instance)
            : base(instance)
        {
        }

        public override void SetInstance(Account instance)
        {
            Name = instance.Name;
            Birthday = instance.Birthday;
            LastLogin = instance.LastLogin;
            State = (instance.State == null) ? string.Empty : instance.State.Name;
            Country = (instance.Country == null) ? string.Empty : instance.Country.Name;
            City = instance.City;
            TimeZone = instance.TimeZone;
            Signature = instance.Signature;
            mIsAdministrator = instance.IsAdministrator;
            IsPasswordExpired = instance.IsPasswordExpired;
            // random picture from the account
            PictureId = ManagedAccount.GetRandomAccountPictureId(instance);
            base.SetInstance(instance);
        }

        public override Account GetInstance(ISession session, ManagedSecurityContext sec)
        {
            Account instance = base.GetInstance(session, sec);
            instance.Name = Name;
            instance.Birthday = Birthday;

            if (Id == 0)
            {
                if (Password.Length < ManagedAccount.MinimumPasswordLength)
                {
                    throw new ManagedAccount.PasswordTooShortException();
                }

                instance.Password = ManagedAccount.GetPasswordHash(Password);
                instance.LastLogin = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(State)) instance.State = ManagedState.Find(session, State, Country);
            if (!string.IsNullOrEmpty(Country)) instance.Country = ManagedCountry.Find(session, Country);
            instance.City = City;
            instance.TimeZone = TimeZone;
            instance.Signature = Signature;
            return instance;
        }

        private bool mIsPasswordExpired = false;

        public bool IsPasswordExpired
        {
            get
            {
                return mIsPasswordExpired;
            }
            set
            {
                mIsPasswordExpired = value;
            }
        }

        private bool mIsAdministrator = false;

        public bool IsAdministrator
        {
            get
            {
                return mIsAdministrator;
            }
        }

        private string mName = string.Empty;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private DateTime mBirthday;

        public DateTime Birthday
        {
            get
            {

                return mBirthday;
            }
            set
            {
                mBirthday = value;
            }
        }

        private DateTime mLastLogin;

        public DateTime LastLogin
        {
            get
            {

                return mLastLogin;
            }
            set
            {
                mLastLogin = value;
            }
        }

        private int mPictureId = 0;

        public int PictureId
        {
            get
            {

                return mPictureId;
            }
            set
            {
                mPictureId = value;
            }
        }

        private string mState = string.Empty;

        public string State
        {
            get
            {

                return mState;
            }
            set
            {
                mState = value;
            }
        }

        private string mCountry = string.Empty;

        public string Country
        {
            get
            {

                return mCountry;
            }
            set
            {
                mCountry = value;
            }
        }

        private string mCity = string.Empty;

        public string City
        {
            get
            {

                return mCity;
            }
            set
            {
                mCity = value;
            }
        }

        private int mTimeZone = -1;

        public int TimeZone
        {
            get
            {

                return mTimeZone;
            }
            set
            {
                mTimeZone = value;
            }
        }

        private string mSignature = string.Empty;

        public string Signature
        {
            get
            {

                return mSignature;
            }
            set
            {
                mSignature = value;
            }
        }

        private string mPassword;

        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                mPassword = value;
            }
        }

        public static int CompareByLastLogin(TransitAccount left, TransitAccount right)
        {
            return right.LastLogin.CompareTo(left.LastLogin);
        }
    }

    /// <summary>
    /// Managed account.
    /// </summary>
    public class ManagedAccount : ManagedService<Account, TransitAccount>
    {
        public static int MinimumPasswordLength = 4;
        public static int MaxOfAnything = 250;

        public class AccessDeniedException : SoapException
        {
            public AccessDeniedException()
                : base("Access denied", SoapException.ClientFaultCode)
            {

            }
        }

        public class NoVerifiedEmailException : SoapException
        {
            public NoVerifiedEmailException()
                : base("You don't have any verified e-mail addresses", SoapException.ClientFaultCode)
            {

            }
        }


        public class AccountNotFoundException : SoapException
        {
            public AccountNotFoundException()
                : base("Account not found", SoapException.ClientFaultCode)
            {

            }
        }

        public class PasswordTooShortException : SoapException
        {
            public PasswordTooShortException()
                : base("Password too short", SoapException.ClientFaultCode)
            {

            }
        }

        public class AccountExistsException : SoapException
        {
            public AccountExistsException()
                : base("Account alRetreivey exists", SoapException.ClientFaultCode)
            {

            }
        }

        public class QuotaExceededException : SoapException
        {
            public QuotaExceededException()
                : base("Quota exceeded", SoapException.ClientFaultCode)
            {

            }
        }

        public string Name
        {
            get
            {
                return mInstance.Name;
            }
        }

        public ManagedAccount()
        {

        }

        public ManagedAccount(ISession session)
            : base(session)
        {

        }

        public ManagedAccount(ISession session, int id)
            : base(session, id)
        {

        }

        public ManagedAccount(ISession session, Account value)
            : base(session, value)
        {

        }

        public DateTime Birthday
        {
            get
            {
                return mInstance.Birthday;
            }
            set
            {
                mInstance.Birthday = value;
            }
        }

        public bool IsPasswordValid(string password)
        {
            return mInstance.Password == GetPasswordHash(password);
        }

#if DEBUG
        private static bool _encrypttickets = true;

        public static bool EncryptTickets
        {
            get
            {
                return _encrypttickets;
            }
            set
            {
                _encrypttickets = value;
            }
        }
#endif

        public static int GetAccountId(string ticket, int def)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return def;
            }

#if DEBUG
            if (!_encrypttickets)
            {
                return int.Parse(ticket);
            }
#endif

            FormsAuthenticationTicket t = FormsAuthentication.Decrypt(ticket);
            if (t == null)
            {
                return def;
            }

            int result = 0;

            int.TryParse(t.Name, out result);

            return result;
        }

        public static int GetAccountId(string ticket)
        {
#if DEBUG
            if (!_encrypttickets)
            {
                return int.Parse(ticket);
            }
#endif

            FormsAuthenticationTicket t = FormsAuthentication.Decrypt(ticket);
            if (t == null)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            return int.Parse(t.Name);
        }

        public static ManagedAccount FindByEmail(ISession session, string emailaddress)
        {
            // find a verified e-mail associated with an account with the same password
            AccountEmail e = (AccountEmail)session.CreateCriteria(typeof(AccountEmail))
                .Add(Expression.Eq("Verified", true))
                .Add(Expression.Eq("Address", emailaddress))
                .UniqueResult();

            if (e == null)
            {
                throw new AccountNotFoundException();
            }

            return new ManagedAccount(session, e.Account);
        }

        #region CRUD

        public override void Delete(ManagedSecurityContext sec)
        {
            ITransaction t = Session.BeginTransaction();

            try
            {
                ManagedDiscussion.FindAndDelete(Session, mInstance.Id, ManagedDiscussion.AccountTagsDiscussion, 0);

                Account newowner = (Account)Session.CreateCriteria(typeof(Account))
                    .Add(Expression.Eq("IsAdministrator", true))
                    .Add(Expression.Not(Expression.Eq("Id", mInstance.Id)))
                    .SetMaxResults(1)
                    .UniqueResult();

                // orphan places
                foreach (Place place in Collection<Place>.GetSafeCollection(mInstance.Places))
                {
                    ManagedPlace mp = new ManagedPlace(Session, place);
                    mp.MigrateToAccount(newowner, sec);
                }

                // orphan public discussions
                foreach (Discussion d in Collection<Discussion>.GetSafeCollection(mInstance.Discussions))
                {
                    if (!d.Personal)
                    {
                        ManagedDiscussion md = new ManagedDiscussion(Session, d);
                        md.MigrateToAccount(newowner);
                    }
                }

                // delete friends and friend requests
                Session.Delete(string.Format("from AccountFriend f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));
                Session.Delete(string.Format("from AccountFriendRequest f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));

                // delete features
                ManagedFeature.Delete(Session, "Account", Id);

                // delete blog authoring access
                Session.Delete(string.Format("from AccountBlogAuthor ba where ba.Account.Id = {0}", Id));

                // delete account
                base.Delete(sec);
                t.Commit();
            }
            catch
            {
                t.Rollback();
                throw;
            }
        }

        public int Create(string name, string password, string emailaddress, DateTime birthday, ManagedSecurityContext sec)
        {
            TransitAccount ta = new TransitAccount();
            ta.Name = name;
            ta.Birthday = birthday;
            ta.Password = password;
            return Create(emailaddress, ta, sec);
        }

        public int Create(string emailaddress, TransitAccount ta, ManagedSecurityContext sec)
        {
            return Create(emailaddress, ta, false, sec);
        }

        public int Create(string emailaddress, TransitAccount ta, bool emailverified, ManagedSecurityContext sec)
        {
            try
            {
                return InternalCreate(emailaddress, ta, emailverified, sec);
            }
            catch
            {
                mInstance = null;
                throw;
            }
        }

        public int CreateWithOpenId(string consumerurl, TransitAccount ta, ManagedSecurityContext sec)
        {
            try
            {
                return InternalCreateWithOpenId(consumerurl, ta, sec);
            }
            catch
            {
                mInstance = null;
                throw;
            }
        }

        public static string GetPasswordHash(string password)
        {
            if (password == null) password = string.Empty;
            return Encoding.Default.GetString(
             new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(password)));
        }

        protected int InternalCreate(string emailaddress, TransitAccount ta, bool emailverified, ManagedSecurityContext sec)
        {
            CreateOrUpdate(ta, sec);

            TransitAccountEmail t_email = new TransitAccountEmail();
            t_email.AccountId = mInstance.Id;
            t_email.Address = new MailAddress(emailaddress).Address;
            t_email.Principal = false;
            t_email.Verified = emailverified;

            ManagedAccountEmail m_instance = new ManagedAccountEmail(Session);
            m_instance.CreateOrUpdate(t_email, sec);
            m_instance.Confirm();

            CreateAccountSystemMessageFolders(sec);
            
            return mInstance.Id;
        }

        protected int InternalCreateWithOpenId(string consumerurl, TransitAccount ta, ManagedSecurityContext sec)
        {
            ta.Password = Guid.NewGuid().ToString(); // random password for recovery
            CreateOrUpdate(ta, sec);

            TransitAccountOpenId t_openid = new TransitAccountOpenId();
            t_openid.IdentityUrl = consumerurl;
            t_openid.AccountId = Id;
            ManagedAccountOpenId m_openid = new ManagedAccountOpenId(Session);
            m_openid.CreateOrUpdate(t_openid, sec);

            CreateAccountSystemMessageFolders(sec);
            return mInstance.Id;
        }

        #endregion

        #region Email

        public int SendAccountEmailMessage(
            string from,
            string to,
            string subject,
            string body,
            bool deletesent)
        {
            AccountEmailMessage m = new AccountEmailMessage();
            m.MailFrom = from;
            m.MailTo = to;
            m.Modified = m.Created = DateTime.UtcNow;
            m.SendError = string.Empty;
            m.Sent = false;
            m.Subject = subject;
            m.Body = body;
            m.DeleteSent = deletesent;
            m.Account = mInstance;

            Session.Save(m);

            return m.Id;
        }

        public void VerifyAllEmails()
        {
            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
            {
                foreach (AccountEmailConfirmation c in e.AccountEmailConfirmations)
                {
                    ManagedAccountEmailConfirmation mac = new ManagedAccountEmailConfirmation(Session, c);
                    mac.Verify(c.Code);
                }
            }
        }

        public bool HasVerifiedEmail
        {
            get
            {
                foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
                {
                    if (e.Verified)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public string ActiveEmailAddress
        {
            get
            {
                string result = null;

                foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
                {
                    result = e.Address;

                    if (e.Verified && e.Principal)
                    {
                        // pickup the principal address first
                        break;
                    }
                }

                return result;
            }
        }

        public static ManagedAccount FindByEmailAndBirthday(ISession session, string emailaddress, DateTime dateofbirth)
        {
            // find a verified e-mail associated with an account with the same password
            AccountEmail e = (AccountEmail)session.CreateCriteria(typeof(AccountEmail))
                .Add(Expression.Eq("Verified", true))
                .Add(Expression.Eq("Address", emailaddress.Trim().ToLower()))
                .UniqueResult();

            if (e == null)
            {
                // there may be no account with a verified e-mail address, so it's ok to reset the password for a
                // matching account with an unverified e-mail address and the right birthday date if there's one

                e = (AccountEmail)session.CreateQuery(
                    "SELECT e FROM AccountEmail e, Account a" +
                    " WHERE e.Account.Id = a.Id" +
                    " AND e.Address = '" + emailaddress.Trim().ToLower() + "'" +
                    " AND a.Birthday = '" + dateofbirth.ToString() + "'")
                    .UniqueResult();
            }

            if (e == null || e.Account.Birthday.Date != dateofbirth.Date)
            {
                throw new AccountNotFoundException();
            }

            return new ManagedAccount(session, e.Account);
        }

        #endregion

        #region Login

        public bool UpdateLastLogin()
        {
            if (mInstance.LastLogin.AddMinutes(30) < DateTime.UtcNow)
            {
                mInstance.LastLogin = DateTime.UtcNow;
                Session.Save(mInstance);
                Session.Flush();
                return true;
            }

            return false;
        }

        public static ManagedAccount Login(ISession session, string email, string password)
        {
            return LoginMd5(session, email, GetPasswordHash(password));
        }

        public static ManagedAccount LoginMd5(ISession session, string email, string passwordhash)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordhash))
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            // find a verified e-mail associated with an account with the same password
            AccountEmail e = (AccountEmail)session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Verified", true))
                    .Add(Expression.Eq("Address", email.Trim().ToLower()))
                    .UniqueResult();

            if (e == null)
            {
                // find an unverified e-mail with the same password
                // allow navigation until someone reclaims the e-mail 

                // note that there may be multiple accounts with the same
                // bogus e-mail address, but if they differ in password
                // the user gets his account

                try
                {
                    e = (AccountEmail)session.CreateCriteria(typeof(AccountEmail))
                        .Add(Expression.Eq("Verified", false))
                        .Add(Expression.Eq("Address", email.Trim().ToLower()))
                        .UniqueResult();
                }
                catch (NonUniqueResultException)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            if (e == null || e.Account.Password != passwordhash)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            e.Account.LastLogin = DateTime.UtcNow;
            session.Save(e.Account);
            return new ManagedAccount(session, e.Account);
        }

        #endregion

        #region Login with OpenId

        public static TransitOpenIdRedirect GetOpenIdRedirect(ISession session, string openidurl, string returnurl)
        {
            // todo: don't use memory store - won't work in a distributed scenario
            Consumer c = new Consumer(new MemoryStore(), new SimpleFetcher());
            AuthRequest r = c.BeginAuth(new Uri(SnCore.Tools.Web.UriBuilder.NormalizeUrl(openidurl)));

            TransitOpenIdRedirect redirect = new TransitOpenIdRedirect(r.token, c.CreateRedirect(
                Consumer.Mode.SETUP, r, new Uri(returnurl),
                ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCore")));

            return redirect;
        }

        public static Uri VerifyOpenId(string token, NameValueCollection query)
        {
            Consumer c = new Consumer(new MemoryStore(), new SimpleFetcher());

            object authresponse = null;
            Consumer.Status s = c.CompleteAuth(token, query, out authresponse);
            if (s == Consumer.Status.SUCCESS && authresponse != null)
            {
                return (Uri)authresponse;
            }

            throw new AccessDeniedException();
        }

        public static ManagedAccount LoginOpenId(ISession session, string token, NameValueCollection query)
        {
            Uri consumerid = VerifyOpenId(token, query);

            // find an openid record that matches
            AccountOpenId o = (AccountOpenId)session.CreateCriteria(typeof(AccountOpenId))
                    .Add(Expression.Eq("IdentityUrl", consumerid.ToString()))
                    .UniqueResult();

            if (o == null)
            {
                throw new SoapException(
                    "Access Denied - no account associated with this OpenId.",
                    SoapException.ClientFaultCode);
            }

            o.Account.LastLogin = DateTime.UtcNow;
            session.Save(o.Account);
            return new ManagedAccount(session, o.Account);
        }

        #endregion

        #region Password

        public void ResetPassword(string newpassword, bool expired)
        {
            if (newpassword.Length < MinimumPasswordLength)
            {
                throw new PasswordTooShortException();
            }

            mInstance.Password = GetPasswordHash(newpassword);
            mInstance.IsPasswordExpired = expired;
            Session.Save(mInstance);
        }

        public void ChangePassword(string oldpassword, string newpassword)
        {
            ChangePasswordMd5(GetPasswordHash(oldpassword), newpassword);
        }

        public void ChangePasswordMd5(string oldpasswordhash, string newpassword)
        {
            if (newpassword.Length < MinimumPasswordLength)
            {
                throw new PasswordTooShortException();
            }

            if (mInstance.Password != oldpasswordhash)
            {
                throw new AccessDeniedException();
            }
            mInstance.Password = GetPasswordHash(newpassword);
            mInstance.IsPasswordExpired = false;
            Session.Save(mInstance);
        }

        #endregion

        #region Rights

        public void PromoteAdministrator()
        {
            mInstance.IsAdministrator = true;
            Session.Save(mInstance);
        }

        public void DemoteAdministrator()
        {
            if (!mInstance.IsAdministrator)
            {
                throw new InvalidOperationException("Account does not have administrative permissions.");
            }

            mInstance.IsAdministrator = false;
            Session.Save(mInstance);
        }

        public bool IsAdministrator()
        {
            return mInstance.IsAdministrator;
        }

        #endregion

        #region Message

        /// <summary>
        /// Send from this account to message.Account.
        /// </summary>
        /// <returns></returns>
        public int SendAccountMessage(TransitAccountMessage m, ManagedSecurityContext sec)
        {
            AccountMessage message = m.GetInstance(Session, sec);

            // check that sending message as self
            if (message.SenderAccountId != 0 && message.SenderAccountId != Id)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            message.Unread = true;
            message.Sent = DateTime.UtcNow;
            message.SenderAccountId = Id;
            message.RecepientAccountId = message.Account.Id;

            if (message.Account.AccountMessages == null) message.Account.AccountMessages = new List<AccountMessage>();
            message.Account.AccountMessages.Add(message);
            Session.Save(message);

            ManagedAccount recepient = new ManagedAccount(Session, message.Account);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                // EmailAccountMessage
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountMessage.aspx?id={0}", message.Id));
            }

            // save a copy in sent items
            AccountMessage s = m.GetInstance(Session, sec);
            s.Account = mInstance;
            s.Sent = message.Sent;
            s.SenderAccountId = Id;
            s.RecepientAccountId = message.Account.Id;
            s.AccountMessageFolder = (AccountMessageFolder)Session.CreateCriteria(typeof(AccountMessageFolder))
                        .Add(Expression.Eq("Account.Id", Id))
                        .Add(Expression.Eq("Name", "sent"))
                        .Add(Expression.IsNull("AccountMessageFolderParent"))
                        .UniqueResult();
            if (mInstance.AccountMessages == null) mInstance.AccountMessages = new List<AccountMessage>();
            mInstance.AccountMessages.Add(s);
            Session.Save(s);

            return message.Id;
        }

        #endregion

        #region MessageFolder

        public void CreateAccountSystemMessageFolders(ManagedSecurityContext sec)
        {
            string[] SystemFolders = 
            {
                "inbox",
                "sent",
                "trash"
            };

            foreach (string folder in SystemFolders)
            {
                try
                {
                    TransitAccountMessageFolder tf = new TransitAccountMessageFolder();
                    tf.Name = folder;
                    tf.System = true;
                    tf.AccountId = Id;

                    ManagedAccountMessageFolder m_folder = new ManagedAccountMessageFolder(Session);
                    m_folder.CreateOrUpdate(tf, sec);
                }
                catch
                {
                    // ignore unique key constraint
                }
            }
        }

        #endregion

        #region Friends

        protected static int GetNDegreeCount(
            ISession session,
            int accountid,
            int deg,
            ref ArrayList knownids)
        {
            ArrayList ids = (ArrayList)
                session.CreateQuery(
                    string.Format("SELECT f.Account.Id FROM AccountFriend f " +
                        "WHERE f.Keen.Id = {0}", accountid))
                    .List();

            ids.AddRange((ArrayList)
                session.CreateQuery(
                    string.Format("SELECT f.Keen.Id FROM AccountFriend f " +
                        "WHERE f.Account.Id = {0}", accountid))
                    .List());

            foreach (int id in ids)
            {
                if (!knownids.Contains(id))
                {
                    knownids.Add(id);

                    if (deg > 1)
                    {
                        GetNDegreeCount(session, id, deg - 1, ref knownids);
                    }
                }
            }

            return knownids.Count;
        }

        public int GetNDegreeCount(int deg)
        {
            if (deg <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Degree", deg, "Degree must be >= 1.");
            }

            ArrayList knownids = new ArrayList();
            knownids.Add(Id);
            return GetNDegreeCount(Session, Id, deg, ref knownids) - 1;
        }

        public bool HasFriend(int friendid)
        {
            return ((int)Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountFriend f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1}) or " +
                    "(f.Keen.Id = {0} and f.Account.Id = {1})",
                    Id, friendid)).UniqueResult() > 0);
        }

        public bool HasFriendRequest(int friendid)
        {
            return ((int)Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountFriendRequest f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1})",
                    Id, friendid)).UniqueResult() > 0);
        }

        public int CreateAccountFriendRequest(int friendid, string message)
        {
            if (friendid == Id)
            {
                throw new SoapException(
                    "You cannot add yourself as a friend.",
                    SoapException.ClientFaultCode);
            }

            AccountFriendRequest request = new AccountFriendRequest();
            // the request belongs to the requester
            request.Account = mInstance;
            request.Keen = (Account)Session.Load(typeof(Account), friendid);
            request.Message = message;
            request.Created = DateTime.UtcNow;

            // check whether a user is alRetreivey friends with
            if (HasFriend(friendid))
            {
                throw new SoapException(string.Format(
                    "{0} is alRetreivey your friend.", request.Keen.Name),
                    SoapException.ClientFaultCode);
            }

            if (HasFriendRequest(friendid))
            {
                throw new SoapException(string.Format(
                    "You have alRetreivey asked {0} to be your friend.", request.Keen.Name),
                    SoapException.ClientFaultCode);
            }

            Session.Save(request);

            ManagedAccount recepient = new ManagedAccount(Session, request.Keen);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    Session,
                    new MailAddress(sentto, recepient.Name).ToString(),
                    string.Format("EmailAccountFriendRequest.aspx?id={0}", request.Id));
            }

            return request.Id;
        }

        #endregion

        #region Tag Words

        public void UpdateTagWords()
        {
            ManagedTagWordCollection tags = new ManagedTagWordCollection();

            // update tagwords from stories
            foreach (AccountStory story in Collection<AccountStory>.GetSafeCollection(mInstance.AccountStories))
            {
                new ManagedAccountStory(Session, story).AddTagWordsTo(tags);
                System.Threading.Thread.Sleep(500);
            }

            // update tagwords from surveys
            foreach (AccountSurveyAnswer answer in Collection<AccountSurveyAnswer>.GetSafeCollection(mInstance.AccountSurveyAnswers))
            {
                new ManagedAccountSurveyAnswer(Session, answer).AddTagWordsTo(tags);
                System.Threading.Thread.Sleep(500);
            }

            foreach (string tag in tags)
            {
                TagWord word = (TagWord)Session.CreateCriteria(typeof(TagWord))
                    .Add(Expression.Eq("Word", tag))
                    .UniqueResult();

                if (word == null)
                {
                    word = new TagWord();
                    word.Word = tag;
                    word.Excluded = false;
                    word.Promoted = false;
                    Session.Save(word);
                }

                TagWordAccount tagword = (TagWordAccount)Session.CreateCriteria(typeof(TagWordAccount))
                    .Add(Expression.Eq("AccountId", Id))
                    .Add(Expression.Eq("TagWord", word))
                    .UniqueResult();

                if (!word.Excluded)
                {
                    if (tagword == null)
                    {
                        tagword = new TagWordAccount();
                        tagword.TagWord = word;
                        tagword.AccountId = Id;
                        tagword.Created = tagword.Modified = DateTime.UtcNow;
                        Session.Save(tagword);
                    }
                }
                else if (tagword != null)
                {
                    Session.Delete(tagword);
                }

                System.Threading.Thread.Sleep(500);
            }

            IList tagwords = Session.CreateCriteria(typeof(TagWordAccount))
                .Add(Expression.Eq("AccountId", Id))
                .List();

            foreach (TagWordAccount tagword in tagwords)
            {
                if (!tags.Contains(tagword.TagWord.Word))
                {
                    Session.Delete(tagword);
                }

                System.Threading.Thread.Sleep(500);
            }

            Session.Flush();
        }

        #endregion

        public static int GetRandomAccountPictureId(Account acct)
        {
            if (acct.AccountPictures == null || acct.AccountPictures.Count == 0)
                return 0;

            List<AccountPicture> copyofcollection = new List<AccountPicture>(acct.AccountPictures.Count);

            foreach (AccountPicture ap in acct.AccountPictures)
            {
                if (!ap.Hidden)
                {
                    copyofcollection.Add(ap);
                }
            }

            return ManagedService<AccountPicture, TransitAccountPicture>.GetRandomElementId(copyofcollection);
        }

        public static Account GetAdminAccount(ISession session)
        {
            Account account = (Account)session.CreateCriteria(typeof(Account))
                .Add(Expression.Eq("IsAdministrator", true))
                .SetMaxResults(1)
                .UniqueResult();

            if (account == null)
            {
                throw new Exception("Missing Administrator");
            }

            return account;
        }

        public static string GetAdminTicket(ISession session)
        {
            Account account = GetAdminAccount(session);
#if DEBUG
            if (!EncryptTickets)
            {
                return account.Id.ToString();
            }
#endif
            return FormsAuthentication.GetAuthCookie(account.Id.ToString(), false).Value;
        }

        public static ManagedSecurityContext GetAdminSecurityContext(ISession session)
        {
            return new ManagedSecurityContext(GetAdminAccount(session));
        }

        public ManagedSecurityContext GetSecurityContext()
        {
            return new ManagedSecurityContext(mInstance);
        }

        protected override void Save(ManagedSecurityContext sec)
        {
            mInstance.Modified = DateTime.UtcNow;
            if (Id == 0) mInstance.Created = mInstance.Modified;
            base.Save(sec);
        }

        public override ACL GetACL()
        {
            ACL acl = base.GetACL();
            acl.Add(new ACLAccount(mInstance, DataOperation.All));
            return acl;
        }
    }
}

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

namespace SnCore.Services
{
    public class AccountActivityQueryOptions
    {
        public string SortOrder = "LastLogin";
        public bool SortAscending = false;
        public bool PicturesOnly = false;
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
                b.Append("EXISTS ELEMENTS(a.AccountPictures)");
            }

            if (!string.IsNullOrEmpty(Name))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("Name LIKE '%{0}%'", Renderer.SqlEncode(Name));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                b.Append(b.Length > 0 ? " AND " : " WHERE ");
                b.AppendFormat("e.Address = '{0}'", Renderer.SqlEncode(Email));
            }

            b.Append(b.Length > 0 ? " AND " : " WHERE ");
            b.Append("e.Account.Id = a.Id");

            return b.ToString();
        }

        public IQuery CreateCountQuery(ISession session)
        {
            return session.CreateQuery("SELECT COUNT(DISTINCT a.Id) FROM Account a, AccountEmail e" + CreateSubQuery(session));
        }

        public IQuery CreateQuery(ISession session)
        {
            StringBuilder b = new StringBuilder();
            b.Append("SELECT DISTINCT a FROM Account a, AccountEmail e");
            b.Append(CreateSubQuery(session));
            if (!string.IsNullOrEmpty(SortOrder))
            {
                b.AppendFormat(" ORDER BY {0} {1}", SortOrder, SortAscending ? "ASC" : "DESC");
            }

            return session.CreateQuery(b.ToString());
        }
    };

    public class TransitAccountActivity : TransitAccount
    {
        private TransitAccountStory mLatestStory = null;

        public TransitAccountStory LatestStory
        {
            get
            {
                return mLatestStory;
            }
            set
            {
                mLatestStory = value;
            }
        }

        private TransitSurvey mLatestSurvey = null;

        public TransitSurvey LatestSurvey
        {
            get
            {
                return mLatestSurvey;
            }
            set
            {
                mLatestSurvey = value;
            }
        }

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
            // latest account story
            DateTime limit = DateTime.UtcNow.AddDays(-14);

            AccountStory story = (AccountStory)session.CreateCriteria(typeof(AccountStory))
                .Add(Expression.Eq("Account.Id", a.Id))
                .Add(Expression.Ge("Modified", limit))
                .AddOrder(Order.Desc("Created"))
                .SetMaxResults(1)
                .UniqueResult();

            if (story != null) LatestStory = new ManagedAccountStory(session, story).TransitAccountStory;

            // latest account survey

            AccountSurveyAnswer answer = (AccountSurveyAnswer)session.CreateCriteria(typeof(AccountSurveyAnswer))
                .Add(Expression.Eq("Account.Id", a.Id))
                .Add(Expression.Ge("Modified", limit))
                .AddOrder(Order.Desc("Created"))
                .SetMaxResults(1)
                .UniqueResult();

            if (answer != null) LatestSurvey = new ManagedSurvey(session, answer.SurveyQuestion.Survey).TransitSurvey;

            // new photos (count one week of photos)

            NewPictures = (int)session.CreateQuery(string.Format("SELECT COUNT(p) FROM AccountPicture p " +
                "WHERE p.Account.Id = {0} AND p.Modified > '{1}'",
                a.Id, limit))
                .UniqueResult();

            NewDiscussionPosts = (int)session.CreateQuery(string.Format("SELECT COUNT(p) FROM DiscussionPost p, Discussion d, DiscussionThread t " +
                "WHERE p.AccountId = {0} AND p.Modified > '{1}' AND p.DiscussionThread.Id = t.Id and t.Discussion.Id = d.Id AND d.Personal = 0",
                a.Id, limit))
                .UniqueResult();

            NewSyndicatedContent = (int)session.CreateQuery(string.Format("SELECT COUNT(f) FROM AccountFeed f " +
                "WHERE f.Account.Id = {0} AND f.Created > '{1}'",
                a.Id, limit))
                .UniqueResult();

        }

        protected int Count
        {
            get
            {
                int result = NewPictures + NewDiscussionPosts + NewSyndicatedContent;
                if (LatestStory != null) result++;
                if (LatestSurvey != null) result++;
                return result;
            }
        }

        public static int CompareByLastActivity(TransitAccountActivity left, TransitAccountActivity right)
        {
            return right.Count - left.Count;
        }

        public static int CompareByLastLogin(TransitAccountActivity left, TransitAccountActivity right)
        {
            return TransitAccount.CompareByLastLogin(left, right);
        }
    }

    public class TransitOpenIdRedirect : TransitService
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

        public TransitOpenIdRedirect(string token, string url)
        {
            mUrl = url;
            mToken = token;
        }
    }

    public class TransitAccountPermissions : TransitService
    {
        private bool mIsAdministrator;

        public bool IsAdministrator
        {
            get
            {
                return mIsAdministrator;
            }
            set
            {
                mIsAdministrator = value;
            }
        }

        public TransitAccountPermissions()
        {
        }

        public TransitAccountPermissions(ManagedAccount a)
        {
            IsAdministrator = a.IsAdministrator();
        }
    }

    public class TransitAccount : TransitService
    {
        public TransitAccount()
        {

        }

        public TransitAccount(Account a)
            : base(a.Id)
        {
            Name = a.Name;
            Birthday = a.Birthday;
            LastLogin = a.LastLogin;
            State = (a.State == null) ? string.Empty : a.State.Name;
            Country = (a.Country == null) ? string.Empty : a.Country.Name;
            City = a.City;
            UtcOffset = a.UtcOffset;
            Signature = a.Signature;
            // random picture from the account
            PictureId = ManagedService.GetRandomElementId(a.AccountPictures);
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

        private int mUtcOffset = 0;

        public int UtcOffset
        {
            get
            {

                return mUtcOffset;
            }
            set
            {
                mUtcOffset = value;
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

        public static int CompareByLastLogin(TransitAccount left, TransitAccount right)
        {
            return right.LastLogin.CompareTo(left.LastLogin);
        }
    }

    /// <summary>
    /// Managed account.
    /// </summary>
    public class ManagedAccount : ManagedService
    {
        public static int MinimumPasswordLength = 4;
        public static int MaxOfAnything = 40;

        public class AccessDeniedException : SoapException
        {
            public AccessDeniedException()
                : base("Access denied", SoapException.ClientFaultCode)
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
                : base("Account already exists", SoapException.ClientFaultCode)
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

        private Account mAccount;

        public string Name
        {
            get
            {
                return mAccount.Name;
            }
        }

        public ManagedAccount(ISession session)
            : base(session)
        {

        }

        public ManagedAccount(ISession session, int id)
            : base(session)
        {
            mAccount = (Account)Session.Load(typeof(Account), id);
        }

        public ManagedAccount(ISession session, Account value)
            : base(session)
        {
            mAccount = value;
        }

        public int Id
        {
            get
            {
                return mAccount.Id;
            }
        }

        public DateTime Birthday
        {
            get
            {
                return mAccount.Birthday;
            }
            set
            {
                mAccount.Birthday = value;
            }
        }

        public bool IsPasswordValid(string password)
        {
            return mAccount.Password == GetPasswordHash(password);
        }

        public TransitAccount TransitAccount
        {
            get
            {
                return new TransitAccount(mAccount);
            }
        }

        public TransitAccountActivity TransitAccountActivity
        {
            get
            {
                return new TransitAccountActivity(Session, mAccount);
            }
        }

        public static int GetAccountId(string ticket, int def)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return def;
            }

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

        public void Delete()
        {
            ITransaction t = Session.BeginTransaction();

            try
            {
                try
                {
                    int DiscussionId = ManagedDiscussion.GetDiscussionId(
                        Session, mAccount.Id, ManagedDiscussion.AccountTagsDiscussion, 0, false);
                    Discussion mDiscussion = (Discussion)Session.Load(typeof(Discussion), DiscussionId);
                    Session.Delete(mDiscussion);
                }
                catch (ManagedDiscussion.DiscussionNotFoundException)
                {

                }

                Session.Delete(string.Format("from AccountFriend f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));
                Session.Delete(string.Format("from AccountFriendRequest f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));
                Session.Delete(string.Format("from Feature f where f.DataObjectId = {0} AND f.DataRowId = {1}", 
                    ManagedDataObject.Find(Session, "Account"), Id));
                Session.Delete(mAccount);
                t.Commit();
            }
            catch
            {
                t.Rollback();
                throw;
            }
        }

        public int Create(string name, string password, string emailaddress, DateTime birthday)
        {
            TransitAccount ta = new TransitAccount();
            ta.Name = name;
            ta.Birthday = birthday;

            return Create(password, emailaddress, ta);
        }

        public int Create(string password, string emailaddress, TransitAccount ta)
        {
            return Create(password, emailaddress, ta, false);
        }

        public int Create(string password, string emailaddress, TransitAccount ta, bool emailverified)
        {
            ITransaction t = Session.BeginTransaction();

            try
            {
                int result = InternalCreate(password, emailaddress, ta, emailverified);
                t.Commit();
                return result;
            }
            catch
            {
                mAccount = null;
                t.Rollback();
                throw;
            }
        }

        public int CreateWithOpenId(string consumerurl, TransitAccount ta)
        {
            ITransaction t = Session.BeginTransaction();

            try
            {
                int result = InternalCreateWithOpenId(consumerurl, ta);
                t.Commit();
                return result;
            }
            catch
            {
                mAccount = null;
                t.Rollback();
                throw;
            }
        }

        public static string GetPasswordHash(string password)
        {
            return Encoding.Default.GetString(
             new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(password)));
        }

        protected int InternalCreate(string password, string emailaddress, TransitAccount ta, bool emailverified)
        {
            if (password.Length < MinimumPasswordLength)
            {
                throw new PasswordTooShortException();
            }

            mAccount = new Account();
            mAccount.Enabled = true;
            mAccount.LastLogin = DateTime.UtcNow;
            mAccount.Password = GetPasswordHash(password);

            Update(ta);

            TransitAccountEmail email = new TransitAccountEmail();
            email.Address = emailaddress;
            Create(email, emailverified);

            CreateAccountSystemMessageFolders();

            return mAccount.Id;
        }

        protected int InternalCreateWithOpenId(string consumerurl, TransitAccount ta)
        {
            mAccount = new Account();
            mAccount.Enabled = true;
            mAccount.LastLogin = DateTime.UtcNow;
            mAccount.Password = GetPasswordHash(Guid.NewGuid().ToString());

            Update(ta);

            TransitAccountOpenId openid = new TransitAccountOpenId();
            openid.IdentityUrl = consumerurl;
            Create(openid);

            CreateAccountSystemMessageFolders();

            return mAccount.Id;
        }

        public void Update(TransitAccount ta)
        {
            mAccount.Name = ta.Name;
            mAccount.Birthday = ta.Birthday;
            mAccount.City = ta.City;
            mAccount.UtcOffset = ta.UtcOffset;
            mAccount.Signature = ta.Signature;

            mAccount.Country = (ta.Country != null && ta.Country.Length > 0)
                ? (Country)Session.Load(typeof(Country), ManagedCountry.GetCountryId(Session, ta.Country))
                : null;

            mAccount.LastLogin = mAccount.Modified = DateTime.UtcNow;
            if (Id == 0) mAccount.Created = mAccount.Modified;

            mAccount.State = (ta.State != null && ta.State.Length > 0)
                ? (State)Session.Load(typeof(State), ManagedState.GetStateId(Session, ta.State, ta.Country))
                : null;

            if (mAccount.State != null && mAccount.Country != null)
            {
                if (mAccount.State.Country.Id != mAccount.Country.Id)
                {
                    throw new ManagedCountry.InvalidCountryException();
                }
            }

            Session.Save(mAccount);
        }

        #endregion

        #region Email

        public void VerifyAllEmails()
        {
            foreach (AccountEmail e in mAccount.AccountEmails)
            {
                foreach (AccountEmailConfirmation c in e.AccountEmailConfirmations)
                {
                    ManagedAccountEmailConfirmation mac = new ManagedAccountEmailConfirmation(Session, c);
                    mac.Verify(c.Code);
                }
            }
        }

        public int Create(TransitAccountEmail email)
        {
            return Create(email, false);
        }

        public int Create(TransitAccountEmail email, bool emailverified)
        {
            AccountEmail e = new AccountEmail();
            e.Account = mAccount;
            e.Address = email.Address.Trim().ToLower();
            e.Verified = emailverified;
            e.Created = e.Modified = DateTime.UtcNow;

            if (mAccount.AccountEmails == null) mAccount.AccountEmails = new ArrayList();

            if (!IsAdministrator() && mAccount.AccountEmails.Count >= MaxOfAnything)
            {
                throw new QuotaExceededException();
            }

            mAccount.AccountEmails.Add(e);
            Session.Save(e);

            ManagedAccountEmail me = new ManagedAccountEmail(Session, e);
            me.Confirm();

            return me.Id;
        }

        public void Update(TransitAccountEmail o)
        {
            // clear principal flags if this e-mail becomes principal
            if (o.Principal)
            {
                foreach (AccountEmail e in mAccount.AccountEmails)
                {
                    if (e.Principal)
                    {
                        e.Principal = false;
                        Session.SaveOrUpdate(e);
                    }
                }
            }

            AccountEmail email = (AccountEmail)Session.Load(typeof(AccountEmail), o.Id);
            email.Modified = DateTime.UtcNow;
            email.Principal = o.Principal;
            Session.SaveOrUpdate(email);
        }

        public string ActiveEmailAddress
        {
            get
            {
                if (mAccount.AccountEmails == null)
                    return null;

                string result = null;

                foreach (AccountEmail e in mAccount.AccountEmails)
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
            if (mAccount.LastLogin.AddMinutes(10) < DateTime.UtcNow)
            {
                mAccount.LastLogin = DateTime.UtcNow;
                Session.SaveOrUpdate(mAccount);
                return true;
            }

            return false;
        }

        public static ManagedAccount Login(ISession session, string emailaddress, string password)
        {
            return LoginMd5(session, emailaddress, GetPasswordHash(password));
        }

        public static ManagedAccount LoginMd5(ISession session, string emailaddress, string passwordhash)
        {
            // find a verified e-mail associated with an account with the same password
            AccountEmail e = (AccountEmail)session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Verified", true))
                    .Add(Expression.Eq("Address", emailaddress.Trim().ToLower()))
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
                        .Add(Expression.Eq("Address", emailaddress.Trim().ToLower()))
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

        public void ResetPassword(string newpassword)
        {
            if (newpassword.Length < MinimumPasswordLength)
            {
                throw new PasswordTooShortException();
            }

            mAccount.Password = GetPasswordHash(newpassword);
            Session.Save(mAccount);
        }

        public void ChangePassword(string oldpassword, string newpassword)
        {
            if (newpassword.Length < MinimumPasswordLength)
            {
                throw new PasswordTooShortException();
            }

            if (mAccount.Password != GetPasswordHash(oldpassword))
            {
                throw new AccessDeniedException();
            }
            mAccount.Password = GetPasswordHash(newpassword);
            Session.Save(mAccount);
        }

        #endregion

        #region Rights

        public void PromoteAdministrator()
        {
            DataObject o = (DataObject)Session.CreateCriteria(typeof(DataObject))
                .Add(Expression.Eq("Name", typeof(Account).Name))
                .UniqueResult();

            if (o == null)
            {
                throw new InvalidProgramException();
            }

            CreateAccountRight(o, new ManagedAccountRightBits(true));
        }

        public void DemoteAdministrator()
        {
            // an administrator has all access to the accounts table

            if (mAccount.AccountRights == null)
            {
                throw new InvalidOperationException("Account does not have special permissions.");
            }

            foreach (AccountRight r in mAccount.AccountRights)
            {
                if (r.DataObject.Name == typeof(Account).Name)
                {
                    if (r.AllowCreate
                        && r.AllowDelete
                        && r.AllowRetrieve
                        && r.AllowUpdate)
                    {
                        mAccount.AccountRights.Remove(r);
                        Session.Delete(r);
                        return;
                    }
                }
            }

            throw new InvalidOperationException("Account does not have administrative permissions.");
        }

        public bool IsAdministrator()
        {
            // an administrator has all access to the accounts table

            if (mAccount.AccountRights == null)
            {
                return false;
            }

            foreach (AccountRight r in mAccount.AccountRights)
            {
                if (r.DataObject.Name == typeof(Account).Name)
                {
                    if (r.AllowCreate
                        && r.AllowDelete
                        && r.AllowRetrieve
                        && r.AllowUpdate)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public ManagedAccountRight GetRight(string objectname)
        {
            foreach (AccountRight r in mAccount.AccountRights)
            {
                ManagedAccountRight mar = new ManagedAccountRight(Session, r);
                if (r.DataObject.Name == typeof(Account).Name && mar.HasAllRights())
                {
                    return mar;
                }
            }
            return null;
        }

        public ManagedAccountRight CreateAccountRight(DataObject o, ManagedAccountRightBits b)
        {
            ManagedAccountRight r = new ManagedAccountRight(Session);
            r.Create(mAccount, o, b);
            return r;
        }

        #endregion

        #region OpenId
        public int Create(TransitAccountOpenId url)
        {
            AccountOpenId o = new AccountOpenId();
            o.Account = mAccount;
            o.IdentityUrl = url.IdentityUrl;
            o.Created = o.Modified = DateTime.UtcNow;

            if (mAccount.AccountOpenIds == null) mAccount.AccountOpenIds = new ArrayList();

            if (!IsAdministrator() && mAccount.AccountOpenIds.Count >= MaxOfAnything)
            {
                throw new QuotaExceededException();
            }

            mAccount.AccountOpenIds.Add(o);
            Session.Save(o);

            ManagedAccountOpenId mo = new ManagedAccountOpenId(Session, o);
            return mo.Id;
        }
        #endregion

        #region Address

        public int CreateOrUpdate(TransitAccountAddress address)
        {
            AccountAddress a = address.GetAccountAddress(Session);
            a.Modified = DateTime.UtcNow;

            if (a.Id == 0)
            {
                a.Created = a.Modified;
                a.Account = mAccount;
                if (mAccount.AccountAddresses == null) mAccount.AccountAddresses = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountAddresses.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountAddresses.Add(a);
            }
            else
            {
                // check whether editing address of self
                if (a.Account.Id != mAccount.Id)
                {
                    throw new AccessDeniedException();
                }
            }

            Session.Save(a);
            return a.Id;
        }

        #endregion

        #region Survey Answer

        public int CreateOrUpdate(TransitAccountSurveyAnswer SurveyAnswer)
        {
            AccountSurveyAnswer p = SurveyAnswer.GetAccountSurveyAnswer(Session);
            p.Modified = DateTime.UtcNow;

            if (p.Id == 0)
            {
                p.Account = mAccount;
                p.Created = p.Modified;
                if (mAccount.AccountSurveyAnswers == null) mAccount.AccountSurveyAnswers = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountSurveyAnswers.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountSurveyAnswers.Add(p);
            }
            else
            {
                // check that editing SurveyAnswer of self
                if (p.Account.Id != Id)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            Session.Save(p);
            return p.Id;
        }

        #endregion

        #region Website

        public int CreateOrUpdate(TransitAccountWebsite website)
        {
            AccountWebsite w = website.GetAccountWebsite(Session);

            if (!Uri.IsWellFormedUriString(website.Url, UriKind.Absolute))
            {
                throw new ManagedAccountWebsite.InvalidUriException();
            }

            if (w.Id == 0)
            {
                w.Account = mAccount;
                if (mAccount.AccountWebsites == null) mAccount.AccountWebsites = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountWebsites.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountWebsites.Add(w);
            }
            else
            {
                // check that editing website of self
                if (w.Account.Id != Id)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            Session.Save(w);
            return w.Id;
        }

        #endregion

        #region Message

        /// <summary>
        /// Send from this account to message.Account.
        /// </summary>
        /// <returns></returns>
        public int SendAccountMessage(TransitAccountMessage m)
        {
            AccountMessage message = m.GetAccountMessage(Session);

            // check that sending message as self
            if (message.SenderAccountId != 0 && message.SenderAccountId != Id)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            message.Sent = DateTime.UtcNow;
            message.SenderAccountId = Id;
            message.RecepientAccountId = message.Account.Id;

            if (message.Account.AccountMessages == null) message.Account.AccountMessages = new ArrayList();
            message.Account.AccountMessages.Add(message);
            Session.Save(message);

            ManagedAccount recepient = new ManagedAccount(Session, message.Account);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                string partialurl = string.Format(
                    "AccountMessageFoldersManage.aspx?id={0}",
                    message.AccountMessageFolder.Id);

                string url = string.Format(
                    "{0}/{1}",
                    ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                    partialurl);

                string replyurl = string.Format(
                    "{0}/AccountMessageEdit.aspx?id={1}&pid={2}&ReturnUrl={3}&#edit",
                    ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                    Id,
                    message.Id,
                    Renderer.UrlEncode(partialurl));

                string messagebody =
                    "<html>" +
                    "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                    "<body>" +
                    "Dear " + Renderer.Render(message.Account.Name) + ",<br>" +
                    "<br>You have a new message in your \"" + Renderer.Render(message.AccountMessageFolder.Name) + "\".<br>" +
                    "<br><b>from:</b> " + Renderer.Render(mAccount.Name) +
                    "<br><b>subject:</b> " + Renderer.Render(message.Subject) +
                    "<br><b>posted:</b> " + message.Sent.ToString() +
                    "<br><br>" +
                    Renderer.RenderEx(message.Body) +
                    "<blockquote>" +
                    "<a href=\"" + url + "\">View</a> or <a href=\"" + replyurl + "\">Reply</a>" +
                    "</blockquote>" +
                    "</body>" +
                    "</html>";

                recepient.SendAccountMailMessage(
                    string.Format("{0} <noreply@{1}>",
                        Renderer.Render(mAccount.Name),
                        ManagedConfiguration.GetValue(Session, "SnCore.Domain", "vestris.com")),
                    sentto,
                    string.Format("{0}: {1} has sent you a message.",
                        ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                        Renderer.Render(mAccount.Name)),
                    messagebody,
                    true);
            }

            // save a copy in sent items
            AccountMessage s = m.GetAccountMessage(Session);
            s.Account = mAccount;
            s.Sent = message.Sent;
            s.SenderAccountId = Id;
            s.RecepientAccountId = message.Account.Id;
            s.AccountMessageFolder = (AccountMessageFolder)Session.CreateCriteria(typeof(AccountMessageFolder))
                        .Add(Expression.Eq("Account.Id", Id))
                        .Add(Expression.Eq("Name", "sent"))
                        .Add(Expression.IsNull("AccountMessageFolderParent"))
                        .UniqueResult();
            if (mAccount.AccountMessages == null) mAccount.AccountMessages = new ArrayList();
            mAccount.AccountMessages.Add(s);
            Session.Save(s);

            return message.Id;
        }

        public int SendAccountMailMessage(
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
            m.Account = mAccount;

            Session.Save(m);

            return m.Id;
        }

        #endregion

        #region Picture

        public int CreateOrUpdate(TransitAccountPictureWithBitmap picture)
        {
            AccountPicture p = picture.GetAccountPicture(Session);
            p.Modified = DateTime.UtcNow;

            if (p.Id == 0)
            {
                p.Created = p.Modified;
                p.Account = mAccount;
                if (mAccount.AccountPictures == null) mAccount.AccountPictures = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountPictures.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountPictures.Add(p);
            }
            else
            {
                if (p.Account.Id != mAccount.Id)
                {
                    throw new AccessDeniedException();
                }
            }

            Session.Save(p);
            return p.Id;
        }

        #endregion

        #region MessageFolder

        public void CreateAccountSystemMessageFolders()
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
                    CreateOrUpdate(tf);
                }
                catch
                {
                    // ignore unique key constraint
                }
            }
        }

        public int CreateOrUpdate(TransitAccountMessageFolder messagefolder)
        {
            AccountMessageFolder e = messagefolder.GetAccountMessageFolder(Session);

            e.Modified = DateTime.UtcNow;

            if (e.Id == 0)
            {
                e.Account = mAccount;
                e.Created = e.Modified;

                if (mAccount.AccountMessageFolders == null) mAccount.AccountMessageFolders = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountMessageFolders.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
            }
            else
            {
                if (e.Account.Id != mAccount.Id)
                {
                    throw new AccessDeniedException();
                }
            }

            try
            {
                Session.Save(e);
            }
            catch
            {
                Session.Evict(e);
                throw;
            }

            if (messagefolder.Id == 0)
            {
                mAccount.AccountMessageFolders.Add(e);
            }

            return e.Id;
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
            IList friends = Session.Find(
                string.Format("from AccountFriend f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1}) or " +
                    "(f.Keen.Id = {0} and f.Account.Id = {1})",
                    Id, friendid));

            return friends.Count > 0;
        }

        public bool HasFriendRequest(int friendid)
        {
            IList friendrequests = Session.Find(
                string.Format("from AccountFriendRequest f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1})",
                    Id, friendid));

            return friendrequests.Count > 0;
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
            request.Account = mAccount;
            request.Keen = (Account)Session.Load(typeof(Account), friendid);
            request.Message = message;
            request.Created = DateTime.UtcNow;

            // check whether a user is already friends with
            if (HasFriend(friendid))
            {
                throw new SoapException(string.Format(
                    "{0} is already your friend.", request.Keen.Name), 
                    SoapException.ClientFaultCode);
            }

            if (HasFriendRequest(friendid))
            {
                throw new SoapException(string.Format(
                    "You have already asked {0} to be your friend.", request.Keen.Name), 
                    SoapException.ClientFaultCode);
            }

            Session.Save(request);

            ManagedAccount recepient = new ManagedAccount(Session, request.Keen);
            string sentto = recepient.ActiveEmailAddress;
            if (sentto != null)
            {
                string viewurl = string.Format(
                    "{0}/AccountFriendRequestsManage.aspx",
                    ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"));

                string acturl = string.Format(
                    "{0}/AccountFriendRequestAct.aspx?id={1}",
                    ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                    request.Id);

                string messagebody =
                    "<html>" +
                    "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                    "<body>" +
                    "Dear " + Renderer.Render(request.Keen.Name) + ",<br>" +
                    "<br><b>" + Renderer.Render(Name) + "</b> wants to be your friend." +
                    "<br><br>" +
                    Renderer.Render(message) +
                    "<blockquote>" +
                    "<a href=\"" + viewurl + "\">View</a> this and other pending requests and decide later." +
                    "<br><a href=\"" + acturl + "&action=accept\">Accept</a> this request." +
                    "<br><a href=\"" + acturl + "&action=reject\">Reject</a> this request (no message will be sent)." +
                    "</blockquote>" +
                    "</body>" +
                    "</html>";

                recepient.SendAccountMailMessage(
                    string.Format("{0} <noreply@{1}>",
                        Renderer.Render(mAccount.Name),
                        ManagedConfiguration.GetValue(Session, "SnCore.Domain", "vestris.com")),
                    sentto,
                    string.Format("{0}: {1} wants to be your friend.",
                        ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                        Renderer.Render(mAccount.Name)),
                    messagebody,
                    true);
            }

            return request.Id;
        }

        #endregion

        #region Story

        public int CreateOrUpdateWithPictures(TransitAccountStory Story, TransitAccountStoryPictureWithPicture[] Pictures)
        {
            ManagedAccountStory s = new ManagedAccountStory(Session, CreateOrUpdate(Story));

            foreach (TransitAccountStoryPictureWithPicture p in Pictures)
            {
                s.AddAccountStoryPicture(p);
            }

            return s.Id;
        }

        public int CreateOrUpdateWithPictures(TransitAccountStory Story, TransitAccountStoryPicture[] Pictures)
        {
            ManagedAccountStory s = new ManagedAccountStory(Session, CreateOrUpdate(Story));

            foreach (TransitAccountStoryPicture p in Pictures)
            {
                s.AddAccountStoryPicture(p);
            }

            return s.Id;
        }

        public int CreateOrUpdate(TransitAccountStory Story)
        {
            AccountStory s = Story.GetAccountStory(Session);

            s.Modified = DateTime.UtcNow;

            if (s.Id == 0)
            {
                s.Account = mAccount;
                s.Created = s.Modified;
                if (mAccount.AccountStories == null) mAccount.AccountStories = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountStories.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountStories.Add(s);
            }
            else
            {
                // check that editing Story of self
                if (s.Account.Id != mAccount.Id && !IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            Session.Save(s);

            return s.Id;
        }

        #endregion

        #region Invitation

        public int CreateOrUpdate(TransitAccountInvitation Invitation)
        {
            AccountInvitation s = Invitation.GetAccountInvitation(Session);

            s.Modified = DateTime.UtcNow;

            if (s.Id == 0)
            {
                s.Account = mAccount;
                s.Created = s.Modified;
                if (mAccount.AccountInvitations == null) mAccount.AccountInvitations = new ArrayList();
                if (!IsAdministrator() && mAccount.AccountInvitations.Count >= MaxOfAnything)
                {
                    throw new QuotaExceededException();
                }
                mAccount.AccountInvitations.Add(s);
            }
            else
            {
                // check that editing Invitation of self
                if (s.Account.Id != Id)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }

            Session.Save(s);

            ManagedAccountInvitation result = new ManagedAccountInvitation(Session, s);
            result.Send();
            return result.Id;
        }

        #endregion

        #region Tag Words

        public void UpdateTagWords()
        {
            ManagedTagWordCollection tags = new ManagedTagWordCollection();

            // update tagwords from stories
            if (mAccount.AccountStories != null)
            {
                foreach (AccountStory story in mAccount.AccountStories)
                {
                    new ManagedAccountStory(Session, story).AddTagWordsTo(tags);
                    System.Threading.Thread.Sleep(500);
                }
            }

            // update tagwords from surveys
            if (mAccount.AccountSurveyAnswers != null)
            {
                foreach (AccountSurveyAnswer answer in mAccount.AccountSurveyAnswers)
                {
                    new ManagedAccountSurveyAnswer(Session, answer).AddTagWordsTo(tags);
                    System.Threading.Thread.Sleep(500);
                }
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
                    }
                    else
                    {
                        tagword.Modified = DateTime.UtcNow;
                    }

                    Session.SaveOrUpdate(tagword);
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

        #region Account Feed

        public int CreateOrUpdate(TransitAccountFeed o)
        {
            AccountFeed feed = o.GetAccountFeed(Session);

            if (Id != 0)
            {
                if (feed.Account.Id != Id && !IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                feed.LastError = "Feed has not been updated since last save.";
            }

            feed.Updated = DateTime.UtcNow;
            if (feed.Id == 0) feed.Created = feed.Updated;
            Session.Save(feed);
            return feed.Id;
        }

        #endregion

        #region Place

        public int CreateOrUpdate(TransitPlace o)
        {
            Place place = o.GetPlace(Session);
            place.Modified = DateTime.UtcNow;

            if (place.Id == 0)
            {
                place.Created = place.Modified;
                place.Account = mAccount;
            }
            else
            {
                if (place.Account.Id != mAccount.Id && !IsAdministrator())
                {
                    ManagedPlace m_place = new ManagedPlace(Session, place.Id);
                    if (!m_place.CanWrite(Id))
                    {
                        throw new AccessDeniedException();
                    }
                }
            }

            Session.Save(place);
            return place.Id;
        }

        #endregion

        #region Account Place Request
        public int CreateOrUpdate(TransitAccountPlaceRequest o)
        {
            AccountPlace e_place = (AccountPlace)Session.CreateCriteria(typeof(AccountPlace))
                .Add(Expression.Eq("Account.Id", o.AccountId))
                .Add(Expression.Eq("Place.Id", o.PlaceId))
                .UniqueResult();

            if (e_place != null)
            {
                throw new SoapException("You already have a relationship with this place.",
                    SoapException.ClientFaultCode);
            }

            AccountPlaceRequest request = o.GetAccountPlaceRequest(Session);

            if (request.Id != 0)
            {
                if (request.Account.Id != mAccount.Id && !IsAdministrator())
                {
                    throw new AccessDeniedException();
                }
            }
            else
            {
                request.Submitted = DateTime.UtcNow;
            }

            Session.Save(request);

            List<string> listto = new List<string>();
            listto.Add(ManagedConfiguration.GetValue(Session, "SnCore.Admin.EmailAddress", "admin@localhost.com"));

            if (request.Place.AccountPlaces != null)
            {
                foreach (AccountPlace place in request.Place.AccountPlaces)
                {
                    if (place.Type.CanWrite)
                    {
                        ManagedAccount acct = new ManagedAccount(Session, place.Account);
                        listto.Add(acct.ActiveEmailAddress);
                    }
                }
            }

            string viewurl = string.Format(
                "{0}/SystemAccountPlaceRequestsManage.aspx?id={1}",
                ManagedConfiguration.GetValue(Session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                request.Place.Id);

            foreach (string sentto in listto)
            {
                if (sentto == null)
                    continue;

                string messagebody =
                    "<html>" +
                    "<style>body { font-size: .80em; font-family: Verdana; }</style>" +
                    "<body>" +
                    "Hi,<br>" +
                    "<br><b>" + Renderer.Render(request.Account.Name) +
                    "</b> wants to own <b>" + Renderer.Render(request.Place.Name) + "</b>." +
                    "<br><br>" +
                    Renderer.Render(request.Message) +
                    "<blockquote>" +
                    "<a href=\"" + viewurl + "\">View</a> this and other pending requests." +
                    "</blockquote>" +
                    "</body>" +
                    "</html>";

                ManagedAccount acct = new ManagedAccount(Session, mAccount);

                acct.SendAccountMailMessage(
                    string.Format("{0} <noreply@{1}>",
                        Renderer.Render(mAccount.Name),
                        ManagedConfiguration.GetValue(Session, "SnCore.Domain", "vestris.com")),
                    sentto,
                    string.Format("{0}: {1} wants to own {2}.",
                        ManagedConfiguration.GetValue(Session, "SnCore.Name", "SnCore"),
                        Renderer.Render(mAccount.Name), request.Place.Name),
                    messagebody,
                    true);
            }

            return request.Id;
        }
        #endregion

        #region Account Place

        public int CreateOrUpdate(TransitAccountPlace o)
        {
            AccountPlace place = o.GetAccountPlace(Session);

            if (place.Id != 0)
            {
                if (place.Account.Id != mAccount.Id && !IsAdministrator())
                {
                    throw new AccessDeniedException();
                }
            }

            place.Modified = DateTime.UtcNow;
            if (place.Id == 0) place.Created = place.Modified;
            Session.Save(place);
            return place.Id;
        }

        #endregion

        #region Account Place Favorite

        public int CreateOrUpdate(TransitAccountPlaceFavorite o)
        {
            AccountPlaceFavorite pf = o.GetAccountPlaceFavorite(Session);

            if (pf.Id != 0)
            {
                // cannot modify an existing favorite
                throw new AccessDeniedException();
            }

            if (pf.Id == 0) pf.Created = DateTime.UtcNow;
            Session.Save(pf);
            return pf.Id;
        }

        #endregion

        #region Account Profile

        public int CreateOrUpdate(TransitAccountProfile o)
        {
            AccountProfile profile = o.GetAccountProfile(Session);

            if (profile.Id != 0)
            {
                if (profile.Account.Id != mAccount.Id && !IsAdministrator())
                {
                    throw new AccessDeniedException();
                }
            }

            profile.Updated = DateTime.UtcNow;
            Session.Save(profile);
            return profile.Id;
        }

        #endregion

    }
}

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
using Janrain.OpenId.Store;
using Janrain.OpenId.Session;
using Janrain.OpenId;
using Janrain.OpenId.Consumer;
using SnCore.Tools;
using SnCore.Data.Hibernate;
using System.Globalization;

namespace SnCore.Services
{
    public class TransitAccountDeleteOptions
    {
        public bool DeleteContent = false;

        public TransitAccountDeleteOptions()
        {

        }

        public override int GetHashCode()
        {
            return PersistentlyHashable.GetHashCode(this);
        }
    };

    public class TransitAccountNumbers
    {
        private int mFirstDegreeCount = 0;
        private int mSecondDegreeCount = 0;
        private int mAllCount = 0;
        private int mPostsCount = 0;

        public int PostsCount
        {
            get
            {
                return mPostsCount;
            }
            set
            {
                mPostsCount = value;
            }
        }

        public int FirstDegreeCount
        {
            get
            {
                return mFirstDegreeCount;
            }
            set
            {
                mFirstDegreeCount = value;
            }
        }

        public int SecondDegreeCount
        {
            get
            {
                return mSecondDegreeCount;
            }
            set
            {
                mSecondDegreeCount = value;
            }
        }

        public int AllCount
        {
            get
            {
                return mAllCount;
            }
            set
            {
                mAllCount = value;
            }
        }

        public int NewCount
        {
            get
            {
                return AllCount - FirstDegreeCount - 1; // all minus first degree minus self
            }
        }

        public TransitAccountNumbers()
        {

        }
    }

    #region OpenID

    public class TransitOpenIdLogin
    {
        private string mConsumerUrl;
        private string mTicket;

        public string ConsumerUrl
        {
            get
            {
                return mConsumerUrl;
            }
            set
            {
                mConsumerUrl = value;
            }
        }

        public string Ticket
        {
            get
            {
                return mTicket;
            }
            set
            {
                mTicket = value;
            }
        }

        public TransitOpenIdLogin()
        {

        }
    }

    public class ManagedOpenIdLogin
    {
        private Uri mConsumerUri;
        private Account mAccount = null;

        public Uri ConsumerUri
        {
            get
            {
                return mConsumerUri;
            }
            set
            {
                mConsumerUri = value;
            }
        }

        public Account Account
        {
            get
            {
                return mAccount;
            }
            set
            {
                mAccount = value;
            }
        }

        public ManagedOpenIdLogin()
        {

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
                // BUGBUG: Janrain OpenId appends port 80 to local urls, which some OpenId providers can't handle
                mUrl = mUrl.Replace(":80/", "/");
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
            Url = url;
            Token = token;
        }
    }

    #endregion

    #region Facebook

    public class TransitFacebookLogin
    {
        private long mFacebookAccountId;
        private string mTicket;

        public long FacebookAccountId
        {
            get
            {
                return mFacebookAccountId;
            }
            set
            {
                mFacebookAccountId = value;
            }
        }

        public string Ticket
        {
            get
            {
                return mTicket;
            }
            set
            {
                mTicket = value;
            }
        }

        public TransitFacebookLogin()
        {

        }
    }

    public class ManagedFacebookLogin
    {
        private long mFacebookAccountId;
        private Account mAccount = null;

        public long FacebookAccountId
        {
            get
            {
                return mFacebookAccountId;
            }
            set
            {
                mFacebookAccountId = value;
            }
        }

        public Account Account
        {
            get
            {
                return mAccount;
            }
            set
            {
                mAccount = value;
            }
        }

        public ManagedFacebookLogin()
        {

        }
    }

    #endregion

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
            IsAdministrator = instance.IsAdministrator;
            IsPasswordExpired = instance.IsPasswordExpired;

            IsVerified = false;
            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(instance.AccountEmails))
            {
                if (e.Verified)
                {
                    IsVerified = true;
                    break;
                }
            }

            LCID = instance.Lcid;
            // random picture from the account
            PictureId = ManagedAccount.GetRandomAccountPictureId(instance);
            Created = instance.Created;
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

            instance.State = string.IsNullOrEmpty(State)
                ? null
                : ManagedState.Find(session, State, Country);

            instance.Country = string.IsNullOrEmpty(Country)
                ? null
                : ManagedCountry.Find(session, Country);

            if (instance.State != null && instance.Country != null)
            {
                if (instance.State.Country.Id != instance.Country.Id)
                {
                    throw new ManagedCountry.InvalidCountryException();
                }
            }

            instance.City = City;
            instance.TimeZone = TimeZone;
            instance.Signature = Signature;
            instance.Lcid = LCID;
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

        private bool mIsVerified = false;

        public bool IsVerified
        {
            get
            {
                return mIsVerified;
            }
            set
            {
                mIsVerified = value;
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

        private bool mIsAdministrator = false;

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

        private int mLCID = 0;

        public int LCID
        {
            get
            {
                return mLCID;
            }
            set
            {
                mLCID = value;
            }
        }

        public string Culture
        {
            get
            {
                if (mLCID == 0) return CultureInfo.InstalledUICulture.Name;
                return new CultureInfo(mLCID).Name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    mLCID = CultureInfo.CreateSpecificCulture(value).LCID;
                }
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
    public class ManagedAccount : ManagedAuditableService<Account, TransitAccount>
    {
        public static int MinimumPasswordLength = 4;
        public static int MaxOfAnything = 250;

        public class InvalidPasswordException : Exception
        {
            public InvalidPasswordException()
                : base("Invalid password")
            {

            }
        }

        public class InvalidUsernamePasswordException : Exception
        {
            public InvalidUsernamePasswordException()
                : base("Invalid username and/or password")
            {

            }
        }

        public class AccessDeniedException : Exception
        {
            private string mRequestUri = string.Empty;

            public string RequestUri
            {
                get
                {
                    return mRequestUri;
                }
                set
                {
                    mRequestUri = value;
                }
            }

            public AccessDeniedException()
                : base("Access denied")
            {

            }

            public AccessDeniedException(string uri)
                : base("Access denied")
            {
                mRequestUri = uri;
            }
        }

        public class NoVerifiedException : Exception
        {
            public NoVerifiedException()
                : base("To help prevent spam, you must confirm your e-mail address and upload a picture before posting anything or making friends")
            {

            }
        }

        public class NoVerifiedEmailException : Exception
        {
            public NoVerifiedEmailException()
                : base("To help prevent spam, you must confirm your e-mail address before posting anything or making friends")
            {

            }
        }

        public class NoAccountPictureException : Exception
        {
            public NoAccountPictureException()
                : base("To help prevent spam, you must upload a picture before posting anything or making friends")
            {

            }
        }

        public class AccountNotFoundException : Exception
        {
            public AccountNotFoundException()
                : base("Account not found")
            {

            }
        }

        public class PasswordTooShortException : Exception
        {
            public PasswordTooShortException()
                : base("Password too short")
            {

            }
        }

        public class AccountExistsException : Exception
        {
            public AccountExistsException()
                : base("Account already exists")
            {

            }
        }

        public class QuotaExceededException : Exception
        {
            public QuotaExceededException()
                : base("Quota exceeded. An administrator has been notified.")
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

        public bool IsPasswordValidMd5(string password)
        {
            return mInstance.Password == password;
        }

        public static int GetAccountId(string ticket, int def)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return def;
            }

            string id = DPAPI.Decrypt(ticket);

            int result = 0;
            int.TryParse(id, out result);
            return result;
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
                ManagedDiscussion.FindAndDelete(
                    Session, mInstance.Id, typeof(Account), 0, sec);

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

                // orphan place pictures
                foreach (PlacePicture placepicture in Collection<PlacePicture>.GetSafeCollection(mInstance.PlacePictures))
                {
                    ManagedPlacePicture mp = new ManagedPlacePicture(Session, placepicture);
                    mp.MigrateToAccount(placepicture.Place.Account, sec);
                }

                // orphan account events
                foreach (AccountEvent accountevent in Collection<AccountEvent>.GetSafeCollection(mInstance.AccountEvents))
                {
                    ManagedAccountEvent mp = new ManagedAccountEvent(Session, accountevent);
                    mp.MigrateToAccount(newowner, sec);
                }

                // orphan accountevent pictures
                foreach (AccountEventPicture accounteventpicture in Collection<AccountEventPicture>.GetSafeCollection(mInstance.AccountEventPictures))
                {
                    ManagedAccountEventPicture mp = new ManagedAccountEventPicture(Session, accounteventpicture);
                    mp.MigrateToAccount(accounteventpicture.AccountEvent.Account, sec);
                }

                // orphan public discussions
                foreach (Discussion d in Collection<Discussion>.GetSafeCollection(mInstance.Discussions))
                {
                    if (!d.Personal)
                    {
                        ManagedDiscussion md = new ManagedDiscussion(Session, d);
                        md.MigrateToAccount(newowner, sec);
                    }
                }

                // leave groups
                foreach (AccountGroupAccount groupaccount in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroupAccounts))
                {
                    ManagedAccountGroupAccount m_groupaccount = new ManagedAccountGroupAccount(Session, groupaccount);
                    m_groupaccount.Delete(sec);
                }

                // orphan group pictures
                foreach (AccountGroupPicture grouppicture in Collection<AccountGroupPicture>.GetSafeCollection(mInstance.AccountGroupPictures))
                {
                    ManagedAccountGroupPicture m_grouppicture = new ManagedAccountGroupPicture(Session, grouppicture);
                    m_grouppicture.MigrateToGroupOwner(sec);
                }

                // orphan place websites
                foreach (PlaceWebsite placewebsite in Collection<PlaceWebsite>.GetSafeCollection(mInstance.PlaceWebsites))
                {
                    ManagedPlaceWebsite mpw = new ManagedPlaceWebsite(Session, placewebsite);
                    mpw.MigrateToAccount(newowner, sec);
                }

                // delete all group invitations to me, group requests are cascade-deleted
                Session.Delete(string.Format("FROM AccountGroupAccountInvitation i WHERE i.Account.Id = {0}", Id));

                // delete friends and friend requests
                Session.Delete(string.Format("from AccountFriend f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));
                Session.Delete(string.Format("from AccountFriendRequest f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));

                // delete features
                ManagedFeature.Delete(Session, "Account", Id);

                // delete blog authoring access
                Session.Delete(string.Format("from AccountBlogAuthor ba where ba.Account.Id = {0}", Id));

                // delete flags
                Session.Delete(string.Format("from AccountFlag af where af.FlaggedAccount.Id = {0}", Id));

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

        public void DeleteContent(ManagedSecurityContext sec)
        {
            ITransaction t = Session.BeginTransaction();

            try
            {
                // delete associated discussions
                ManagedDiscussion.FindAndDelete(
                    Session, mInstance.Id, typeof(Account), 0, sec);

                // delete discussion posts
                IList<DiscussionPost> posts = ManagedDiscussionPost.GetDiscussionPosts(Session, mInstance.Id);
                foreach (DiscussionPost post in posts)
                {
                    ManagedDiscussionPost m_post = new ManagedDiscussionPost(Session, post);
                    m_post.Delete(sec);
                }

                // delete places
                foreach (Place place in Collection<Place>.GetSafeCollection(mInstance.Places))
                {
                    ManagedPlace mp = new ManagedPlace(Session, place);
                    mp.Delete(sec);
                }

                // deleteaccount events
                foreach (AccountEvent accountevent in Collection<AccountEvent>.GetSafeCollection(mInstance.AccountEvents))
                {
                    ManagedAccountEvent mp = new ManagedAccountEvent(Session, accountevent);
                    mp.Delete(sec);
                }

                // delete public discussions
                foreach (Discussion d in Collection<Discussion>.GetSafeCollection(mInstance.Discussions))
                {
                    if (!d.Personal)
                    {
                        ManagedDiscussion md = new ManagedDiscussion(Session, d);
                        md.Delete(sec);
                    }
                }

                // leave groups
                foreach (AccountGroupAccount groupaccount in Collection<AccountGroupAccount>.GetSafeCollection(mInstance.AccountGroupAccounts))
                {
                    ManagedAccountGroupAccount m_groupaccount = new ManagedAccountGroupAccount(Session, groupaccount);
                    m_groupaccount.Delete(sec);
                }

                // delete group pictures
                foreach (AccountGroupPicture grouppicture in Collection<AccountGroupPicture>.GetSafeCollection(mInstance.AccountGroupPictures))
                {
                    ManagedAccountGroupPicture m_grouppicture = new ManagedAccountGroupPicture(Session, grouppicture);
                    m_grouppicture.Delete(sec);
                }

                // delete  place websites
                foreach (PlaceWebsite placewebsite in Collection<PlaceWebsite>.GetSafeCollection(mInstance.PlaceWebsites))
                {
                    ManagedPlaceWebsite mpw = new ManagedPlaceWebsite(Session, placewebsite);
                    mpw.Delete(sec);
                }

                // delete all group invitations to me, group requests are cascade-deleted
                Session.Delete(string.Format("FROM AccountGroupAccountInvitation i WHERE i.Account.Id = {0}", Id));

                // delete friends and friend requests
                Session.Delete(string.Format("from AccountFriend f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));
                Session.Delete(string.Format("from AccountFriendRequest f where f.Account.Id = {0} or f.Keen.Id = {0}", Id));

                // delete features
                ManagedFeature.Delete(Session, "Account", Id);

                // delete blog authoring access
                Session.Delete(string.Format("from AccountBlogAuthor ba where ba.Account.Id = {0}", Id));

                // delete flags
                Session.Delete(string.Format("from AccountFlag af where af.FlaggedAccount.Id = {0}", Id));
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

        public int CreateWithOpenId(string consumerurl, string email, TransitAccount ta, ManagedSecurityContext sec)
        {
            try
            {
                return InternalCreateWithOpenId(consumerurl, email, ta, sec);
            }
            catch
            {
                mInstance = null;
                throw;
            }
        }

        public int CreateWithFacebook(long facebookAccountId, string email, TransitAccount ta, ManagedSecurityContext sec)
        {
            try
            {
                return InternalCreateWithFacebook(facebookAccountId, email, ta, sec);
            }
            catch
            {
                mInstance = null;
                throw;
            }
        }

        public static string GetPasswordHash(string password)
        {
            return Encoding.Default.GetString(GetPasswordHashBytes(password));
        }

        public static byte[] GetPasswordHashBytes(string password)
        {
            if (password == null) password = string.Empty;
            return new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(password));
        }

        protected int InternalCreate(string emailaddress, TransitAccount ta, bool emailverified, ManagedSecurityContext sec)
        {
            CreateOrUpdate(ta, sec);
            InternalCreateEmail(emailaddress, emailverified, sec);
            CreateAccountSystemMessageFolders(sec);
            CreateAccountSubscriptions(sec);
            return mInstance.Id;
        }

        private void InternalCreateEmail(string email, bool verified, ManagedSecurityContext sec)
        {
            TransitAccountEmail t_email = new TransitAccountEmail();
            t_email.AccountId = mInstance.Id;
            t_email.Address = new MailAddress(email).Address;
            t_email.Principal = false;
            t_email.Verified = verified;
            t_email.Failed = false;

            ManagedAccountEmail m_email = new ManagedAccountEmail(Session);
            m_email.CreateOrUpdate(t_email, sec);
            m_email.Confirm(sec);

            mInstance.AccountEmails = new List<AccountEmail>();
            mInstance.AccountEmails.Add(m_email.Instance);
        }

        protected int InternalCreateWithOpenId(string consumerurl, string email, TransitAccount ta, ManagedSecurityContext sec)
        {
            ta.Password = Guid.NewGuid().ToString(); // random password for recovery
            CreateOrUpdate(ta, sec);
            InternalCreateEmail(email, false, sec);
            CreateAccountSystemMessageFolders(sec);
            CreateAccountSubscriptions(sec);

            // the OpenId identity has been verified, hence it's safe to assume that this is the same user
            // OpenId doesn't have a reclaim and at this point only the verified OpenId would be phishing for himself

            AccountOpenId existing_openid = Session.CreateCriteria(typeof(AccountOpenId))
                .Add(Expression.Eq("IdentityUrl", consumerurl))
                .UniqueResult<AccountOpenId>();

            if (existing_openid != null)
            {
                throw new Exception(string.Format("An account with the OpenId \"{0}\" already exists. Please log-in instead.",
                    existing_openid.IdentityUrl));
            }

            TransitAccountOpenId t_openid = new TransitAccountOpenId();
            t_openid.IdentityUrl = consumerurl;
            t_openid.AccountId = Id;
            ManagedAccountOpenId m_openid = new ManagedAccountOpenId(Session);
            m_openid.CreateOrUpdate(t_openid, sec);

            return mInstance.Id;
        }

        protected int InternalCreateWithFacebook(long facebookAccountId, string email, TransitAccount ta, ManagedSecurityContext sec)
        {
            // the Facebook identity has been verified, hence it's safe to assume that this is the same user
            // Facebook doesn't have a reclaim and at this point only the verified OpenId would be phishing for himself

            AccountFacebook existing_facebook_account = Session.CreateCriteria(typeof(AccountFacebook))
                .Add(Expression.Eq("FacebookAccountId", facebookAccountId))
                .UniqueResult<AccountFacebook>();

            if (existing_facebook_account != null)
            {
                throw new Exception(string.Format("An account for the Facebook user \"{0}\" already exists. Please log-in instead.",
                    existing_facebook_account.FacebookAccountId));
            }

            ta.Password = Guid.NewGuid().ToString(); // random password for recovery
            CreateOrUpdate(ta, sec);
            InternalCreateEmail(email, true, sec);
            CreateAccountSystemMessageFolders(sec);
            CreateAccountSubscriptions(sec);

            TransitAccountFacebook t_facebook_account = new TransitAccountFacebook();
            t_facebook_account.FacebookAccountId = facebookAccountId;
            t_facebook_account.AccountId = Id;
            ManagedAccountFacebook m_facebook_account = new ManagedAccountFacebook(Session);
            m_facebook_account.CreateOrUpdate(t_facebook_account, sec);

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

        public void AddDefaultPicture()
        {
            AccountPicture p = new AccountPicture();
            p.Account = mInstance;
            p.Bitmap = new byte[128];
            p.Name = "default picture";
            p.Position = 0;
            p.Description = "";
            p.Created = p.Modified = DateTime.UtcNow;
            Session.Save(p);

            if (mInstance.AccountPictures == null)
                mInstance.AccountPictures = new List<AccountPicture>();

            mInstance.AccountPictures.Add(p);
        }

        public void VerifyAllEmails()
        {
            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
            {
                foreach (AccountEmailConfirmation c in Collection<AccountEmailConfirmation>.GetSafeCollection(e.AccountEmailConfirmations))
                {
                    ManagedAccountEmailConfirmation mac = new ManagedAccountEmailConfirmation(Session, c);
                    mac.Verify(c.Code);
                }
            }
        }

        public bool HasPicture(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);
            return Collection<AccountPicture>.GetSafeCollection(mInstance.AccountPictures).Count > 0;
        }

        public bool HasVerifiedEmail(ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);

            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
            {
                if (e.Verified)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetVerifiedEmailAddress(out string address, ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);

            address = null;

            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
            {
                if (e.Verified && !e.Failed)
                {
                    address = e.Address;
                    if (e.Principal)
                    {
                        // pickup the principal address first
                        break;
                    }
                }
            }

            return !string.IsNullOrEmpty(address);
        }

        public bool TryGetActiveEmailAddress(out string address, ManagedSecurityContext sec)
        {
            GetACL().Check(sec, DataOperation.Retreive);

            address = null;

            foreach (AccountEmail e in Collection<AccountEmail>.GetSafeCollection(mInstance.AccountEmails))
            {
                address = e.Address;

                if (e.Verified && e.Principal && !e.Failed)
                {
                    // pickup the principal address first
                    break;
                }
            }

            return !string.IsNullOrEmpty(address);
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
                    " AND a.Birthday = '" + dateofbirth.ToString(DateTimeFormatInfo.InvariantInfo) + "'")
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

        public static ManagedAccount LoginMd5(ISession session, string email, byte[] passwordhash)
        {
            return LoginMd5(session, email, Encoding.Default.GetString(passwordhash));
        }

        public static ManagedAccount LoginMd5(ISession session, string email, string passwordhash)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordhash))
            {
                throw new ManagedAccount.InvalidUsernamePasswordException();
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
                    throw new ManagedAccount.InvalidUsernamePasswordException();
                }
            }

            if (e == null || e.Account.Password != passwordhash)
            {
                throw new ManagedAccount.InvalidUsernamePasswordException();
            }

            e.Account.LastLogin = DateTime.UtcNow;
            session.Save(e.Account);
            return new ManagedAccount(session, e.Account);
        }

        #endregion

        #region Login with OpenId

        public static TransitOpenIdRedirect GetOpenIdRedirect(ISession session, string openidurl, string returnurl)
        {
            Consumer c = new Consumer(SimpleSessionState.GetInstance(), MemoryStore.GetInstance());
            AuthRequest request = c.Begin(new Uri(SnCore.Tools.Web.UriBuilder.NormalizeUrl(openidurl)));

            Uri redirect = request.CreateRedirect(
                ManagedConfiguration.GetValue(session, "SnCore.WebSite.Url", "http://localhost/SnCore"),
                new Uri(new Uri(returnurl).GetLeftPart(UriPartial.Path)),
                AuthRequest.Mode.SETUP);

            return new TransitOpenIdRedirect(request.Token, redirect.ToString());
        }

        public static Uri VerifyOpenId(string token, NameValueCollection query)
        {
            Consumer c = new Consumer(SimpleSessionState.GetInstance(), MemoryStore.GetInstance());
            ConsumerResponse response = c.Complete(query);
            return response.IdentityUrl;
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
                throw new Exception(
                    "Access Denied - " +
                    "your OpenId is valid, but you have to register on this site using an e-mail address first. " +
                    "You can add an OpenId once you've joined.");
            }

            o.Account.LastLogin = DateTime.UtcNow;
            session.Save(o.Account);
            return new ManagedAccount(session, o.Account);
        }

        public static ManagedOpenIdLogin TryLoginOpenId(ISession session, string token, NameValueCollection query)
        {
            ManagedOpenIdLogin t_result = new ManagedOpenIdLogin();

            t_result.ConsumerUri = VerifyOpenId(token, query);
            AccountOpenId account = (AccountOpenId)session.CreateCriteria(typeof(AccountOpenId))
                    .Add(Expression.Eq("IdentityUrl", t_result.ConsumerUri.ToString()))
                    .UniqueResult();

            if (account != null)
            {
                t_result.Account = account.Account;
                t_result.Account.LastLogin = DateTime.UtcNow;
                session.Save(t_result.Account);
            }

            return t_result;
        }

        #endregion

        #region Facebook

        public static bool VerifyFacebookLogin(ISession session, string signature, NameValueCollection cookies)
        {
            string facebookAPIKey = ManagedConfiguration.GetValue(session, "Facebook.APIKey", string.Empty);
            if (string.IsNullOrEmpty(facebookAPIKey))
                throw new Exception("Missing Facebook.APIKey");

            string facebookSecret = ManagedConfiguration.GetValue(session, "Facebook.Secret", string.Empty);
            if (string.IsNullOrEmpty(facebookSecret))
                throw new Exception("Missing Facebook.Secret");

            var sb = new StringBuilder();
            foreach (String s in cookies.AllKeys)
            {
                sb.AppendFormat("{0}={1}", s, cookies[s]);
            }

            sb.Append(facebookSecret);
            string stringToHash = sb.ToString();

            StringBuilder computedHash = new StringBuilder();
            byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            foreach (byte b in hash)
            {
                computedHash.AppendFormat("{0:x2}", b);
            }

            return computedHash.ToString().ToLowerInvariant() == signature.ToLowerInvariant();
        }

        public static long GetFacebookFacebookAccountId(ISession session, NameValueCollection cookies)
        {
            return long.Parse(cookies["user"]);
        }

        public static ManagedAccount LoginFacebook(ISession session, string signature, NameValueCollection cookies)
        {
            if (!VerifyFacebookLogin(session, signature, cookies))
                throw new AccessDeniedException("Invalid FaceBook Connect signature");

            long facebookAccountId = GetFacebookFacebookAccountId(session, cookies);

            // find a facebook account record that matches
            AccountFacebook o = (AccountFacebook)session.CreateCriteria(typeof(AccountFacebook))
                    .Add(Expression.Eq("FacebookAccountId", facebookAccountId))
                    .UniqueResult();

            if (o == null)
            {
                throw new Exception(
                    "Access Denied - " +
                    "your Facebook connect login is valid, but you have to register on this site using an e-mail address first." +
                    "You can add an Facebook account after you've joined.");
            }

            o.Account.LastLogin = DateTime.UtcNow;
            session.Save(o.Account);
            return new ManagedAccount(session, o.Account);
        }

        public static ManagedFacebookLogin TryLoginFacebook(ISession session, string signature, NameValueCollection cookies)
        {
            if (!VerifyFacebookLogin(session, signature, cookies))
                throw new AccessDeniedException("Invalid FaceBook Connect signature");

            ManagedFacebookLogin t_result = new ManagedFacebookLogin();
            t_result.FacebookAccountId = GetFacebookFacebookAccountId(session, cookies);

            AccountFacebook account = (AccountFacebook)session.CreateCriteria(typeof(AccountFacebook))
                    .Add(Expression.Eq("FacebookAccountId", t_result.FacebookAccountId))
                    .UniqueResult();

            if (account != null)
            {
                t_result.Account = account.Account;
                t_result.Account.LastLogin = DateTime.UtcNow;
                session.Save(t_result.Account);
            }

            return t_result;
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

        #region MessageFolder

        public AccountMessageFolder FindRootFolder(string name)
        {
            try
            {
                return ManagedAccountMessageFolder.FindRootFolder(
                    Session, Id, name);
            }
            catch (ObjectNotFoundException)
            {
                return null;
            }
        }

        public void CreateAccountSubscriptions(ManagedSecurityContext sec)
        {
            // subscribe to friend broadcasts
            TransitAccountRssWatch friends_broadcasts = new TransitAccountRssWatch();
            friends_broadcasts.AccountId = mInstance.Id;
            friends_broadcasts.Enabled = true;
            friends_broadcasts.Name = "Friends' Broadcasts";
            friends_broadcasts.UpdateFrequency = 24;
            friends_broadcasts.Url = "AccountFriendAuditEntriesRss.aspx?Title=Friends'+Broadcasts&Broadcast=True";
            ManagedAccountRssWatch m_friends_broadcasts = new ManagedAccountRssWatch(Session);
            m_friends_broadcasts.CreateOrUpdate(friends_broadcasts, sec);
        }

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
                AccountMessageFolder instance = FindRootFolder(folder);

                if (instance != null)
                    continue;

                TransitAccountMessageFolder tf = new TransitAccountMessageFolder();
                tf.Name = folder;
                tf.System = true;
                tf.AccountId = Id;

                ManagedAccountMessageFolder m_folder = new ManagedAccountMessageFolder(Session);
                m_folder.CreateOrUpdate(tf, sec);
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

        public int GetNDegreeCount(ManagedSecurityContext sec, int deg)
        {
            GetACL().Check(sec, DataOperation.Retreive);

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
            return (Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountFriend f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1}) or " +
                    "(f.Keen.Id = {0} and f.Account.Id = {1})",
                    Id, friendid)).UniqueResult<int>() > 0);
        }

        public bool HasFriendRequest(int friendid)
        {
            return (Session.CreateQuery(
                string.Format("SELECT COUNT(*) FROM AccountFriendRequest f where " +
                    "(f.Account.Id = {0} and f.Keen.Id = {1})",
                    Id, friendid)).UniqueResult<int>() > 0);
        }

        public int CreateAccountFriendRequest(ManagedSecurityContext sec, int friendid, string message)
        {
            sec.CheckVerified();

            if (friendid == Id)
            {
                throw new Exception(
                    "You cannot add yourself as a friend.");
            }

            AccountFriendRequest request = new AccountFriendRequest();
            // the request belongs to the requester
            request.Account = mInstance;
            request.Keen = Session.Load<Account>(friendid);
            request.Message = message;
            request.Created = DateTime.UtcNow;

            // check whether a user is already friends with
            if (HasFriend(friendid))
            {
                throw new Exception(string.Format(
                    "{0} is already your friend.", request.Keen.Name));
            }

            if (HasFriendRequest(friendid))
            {
                throw new Exception(string.Format(
                    "You have already asked {0} to be your friend.", request.Keen.Name));
            }

            Session.Save(request);
            Session.Flush();

            ManagedAccount recepient = new ManagedAccount(Session, request.Keen);
            ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                Session, recepient, string.Format("EmailAccountFriendRequest.aspx?id={0}", request.Id));

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

        public static int GetRandomAccountPictureId(ISession session, int id)
        {
            try
            {
                Account account = session.Load<Account>(id);
                return GetRandomAccountPictureId(account);
            }
            catch (ObjectNotFoundException)
            {
                return 0;
            }
        }

        public static int GetRandomAccountPictureId(Account acct)
        {
            if (acct == null || acct.AccountPictures == null || acct.AccountPictures.Count == 0)
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
            return GetUserTicket(session, account.Id);
        }

        public static string GetUserTicket(ISession session, int id)
        {
            Account account = session.Load<Account>(id);
            return GetTicketFromAccount(account);
        }

        public static ManagedSecurityContext GetUserSecurityContext(ISession session, int user_id)
        {
            return new ManagedSecurityContext(session.Load<Account>(user_id));
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

        public override ACL GetACL(Type type)
        {
            ACL acl = base.GetACL(type);
            acl.Add(new ACLEveryoneAllowRetrieve());
            acl.Add(new ACLAccount(mInstance, DataOperation.All));

            if (ManagedDiscussion.IsDiscussionType(type))
            {
                acl.Add(new ACLAuthenticatedAllowCreate());
            }

            return acl;
        }

        public TransitAccountFriend GetTransformedInstanceFromAccountFriend(ISession session, ManagedSecurityContext sec, AccountFriend friend)
        {
            ManagedAccountFriend m_instance = new ManagedAccountFriend();
            if (friend.Account.Id != mInstance.Id)
            {
                Account temp = friend.Account;
                friend.Account = friend.Keen;
                friend.Keen = temp;
            }
            m_instance.SetInstance(session, friend);
            return m_instance.GetTransitInstance(sec);
        }

        public static string GetAccountNameWithDefault(ISession session, int id)
        {
            try
            {
                Account account = session.Load<Account>(id);
                return account.Name;
            }
            catch (ObjectNotFoundException)
            {
                return "Unknown User";
            }
        }

        public override IList<AccountAuditEntry> CreateAccountAuditEntries(ISession session, ManagedSecurityContext sec, DataOperation op)
        {
            List<AccountAuditEntry> result = new List<AccountAuditEntry>();

            switch (op)
            {
                case DataOperation.Create:
                    result.Add(ManagedAccountAuditEntry.CreateSystemAccountAuditEntry(session, mInstance,
                        string.Format("[user:{0}] joined", mInstance.Id), string.Format("AccountView.aspx?id={0}", mInstance.Id)));
                    break;
                case DataOperation.Update:
                    result.Add(ManagedAccountAuditEntry.CreatePublicAccountAuditEntry(session, mInstance,
                        string.Format("[user:{0}] updated the public profile", mInstance.Id), string.Format("AccountView.aspx?id={0}", mInstance.Id)));
                    break;
                case DataOperation.Delete:
                    result.Add(ManagedAccountAuditEntry.CreateSystemAccountAuditEntry(session, mInstance,
                        string.Format("{0} ({1}) has been deleted", mInstance.Name, mInstance.Id), string.Format("AccountView.aspx?id={0}", mInstance.Id)));
                    break;
            }

            return result;
        }

        public static string GetTicketFromAccount(Account acct)
        {
            return GetTicketFromAccountId(acct.Id);
        }

        public static string GetTicketFromAccountId(int id)
        {
            return DPAPI.Encrypt(id.ToString());
        }

        public static int GetAccountIdFromTicket(string ticket)
        {
            try
            {
                // new: use DPAPI
                return int.Parse(DPAPI.Decrypt(ticket));
            }
            catch
            {
                try
                {
                    // backwards compatbility: forms encryption
                    FormsAuthenticationTicket t = FormsAuthentication.Decrypt(ticket);
                    if (t == null) throw new ManagedAccount.AccessDeniedException();
                    int result = int.Parse(t.Name);
                    return result;
                }
                catch
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }
        }
    }
}

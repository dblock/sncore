using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using SnCore.Tools;
using System.Net.Mail;
using System.Text;
using System.Drawing;
using SnCore.Tools.Drawing;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web account services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebAccountService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebAccountService : WebService
    {
        public WebAccountService()
        {

        }

        #region Login

        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="email">verified e-mail address</param>
        /// <param name="password">valid password</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Login.")]
        public string Login(string email, string password)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.Login(session, email, password);
                string ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                SnCore.Data.Hibernate.Session.Flush();
                return ticket;
            }
        }

        /// <summary>
        /// Login to an account with an OpenId.
        /// </summary>
        /// <param name="openidurl">openid url</param>
        /// <param name="returnurl">return url</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Login to an account.")]
        public string LoginOpenId(string token, string[] names, string[] values)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.LoginOpenId(session, token, new NameValueCollectionSerializer(names, values).Collection);
                string ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                SnCore.Data.Hibernate.Session.Flush();
                return ticket;
            }
        }

        /// <summary>
        /// Try a login to an account with an OpenId. 
        /// Returns a package that may contain a ticket or a verified open id consumer uri.
        /// </summary>
        /// <param name="openidurl">openid url</param>
        /// <param name="returnurl">return url</param>
        /// <returns>authentication package that might contain a ticket for the current session</returns>
        [WebMethod(Description = "Login to an account.")]
        public TransitOpenIdLogin TryLoginOpenId(string token, string[] names, string[] values)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitOpenIdLogin t_result = new TransitOpenIdLogin();
                ManagedOpenIdLogin t_login = ManagedAccount.TryLoginOpenId(session, token, new NameValueCollectionSerializer(names, values).Collection);
                t_result.ConsumerUrl = t_login.ConsumerUri.ToString();
                if (t_login.Account != null)
                {
                    string ticket = ManagedAccount.GetTicketFromAccount(t_login.Account);
                    SnCore.Data.Hibernate.Session.Flush();
                    t_result.Ticket = ticket;
                }
                return t_result;
            }
        }

        /// <summary>
        /// Login to an account with a Facebook identity.
        /// </summary>
        /// <param name="names">cookie names</param>
        /// <param name="values">cookie values</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Login to an account with a Facebook Connect identity.")]
        public string LoginFacebook(string signature, string[] names, string[] values)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.LoginFacebook(session, signature, 
                    new NameValueCollectionSerializer(names, values).Collection);
                string ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                SnCore.Data.Hibernate.Session.Flush();
                return ticket;
            }
        }

        /// <summary>
        /// Try a login to an account with a Facebook Connect identity. 
        /// Returns a package that may contain a ticket or a verified facebook user id.
        /// </summary>
        /// <param name="facebookurl">facebook url</param>
        /// <param name="returnurl">return url</param>
        /// <returns>authentication package that might contain a ticket for the current session</returns>
        [WebMethod(Description = "Login to an account using a Facebook Connect identity.")]
        public TransitFacebookLogin TryLoginFacebook(string signature, string[] names, string[] values)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitFacebookLogin t_result = new TransitFacebookLogin();
                ManagedFacebookLogin t_login = ManagedAccount.TryLoginFacebook(session, signature,
                    new NameValueCollectionSerializer(names, values).Collection);
                t_result.FacebookAccountId = t_login.FacebookAccountId;
                if (t_login.Account != null)
                {
                    string ticket = ManagedAccount.GetTicketFromAccount(t_login.Account);
                    SnCore.Data.Hibernate.Session.Flush();
                    t_result.Ticket = ticket;
                }
                return t_result;
            }
        }

        /// <summary>
        /// Login to an account using a verified e-mail address and a password hash.
        /// Using the password hash avoids transferring the actual password accross an unsecure network.
        /// </summary>
        /// <param name="emailaddress">verified e-mail address</param>
        /// <param name="passwordhash">valid password MD5 hash</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Login using a password hash.")]
        public string LoginMd5(string emailaddress, byte[] passwordhash)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.LoginMd5(session, emailaddress, passwordhash);
                string ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                SnCore.Data.Hibernate.Session.Flush();
                return ticket;
            }
        }

        #endregion

        #region Beta

        /// <summary>
        /// Verify beta password.
        /// </summary>
        /// <param name="password">password</param>
        [WebMethod(Description = "Verify beta password.")]
        public void VerifyBetaPassword(string betapassword)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
            }
        }

        /// <summary>
        /// Check whether a beta password is set.
        /// </summary>
        [WebMethod(Description = "Check whether a beta password is set.", CacheDuration = 60)]
        public bool IsBetaPasswordSet()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                return !string.IsNullOrEmpty(s);
            }
        }

        #endregion

        #region SignUp

        /// <summary>
        /// Create an account.
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="emailaddress">e-mail address</param>
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account.")]
        public int CreateAccount(string betapassword, string emailaddress, TransitAccount ta)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                acct.Create(emailaddress, ta, ManagedAccount.GetAdminSecurityContext(session));
                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Create an account with openid.
        /// </summary>
        /// <param name="ta">transit account informatio</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account with openid.")]
        public int CreateAccountWithOpenId(string betapassword, 
            string token, string[] names, string[] values,
            string email, TransitAccount ta)
        {
            string consumerurl = VerifyOpenId(token, names, values);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                acct.CreateWithOpenId(consumerurl, email, ta, ManagedAccount.GetAdminSecurityContext(session));
                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Create an account with a facebook id.
        /// </summary>
        /// <param name="betapassword"></param>
        /// <param name="signature"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [WebMethod(Description = "Create an account with facebook id.")]
        public int CreateAccountWithFacebook(string betapassword, string signature, string[] names, string[] values)
        {
            TransitFacebookLogin t_facebook = TryLoginFacebook(signature, names, values);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);

                // fetch account information
                Facebook.Session.ConnectSession facebookSession = new Facebook.Session.ConnectSession(
                    ManagedConfiguration.GetValue(session, "Facebook.APIKey", ""),
                    ManagedConfiguration.GetValue(session, "Facebook.Secret", ""));

                TransitAccount ta = new TransitAccount();
                NameValueCollectionSerializer facebookCookies = new NameValueCollectionSerializer(names, values);
                facebookSession.SessionKey = facebookCookies.Collection["session_key"];
                facebookSession.UserId = t_facebook.FacebookAccountId;
                Facebook.Rest.Api facebookAPI = new Facebook.Rest.Api(facebookSession);
                Facebook.Schema.user user = facebookAPI.Users.GetInfo();
                ta.Name = user.name;
                DateTime birthday = DateTime.Today;
                DateTime.TryParse(user.birthday_date, out birthday);
                ta.Birthday = birthday;
                acct.CreateWithFacebook(t_facebook.FacebookAccountId, user.proxied_email, 
                    ta, ManagedAccount.GetAdminSecurityContext(session));

                if (user.current_location != null)
                {
                    ta.City = user.current_location.city;

                    int country_id;
                    if (ManagedCountry.TryGetCountryId(session, user.current_location.country, out country_id))
                        ta.Country = user.current_location.country;
                    
                    int state_id;
                    if (ManagedState.TryGetStateId(session, user.current_location.state, user.current_location.country, out state_id))
                        ta.State = user.current_location.state;
                }

                if (user.picture_big != null)
                {
                    TransitAccountPicture t_picture = new TransitAccountPicture();
                    t_picture.AccountId = acct.Id;
                    ThumbnailBitmap bitmap = new ThumbnailBitmap(new Bitmap(user.picture_big));
                    t_picture.Bitmap = bitmap.Bitmap;
                    t_picture.Thumbnail = bitmap.Thumbnail;
                    t_picture.Name = user.pic;
                    ManagedAccountPicture m_picture = new ManagedAccountPicture(session);
                    m_picture.CreateOrUpdate(t_picture, ManagedAccount.GetAdminSecurityContext(session));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Create an account with a facebook ID, e-mail address and a transit account.
        /// </summary>
        /// <param name="betapassword"></param>
        /// <param name="email"></param>
        /// <param name="names"></param>
        /// <param name="signature"></param>
        /// <param name="values"></param>
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account with facebook id.")]
        public int CreateAccountWithFacebookAndEmail(string betapassword, string signature, string[] names, string[] values, string email, TransitAccount ta)
        {
            TransitFacebookLogin t_facebook = TryLoginFacebook(signature, names, values);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                acct.CreateWithFacebook(t_facebook.FacebookAccountId, email, ta, ManagedAccount.GetAdminSecurityContext(session));
                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Get an OpenId redirect.
        /// </summary>
        /// <param name="openidurl">openid url</param>
        /// <param name="returnurl">return url</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Get an openid redirect.")]
        public TransitOpenIdRedirect GetOpenIdRedirect(string openidurl, string returnurl)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return ManagedAccount.GetOpenIdRedirect(session, openidurl, returnurl);
            }
        }

        /// <summary>
        /// Verify an OpenId.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        [WebMethod(Description = "Verify an OpenId.")]
        public string VerifyOpenId(string token, string[] names, string[] values)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return ManagedAccount.VerifyOpenId(token, new NameValueCollectionSerializer(names, values).Collection).ToString();
            }
        }

        #endregion

        #region Account

        /// <summary>
        /// Get accounts count.
        /// </summary>
        /// <returns>accounts count</returns>
        [WebMethod(Description = "Get accounts count.", CacheDuration = 60)]
        public int GetAccountsCount(string ticket)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get accounts.
        /// </summary>
        /// <returns>accounts count</returns>
        [WebMethod(Description = "Get accounts.", CacheDuration = 60)]
        public List<TransitAccount> GetAccounts(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options);
        }


        /// <summary>
        /// Get account id.
        /// </summary>
        /// <param name="ticket">authentication ticket previously obtained from SnCore::WebAccountService::Login or SnCore::WebAccountService::LoginMd5</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Get account id.", CacheDuration = 60)]
        public int GetAccountId(string ticket)
        {
            return ManagedAccount.GetAccountIdFromTicket(ticket);
        }

        /// <summary>
        /// Get logged in account information. 
        /// Also updates last login time on five minute intervals.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get account information.")]
        public TransitAccount GetAccount(string ticket, bool updatelastlogin)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                try
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                    ManagedAccount account = new ManagedAccount(session, sec.Account);
                    if (updatelastlogin) account.UpdateLastLogin();
                    return account.GetTransitInstance(sec);
                }
                catch (ObjectNotFoundException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get an administrative account.
        /// </summary>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get an administrative account.", CacheDuration = 60)]
        public TransitAccount GetAdminAccount(string ticket)
        {
            ICriterion[] expressions =  { Expression.Eq("IsAdministrator", true) };            
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetByCriterion(
                ticket, expressions, 1);
        }

        /// <summary>
        /// Get account information.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get account information.", CacheDuration = 60)]
        public TransitAccount GetAccountById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account</param>
        [WebMethod(Description = "Create or update an account.")]
        public int CreateOrUpdateAccount(string ticket, TransitAccount t_instance)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Delete an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account to delete</param>
        [WebMethod(Description = "Delete an account.")]
        public void DeleteAccount(string ticket, int id)
        {
            DeleteAccountWithOptions(ticket, id, null);
        }

        /// <summary>
        /// Delete an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account to delete</param>
        [WebMethod(Description = "Delete an account.")]
        public void DeleteAccountWithOptions(string ticket, int id, TransitAccountDeleteOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount user = new ManagedAccount(session, id);

                if (user.IsAdministrator())
                {
                    throw new Exception(
                        "You cannot delete an administrative account.");
                }

                if (sec.Account.Id != user.Id)
                {
                    if (!sec.IsAdministrator())
                    {
                        // only admin can delete other people's account
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                if (options != null && options.DeleteContent)
                {
                    if (!sec.IsAdministrator())
                    {
                        // only admin can delete other people's content
                        throw new ManagedAccount.AccessDeniedException();
                    }

                    user.DeleteContent(sec);
                }
            }

            WebServiceImpl<TransitAccount, ManagedAccount, Account>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Find an account by e-mail address.
        /// </summary>
        /// <param name="emailaddress">verified e-mail address</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Find an account by e-mail address.")]
        public TransitAccount FindByEmail(string ticket, string emailaddress)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount m_account = ManagedAccount.FindByEmail(session, emailaddress);
                return m_account.GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Find all accounts by e-mail address.
        /// </summary>
        /// <param name="emailaddress">e-mail address</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Find an account by e-mail address.")]
        public List<TransitAccount> FindAllByEmail(string ticket, string emailaddress, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetListFromIds(
                ticket, options, string.Format("SELECT AccountEmail.Account.Id FROM AccountEmail AccountEmail" +
                    " WHERE AccountEmail.Address = '{0}'", Renderer.SqlEncode(emailaddress)));
        }

        #endregion

        #region AccountInvitation

        /// <summary>
        /// Create an account with an e-mail invitation and login.
        /// </summary>
        /// <param name="invitationid"></param>
        /// <param name="code"></param>
        /// <param name="ta"></param>
        /// <returns></returns>
        [WebMethod(Description = "Create an account and login.")]
        public string CreateAccountWithInvitationAndLogin(int invitationid, string code, TransitAccount ta)
        {
            int id = CreateAccountWithInvitation(invitationid, code, ta);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, id);
                string ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                return ticket;
            }
        }

        /// <summary>
        /// Create an account with an e-mail invitation.
        /// </summary>
        /// <param name="invitationid">invitation id</param>
        /// <param name="code">code</param>
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account.")]
        public int CreateAccountWithInvitation(int invitationid, string code, TransitAccount ta)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountInvitation invitation = new ManagedAccountInvitation(session, invitationid);
                if (invitation.Code != code)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                int id = acct.Create(invitation.Instance.Email, ta, true, ManagedAccount.GetAdminSecurityContext(session));

                // become friends with the person who invited you
                TransitAccountFriend t_friend = new TransitAccountFriend();
                AccountFriend friend = new AccountFriend();
                friend.Account = session.Load<Account>(invitation.AccountId);
                friend.Keen = session.Load<Account>(id);
                friend.Created = DateTime.UtcNow;
                session.Save(friend);
                SnCore.Data.Hibernate.Session.Flush();

                // notify that a friend has joined
                ManagedAccount recepient = new ManagedAccount(session, invitation.AccountId);
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(session, recepient,
                    string.Format("EmailAccountInvitationAccept.aspx?id={0}&aid={1}", invitation.Id, id));

                // if this is a group invitation, join the group
                if (invitation.Instance.AccountGroup != null)
                {
                    AccountGroupAccountInvitation groupInvitation = new AccountGroupAccountInvitation();
                    groupInvitation.Account = session.Load<Account>(id);
                    groupInvitation.AccountGroup = invitation.Instance.AccountGroup;
                    groupInvitation.Created = invitation.Instance.Created;
                    groupInvitation.Modified = invitation.Instance.Modified;
                    groupInvitation.Requester = session.Load<Account>(invitation.AccountId);
                    ManagedAccountGroupAccountInvitation mGroupInvitation = new ManagedAccountGroupAccountInvitation(
                        session, groupInvitation);
                    mGroupInvitation.JoinGroup(ManagedAccount.GetAdminSecurityContext(session), string.Empty);
                }

                invitation.Delete(ManagedAccount.GetAdminSecurityContext(session));

                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Decline an invitation to the system.
        /// </summary>
        /// <param name="id">invitation id</param>
        /// <param name="code">invitation code</param>
        [WebMethod(Description = "Decline an invitation.")]
        public void DeclineInvitation(int id, string code)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountInvitation invitation = new ManagedAccountInvitation(session, id);
                if (invitation.Code != code)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount recepient = new ManagedAccount(session, invitation.AccountId);
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(session, recepient,
                    string.Format("EmailAccountInvitationReject.aspx?id={0}", invitation.Id));

                invitation.Delete(ManagedAccount.GetAdminSecurityContext(session));
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Invite a person.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="invitation">transit invitation</param>
        [WebMethod(Description = "Invite a person.")]
        public TransitAccountInvitation GetAccountInvitationByEmail(string ticket, int id, string email)
        {
            ICriterion[] expressions = 
            {
                Expression.Eq("Email", email),
                Expression.Eq("Account.Id", id)
            };

            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetByCriterion(
                ticket, expressions);
        }

        /// <summary>
        /// Invite a person.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="invitation">transit invitation</param>
        [WebMethod(Description = "Invite a person.")]
        public int CreateOrUpdateAccountInvitation(string ticket, TransitAccountInvitation invitation)
        {
            if (invitation.Id == 0)
            {
                TransitAccountInvitation t_instance = GetAccountInvitationByEmail(
                    ticket, invitation.AccountId, invitation.Email);
                
                if (t_instance != null)
                {
                    throw new Exception("Existing invitation pending");
                }
            }

            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.CreateOrUpdate(
                ticket, invitation);
        }

        /// <summary>
        /// Get account invitations.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account invitations</returns>
        [WebMethod(Description = "Get account invitations.")]
        public List<TransitAccountInvitation> GetAccountInvitationsByAccountId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account invitations count by id.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of outstanding account invitations</returns>
        [WebMethod(Description = "Get account invitations count.", CacheDuration = 60)]
        public int GetAccountInvitationsCountByAccountId(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account invitations.
        /// </summary>
        /// <returns>transit account invitations</returns>
        [WebMethod(Description = "Get all account invitations.")]
        public List<TransitAccountInvitation> GetAccountInvitations(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Get all account invitations count.
        /// </summary>
        /// <returns>number of outstanding account invitations</returns>
        [WebMethod(Description = "Get account invitations count.", CacheDuration = 60)]
        public int GetAccountInvitationsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get account invitation by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">invitation id</param>
        /// <returns>transit account invitation</returns>
        [WebMethod(Description = "Get account invitation by id.", CacheDuration = 60)]
        public TransitAccountInvitation GetAccountInvitationById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account invitation by id and code.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">invitation id</param>
        /// <returns>transit account invitation</returns>
        [WebMethod(Description = "Get account invitation by id and code.", CacheDuration = 60)]
        public TransitAccountInvitation GetAccountInvitationByIdAndCode(string ticket, int id, string code)
        {
            string admin_ticket = ticket;
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                admin_ticket = ManagedAccount.GetAdminTicket(session);
            }

            TransitAccountInvitation t_instance = WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetById(
                admin_ticket, id);

            if (t_instance.Code != code)
            {
                throw new ManagedAccount.AccessDeniedException();
            }

            return t_instance;
        }

        /// <summary>
        /// Delete an invitation.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">invitation id</param>
        [WebMethod(Description = "Delete a invitation.")]
        public void DeleteAccountInvitation(string ticket, int id)
        {
            WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.Delete(
                ticket, id);
        }

        #endregion

        #region Promote and Demote

        /// <summary>
        /// Promote a user to an administrator.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [WebMethod(Description = "Promote a user to an administrator.")]
        public void PromoteAdministrator(string ticket, int id)
        {
            int userid = GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.IsAdministrator())
                {
                    throw new Exception("User is already an administrator.");
                }

                acct.PromoteAdministrator();

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Demote a user to from an administrator.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">user id</param>
        /// <returns></returns>
        [WebMethod(Description = "Demote a user from an administrator.")]
        public void DemoteAdministrator(string ticket, int id)
        {
            int userid = GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (user.Id == id)
                {
                    throw new Exception("You cannot demote self.");
                }

                ManagedAccount acct = new ManagedAccount(session, id);
                if (!acct.IsAdministrator())
                {
                    throw new Exception("User is not an administrator.");
                }

                acct.DemoteAdministrator();

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Impersonate

        /// <summary>
        /// Impersonate an account (a user).
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>authentication ticket for the impersonated user</returns>
        [WebMethod(Description = "Impersonate an account (a user).")]
        public string Impersonate(string ticket, int id)
        {
            int userid = GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session, id);
                string return_ticket = ManagedAccount.GetTicketFromAccountId(acct.Id);
                SnCore.Data.Hibernate.Session.Flush();
                return return_ticket;
            }
        }

        #endregion

        #region Password

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="ticket">athentication ticket</param>
        /// <param name="oldpassword">old password</param>
        /// <param name="newpassword">new password</param>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Change password.")]
        public void ChangePassword(string ticket, int accountid, string oldpassword, string newpassword)
        {
            ChangePasswordMd5(
                ticket,
                accountid,
                string.IsNullOrEmpty(oldpassword) ? string.Empty : ManagedAccount.GetPasswordHash(oldpassword),
                newpassword);
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="ticket">athentication ticket</param>
        /// <param name="oldpassword">old password</param>
        /// <param name="newpassword">new password</param>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Change password with an existing Md5 hash.")]
        public void ChangePasswordMd5(string ticket, int accountid, string oldpasswordhash, string newpassword)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                if (userid != accountid)
                {
                    ManagedAccount requester = new ManagedAccount(session, userid);
                    if (!requester.IsAdministrator())
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }

                    ManagedAccount account = new ManagedAccount(session, accountid);
                    account.ResetPassword(newpassword, true);
                }
                else
                {
                    ManagedAccount account = new ManagedAccount(session, accountid);
                    account.ChangePasswordMd5(oldpasswordhash, newpassword);
                }

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Check password.
        /// </summary>
        /// <param name="ticket">athentication ticket</param>
        /// <param name="password">current password</param>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Check password.")]
        public bool IsPasswordValid(string ticket, int accountid, string password)
        {
            return IsPasswordValidMd5(ticket, accountid, ManagedAccount.GetPasswordHash(password));
        }

        /// <summary>
        /// Check password with an Md5 hash.
        /// </summary>
        /// <param name="ticket">athentication ticket</param>
        /// <param name="password">current password hash</param>
        /// <param name="accountid">account id</param>
        [WebMethod(Description = "Check password with an existing Md5 hash.")]
        public bool IsPasswordValidMd5(string ticket, int accountid, string password)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                if (userid != accountid)
                {
                    ManagedAccount requester = new ManagedAccount(session, userid);
                    if (!requester.IsAdministrator())
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                ManagedAccount account = new ManagedAccount(session, accountid);
                return account.IsPasswordValidMd5(password);
            }
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        [WebMethod(Description = "Reset password.")]
        public void ResetPassword(string emailaddress, DateTime dateofbirth)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = ManagedAccount.FindByEmailAndBirthday(session, emailaddress, dateofbirth);
                string newpassword = RandomPassword.Generate();
                a.ResetPassword(newpassword, true);

                session.Flush();

                // EmailAccountMessage
                ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                    session, a,
                    string.Format("EmailAccountPasswordReset.aspx?id={0}&Password={1}", a.Id, Renderer.UrlEncode(newpassword)));

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region AccountEmail

        /// <summary>
        /// Create or update an account e-mail.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="tae">transit account e-mail</param>
        [WebMethod(Description = "Create or update an account e-mail.")]
        public int CreateOrUpdateAccountEmail(string ticket, TransitAccountEmail tae)
        {
            int id = WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.CreateOrUpdate(
                ticket, tae);

            if (tae.Id == 0)
            {
                using (SnCore.Data.Hibernate.Session.OpenConnection())
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                    ManagedAccountEmail m_instance = new ManagedAccountEmail(session, id);
                    m_instance.Confirm(sec);
                    SnCore.Data.Hibernate.Session.Flush();
                }
            }

            return id;
        }

        /// <summary>
        /// Get account e-mails count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account e-mails</returns>
        [WebMethod(Description = "Get account e-mails count.")]
        public int GetAccountEmailsCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get account e-mails.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit e-mail</returns>
        [WebMethod(Description = "Get account e-mails.")]
        public List<TransitAccountEmail> GetAccountEmails(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Asc("Address") };
            return WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get an account e-mail by id.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get an account e-mail by id.")]
        public TransitAccountEmail GetAccountEmailById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete an account e-mail.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">e-mail id</param>
        [WebMethod(Description = "Delete an account e-mail.")]
        public void DeleteAccountEmail(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Verify an e-mail.
        /// </summary>
        /// <param name="password">account password</param>
        /// <param name="id">e-mail confirmation request id</param>
        /// <param name="code">e-mail confirmation request code</param>
        /// <returns>verified e-mail address</returns>
        [WebMethod(Description = "Verify an e-mail.")]
        public string VerifyAccountEmail(int id, string code)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return ManagedAccountEmailConfirmation.Verify(session, id, code);
            }
        }

        /// <summary>
        /// Re-send a confirmation for an e-mail address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="email">transit e-mail</param>
        [WebMethod(Description = "Re-send a confirmation for an e-mail address.")]
        public void ConfirmAccountEmail(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountEmail e = new ManagedAccountEmail(session, id);
                e.Confirm(sec);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Check whether a user has a verified e-mail address.
        /// </summary>
        [WebMethod(Description = "Check whether the user has a verified e-mail address.")]
        public bool HasVerifiedEmail(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount a = new ManagedAccount(session, id);
                return a.HasVerifiedEmail(sec);
            }
        }

        /// <summary>
        /// Check whether a user has a profile picture.
        /// </summary>
        [WebMethod(Description = "Check whether the user has a profile photo.")]
        public bool HasPicture(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount a = new ManagedAccount(session, id);
                return a.HasPicture(sec);
            }
        }

        /// <summary>
        /// Return an active e-mail address.
        /// </summary>
        [WebMethod(Description = "Check whether the user has a verified e-mail address.")]
        public string GetActiveEmailAddress(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount a = new ManagedAccount(session, id);
                a.GetACL().Check(sec, DataOperation.All);
                string mailto;
                a.TryGetActiveEmailAddress(out mailto, sec);
                return mailto;
            }
        }

        #endregion

        #region AccountEmailConfirmation

        /// <summary>
        /// Get account e-mail confirmations count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account e-mail confirmations</returns>
        [WebMethod(Description = "Get pending account e-mail confirmations count.")]
        public int GetAccountEmailConfirmationsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.GetCount(
                ticket, string.Format("WHERE AccountEmailConfirmation.AccountEmail.Account.Id = {0}" +
                    " AND AccountEmailConfirmation.AccountEmail.Verified = 0", id));
        }

        /// <summary>
        /// Get account e-mail confirmations.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit e-mail confirmation</returns>
        [WebMethod(Description = "Get pending account e-mail confirmations.")]
        public List<TransitAccountEmailConfirmation> GetAccountEmailConfirmations(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.GetList(
                ticket, options, string.Format("SELECT AccountEmailConfirmation FROM AccountEmailConfirmation AccountEmailConfirmation" +
                    " WHERE AccountEmailConfirmation.AccountEmail.Account.Id = {0}" +
                    " AND AccountEmailConfirmation.AccountEmail.Verified = 0", id));
        }

        /// <summary>
        /// Get an account e-mail confirmation by id.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get an account e-mail confirmation by id.")]
        public TransitAccountEmailConfirmation GetAccountEmailConfirmationById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete an account e-mail confirmation.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        [WebMethod(Description = "Delete an account e-mail confirmation.")]
        public void DeleteAccountEmailConfirmation(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.Delete(
                ticket, id);
        }

        #endregion

        #region OpenId

        /// <summary>
        /// Get account openids.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account openids</returns>
        [WebMethod(Description = "Get openids.")]
        public List<TransitAccountOpenId> GetAccountOpenIds(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountOpenId, ManagedAccountOpenId, AccountOpenId>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get the number of open ids associated with an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account id</param>
        [WebMethod(Description = "Get openids count.")]
        public int GetAccountOpenIdsCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountOpenId, ManagedAccountOpenId, AccountOpenId>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete an openid.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">openid id</param>
        [WebMethod(Description = "Delete an openid.")]
        public void DeleteAccountOpenId(string ticket, int id)
        {
            WebServiceImpl<TransitAccountOpenId, ManagedAccountOpenId, AccountOpenId>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Add an openid.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="openid">transit openid</param>
        [WebMethod(Description = "Add an openid.")]
        public int CreateAccountOpenId(string ticket, string token, string[] names, string[] values)
        {
            string consumerid = VerifyOpenId(token, names, values);
            TransitAccountOpenId to = new TransitAccountOpenId();
            to.IdentityUrl = consumerid;
            return WebServiceImpl<TransitAccountOpenId, ManagedAccountOpenId, AccountOpenId>.CreateOrUpdate(
                ticket, to);
        }

        /// <summary>
        /// Get an account openid.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get an account openid.")]
        public TransitAccountOpenId GetAccountOpenIdById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountOpenId, ManagedAccountOpenId, AccountOpenId>.GetById(
                ticket, id);
        }

        #endregion

        #region Facebook

        /// <summary>
        /// Get account facebook IDs.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account facebook ids</returns>
        [WebMethod(Description = "Get facebook IDs.")]
        public List<TransitAccountFacebook> GetAccountFacebooks(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFacebook, ManagedAccountFacebook, AccountFacebook>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get the number of open ids associated with an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account id</param>
        [WebMethod(Description = "Get facebook id count.")]
        public int GetAccountFacebooksCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFacebook, ManagedAccountFacebook, AccountFacebook>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete a facebook ID.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">facebook id</param>
        [WebMethod(Description = "Delete a facebook id.")]
        public void DeleteAccountFacebook(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFacebook, ManagedAccountFacebook, AccountFacebook>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Add a facebook id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="facebook">transit facebook</param>
        [WebMethod(Description = "Add a facebook id.")]
        public int CreateAccountFacebook(string ticket, string signature, string[] names, string[] values)
        {
            TransitFacebookLogin t_login = TryLoginFacebook(signature, names, values);
            TransitAccountFacebook t_facebook = new TransitAccountFacebook();
            t_facebook.AccountId = GetAccountId(ticket);
            t_facebook.FacebookAccountId = t_login.FacebookAccountId;
            return WebServiceImpl<TransitAccountFacebook, ManagedAccountFacebook, AccountFacebook>.CreateOrUpdate(
                ticket, t_facebook);
        }

        /// <summary>
        /// Get an account facebook id.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get an account facebook id.")]
        public TransitAccountFacebook GetAccountFacebookById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFacebook, ManagedAccountFacebook, AccountFacebook>.GetById(
                ticket, id);
        }

        #endregion

        #region Search

        protected IList<Account> InternalSearchAccounts(ISession session, string s, ServiceQueryOptions options)
        {
            // search for an account matching the name exactly
            IList<Account> accounts = session.CreateCriteria(typeof(Account)).Add(
                Expression.Or(
                    Expression.Eq("Name", Renderer.SqlEncode(s)),
                    Expression.Eq("City", Renderer.SqlEncode(s))
                )).List<Account>();

            // search for everything else
            if (accounts.Count == 0)
            {
                int maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
                ISQLQuery query = session.CreateSQLQuery(

                        "CREATE TABLE #Results ( Account_Id int, RANK int )\n" +
                        "CREATE TABLE #Unique_Results ( Account_Id int, RANK int )\n" +

                        "INSERT #Results\n" +
                        "SELECT account.Account_Id, ft.[RANK] FROM Account account\n" +
                        "INNER JOIN FREETEXTTABLE (Account, [Name], '" + Renderer.SqlEncode(s) + "', " +
                            maxsearchresults.ToString() + ") AS ft ON account.Account_Id = ft.[KEY]\n" +

                        //"INSERT #Results\n" +
                    //"SELECT account.Account_Id, ft.[RANK] FROM Account account, AccountSurveyAnswer accountsurveyanswer\n" +
                    //"INNER JOIN FREETEXTTABLE (AccountSurveyAnswer, ([Answer]), '" + Renderer.SqlEncode(s) + "', " +
                    //    maxsearchresults.ToString() + ") AS ft ON accountsurveyanswer.AccountSurveyAnswer_Id = ft.[KEY] \n" +
                    //"WHERE accountsurveyanswer.Account_Id = account.Account_Id\n" +

                        "INSERT #Results\n" +
                        "SELECT account.Account_Id, ft.[RANK] FROM Account account, AccountPropertyValue accountpropertyvalue\n" +
                        "INNER JOIN FREETEXTTABLE (AccountPropertyValue, ([Value]), '" + Renderer.SqlEncode(s) + "', " +
                            maxsearchresults.ToString() + ") AS ft ON accountpropertyvalue.AccountPropertyValue_Id = ft.[KEY] \n" +
                        "WHERE accountpropertyvalue.Account_Id = account.Account_Id\n" +

                        "INSERT #Unique_Results\n" +
                        "SELECT DISTINCT Account_Id, SUM(RANK)\n" +
                        "FROM #Results GROUP BY Account_Id\n" +
                        "ORDER BY SUM(RANK) DESC\n" +

                        "SELECT " + (options != null ? options.GetSqlQueryTop() : string.Empty) +
                        "{Account.*} FROM {Account}, #Unique_Results\n" +
                        "WHERE Account.Account_Id = #Unique_Results.Account_Id\n" +
                        "ORDER BY #Unique_Results.RANK DESC\n" +

                        "DROP TABLE #Results\n" +
                        "DROP TABLE #Unique_Results\n")
                        .AddEntity("Account", typeof(Account));

                accounts = query.List<Account>();
            }

            return WebServiceQueryOptions<Account>.Apply(options, accounts);
        }

        /// <summary>
        /// Search accounts.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search accounts.", CacheDuration = 60)]
        public List<TransitAccountActivity> SearchAccounts(string ticket, string s, ServiceQueryOptions options)
        {
            if (string.IsNullOrEmpty(s))
                return new List<TransitAccountActivity>();

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                IList<Account> accounts = InternalSearchAccounts(session, s, options);
                List<TransitAccountActivity> result = new List<TransitAccountActivity>(accounts.Count);
                foreach (Account account in accounts)
                {
                    result.Add(new ManagedAccountActivity(session, account).GetTransitInstance(sec));
                }
                return result;
            }
        }

        /// <summary>
        /// Return the number of accounts matching a query.
        /// </summary>
        /// <returns>number of accounts</returns>
        [WebMethod(Description = "Return the number of accounts matching a query.", CacheDuration = 60)]
        public int SearchAccountsCount(string ticket, string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return InternalSearchAccounts(session, s, null).Count;
            }
        }

        #endregion

        #region AccountPropertyGroup

        /// <summary>
        /// Create or update a property group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit property group</param>
        [WebMethod(Description = "Create or update a property group.")]
        public int CreateOrUpdateAccountPropertyGroup(string ticket, TransitAccountPropertyGroup pg)
        {
            return WebServiceImpl<TransitAccountPropertyGroup, ManagedAccountPropertyGroup, AccountPropertyGroup>.CreateOrUpdate(
                ticket, pg);
        }

        /// <summary>
        /// Get a property group.
        /// </summary>
        /// <returns>transit property group</returns>
        [WebMethod(Description = "Get a property group.")]
        public TransitAccountPropertyGroup GetAccountPropertyGroupById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPropertyGroup, ManagedAccountPropertyGroup, AccountPropertyGroup>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all property groups count.
        /// </summary>
        /// <returns>number of transit property groups</returns>
        [WebMethod(Description = "Get all property groups.")]
        public int GetAccountPropertyGroupsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountPropertyGroup, ManagedAccountPropertyGroup, AccountPropertyGroup>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all property groups.
        /// </summary>
        /// <returns>list of transit property groups</returns>
        [WebMethod(Description = "Get all property groups.")]
        public List<TransitAccountPropertyGroup> GetAccountPropertyGroups(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountPropertyGroup, ManagedAccountPropertyGroup, AccountPropertyGroup>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Delete a property group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">property group id</param>
        [WebMethod(Description = "Delete a property group.")]
        public void DeleteAccountPropertyGroup(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPropertyGroup, ManagedAccountPropertyGroup, AccountPropertyGroup>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountProperty

        /// <summary>
        /// Create or update an account property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit account property</param>
        [WebMethod(Description = "Create or update a property.")]
        public int CreateOrUpdateAccountProperty(string ticket, TransitAccountProperty t_instance)
        {
            return WebServiceImpl<TransitAccountProperty, ManagedAccountProperty, AccountProperty>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account property.
        /// </summary>
        /// <returns>transit account property</returns>
        [WebMethod(Description = "Get a property.")]
        public TransitAccountProperty GetAccountPropertyById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountProperty, ManagedAccountProperty, AccountProperty>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account properties count.
        /// </summary>
        /// <returns>number of account properties</returns>
        [WebMethod(Description = "Get all account properties count.")]
        public int GetAccountPropertiesCount(string ticket, int gid)
        {
            ICriterion[] expressions = { Expression.Eq("AccountPropertyGroup.Id", gid) };
            return WebServiceImpl<TransitAccountProperty, ManagedAccountProperty, AccountProperty>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account properties.
        /// </summary>
        /// <returns>list of transit properties</returns>
        [WebMethod(Description = "Get all account properties.")]
        public List<TransitAccountProperty> GetAccountProperties(string ticket, int gid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountPropertyGroup.Id", gid) };
            return WebServiceImpl<TransitAccountProperty, ManagedAccountProperty, AccountProperty>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete an account property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an account property.")]
        public void DeleteAccountProperty(string ticket, int id)
        {
            WebServiceImpl<TransitAccountProperty, ManagedAccountProperty, AccountProperty>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountPropertyValue

        /// <summary>
        /// Get accounts that match a property value by name.
        /// </summary>
        /// <returns>transit accounts</returns>
        [WebMethod(Description = "Get accounts that match a property value by name.")]
        public List<TransitAccount> GetAccountsByPropertyValue(string ticket, string groupname, string propertyname, string propertyvalue, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetList(
                ticket, options,
                   "SELECT {account.*} FROM AccountProperty p, AccountPropertyGroup g, AccountPropertyValue v, Account {account}" +
                   " WHERE {account}.Account_Id = v.Account_Id" +
                   " AND v.Account_Id = {account}.Account_Id" +
                   " AND v.AccountProperty_Id = p.AccountProperty_Id" +
                   " AND p.AccountPropertyGroup_Id = g.AccountPropertyGroup_Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%" + Renderer.SqlEncode(propertyvalue) + "%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'",
                   "account");
        }

        /// <summary>
        /// Get the number of accounts that match a property value by name.
        /// </summary>
        /// <returns>transit accounts</returns>
        [WebMethod(Description = "Get the number of accounts that match a property value by name.")]
        public int GetAccountsByPropertyValueCount(string ticket, string groupname, string propertyname, string propertyvalue)
        {
            return WebServiceImpl<TransitAccount, ManagedAccount, Account>.GetCount(
                ticket,
                   ", AccountProperty p, AccountPropertyGroup g, AccountPropertyValue v" +
                   " WHERE Account.Id = v.Account.Id" +
                   " AND v.Account.Id = Account.Id" +
                   " AND v.AccountProperty.Id = p.Id" +
                   " AND p.AccountPropertyGroup.Id = g.Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%{" + Renderer.SqlEncode(propertyvalue) + "}%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'");
        }

        /// <summary>
        /// Get a account property value by name.
        /// </summary>
        /// <returns>transit account property value</returns>
        [WebMethod(Description = "Get a account property value by group and name.")]
        public TransitAccountPropertyValue GetAccountPropertyValueByName(string ticket, int accountid, string groupname, string propertyname)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                AccountPropertyGroup ppg = (AccountPropertyGroup)session.CreateCriteria(typeof(AccountPropertyGroup))
                    .Add(Expression.Eq("Name", groupname))
                    .UniqueResult();

                if (ppg == null)
                {
                    throw new Exception(string.Format(
                        "No property group with the name \"{0}\" found.", groupname));
                }

                AccountProperty pp = (AccountProperty)session.CreateCriteria(typeof(AccountProperty))
                    .Add(Expression.Eq("Name", propertyname))
                    .Add(Expression.Eq("AccountPropertyGroup.Id", ppg.Id))
                    .UniqueResult();

                if (pp == null)
                {
                    throw new Exception(string.Format(
                        "No property with the name \"{0}\" found.", propertyname));
                }

                AccountPropertyValue ppv = (AccountPropertyValue)session.CreateCriteria(typeof(AccountPropertyValue))
                    .Add(Expression.Eq("Account.Id", accountid))
                    .Add(Expression.Eq("AccountProperty.Id", pp.Id))
                    .UniqueResult();

                if (ppv == null)
                {
                    throw new Exception(string.Format(
                        "No property value for \"{0}\" of account \"{0}\" of group \"{0}\" found.",
                        propertyname, accountid, groupname));
                }

                ManagedAccountPropertyValue result = new ManagedAccountPropertyValue(session, ppv);
                return result.GetTransitInstance(sec);
            }
        }

        /// <summary>
        /// Create or update an account property value.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account property value</param>
        [WebMethod(Description = "Create or update an account property value.")]
        public int CreateOrUpdateAccountPropertyValue(string ticket, TransitAccountPropertyValue propertyvalue)
        {
            return WebServiceImpl<TransitAccountPropertyValue, ManagedAccountPropertyValue, AccountPropertyValue>.CreateOrUpdate(
                ticket, propertyvalue);
        }

        /// <summary>
        /// Get an account property value.
        /// </summary>
        /// <returns>transit account property value</returns>
        [WebMethod(Description = "Get an account property value.")]
        public TransitAccountPropertyValue GetAccountPropertyValueById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPropertyValue, ManagedAccountPropertyValue, AccountPropertyValue>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account property values.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get account property values.", CacheDuration = 60)]
        public List<TransitAccountPropertyValue> GetAccountPropertyValues(string ticket, int id, int groupid, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountPropertyValue, ManagedAccountPropertyValue, AccountPropertyValue>.GetList(
                ticket, options, string.Format("SELECT AccountPropertyValue FROM AccountPropertyValue AccountPropertyValue" +
                    " WHERE AccountPropertyValue.Account.Id = {0} " +
                    " AND AccountPropertyValue.AccountProperty.AccountPropertyGroup.Id = {1}" +
                    " AND AccountPropertyValue.AccountProperty.Publish = 1", id, groupid));
        }

        /// <summary>
        /// Get account property values count.
        /// </summary>
        /// <returns>number of account property values</returns>
        [WebMethod(Description = "Get account property values count.", CacheDuration = 60)]
        public int GetAccountPropertyValuesCount(string ticket, int id, int groupid)
        {
            return WebServiceImpl<TransitAccountPropertyValue, ManagedAccountPropertyValue, AccountPropertyValue>.GetCount(
                ticket, string.Format(" WHERE AccountPropertyValue.Account.Id = {0} " +
                    " AND AccountPropertyValue.AccountProperty.AccountPropertyGroup.Id = {1}" +
                    " AND AccountPropertyValue.AccountProperty.Publish = 1", id, groupid));
        }

        /// <summary>
        /// Get all account property values, including unfilled ones.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get all account property values, including unfilled ones.", CacheDuration = 60)]
        public List<TransitAccountPropertyValue> GetAllAccountPropertyValues(string ticket, int accountid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                ICriteria c = session.CreateCriteria(typeof(AccountProperty));
                if (groupid > 0) c.Add(Expression.Eq("AccountPropertyGroup.Id", groupid));
                IList properties = c.List();

                List<TransitAccountPropertyValue> result = new List<TransitAccountPropertyValue>(properties.Count);

                foreach (AccountProperty property in properties)
                {
                    AccountPropertyValue value = (AccountPropertyValue)session.CreateCriteria(typeof(AccountPropertyValue))
                        .Add(Expression.Eq("Account.Id", accountid))
                        .Add(Expression.Eq("AccountProperty.Id", property.Id))
                        .UniqueResult();

                    if (value == null)
                    {
                        value = new AccountPropertyValue();
                        value.AccountProperty = property;
                        value.Value = property.DefaultValue;
                        value.Account = session.Load<Account>(accountid);
                    }

                    ManagedAccountPropertyValue m_instance = new ManagedAccountPropertyValue(session, value);
                    result.Add(m_instance.GetTransitInstance(sec));
                }

                return result;
            }
        }

        /// <summary>
        /// Delete an account property value.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an account property value.")]
        public void DeleteAccountPropertyValue(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPropertyValue, ManagedAccountPropertyValue, AccountPropertyValue>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountAttribute

        /// <summary>
        /// Create or update an account attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account attribute</param>
        [WebMethod(Description = "Create or update an account attribute.")]
        public int CreateOrUpdateAccountAttribute(string ticket, TransitAccountAttribute attribute)
        {
            return WebServiceImpl<TransitAccountAttribute, ManagedAccountAttribute, AccountAttribute>.CreateOrUpdate(
                ticket, attribute);
        }

        /// <summary>
        /// Get account attributes.
        /// </summary>
        /// <returns>transit account attribute</returns>
        [WebMethod(Description = "Get account attributes.")]
        public TransitAccountAttribute GetAccountAttributeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountAttribute, ManagedAccountAttribute, AccountAttribute>.GetById(
                ticket, id);
        }


        /// <summary>
        /// Get account attributes count.
        /// </summary>
        /// <returns>number of account attributes</returns>
        [WebMethod(Description = "Get account attributes count.", CacheDuration = 60)]
        public int GetAccountAttributesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountAttribute, ManagedAccountAttribute, AccountAttribute>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get account attributes.
        /// </summary>
        /// <returns>list of account attributes</returns>
        [WebMethod(Description = "Get account attributes.", CacheDuration = 60)]
        public List<TransitAccountAttribute> GetAccountAttributes(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountAttribute, ManagedAccountAttribute, AccountAttribute>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete an account attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account attribute id</param>
        [WebMethod(Description = "Delete an account attribute.")]
        public void DeleteAccountAttribute(string ticket, int id)
        {
            WebServiceImpl<TransitAccountAttribute, ManagedAccountAttribute, AccountAttribute>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountRedirect

        /// <summary>
        /// Get account redirects count by account id.
        /// </summary>
        [WebMethod(Description = "Get account redirects count by account id.")]
        public int GetAccountRedirectsCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account redirects count.
        /// </summary>
        [WebMethod(Description = "Get all account redirects count.")]
        public int GetAllAccountRedirectsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get account redirects.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account redirects</returns>
        [WebMethod(Description = "Get account redirects.", CacheDuration = 60)]
        public List<TransitAccountRedirect> GetAccountRedirects(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account redirects.
        /// </summary>
        /// <returns>transit account redirects</returns>
        [WebMethod(Description = "Get all account redirects.", CacheDuration = 60)]
        public List<TransitAccountRedirect> GetAllAccountRedirects(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get account redirect by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">redirect id</param>
        /// <returns>transit account redirect</returns>
        [WebMethod(Description = "Get account redirect by id.")]
        public TransitAccountRedirect GetAccountRedirectById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account redirect by source uri.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account redirect</returns>
        [WebMethod(Description = "Get account redirect by source uri.")]
        public TransitAccountRedirect GetAccountRedirectBySourceUri(string ticket, int id, string uri)
        {
            ICriterion[] expressions = 
            {
                Expression.Eq("Account.Id", id),
                Expression.Eq("SourceUri", uri)
            };

            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetByCriterion(
                ticket, expressions);
        }

        /// <summary>
        /// Get account redirect by target uri.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account redirect</returns>
        [WebMethod(Description = "Get account redirect by target uri.")]
        public TransitAccountRedirect GetAccountRedirectByTargetUri(string ticket, int id, string uri)
        {
            ICriterion[] expressions = 
            {
                Expression.Eq("Account.Id", id),
                Expression.Eq("TargetUri", uri)
            };

            return WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.GetByCriterion(
                ticket, expressions, 1);
        }

        /// <summary>
        /// Create or update a redirect.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="redirect">new redirect</param>
        [WebMethod(Description = "Create or update a redirect.")]
        public int CreateOrUpdateAccountRedirect(string ticket, TransitAccountRedirect redirect)
        {
            int id = WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.CreateOrUpdate(
                ticket, redirect);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountRedirect.UpdateMap(session);
            }

            return id;
        }

        /// <summary>
        /// Delete a redirect.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">redirect id</param>
        [WebMethod(Description = "Delete a redirect.")]
        public void DeleteAccountRedirect(string ticket, int id)
        {
            WebServiceImpl<TransitAccountRedirect, ManagedAccountRedirect, AccountRedirect>.Delete(
                ticket, id);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountRedirect.UpdateMap(session);
            }
        }

        #endregion

        #region AccountAddress

        /// <summary>
        /// Create or update an account address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit account address</param>
        [WebMethod(Description = "Create or update an account address.")]
        public int CreateOrUpdateAccountAddress(string ticket, TransitAccountAddress t_instance)
        {
            return WebServiceImpl<TransitAccountAddress, ManagedAccountAddress, AccountAddress>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account address.
        /// </summary>
        /// <returns>transit account address</returns>
        [WebMethod(Description = "Get an account address.")]
        public TransitAccountAddress GetAccountAddressById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountAddress, ManagedAccountAddress, AccountAddress>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account addresses count.
        /// </summary>
        /// <returns>number of account addresses</returns>
        [WebMethod(Description = "Get all account addresses count.")]
        public int GetAccountAddressesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountAddress, ManagedAccountAddress, AccountAddress>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account addresses.
        /// </summary>
        /// <returns>list of transit addresses</returns>
        [WebMethod(Description = "Get all account addresses.")]
        public List<TransitAccountAddress> GetAccountAddresses(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountAddress, ManagedAccountAddress, AccountAddress>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete an account address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an account address.")]
        public void DeleteAccountAddress(string ticket, int id)
        {
            WebServiceImpl<TransitAccountAddress, ManagedAccountAddress, AccountAddress>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountWebsite

        /// <summary>
        /// Create or update an account website.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit account website</param>
        [WebMethod(Description = "Create or update an account website.")]
        public int CreateOrUpdateAccountWebsite(string ticket, TransitAccountWebsite t_instance)
        {
            return WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.CreateOrUpdate(
                ticket, t_instance);
        }

        /// <summary>
        /// Get an account website.
        /// </summary>
        /// <returns>transit account website</returns>
        [WebMethod(Description = "Get an account website.")]
        public TransitAccountWebsite GetAccountWebsiteById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account websites count.
        /// </summary>
        /// <returns>number of account websites</returns>
        [WebMethod(Description = "Get all account websites count.")]
        public int GetAccountWebsitesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account websites.
        /// </summary>
        /// <returns>list of transit websites</returns>
        [WebMethod(Description = "Get all account websites.")]
        public List<TransitAccountWebsite> GetAccountWebsites(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Delete an account website.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">website id</param>
        [WebMethod(Description = "Delete an account website.")]
        public void DeleteAccountWebsite(string ticket, int id)
        {
            WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get an account website if modified since.
        /// </summary>
        /// <param name="id">website id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get account website if modified since.", BufferResponse = true)]
        public TransitAccountWebsite GetAccountWebsiteIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAccountWebsite t_instance = WebServiceImpl<TransitAccountWebsite, ManagedAccountWebsite, AccountWebsite>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        #endregion

        #region AccountPicture

        /// <summary>
        /// Create or update a account picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="accountpicture">transit account picture</param>
        [WebMethod(Description = "Create or update a account picture.", BufferResponse = true)]
        public int CreateOrUpdateAccountPicture(string ticket, TransitAccountPicture accountpicture)
        {
            return WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.CreateOrUpdate(
                ticket, accountpicture);
        }

        /// <summary>
        /// Move an account picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="disp">displace by positions</param>
        /// <param name="id">picture id</param>
        [WebMethod(Description = "Move an account picture.")]
        public void MoveAccountPicture(string ticket, int id, int disp)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountPicture m_instance = new ManagedAccountPicture(session, id);
                m_instance.Move(sec, disp);
            }
        }

        /// <summary>
        /// Get a account picture.
        /// </summary>
        /// <returns>transit account picture</returns>
        [WebMethod(Description = "Get a account picture.")]
        public TransitAccountPicture GetAccountPictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get account picture picture if modified since.
        /// </summary>
        /// <param name="id">account picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get account picture picture data if modified since.", BufferResponse = true)]
        public TransitAccountPicture GetAccountPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAccountPicture t_instance = WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Get account pictures count.
        /// </summary>
        [WebMethod(Description = "Get account pictures count.")]
        public int GetAccountPicturesCount(string ticket, int id, AccountPicturesQueryOptions qopt)
        {
            StringBuilder query = new StringBuilder();
            query.AppendFormat("WHERE AccountPicture.Account.Id = {0}", id);
            if (qopt != null && !qopt.Hidden) query.Append(" AND AccountPicture.Hidden = 0");
            return WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.GetCount(
                ticket, query.ToString());
        }

        /// <summary>
        /// Get all account pictures.
        /// </summary>
        /// <param name="placeid">place id</param>
        /// <returns>list of transit account pictures</returns>
        [WebMethod(Description = "Get all account pictures.")]
        public List<TransitAccountPicture> GetAccountPictures(string ticket, int id, AccountPicturesQueryOptions qopt, ServiceQueryOptions options)
        {
            List<ICriterion> expressions = new List<ICriterion>();
            expressions.Add(Expression.Eq("Account.Id", id));
            if (qopt != null && !qopt.Hidden) expressions.Add(Expression.Eq("Hidden", false));
            Order[] orders = { Order.Asc("Position"), Order.Desc("Created") };
            return WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.GetList(
                ticket, options, expressions.ToArray(), orders);
        }

        /// <summary>
        /// Delete a account picture
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="picture id">id</param>
        [WebMethod(Description = "Delete a account picture.")]
        public void DeleteAccountPicture(string ticket, int id)
        {
            WebServiceImpl<TransitAccountPicture, ManagedAccountPicture, AccountPicture>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountSurveyAnswer

        /// <summary>
        /// Get account survey answers count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>number of answers filled</returns>
        [WebMethod(Description = "Get account survey answers.", CacheDuration = 60)]
        public int GetAccountSurveyAnswersCount(string ticket, int id, int surveyid)
        {
            return WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.GetCount(
                ticket, string.Format(", SurveyQuestion q WHERE AccountSurveyAnswer.Account.Id = {0} " +
                    "AND AccountSurveyAnswer.SurveyQuestion.Id = q.Id and q.Survey.Id = {1}",
                    id, surveyid));
        }

        /// <summary>
        /// Get account survey answers.
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get account survey answers.", CacheDuration = 60)]
        public List<TransitAccountSurveyAnswer> GetAccountSurveyAnswers(string ticket, int id, int surveyid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);

                IList questions = session.CreateCriteria(typeof(SurveyQuestion))
                    .Add(Expression.Eq("Survey.Id", surveyid))
                    .List();

                List<TransitAccountSurveyAnswer> result = new List<TransitAccountSurveyAnswer>(questions.Count);
                foreach (SurveyQuestion q in questions)
                {
                    AccountSurveyAnswer a = (AccountSurveyAnswer)session.CreateCriteria(typeof(AccountSurveyAnswer))
                        .Add(Expression.Eq("SurveyQuestion.Id", q.Id))
                        .Add(Expression.Eq("Account.Id", id))
                        .UniqueResult();

                    if (a == null)
                    {
                        TransitAccountSurveyAnswer f = new TransitAccountSurveyAnswer();
                        f.SurveyQuestion = q.Question;
                        f.SurveyQuestionId = q.Id;
                        result.Add(f);
                    }
                    else
                    {
                        result.Add(new ManagedAccountSurveyAnswer(session, a).GetTransitInstance(sec));
                    }
                }

                return WebServiceQueryOptions<TransitAccountSurveyAnswer>.Apply(options, result);
            }
        }

        /// <summary>
        /// Get survey answers count for a single question.
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>answers count</returns>
        [WebMethod(Description = "Get survey answers count for a single question.", CacheDuration = 60)]
        public int GetAccountSurveyAnswersCountByQuestionId(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("SurveyQuestion.Id", id) };
            return WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get survey answers for a single question.
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get survey answers for a single question.", CacheDuration = 60)]
        public List<TransitAccountSurveyAnswer> GetAccountSurveyAnswersByQuestionId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("SurveyQuestion.Id", id) };
            Order[] orders = { Order.Desc("Modified") };
            return WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account survey answer by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">survey answer id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get account survey answer by id.")]
        public TransitAccountSurveyAnswer GetAccountSurveyAnswerById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update a survey answer.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="answer">survey answer</param>
        [WebMethod(Description = "Add a survey answer.")]
        public int CreateOrUpdateAccountSurveyAnswer(string ticket, TransitAccountSurveyAnswer answer)
        {
            return WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.CreateOrUpdate(
                ticket, answer);
        }

        /// <summary>
        /// Delete a survey answer.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">survey answer id</param>
        [WebMethod(Description = "Delete a survey answer.")]
        public void DeleteAccountSurveyAnswer(string ticket, int id)
        {
            WebServiceImpl<TransitAccountSurveyAnswer, ManagedAccountSurveyAnswer, AccountSurveyAnswer>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get surveys answered by an account.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit surveys</returns>
        [WebMethod(Description = "Get surveys answered by account.", CacheDuration = 60)]
        public List<TransitSurvey> GetAccountSurveysById(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitSurvey, ManagedSurvey, Survey>.GetList(
                ticket, options, string.Format(
                    "SELECT DISTINCT s FROM Survey s, Account a, SurveyQuestion q, AccountSurveyAnswer sa " +
                    "WHERE s.Id = q.Survey.Id AND sa.SurveyQuestion.Id = q.Id AND a.Id = sa.Account.Id " +
                    "AND a.Id = {0} ORDER BY s.Id DESC", id));
        }

        #endregion

        #region AccountMessageFolder

        /// <summary>
        /// Create account system folders.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Create account system folders.")]
        public void CreateAccountSystemMessageFolders(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount acct = new ManagedAccount(session, id);
                acct.CreateAccountSystemMessageFolders(sec);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account message folders.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Get account message folders.")]
        public List<TransitAccountMessageFolder> GetAccountMessageFolders(string ticket, int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                // sort the tree
                IList folders = session.CreateCriteria(typeof(AccountMessageFolder))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("System"))
                    .AddOrder(Order.Asc("Name"))
                    .List();
                AccountMessageFolderTree tree = new AccountMessageFolderTree(folders);
                IEnumerator<AccountMessageFolder> enumerator = tree.GetDepthFirstEnumerator();
                List<TransitAccountMessageFolder> result = new List<TransitAccountMessageFolder>();
                while (enumerator.MoveNext())
                {
                    ManagedAccountMessageFolder m_folder = new ManagedAccountMessageFolder(session, enumerator.Current);
                    result.Add(m_folder.GetTransitInstance(sec));
                }

                return WebServiceQueryOptions<TransitAccountMessageFolder>.Apply(options, result);
            }
        }

        /// <summary>
        /// Get account message folders count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Get account message folders count.")]
        public int GetAccountMessageFoldersCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountMessageFolder, ManagedAccountMessageFolder, AccountMessageFolder>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get account message folder by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">message folder id</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Get account message folder by id.")]
        public TransitAccountMessageFolder GetAccountMessageFolderById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountMessageFolder, ManagedAccountMessageFolder, AccountMessageFolder>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Add a message folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="message folder">new message folder</param>
        [WebMethod(Description = "Add a message folder.")]
        public int CreateOrUpdateAccountMessageFolder(string ticket, TransitAccountMessageFolder messagefolder)
        {
            return WebServiceImpl<TransitAccountMessageFolder, ManagedAccountMessageFolder, AccountMessageFolder>.CreateOrUpdate(
                ticket, messagefolder);
        }

        /// <summary>
        /// Delete a message folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">message folder id</param>
        [WebMethod(Description = "Delete a message folder.")]
        public void DeleteAccountMessageFolder(string ticket, int id)
        {
            WebServiceImpl<TransitAccountMessageFolder, ManagedAccountMessageFolder, AccountMessageFolder>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get account message folder by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="folder">message folder name</param>
        /// <returns>transit account message folder</returns>
        [WebMethod(Description = "Get account message folder by id.", CacheDuration = 60)]
        public TransitAccountMessageFolder GetAccountMessageSystemFolder(string ticket, int id, string folder)
        {
            ICriterion[] expression = 
            {
                Expression.Eq("Account.Id", id),
                Expression.Eq("Name", folder),
                // Expression.Eq("System", true),
                Expression.IsNull("AccountMessageFolderParent")                        
            };

            return WebServiceImpl<TransitAccountMessageFolder, ManagedAccountMessageFolder, AccountMessageFolder>.GetByCriterion(
                ticket, expression);
        }

        #endregion

        #region AccountMessage

        /// <summary>
        /// Get account messages count.
        /// </summary>
        /// <returns>transit account messages count</returns>
        [WebMethod(Description = "Get account messages count.")]
        public int GetAccountMessagesCount(string ticket, int folderid)
        {
            ICriterion[] expressions = { Expression.Eq("AccountMessageFolder.Id", folderid) };
            return WebServiceImpl<TransitAccountMessage, ManagedAccountMessage, AccountMessage>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get account messages.
        /// </summary>
        /// <returns>transit account messages</returns>
        [WebMethod(Description = "Get account messages.")]
        public List<TransitAccountMessage> GetAccountMessages(string ticket, int folderid, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountMessageFolder.Id", folderid) };
            Order[] orders = { Order.Desc("Sent") };
            return WebServiceImpl<TransitAccountMessage, ManagedAccountMessage, AccountMessage>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account message by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">message id</param>
        /// <returns>transit account messages</returns>
        [WebMethod(Description = "Get account message by id.")]
        public TransitAccountMessage GetAccountMessageById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountMessage, ManagedAccountMessage, AccountMessage>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Create or update a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="message">new message</param>
        [WebMethod(Description = "Add a message.")]
        public int CreateOrUpdateAccountMessage(string ticket, TransitAccountMessage message)
        {
            return WebServiceImpl<TransitAccountMessage, ManagedAccountMessage, AccountMessage>.CreateOrUpdate(
                ticket, message);
        }

        /// <summary>
        /// Delete a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">message id</param>
        [WebMethod(Description = "Delete a message.")]
        public void DeleteAccountMessage(string ticket, int id)
        {
            WebServiceImpl<TransitAccountMessage, ManagedAccountMessage, AccountMessage>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Delete messages in a folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">folder id</param>
        [WebMethod(Description = "Delete messages in a folder.")]
        public void DeleteAccountMessagesByFolder(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountMessageFolder f = new ManagedAccountMessageFolder(session, id);
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                f.DeleteAccountMessages(sec);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Move a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="messageid">message id</param>
        /// <param name="folderid">target folder</param>
        [WebMethod(Description = "Move a message.")]
        public void MoveAccountMessageToFolderById(string ticket, int messageid, int folderid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountMessage m_instance = new ManagedAccountMessage(session, messageid);
                m_instance.MoveTo(sec, folderid);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }


        /// <summary>
        /// Move a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="messageid">message id</param>
        /// <param name="folder">target folder name</param>
        [WebMethod(Description = "Move a message.")]
        public void MoveAccountMessageToFolder(string ticket, int id, int messageid, string folder)
        {
            int folderid = GetAccountMessageSystemFolder(ticket, id, folder).Id;
            MoveAccountMessageToFolderById(ticket, messageid, folderid);
        }

        #endregion

        #region AccountEmailMessage

        /// <summary>
        /// Create or update a AccountEmailMessage.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="AccountEmailMessage">transit message</param>
        [WebMethod(Description = "Create or update a message.")]
        public int CreateOrUpdateAccountEmailMessage(string ticket, TransitAccountEmailMessage message)
        {
            return WebServiceImpl<TransitAccountEmailMessage, ManagedAccountEmailMessage, AccountEmailMessage>.CreateOrUpdate(
                ticket, message);
        }

        /// <summary>
        /// Send an AccountEmailMessage immediately (administrators only).
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="message">message</param>
        public void SendAccountEmailMessage(string ticket, TransitAccountEmailMessage message)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountEmailMessage m_instance = new ManagedAccountEmailMessage(session);
                m_instance.Send(sec, message);
            }
        }

        /// <summary>
        /// Get a AccountEmailMessage.
        /// </summary>
        /// <returns>transit AccountEmailMessage</returns>
        [WebMethod(Description = "Get a AccountEmailMessage.")]
        public TransitAccountEmailMessage GetAccountEmailMessageById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEmailMessage, ManagedAccountEmailMessage, AccountEmailMessage>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account e-mail messages.
        /// </summary>
        /// <returns>list of transit account e-mail messages</returns>
        [WebMethod(Description = "Get all account e-mail messages.")]
        public List<TransitAccountEmailMessage> GetAccountEmailMessages(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountEmailMessage, ManagedAccountEmailMessage, AccountEmailMessage>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all account e-mail messages count.
        /// </summary>
        /// <returns>number of account e-mail messages</returns>
        [WebMethod(Description = "Get all account e-mail messages count.")]
        public int GetAccountEmailMessagesCount(string ticket)
        {
            return WebServiceImpl<TransitAccountEmailMessage, ManagedAccountEmailMessage, AccountEmailMessage>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete an account e-mail message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an account e-mail message.")]
        public void DeleteAccountEmailMessage(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEmailMessage, ManagedAccountEmailMessage, AccountEmailMessage>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountFlagType

        /// <summary>
        /// Create or update an flag type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="flagtype">transit flag type</param>
        [WebMethod(Description = "Create or update an flag type.")]
        public int CreateOrUpdateAccountFlagType(string ticket, TransitAccountFlagType flagtype)
        {
            return WebServiceImpl<TransitAccountFlagType, ManagedAccountFlagType, AccountFlagType>.CreateOrUpdate(
                ticket, flagtype);
        }

        /// <summary>
        /// Get an flag type.
        /// </summary>
        /// <returns>transit flag type</returns>
        [WebMethod(Description = "Get an flag type.")]
        public TransitAccountFlagType GetAccountFlagTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFlagType, ManagedAccountFlagType, AccountFlagType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all flag types.
        /// </summary>
        /// <returns>list of transit flag types</returns>
        [WebMethod(Description = "Get all flag types.")]
        public List<TransitAccountFlagType> GetAccountFlagTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFlagType, ManagedAccountFlagType, AccountFlagType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all flag types count.
        /// </summary>
        /// <returns>list of transit flag types</returns>
        [WebMethod(Description = "Get all flag types count.")]
        public int GetAccountFlagTypesCount(string ticket)
        {
            return WebServiceImpl<TransitAccountFlagType, ManagedAccountFlagType, AccountFlagType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete an flag type
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete an flag type.")]
        public void DeleteAccountFlagType(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFlagType, ManagedAccountFlagType, AccountFlagType>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountFlag

        /// <summary>
        /// Flag an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="flag">transit flag</param>
        [WebMethod(Description = "Flag an account.")]
        public int CreateOrUpdateAccountFlag(string ticket, TransitAccountFlag flag)
        {
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.CreateOrUpdate(
                ticket, flag);
        }

        /// <summary>
        /// Get a flagged account.
        /// </summary>
        /// <returns>transit flag</returns>
        [WebMethod(Description = "Get a flagged account.")]
        public TransitAccountFlag GetAccountFlagById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account flags.
        /// </summary>
        /// <returns>list of transit flags</returns>
        [WebMethod(Description = "Get all account flags.")]
        public List<TransitAccountFlag> GetAccountFlags(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all account flags count.
        /// </summary>
        /// <returns>number of transit flags</returns>
        [WebMethod(Description = "Get all account flags count.")]
        public int GetAccountFlagsCount(string ticket)
        {
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all account flags by account id.
        /// </summary>
        /// <returns>list of transit flags</returns>
        [WebMethod(Description = "Get all account flags by account id.")]
        public List<TransitAccountFlag> GetAccountFlagsByAccountId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account flags count by account id.
        /// </summary>
        /// <returns>number of transit flags</returns>
        [WebMethod(Description = "Get all account flags count by account id.")]
        public int GetAccountFlagsByAccountIdCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get all account flags by flagged account id.
        /// </summary>
        /// <returns>list of transit flags</returns>
        [WebMethod(Description = "Get all account flags by flagged account id.")]
        public List<TransitAccountFlag> GetAccountFlagsByFlaggedAccountId(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("FlaggedAccount.Id", id) };
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account flags count by flagged account id.
        /// </summary>
        /// <returns>number of transit flags</returns>
        [WebMethod(Description = "Get all account flags count by flagged account id.")]
        public int GetAccountFlagsByFlaggedAccountIdCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("FlaggedAccount.Id", id) };
            return WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete a flag
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a flag.")]
        public void DeleteAccountFlag(string ticket, int id)
        {
            WebServiceImpl<TransitAccountFlag, ManagedAccountFlag, AccountFlag>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountQuota

        /// <summary>
        /// Set a quota for an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="quota">transit quota</param>
        [WebMethod(Description = "Set a quota for an account.")]
        public int CreateOrUpdateAccountQuota(string ticket, TransitAccountQuota quota)
        {
            return WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.CreateOrUpdate(
                ticket, quota);
        }

        /// <summary>
        /// Get an account quota.
        /// </summary>
        /// <returns>transit quota</returns>
        [WebMethod(Description = "Get a an account quota.")]
        public TransitAccountQuota GetAccountQuotaById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all account quotas by account id.
        /// </summary>
        /// <returns>list of transit quotas</returns>
        [WebMethod(Description = "Get all account quotas by account id.")]
        public List<TransitAccountQuota> GetAccountQuotas(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.GetList(
                ticket, options, expressions, null);
        }

        /// <summary>
        /// Get all account quotas count by account id.
        /// </summary>
        /// <returns>number of transit quotas</returns>
        [WebMethod(Description = "Get all account quotas count by account id.")]
        public int GetAccountQuotasCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Delete a quota
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        [WebMethod(Description = "Delete a quota.")]
        public void DeleteAccountQuota(string ticket, int id)
        {
            WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get account quota by object name and account id.
        /// </summary>
        /// <returns>account quota</returns>
        [WebMethod(Description = "Get account quota by object name and account id.")]
        public TransitAccountQuota GetAccountQuotaByObjectName(string ticket, int id, string name)
        {
            return WebServiceImpl<TransitAccountQuota, ManagedAccountQuota, AccountQuota>.GetByQuery(
                ticket, string.Format("FROM AccountQuota AccountQuota" +
                    " WHERE AccountQuota.Account.Id = {0}" +
                    " AND AccountQuota.DataObject.Name = '{1}'", id, Renderer.SqlEncode(name)));
        }

        #endregion

        #region AccountNumbers

        /// <summary>
        /// Get account numbers.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account id</param>
        /// <returns>account numbers</returns>
        [WebMethod(Description = "Get account numbers.")]
        public TransitAccountNumbers GetAccountNumbersByAccountId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                TransitAccountNumbers t_result = new TransitAccountNumbers();
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                
                ManagedAccount acct = new ManagedAccount(session, id);
                
                // second degree count
                t_result.SecondDegreeCount = acct.GetNDegreeCount(sec, 2);
                // first degree count
                t_result.FirstDegreeCount = session.CreateQuery(
                    string.Format("SELECT COUNT(*) FROM AccountFriend AccountFriend" +
                        " WHERE (AccountFriend.Account.Id = {0} OR AccountFriend.Keen.Id = {0})",
                        id)).UniqueResult<int>();
                // all accounts count
                t_result.AllCount = session.CreateQuery(
                    "SELECT COUNT(*) FROM Account Account").UniqueResult<int>();
                // discussion posts count
                DiscussionQueryOptions qopt = new DiscussionQueryOptions();
                qopt.AccountId = id;
                t_result.PostsCount = session.CreateQuery(
                    string.Format("SELECT COUNT(*) FROM DiscussionPost DiscussionPost {0}", 
                        qopt.CountQuery)).UniqueResult<int>();
                return t_result;
            }
        }
        #endregion
    }
}

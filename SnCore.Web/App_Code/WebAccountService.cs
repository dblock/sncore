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
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Web.Services.Protocols;
using SnCore.Tools.Web;
using SnCore.Tools;
using System.Net.Mail;
using System.Text;

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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.Login(session, email, password);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(acct.Id.ToString(), false);
                SnCore.Data.Hibernate.Session.Flush();
                return cookie.Value;
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.LoginOpenId(session, token, new NameValueCollectionSerializer(names, values).Collection);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(acct.Id.ToString(), false);
                SnCore.Data.Hibernate.Session.Flush();
                return cookie.Value;
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
        public string LoginMd5(string emailaddress, string passwordhash)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.LoginMd5(session, emailaddress, passwordhash);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(acct.Id.ToString(), false);
                SnCore.Data.Hibernate.Session.Flush();
                return cookie.Value;
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account with openid.")]
        public int CreateAccountWithOpenId(string betapassword, string consumerurl, TransitAccount ta)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                if (s != betapassword)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                acct.CreateWithOpenId(consumerurl, ta, ManagedAccount.GetAdminSecurityContext(session));
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return ManagedAccount.VerifyOpenId(token, new NameValueCollectionSerializer(names, values).Collection).ToString();
            }
        }

        #endregion

        #region Account

        /// <summary>
        /// Get account id.
        /// </summary>
        /// <param name="ticket">authentication ticket previously obtained from SnCore::WebAccountService::Login or SnCore::WebAccountService::LoginMd5</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Get account id.", CacheDuration = 60)]
        public int GetAccountId(string ticket)
        {
            return ManagedAccount.GetAccountId(ticket);
        }

        /// <summary>
        /// Get logged in account information. 
        /// Also updates last login time on five minute intervals.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account</returns>
        [WebMethod(Description = "Get account information.")]
        public TransitAccount GetAccount(string ticket)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                try
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                    ManagedAccount account = new ManagedAccount(session, sec.Account);
                    account.UpdateLastLogin();
                    return account.GetTransitInstance(sec);
                }
                catch (ObjectNotFoundException)
                {
                    return null;
                }
            }
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
        /// Delete your account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="password">account password</param>
        [WebMethod(Description = "Delete your account.")]
        public void DeleteAccount(string ticket, string password)
        {
            DeleteAccountById(ticket, ManagedAccount.GetAccountId(ticket), password);
        }

        /// <summary>
        /// Delete an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account to delete</param>
        /// <param name="password">current account password</param>
        [WebMethod(Description = "Delete your account.")]
        public void DeleteAccountById(string ticket, int id, string password)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount user = new ManagedAccount(session, id);

                if (user.IsAdministrator())
                {
                    throw new SoapException(
                        "You cannot delete an administrative account.",
                        SoapException.ClientFaultCode);
                }

                if (sec.Account.Id != user.Id)
                {
                    if (!sec.IsAdministrator())
                    {
                        // only admin can delete other people's account
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }
                
                if (! sec.IsAdministrator() && ! user.IsPasswordValid(password))
                {
                    // the requester is the same as the account being deleted, password didn't match
                    throw new ManagedAccount.AccessDeniedException();
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccount m_account = ManagedAccount.FindByEmail(session, emailaddress);
                return m_account.GetTransitInstance(sec);
            }
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

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount account = new ManagedAccount(session, id);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(account.Id.ToString(), false);
                return cookie.Value;
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountInvitation invitation = new ManagedAccountInvitation(session, invitationid);
                if (invitation.Code != code)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session);
                int id = acct.Create(invitation.Instance.Email, ta, ManagedAccount.GetAdminSecurityContext(session));

                TransitAccountFriend t_friend = new TransitAccountFriend();
                AccountFriend friend = new AccountFriend();
                friend.Account = (Account)session.Load(typeof(Account), invitation.AccountId);
                friend.Keen = (Account)session.Load(typeof(Account), id);
                friend.Created = DateTime.UtcNow;
                session.Save(friend);

                invitation.Delete(ManagedAccount.GetAdminSecurityContext(session));

                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        [WebMethod(Description = "Decline an invitation.")]
        public void DeclineInvitation(int id, string code)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountInvitation invitation = new ManagedAccountInvitation(session, id);
                if (invitation.Code != code)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

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
                try
                {
                    TransitAccountInvitation t_instance = GetAccountInvitationByEmail(ticket, invitation.AccountId, invitation.Email);
                    throw new Exception("Existing Invitation Pending");
                }
                catch (ObjectNotFoundException)
                {
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
        public List<TransitAccountInvitation> GetAccountInvitations(string ticket, int id, ServiceQueryOptions options)
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
        public int GetAccountInvitationsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountInvitation, ManagedAccountInvitation, AccountInvitation>.GetCount(
                ticket, string.Format("WHERE AccountInvitation.Account.Id = {0}", id));
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
        /// Delete a invitation.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="invitationid">invitation id</param>
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

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
                    throw new SoapException("User is already an administrator.",
                        SoapException.ClientFaultCode);
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

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
                    throw new SoapException("You cannot demote self.", SoapException.ClientFaultCode);
                }

                ManagedAccount acct = new ManagedAccount(session, id);
                if (!acct.IsAdministrator())
                {
                    throw new SoapException("User is not an administrator.",
                        SoapException.ClientFaultCode);
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

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // check permissions: userid must have admin rights to the Accounts table
                ManagedAccount user = new ManagedAccount(session, userid);
                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount acct = new ManagedAccount(session, id);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(acct.Id.ToString(), false);
                SnCore.Data.Hibernate.Session.Flush();
                return cookie.Value;
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
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
                    account.ResetPassword(newpassword, false);
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
        /// Reset password.
        /// </summary>
        [WebMethod(Description = "Reset password.")]
        public void ResetPassword(string emailaddress, DateTime dateofbirth)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = ManagedAccount.FindByEmailAndBirthday(session, emailaddress, dateofbirth);
                string newpassword = RandomPassword.Generate();
                a.ResetPassword(newpassword, true);

                // EmailAccountMessage
                ManagedSiteConnector.SendAccountEmailMessageUriAsAdmin(
                    session,
                    new MailAddress(a.ActiveEmailAddress, a.Name).ToString(),
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
                using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedAccountEmail m_instance = new ManagedAccountEmail(session, id);
                    m_instance.Confirm();
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
            return WebServiceImpl<TransitAccountEmail, ManagedAccountEmail, AccountEmail>.GetCount(
                ticket, string.Format("WHERE AccountEmail.Account.Id = {0}", id));
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
        /// <param name="ticket"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public string VerifyAccountEmail(string password, int id, string code)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEmailConfirmation c = new ManagedAccountEmailConfirmation(session, id);
                string emailaddress = c.Verify(password, code);
                SnCore.Data.Hibernate.Session.Flush();
                return emailaddress;
            }
        }

        #endregion

        #region AccountEmailConfirmation

        /// <summary>
        /// Get account e-mail confirmations count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account e-mail confirmations</returns>
        [WebMethod(Description = "Get account e-mail confirmations count.")]
        public int GetAccountEmailConfirmationsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.GetCount(
                ticket, string.Format("WHERE AccountEmailConfirmation.AccountEmail.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get account e-mail confirmations.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit e-mail confirmation</returns>
        [WebMethod(Description = "Get account e-mail confirmations.")]
        public List<TransitAccountEmailConfirmation> GetAccountEmailConfirmations(string ticket, int id, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.GetList(
                ticket, options, string.Format("SELECT AccountEmailConfirmation FROM AccountEmailConfirmation AccountEmailConfirmation WHERE AccountEmailConfirmation.AccountEmail.Account.Id = {0}", id));
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
        /// <returns></returns>
        [WebMethod(Description = "Delete an account e-mail confirmation.")]
        public void DeleteAccountEmailConfirmation(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEmailConfirmation, ManagedAccountEmailConfirmation, AccountEmailConfirmation>.Delete(
                ticket, id);
        }

        #endregion
    }
}

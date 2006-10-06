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
        [WebMethod(Description = "Check whether a beta password is set.", CacheDuration=60)]
        public bool IsBetaPasswordSet()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                string s = ManagedConfiguration.GetValue(session, "SnCore.Beta.Password", string.Empty);
                return ! string.IsNullOrEmpty(s);
            }
        }

        /// <summary>
        /// Create an account.
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="password">password</param>
        /// <param name="emailaddress">e-mail address</param>
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account.")]
        public int CreateAccount(
                  string betapassword,
                  string password,
                  string emailaddress,
                  TransitAccount ta)
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
                acct.Create(password, emailaddress, ta);
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
        public int CreateAccountWithOpenId(
                  string betapassword,
                  string consumerurl,
                  TransitAccount ta)
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
                acct.CreateWithOpenId(consumerurl, ta);
                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        /// <summary>
        /// Create an account with an e-mail invitation.
        /// </summary>
        /// <param name="invitationid">invitation id</param>
        /// <param name="code">code</param>
        /// <param name="name">user name</param>
        /// <param name="password">password</param>
        /// <param name="emailaddress">e-mail address</param>
        /// <param name="ta">transit account information</param>
        /// <returns>account id</returns>
        [WebMethod(Description = "Create an account.")]
        public int CreateAccountInvitation(
                  int invitationid,
                  string code,
                  string password,
                  string emailaddress,
                  TransitAccount ta)
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
                int newid = acct.Create(password, emailaddress, ta, true);

                AccountFriend friend = new AccountFriend();
                friend.Account = (Account)session.Load(typeof(Account), invitation.AccountId);
                friend.Keen = (Account)session.Load(typeof(Account), newid);
                friend.Created = DateTime.UtcNow;
                session.Save(friend);

                invitation.Delete();

                SnCore.Data.Hibernate.Session.Flush();
                return acct.Id;
            }
        }

        [WebMethod(Description = "Decline an invitation.")]
        public void DeclineInvitation(int invitationid, string code)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccountInvitation invitation = new ManagedAccountInvitation(session, invitationid);
                if (invitation.Code != code)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                invitation.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Find an account by e-mail address.
        /// </summary>
        /// <param name="emailaddress">verified e-mail address</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Find an account by e-mail address.")]
        public TransitAccount FindByEmail(string emailaddress)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccount result = ManagedAccount.FindByEmail(session, emailaddress).TransitAccount;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Login to an account.
        /// </summary>
        /// <param name="emailaddress">verified e-mail address</param>
        /// <param name="password">valid password</param>
        /// <returns>authentication ticket for the current session</returns>
        [WebMethod(Description = "Login to an account.")]
        public string Login(string emailaddress, string password)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = ManagedAccount.Login(session, emailaddress, password);
                HttpCookie cookie = FormsAuthentication.GetAuthCookie(acct.Id.ToString(), false);
                SnCore.Data.Hibernate.Session.Flush();
                return cookie.Value;
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

        /// <summary>
        /// Get account permissions.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>permissions information</returns>
        [WebMethod(Description = "Get account permissions.", CacheDuration = 60)]
        public TransitAccountPermissions GetAccountPermissions(string ticket)
        {
            return GetAccountPermissionsById(GetAccountId(ticket));
        }

        /// <summary>
        /// Get account permissions.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>permissions information</returns>
        [WebMethod(Description = "Get account permissions.", CacheDuration = 60)]
        public TransitAccountPermissions GetAccountPermissionsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, id);
                return new TransitAccountPermissions(user);
            }
        }

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

        /// <summary>
        /// Verify an e-mail.
        /// </summary>
        /// <param name="password">account password</param>
        /// <param name="id">e-mail confirmation request id</param>
        /// <param name="code">e-mail confirmation request code</param>
        /// <returns>verified e-mail address</returns>
        [WebMethod(Description = "Verify an e-mail.")]
        public string VerifyEmail(string password, int id, string code)
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

        [WebMethod(Description = "Get an account e-mail confirmation.")]
        public TransitAccountEmailConfirmation GetAccountEmailConfirmationById(string ticket, int id)
        {
            int user_id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, user_id);
                ManagedAccountEmailConfirmation c = new ManagedAccountEmailConfirmation(session, id);

                // the e-mail confirmation contains the code that should only be readable by the user via his e-mail
                // hence only admin can see it otherwise
                if (! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return c.TransitAccountEmailConfirmation;
            }
        }

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
            int id = GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                try
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedAccount a = new ManagedAccount(session, id);
                    a.UpdateLastLogin();
                    SnCore.Data.Hibernate.Session.Flush();
                    return a.TransitAccount;
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
        public TransitAccount GetAccountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                try
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedAccount a = new ManagedAccount(session, id);
                    return a.TransitAccount;
                }
                catch (ObjectNotFoundException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get account picture without data.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get account picture without data.", BufferResponse = true)]
        public TransitAccountPicture GetAccountPictureById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture a = new ManagedAccountPicture(session, id);
                TransitAccountPicture p = a.TransitAccountPicture;
                return p;
            }
        }

        /// <summary>
        /// Get account picture data.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get account picture data.", BufferResponse = true)]
        public TransitAccountPictureWithBitmap GetAccountPictureWithBitmapById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture a = new ManagedAccountPicture(session, id);
                return a.TransitAccountPictureWithBitmap;
            }
        }

        /// <summary>
        /// Get account picture data if modified since.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get account picture data if modified since.", BufferResponse = true)]
        public TransitAccountPictureWithBitmap GetAccountPictureWithBitmapByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture p = new ManagedAccountPicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitAccountPictureWithBitmap;
            }
        }

        /// <summary>
        /// Get picture thumbnail.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture, thumbnail only</returns>
        [WebMethod(Description = "Get account picture thumbnail.", BufferResponse = true)]
        public TransitAccountPictureWithThumbnail GetAccountPictureWithThumbnailById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture p = new ManagedAccountPicture(session, id);
                return p.TransitAccountPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get picture thumbnail.
        /// </summary>
        /// <param name="id">picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture, thumbnail only</returns>
        [WebMethod(Description = "Get account picture thumbnail if modified since.", BufferResponse = true)]
        public TransitAccountPictureWithThumbnail GetAccountPictureWithThumbnailByIdIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture p = new ManagedAccountPicture(session, id);

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p.TransitAccountPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Update an account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ta">transit account</param>
        [WebMethod(Description = "Update an account.")]
        public void UpdateAccount(string ticket, TransitAccount ta)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                if (ta.Id != 0 && ta.Id != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                user.Update(ta);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Update an account e-mail.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="tae">transit account e-mail</param>
        [WebMethod(Description = "Update an account e-mail.")]
        public void UpdateAccountEmail(string ticket, TransitAccountEmail tae)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                if (tae.Id == 0)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                ManagedAccountEmail email = new ManagedAccountEmail(session, tae);
                if (email.Account.Id != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                email.Account.Update(tae);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

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

        [WebMethod(Description = "Send an account e-mail message.")]
        public int SendAccountEmailMessage(string ticket, TransitAccountEmailMessage message)
        {
            int user_id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                AccountEmailMessage m = message.GetAccountEmailMessage();
                m.Account = (Account)session.Load(typeof(Account), user_id);
                ManagedAccount user = new ManagedAccount(session, m.Account);

                if (!user.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();
                
                m.MailFrom = new MailAddress(user.ActiveEmailAddress, user.Name).ToString();
                m.Sent = false;
                m.Created = m.Modified = DateTime.UtcNow;
                
                session.Save(m);
                SnCore.Data.Hibernate.Session.Flush();
                return m.Id;
            }
        }

        /// <summary>
        /// Get e-mail addresses count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account e-mails</returns>
        [WebMethod(Description = "Get e-mail addresses count.")]
        public int GetAccountEmailsCount(string ticket)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountEmail s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get e-mail addresses.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit e-mail</returns>
        [WebMethod(Description = "Get e-mail addresses.")]
        public List<TransitAccountEmail> GetAccountEmails(string ticket, ServiceQueryOptions options)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountEmail))
                    .Add(Expression.Eq("Account.Id", id));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountEmail> result = new List<TransitAccountEmail>(list.Count);
                foreach (AccountEmail e in list)
                {
                    result.Add(new TransitAccountEmail(e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Add an e-mail address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="email">transit e-mail</param>
        [WebMethod(Description = "Add an e-mail address.")]
        public void AddAccountEmail(string ticket, TransitAccountEmail email)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                a.Create(email);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Re-send a confirmation for an e-mail address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="email">transit e-mail</param>
        [WebMethod(Description = "Re-send a confirmation for an e-mail address.")]
        public void ConfirmAccountEmail(string ticket, int emailid)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountEmail e = new ManagedAccountEmail(session, emailid);

                if (e.Account.Id != user.Id && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                e.Confirm();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete an e-mail address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="emailid">e-mail id</param>
        [WebMethod(Description = "Delete an e-mail address.")]
        public void DeleteAccountEmail(string ticket, int emailid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEmail email = new ManagedAccountEmail(session, emailid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (email.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                email.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account address by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">address id</param>
        /// <returns>transit address</returns>
        [WebMethod(Description = "Get account address by id.")]
        public TransitAccountAddress GetAccountAddressById(string ticket, int id)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for account addresses
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountAddress m = new ManagedAccountAddress(session, id);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (m.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                return m.TransitAccountAddress;
            }
        }

        /// <summary>
        /// Get account addresses.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account addresses</returns>
        [WebMethod(Description = "Get account addresses.")]
        public List<TransitAccountAddress> GetAccountAddresses(string ticket)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountAddress))
                    .Add(Expression.Eq("Account.Id", id))
                    .List();

                List<TransitAccountAddress> result = new List<TransitAccountAddress>(list.Count);
                foreach (AccountAddress e in list)
                {
                    result.Add(new TransitAccountAddress(e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Add an address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="address">new address</param>
        [WebMethod(Description = "Add an address.")]
        public int AddAccountAddress(string ticket, TransitAccountAddress address)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(address);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account survey answer counts.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>number of answers filled</returns>
        [WebMethod(Description = "Get account survey answers.")]
        public int GetAccountSurveyAnswersCount(string ticket, int surveyid)
        {
            return GetAccountSurveyAnswersCountById(GetAccountId(ticket), surveyid);
        }

        /// <summary>
        /// Get account survey answers count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>number of answers filled</returns>
        [WebMethod(Description = "Get account survey answers.", CacheDuration = 60)]
        public int GetAccountSurveyAnswersCountById(int id, int surveyid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(a) FROM AccountSurveyAnswer a, SurveyQuestion q" +
                    " where a.Account.Id = {0} and a.SurveyQuestion.Id = q.Id and q.Survey.Id = {1}",
                    id, surveyid)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account survey answers.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get account survey answers.")]
        public List<TransitAccountSurveyAnswer> GetAccountSurveyAnswers(string ticket, int surveyid)
        {
            return GetAccountSurveyAnswersById(GetAccountId(ticket), surveyid);
        }

        /// <summary>
        /// Get account survey answers.
        /// </summary>
        /// <param name="id">account id</param>
        /// <param name="surveyid">survey id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get account survey answers.", CacheDuration = 60)]
        public List<TransitAccountSurveyAnswer> GetAccountSurveyAnswersById(int id, int surveyid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
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
                        result.Add(new ManagedAccountSurveyAnswer(session, a).TransitAccountSurveyAnswer);
                    }
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get survey answers count for a single question.
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>answers count</returns>
        [WebMethod(Description = "Get survey answers count for a single question.", CacheDuration = 60)]
        public int GetAccountSurveyAnswersCountByQuestionId(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(a) FROM AccountSurveyAnswer a WHERE a.SurveyQuestion.Id = "
                    + id.ToString()).UniqueResult();
            }
        }

        /// <summary>
        /// Get survey answers for a single question.
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns>transit account survey answers</returns>
        [WebMethod(Description = "Get survey answers for a single question.", CacheDuration = 60)]
        public List<TransitAccountSurveyAnswer> GetAccountSurveyAnswersByQuestionId(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountSurveyAnswer))
                    .Add(Expression.Eq("SurveyQuestion.Id", id))
                    .AddOrder(Order.Desc("Modified"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList answers = c.List();
                List<TransitAccountSurveyAnswer> result = new List<TransitAccountSurveyAnswer>(answers.Count);
                foreach (AccountSurveyAnswer a in answers)
                {
                    result.Add(new ManagedAccountSurveyAnswer(session, a).TransitAccountSurveyAnswer);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for survey answers
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountSurveyAnswer(session, id).TransitAccountSurveyAnswer;
            }
        }

        /// <summary>
        /// Add a survey answer.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="answer">survey answer</param>
        [WebMethod(Description = "Add a survey answer.")]
        public int AddAccountSurveyAnswer(string ticket, TransitAccountSurveyAnswer answer)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(answer);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a survey answer.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="surveyanswerid">survey answer id</param>
        [WebMethod(Description = "Delete a survey answer.")]
        public void DeleteAccountSurveyAnswer(string ticket, int surveyanswerid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountSurveyAnswer a = new ManagedAccountSurveyAnswer(session, surveyanswerid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (a.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                a.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }


        /// <summary>
        /// Delete an address.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="addressid">address id</param>
        [WebMethod(Description = "Delete an address.")]
        public void DeleteAccountAddress(string ticket, int addressid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountAddress address = new ManagedAccountAddress(session, addressid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (address.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                address.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Add a picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="picture">new picture</param>
        [WebMethod(Description = "Add a picture.")]
        public int AddAccountPicture(string ticket, TransitAccountPictureWithBitmap picture)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(picture);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="pictureid">picture id</param>
        [WebMethod(Description = "Delete a picture.")]
        public void DeleteAccountPicture(string ticket, int pictureid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountPicture picture = new ManagedAccountPicture(session, pictureid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (picture.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                picture.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account pictures count.
        /// </summary>
        [WebMethod(Description = "Get account pictures count.")]
        public int GetAccountPicturesCount(string ticket)
        {
            return GetAccountPicturesCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account pictures count.
        /// </summary>
        [WebMethod(Description = "Get account pictures count.")]
        public int GetAccountPicturesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountPicture s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account pictures.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account pictures</returns>
        [WebMethod(Description = "Get account pictures.")]
        public List<TransitAccountPicture> GetAccountPictures(string ticket, ServiceQueryOptions options)
        {
            return GetAccountPicturesById(GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account pictures.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account pictures</returns>
        [WebMethod(Description = "Get account pictures.", CacheDuration = 60)]
        public List<TransitAccountPicture> GetAccountPicturesById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountPicture))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"));            

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountPicture> result = new List<TransitAccountPicture>(list.Count);
                foreach (AccountPicture e in list)
                {
                    result.Add(new ManagedAccountPicture(session, e).TransitAccountPicture);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account websites count.
        /// </summary>
        [WebMethod(Description = "Get account websites count.")]
        public int GetAccountWebsitesCount(string ticket)
        {
            return GetAccountWebsitesCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account websites count by account id.
        /// </summary>
        [WebMethod(Description = "Get account websites count by account id.")]
        public int GetAccountWebsitesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(s) FROM AccountWebsite s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account websites.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account websites</returns>
        [WebMethod(Description = "Get account websites.")]
        public List<TransitAccountWebsite> GetAccountWebsites(string ticket, ServiceQueryOptions options)
        {
            return GetAccountWebsitesById(GetAccountId(ticket), options);
        }

        /// <summary>
        /// Get account websites.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account websites</returns>
        [WebMethod(Description = "Get account websites.", CacheDuration = 60)]
        public List<TransitAccountWebsite> GetAccountWebsitesById(int id, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountWebsite))
                    .Add(Expression.Eq("Account.Id", id));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList list = c.List();

                List<TransitAccountWebsite> result = new List<TransitAccountWebsite>(list.Count);
                foreach (AccountWebsite e in list)
                {
                    result.Add(new TransitAccountWebsite(e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account website by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">website id</param>
        /// <returns>transit account website</returns>
        [WebMethod(Description = "Get account website by id.")]
        public TransitAccountWebsite GetAccountWebsiteById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for website
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountWebsite(session, id).TransitAccountWebsite;
            }
        }

        /// <summary>
        /// Add a website.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="website">new website</param>
        [WebMethod(Description = "Add a website.")]
        public int AddAccountWebsite(string ticket, TransitAccountWebsite website)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(website);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a website.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="websiteid">website id</param>
        [WebMethod(Description = "Delete a website.")]
        public void DeleteAccountWebsite(string ticket, int websiteid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, id);
                ManagedAccountWebsite website = new ManagedAccountWebsite(session, websiteid);
                if (website.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                website.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Create account system folders.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Create account system folders.")]
        public void CreateAccountSystemMessageFolders(string ticket)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, userid);
                acct.CreateAccountSystemMessageFolders();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account message folders.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account message folders</returns>
        [WebMethod(Description = "Get account message folders.")]
        public List<TransitAccountMessageFolder> GetAccountMessageFolders(string ticket)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                Account a = (Account)session.Load(typeof(Account), userid);
                // sort the tree
                IList folders = session.CreateCriteria(typeof(AccountMessageFolder))
                    .Add(Expression.Eq("Account.Id", a.Id))
                    .AddOrder(Order.Asc("Name"))
                    .List();
                AccountMessageFolderTree tree = new AccountMessageFolderTree(folders);
                IEnumerator<AccountMessageFolder> enumerator = tree.GetDepthFirstEnumerator();
                List<TransitAccountMessageFolder> result = new List<TransitAccountMessageFolder>();
                while (enumerator.MoveNext())
                {
                    TransitAccountMessageFolder folder = new TransitAccountMessageFolder(enumerator.Current);
                    result.Add(folder);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
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
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountMessageFolder f = new TransitAccountMessageFolder(
                    (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), id));
                ManagedAccount user = new ManagedAccount(session, userid);

                if (userid != f.AccountId && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return f;
            }
        }

        /// <summary>
        /// Add a message folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="message folder">new message folder</param>
        [WebMethod(Description = "Add a message folder.")]
        public int AddAccountMessageFolder(string ticket, TransitAccountMessageFolder messagefolder)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (messagefolder.System)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (messagefolder.Id > 0)
                {
                    AccountMessageFolder folder = (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), messagefolder.Id);
                    if (folder.System)
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                int result = a.CreateOrUpdate(messagefolder);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a message folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="messagefolderid">message folder id</param>
        [WebMethod(Description = "Delete a message folder.")]
        public void DeleteAccountMessageFolder(string ticket, int messagefolderid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount acct = new ManagedAccount(session, id);
                ManagedAccountMessageFolder folder = new ManagedAccountMessageFolder(session, messagefolderid);
                if (folder.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                folder.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }


        /// <summary>
        /// Get account messages.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account messages</returns>
        [WebMethod(Description = "Get account messages.")]
        public List<TransitAccountMessage> GetAccountMessages(string ticket, int folderid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                // both expressions ensure you can't retrieve messages that don't belong to you
                IList list = session.CreateCriteria(typeof(AccountMessage))
                    .Add(Expression.Eq("Account.Id", id))
                    .Add(Expression.Eq("AccountMessageFolder.Id", folderid))
                    .AddOrder(Order.Desc("Sent"))
                    .List();

                List<TransitAccountMessage> result = new List<TransitAccountMessage>(list.Count);
                foreach (AccountMessage e in list)
                {
                    result.Add(new ManagedAccountMessage(session, e).TransitAccountMessage);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
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
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountMessage message = new ManagedAccountMessage(session, id).TransitAccountMessage;
                ManagedAccount user = new ManagedAccount(session, userid);
                if (message.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                return message;
            }
        }

        /// <summary>
        /// Add a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="message">new message</param>
        [WebMethod(Description = "Add a message.")]
        public void AddAccountMessage(string ticket, TransitAccountMessage message)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                message.SenderAccountId = id;
                a.SendAccountMessage(message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Mark message as read/unread.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">message id</param>
        /// <param name="unread">value of unread flag</param>
        [WebMethod(Description = "Mark message as read or unread.")]
        public void MarkMessageAsReadUnread(string ticket, int id, bool unread)
        {
            int user_id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, user_id);
                ManagedAccountMessage m_message = new ManagedAccountMessage(session, id);

                if (m_message.AccountId != user_id && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_message.MarkMessageAsReadUnread(unread);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="messageid">message id</param>
        [WebMethod(Description = "Delete a message.")]
        public void DeleteAccountMessage(string ticket, int messageid)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountMessage message = new ManagedAccountMessage(session, messageid);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (message.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                message.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete messages in a folder.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="folderid">folder id</param>
        [WebMethod(Description = "Delete messages in a folder.")]
        public void DeleteAccountMessagesByFolder(string ticket, int folderid)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountMessageFolder f = new ManagedAccountMessageFolder(session, folderid);
                ManagedAccount user = new ManagedAccount(session, userid);
                if (f.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                f.DeleteAccountMessages();
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
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                AccountMessage message = (AccountMessage)session.Load(typeof(AccountMessage), messageid);
                if (message.Account.Id != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                AccountMessageFolder folder = (AccountMessageFolder)session.Load(typeof(AccountMessageFolder), folderid);
                if (folder.Account.Id != userid)
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (folder.Id == message.AccountMessageFolder.Id)
                {
                    return;
                }

                message.AccountMessageFolder = folder;
                session.Save(message);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get account message folder by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="folder">message folder name</param>
        /// <returns>transit account message folder</returns>
        [WebMethod(Description = "Get account message folder by id.", CacheDuration = 60)]
        public TransitAccountMessageFolder GetAccountMessageSystemFolder(string ticket, string folder)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountMessageFolder f = new ManagedAccountMessageFolder(session,
                    (AccountMessageFolder)session.CreateCriteria(typeof(AccountMessageFolder))
                        .Add(Expression.Eq("Account.Id", userid))
                        .Add(Expression.Eq("Name", folder))
                        .Add(Expression.IsNull("AccountMessageFolderParent"))
                        .UniqueResult()).TransitAccountMessageFolder;

                return f;
            }
        }

        /// <summary>
        /// Move a message.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="messageid">message id</param>
        /// <param name="folder">target folder name</param>
        [WebMethod(Description = "Move a message.")]
        public void MoveAccountMessageToFolder(string ticket, int messageid, string folder)
        {
            int id = GetAccountMessageSystemFolder(ticket, folder).Id;
            MoveAccountMessageToFolderById(ticket, messageid, id);
        }

        /// <summary>
        /// Invite a person.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="invitation">transit invitation</param>
        [WebMethod(Description = "Invite a person.")]
        public int AddAccountInvitation(string ticket, TransitAccountInvitation invitation)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

                int result = a.CreateOrUpdate(invitation);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account invitations.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account invitations</returns>
        [WebMethod(Description = "Get account invitations.")]
        public List<TransitAccountInvitation> GetAccountInvitations(string ticket, ServiceQueryOptions options)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountInvitation))
                    .Add(Expression.Eq("Account.Id", userid))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountInvitation> result = new List<TransitAccountInvitation>(list.Count);
                foreach (AccountInvitation e in list)
                {
                    TransitAccountInvitation i = new TransitAccountInvitation(e);
                    i.Code = string.Empty;
                    result.Add(i);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account invitations count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of outstanding account invitations</returns>
        [WebMethod(Description = "Get account invitations count.")]
        public int GetAccountInvitationsCount(string ticket)
        {
            return GetAccountInvitationsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account invitations count by id.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>number of outstanding account invitations</returns>
        [WebMethod(Description = "Get account invitations count.", CacheDuration = 60)]
        public int GetAccountInvitationsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(i) FROM AccountInvitation i WHERE i.Account.Id = "
                    + id.ToString()).UniqueResult();
            }
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
            int user_id = ManagedAccount.GetAccountId(ticket, 0);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // an invitation can be retrieved for a new user to sign-up
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountInvitation i = new ManagedAccountInvitation(session, id).TransitAccountInvitation;

                if (user_id > 0)
                {
                    ManagedAccount user = new ManagedAccount(session, user_id);
                    if (user.IsAdministrator())
                    {
                        return i;
                    }
                }

                i.Code = string.Empty;
                i.Message = string.Empty;
                return i;
            }
        }

        /// <summary>
        /// Delete a invitation.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="invitationid">invitation id</param>
        [WebMethod(Description = "Delete a invitation.")]
        public void DeleteAccountInvitation(string ticket, int invitationid)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountInvitation i = new ManagedAccountInvitation(session, invitationid);
                if (i.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                i.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete your account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="password">account password</param>
        [WebMethod(Description = "Delete your account.")]
        public void DeleteAccount(string ticket, string password)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (user.IsAdministrator())
                {
                    throw new SoapException(
                        "You cannot delete an administrative account.",
                        SoapException.ClientFaultCode);
                }

                if (!user.IsPasswordValid(password))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                user.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Delete your account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account to delete</param>
        /// <param name="password">current account password</param>
        [WebMethod(Description = "Delete your account.")]
        public void DeleteAccountById(string ticket, int id, string password)
        {
            int userid = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount requester = new ManagedAccount(session, userid);
                ManagedAccount user = new ManagedAccount(session, id);

                if (user.IsAdministrator())
                {
                    throw new SoapException(
                        "You cannot delete an administrative account.",
                        SoapException.ClientFaultCode);
                }

                if (requester.Id != user.Id)
                {
                    if (!requester.IsAdministrator())
                    {
                        throw new SoapException(
                            "You must be an administrator to delete accounts.",
                            SoapException.ClientFaultCode);
                    }
                }

                if (!requester.IsPasswordValid(password))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                user.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }


        #region Surveys

        /// <summary>
        /// Get surveys answered by account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit surveys</returns>
        [WebMethod(Description = "Get surveys answered by account.")]
        public List<TransitSurvey> GetAccountSurveys(string ticket)
        {
            return GetAccountSurveysById(GetAccountId(ticket));
        }

        /// <summary>
        /// Get surveys answered by account.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit surveys</returns>
        [WebMethod(Description = "Get surveys answered by account.", CacheDuration = 60)]
        public List<TransitSurvey> GetAccountSurveysById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateQuery(string.Format(
                    "SELECT DISTINCT s FROM Survey s, Account a, SurveyQuestion q, AccountSurveyAnswer sa " +
                    "WHERE s.Id = q.Survey.Id AND sa.SurveyQuestion.Id = q.Id AND a.Id = sa.Account.Id " +
                    "AND a.Id = {0} ORDER BY s.Id DESC", id))
                    .List();

                List<TransitSurvey> result = new List<TransitSurvey>(list.Count);
                foreach (Survey s in list)
                {
                    result.Add(new ManagedSurvey(session, s).TransitSurvey);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        #endregion

        #region Search

        protected IList InternalSearchAccounts(ISession session, string s, ServiceQueryOptions options)
        {
            int maxsearchresults = ManagedConfiguration.GetValue(session, "SnCore.MaxSearchResults", 128);
            IQuery query = session.CreateSQLQuery(

                    "CREATE TABLE #Results ( Account_Id int, RANK int )\n" +
                    "CREATE TABLE #Unique_Results ( Account_Id int, RANK int )\n" +

                    "INSERT #Results\n" +
                    "SELECT account.Account_Id, ft.[RANK] FROM Account account\n" +
                    "INNER JOIN FREETEXTTABLE (Account, [Name], '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON account.Account_Id = ft.[KEY]\n" +

                    "INSERT #Results\n" +
                    "SELECT account.Account_Id, ft.[RANK] FROM Account account, AccountSurveyAnswer accountsurveyanswer\n" +
                    "INNER JOIN FREETEXTTABLE (AccountSurveyAnswer, ([Answer]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON accountsurveyanswer.AccountSurveyAnswer_Id = ft.[KEY] \n" +
                    "WHERE accountsurveyanswer.Account_Id = account.Account_Id\n" +

                    "INSERT #Results\n" +
                    "SELECT account.Account_Id, ft.[RANK] FROM Account account, AccountPropertyValue accountpropertyvalue\n" +
                    "INNER JOIN FREETEXTTABLE (AccountPropertyValue, ([Value]), '" + Renderer.SqlEncode(s) + "', " +
                        maxsearchresults.ToString() + ") AS ft ON accountpropertyvalue.AccountPropertyValue_Id = ft.[KEY] \n" +
                    "WHERE accountpropertyvalue.Account_Id = account.Account_Id\n" +

                    "INSERT #Unique_Results\n" +
                    "SELECT DISTINCT Account_Id, SUM(RANK)\n" +
                    "FROM #Results GROUP BY Account_Id\n" +
                    "ORDER BY SUM(RANK) DESC\n" +

                    "SELECT {Account.*} FROM {Account}, #Unique_Results\n" +
                    "WHERE Account.Account_Id = #Unique_Results.Account_Id\n" +
                    "ORDER BY #Unique_Results.RANK DESC\n" +

                    "DROP TABLE #Results\n" +
                    "DROP TABLE #Unique_Results\n",

                    "Account",
                    typeof(Account));

            if (options != null)
            {
                query.SetFirstResult(options.FirstResult);
                query.SetMaxResults(options.PageSize);
            }

            return query.List();
        }

        /// <summary>
        /// Search accounts.
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "Search accounts.", CacheDuration = 60)]
        public List<TransitAccountActivity> SearchAccounts(string s, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList accounts = InternalSearchAccounts(session, s, options);

                List<TransitAccountActivity> result = new List<TransitAccountActivity>(accounts.Count);
                foreach (Account account in accounts)
                {
                    result.Add(new ManagedAccount(session, account).TransitAccountActivity);
                }

                return result;
            }
        }

        /// <summary>
        /// Return the number of accounts matching a query.
        /// </summary>
        /// <returns>number of accounts</returns>
        [WebMethod(Description = "Return the number of accounts matching a query.", CacheDuration = 60)]
        public int SearchAccountsCount(string s)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return InternalSearchAccounts(session, s, null).Count;
            }
        }

        #endregion

        #region OpenId

        /// <summary>
        /// Get account openids.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account openids</returns>
        [WebMethod(Description = "Get openids.")]
        public List<TransitAccountOpenId> GetAccountOpenIds(string ticket)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountOpenId))
                    .Add(Expression.Eq("Account.Id", id))
                    .List();

                List<TransitAccountOpenId> result = new List<TransitAccountOpenId>(list.Count);
                foreach (AccountOpenId e in list)
                {
                    result.Add(new TransitAccountOpenId(e));
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an openid.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">openid id</param>
        [WebMethod(Description = "Delete an openid.")]
        public void DeleteAccountOpenId(string ticket, int id)
        {
            int user_id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountOpenId openid = new ManagedAccountOpenId(session, id);
                ManagedAccount acct = new ManagedAccount(session, user_id);
                if (openid.Account.Id != acct.Id && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                openid.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Add an openid.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="openid">transit openid</param>
        [WebMethod(Description = "Add an openid.")]
        public void AddAccountOpenId(string ticket, TransitAccountOpenId openid)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                a.Create(openid);
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Check whether a user has a verified e-mail address.
        /// </summary>
        [WebMethod(Description = "Check whether the user has a verified e-mail address.")]
        public bool HasVerifiedEmail(string ticket)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                return a.HasVerifiedEmail;
            }
        }

        /// <summary>
        /// Return an active e-mail address.
        /// </summary>
        [WebMethod(Description = "Check whether the user has a verified e-mail address.")]
        public string GetActiveEmailAddress(string ticket)
        {
            int id = GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                return a.ActiveEmailAddress;
            }
        }

        #endregion

        #region Account Property Group

        /// <summary>
        /// Create or update a property group.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit property group</param>
        [WebMethod(Description = "Create or update a property group.")]
        public int CreateOrUpdateAccountPropertyGroup(string ticket, TransitAccountPropertyGroup pg)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountPropertyGroup m_propertygroup = new ManagedAccountPropertyGroup(session);
                m_propertygroup.CreateOrUpdate(pg);
                SnCore.Data.Hibernate.Session.Flush();
                return m_propertygroup.Id;
            }
        }

        /// <summary>
        /// Get a property group.
        /// </summary>
        /// <returns>transit property group</returns>
        [WebMethod(Description = "Get a property group.")]
        public TransitAccountPropertyGroup GetAccountPropertyGroupById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountPropertyGroup result = new ManagedAccountPropertyGroup(session, id).TransitAccountPropertyGroup;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all property groups.
        /// </summary>
        /// <returns>list of transit property groups</returns>
        [WebMethod(Description = "Get all property groups.")]
        public List<TransitAccountPropertyGroup> GetAccountPropertyGroups()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList pgs = session.CreateCriteria(typeof(AccountPropertyGroup)).List();
                List<TransitAccountPropertyGroup> result = new List<TransitAccountPropertyGroup>(pgs.Count);
                foreach (AccountPropertyGroup pg in pgs)
                {
                    result.Add(new ManagedAccountPropertyGroup(session, pg).TransitAccountPropertyGroup);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a property group
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property group.")]
        public void DeleteAccountPropertyGroup(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountPropertyGroup m_propertygroup = new ManagedAccountPropertyGroup(session, id);
                m_propertygroup.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Account Property

        /// <summary>
        /// Create or update a property.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="property">transit property</param>
        [WebMethod(Description = "Create or update a property.")]
        public int CreateOrUpdateAccountProperty(string ticket, TransitAccountProperty p)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountProperty m_property = new ManagedAccountProperty(session);
                m_property.CreateOrUpdate(p);
                SnCore.Data.Hibernate.Session.Flush();
                return m_property.Id;
            }
        }

        /// <summary>
        /// Get a property.
        /// </summary>
        /// <returns>transit property</returns>
        [WebMethod(Description = "Get a property.")]
        public TransitAccountProperty GetAccountPropertyById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountProperty result = new ManagedAccountProperty(session, id).TransitAccountProperty;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all properties.
        /// </summary>
        /// <returns>list of transit properties</returns>
        [WebMethod(Description = "Get all properties.")]
        public List<TransitAccountProperty> GetAccountProperties(int gid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList ps = session.CreateCriteria(typeof(AccountProperty))
                    .Add(Expression.Eq("AccountPropertyGroup.Id", gid))
                    .List();
                
                List<TransitAccountProperty> result = new List<TransitAccountProperty>(ps.Count);
                foreach (AccountProperty p in ps)
                {
                    result.Add(new ManagedAccountProperty(session, p).TransitAccountProperty);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a property
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a property.")]
        public void DeleteAccountProperty(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);

            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountProperty m_property = new ManagedAccountProperty(session, id);
                m_property.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

        #region Account Property Value

        /// <summary>
        /// Get accounts that match a property value by name.
        /// </summary>
        /// <returns>transit accounts</returns>
        [WebMethod(Description = "Get accounts that match a property value by name.")]
        public List<TransitAccount> GetAccountsByPropertyValue(
            string groupname, string propertyname, string propertyvalue, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                   "SELECT {account.*} FROM AccountProperty p, AccountPropertyGroup g, AccountPropertyValue v, Account {account}" +
                   " WHERE {account}.Account_Id = v.Account_Id" +
                   " AND v.Account_Id = {account}.Account_Id" +
                   " AND v.AccountProperty_Id = p.AccountProperty_Id" +
                   " AND p.AccountPropertyGroup_Id = g.AccountPropertyGroup_Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%{" + Renderer.SqlEncode(propertyvalue) + "}%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'",
                   "account",
                   typeof(Account));

                if (options != null)
                {
                    query.SetFirstResult(options.FirstResult);
                    query.SetMaxResults(options.PageSize);
                }

                IList list = query.List();

                List<TransitAccount> result = new List<TransitAccount>(list.Count);

                foreach (Account account in list)
                {
                    result.Add(new ManagedAccount(session, account).TransitAccount);
                }

                return result;
            }
        }

        /// <summary>
        /// Get the number of accounts that match a property value by name.
        /// </summary>
        /// <returns>transit accounts</returns>
        [WebMethod(Description = "Get the number of accounts that match a property value by name.")]
        public int GetAccountsByPropertyValueCount(
            string groupname, string propertyname, string propertyvalue)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateQuery(
                   "SELECT COUNT(account) FROM AccountProperty p, AccountPropertyGroup g, AccountPropertyValue v, Account account" +
                   " WHERE account.Id = v.Account.Id" +
                   " AND v.Account.Id = account.Id" +
                   " AND v.AccountProperty.Id = p.Id" +
                   " AND p.AccountPropertyGroup.Id = g.Id" +
                   " AND p.Name = '" + Renderer.SqlEncode(propertyname) + "'" +
                   " AND (" +
                   "  v.Value LIKE '" + Renderer.SqlEncode(propertyvalue) + "'" +
                   "  OR v.Value LIKE '%{" + Renderer.SqlEncode(propertyvalue) + "}%'" +
                   " ) AND g.Name = '" + Renderer.SqlEncode(groupname) + "'");

                return (int)query.UniqueResult();
            }
        }

        /// <summary>
        /// Get a account property value by name.
        /// </summary>
        /// <returns>transit account property value</returns>
        [WebMethod(Description = "Get a account property value by group and name.")]
        public TransitAccountPropertyValue GetAccountPropertyValueByName(int accountid, string groupname, string propertyname)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

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

                TransitAccountPropertyValue result = new ManagedAccountPropertyValue(session, ppv).TransitAccountPropertyValue;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if ((propertyvalue.AccountId != 0) && (propertyvalue.AccountId != user.Id) && (!user.IsAdministrator()))
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                if (propertyvalue.AccountId == 0) propertyvalue.AccountId = user.Id;
                ManagedAccount account = new ManagedAccount(session, propertyvalue.AccountId);
                int result = account.CreateOrUpdate(propertyvalue);

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get an account property value.
        /// </summary>
        /// <returns>transit account property value</returns>
        [WebMethod(Description = "Get an account property value.")]
        public TransitAccountPropertyValue GetAccountPropertyValueById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountPropertyValue result = new ManagedAccountPropertyValue(session, id).TransitAccountPropertyValue;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get account property values.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get account property values.")]
        public List<TransitAccountPropertyValue> GetAccountPropertyValues(string ticket, int groupid)
        {
            return GetAccountPropertyValuesById(ManagedAccount.GetAccountId(ticket), groupid);
        }

        /// <summary>
        /// Get account property values.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get account property values.", CacheDuration = 60)]
        public List<TransitAccountPropertyValue> GetAccountPropertyValuesById(int accountid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList propertyvalues = session.CreateCriteria(typeof(AccountPropertyValue))
                    .Add(Expression.Eq("Account.Id", accountid))
                    // .Add(Expression.Eq("AccountProperty.AccountPropertyGroup.Id", groupid))
                    // .Add(Expression.Eq("AccountProperty.Publish", true))
                    .List();

                List<TransitAccountPropertyValue> result = new List<TransitAccountPropertyValue>(propertyvalues.Count);
                foreach (AccountPropertyValue propertyvalue in propertyvalues)
                {
                    if ((propertyvalue.AccountProperty.AccountPropertyGroup.Id == groupid) 
                        && propertyvalue.AccountProperty.Publish)
                    {
                        result.Add(new ManagedAccountPropertyValue(session, propertyvalue).TransitAccountPropertyValue);
                    }
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account property values.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get all account property values, including unfilled ones.")]
        public List<TransitAccountPropertyValue> GetAllAccountPropertyValues(string ticket, int groupid)
        {
            return GetAllAccountPropertyValuesById(ManagedAccount.GetAccountId(ticket), groupid);
        }

        /// <summary>
        /// Get all account property values, including unfilled ones.
        /// </summary>
        /// <returns>list of account property values</returns>
        [WebMethod(Description = "Get all account property values, including unfilled ones.", CacheDuration = 60)]
        public List<TransitAccountPropertyValue> GetAllAccountPropertyValuesById(int accountid, int groupid)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ICriteria c = session.CreateCriteria(typeof(AccountProperty));
                if (groupid > 0) c.Add(Expression.Eq("AccountPropertyGroup.Id", groupid));
                IList properties = c.List();

                List<TransitAccountPropertyValue> result = new List<TransitAccountPropertyValue>(properties.Count);

                foreach (AccountProperty property in properties)
                {
                    AccountPropertyValue value = (AccountPropertyValue) session.CreateCriteria(typeof(AccountPropertyValue))
                        .Add(Expression.Eq("Account.Id", accountid))
                        .Add(Expression.Eq("AccountProperty.Id", property.Id))
                        .UniqueResult();

                    if (value == null)
                    {
                        value = new AccountPropertyValue();
                        value.AccountProperty = property;
                        value.Value = property.DefaultValue;
                        value.Account = (Account) session.Load(typeof(Account), accountid);
                    }

                    result.Add(new TransitAccountPropertyValue(value));
                }

                return result;
            }
        }

        /// <summary>
        /// Delete an account property value.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account property value.")]
        public void DeleteAccountPropertyValue(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);
                ManagedAccountPropertyValue m_propertyvalue = new ManagedAccountPropertyValue(session, id);

                if (m_propertyvalue.AccountId != userid && !user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                m_propertyvalue.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }
        #endregion

        #region Account Attribute
        /// <summary>
        /// Create or update an account attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit account attribute</param>
        [WebMethod(Description = "Create or update an account attribute.")]
        public int CreateOrUpdateAccountAttribute(string ticket, TransitAccountAttribute attribute)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccount account = new ManagedAccount(session, attribute.AccountId);

                AccountAttribute a = attribute.GetAccountAttribute(session);
                if (a.Id == 0) a.Created = DateTime.UtcNow;
                session.Save(a);

                SnCore.Data.Hibernate.Session.Flush();
                return a.Id;
            }
        }

        /// <summary>
        /// Get account attributes.
        /// </summary>
        /// <returns>transit account attribute</returns>
        [WebMethod(Description = "Get account attributes.")]
        public TransitAccountAttribute GetAccountAttributeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountAttribute result = new ManagedAccountAttribute(session, id).TransitAccountAttribute;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get account attributes count.
        /// </summary>
        /// <returns>number of account attributes</returns>
        [WebMethod(Description = "Get account attributes count.", CacheDuration = 60)]
        public int GetAccountAttributesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery(string.Format(
                    "SELECT COUNT(a) FROM AccountAttribute a WHERE a.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account attributes.
        /// </summary>
        /// <returns>list of account attributes</returns>
        [WebMethod(Description = "Get account attributes.", CacheDuration = 60)]
        public List<TransitAccountAttribute> GetAccountAttributesById(int accountid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountAttribute))
                    .Add(Expression.Eq("Account.Id", accountid));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList attributes = c.List();

                List<TransitAccountAttribute> result = new List<TransitAccountAttribute>(attributes.Count);
                foreach (AccountAttribute attribute in attributes)
                {
                    result.Add(new ManagedAccountAttribute(session, attribute).TransitAccountAttribute);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an account attribute.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an account attribute.")]
        public void DeleteAccountAttribute(string ticket, int id)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (! user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountAttribute m_attribute = new ManagedAccountAttribute(session, id);
                m_attribute.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }
        #endregion

    }
}

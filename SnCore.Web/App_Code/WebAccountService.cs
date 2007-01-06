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

        #endregion

        #region Account

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

        #endregion
    }
}

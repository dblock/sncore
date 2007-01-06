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
    /// Managed web security and authentication services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebAuthService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebAuthService : WebService
    {
        public WebAuthService()
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

        #endregion

        #region Account Information

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

        #endregion
    }
}

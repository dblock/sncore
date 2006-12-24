using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web license services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebLicenseService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebLicenseService : WebService
    {

        public WebLicenseService()
        {

        }

        #region License

        /// <summary>
        /// Get account licenses.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account licenses</returns>
        [WebMethod(Description = "Get account licenses.")]
        public List<TransitAccountLicense> GetAccountLicenses(string ticket)
        {
            return GetAccountLicensesById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account licenses.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account licenses</returns>
        [WebMethod(Description = "Get account licenses.", CacheDuration = 60)]
        public List<TransitAccountLicense> GetAccountLicensesById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountLicense))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"))
                    .List();

                List<TransitAccountLicense> result = new List<TransitAccountLicense>(list.Count);
                foreach (AccountLicense e in list)
                {
                    result.Add(new TransitAccountLicense(e));
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account licenses count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account licenses</returns>
        [WebMethod(Description = "Get account licenses count.")]
        public int GetAccountLicensesCount(string ticket)
        {
            return GetAccountLicensesCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account licenses count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account licenses count</returns>
        [WebMethod(Description = "Get account licenses count.", CacheDuration = 60)]
        public int GetAccountLicensesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM AccountLicense s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account license by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">license id</param>
        /// <returns>transit account license</returns>
        [WebMethod(Description = "Get account license by id.")]
        public TransitAccountLicense GetAccountLicenseById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for license
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountLicense(session, id).TransitAccountLicense;
            }
        }

        /// <summary>
        /// Get account license.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account license</returns>
        [WebMethod(Description = "Get account license.")]
        public TransitAccountLicense GetAccountLicense(string ticket)
        {
            return GetAccountLicenseByAccountId(ticket, ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account license by account id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">account id</param>
        /// <returns>transit account license</returns>
        [WebMethod(Description = "Get account license by account id.")]
        public TransitAccountLicense GetAccountLicenseByAccountId(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for license
                ISession session = SnCore.Data.Hibernate.Session.Current;
                AccountLicense al = (AccountLicense) session.CreateCriteria(typeof(AccountLicense))
                    .Add(Expression.Eq("Account.Id", id))
                    .UniqueResult();

                if (al == null) return null;
                return new TransitAccountLicense(al);
            }
        }

        /// <summary>
        /// Create or update a license.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="License">new License</param>
        [WebMethod(Description = "Create a license.")]
        public int CreateOrUpdateAccountLicense(string ticket, TransitAccountLicense license)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(license);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a license.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="licenseid">license id</param>
        [WebMethod(Description = "Delete a license.")]
        public void DeleteAccountLicense(string ticket, int licenseid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountLicense s = new ManagedAccountLicense(session, licenseid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion    
    }
}
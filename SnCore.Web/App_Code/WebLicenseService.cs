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
        /// <param name="id">account id</param>
        /// <returns>transit account licenses</returns>
        [WebMethod(Description = "Get account licenses.", CacheDuration = 60)]
        public List<TransitAccountLicense> GetAccountLicenses(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Get account licenses count.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account licenses count</returns>
        [WebMethod(Description = "Get account licenses count.", CacheDuration = 60)]
        public int GetAccountLicensesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.GetCount(
                ticket, expressions);
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
            return WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.GetById(
                ticket, id);
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
            return WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.GetByCriterion(
                ticket, Expression.Eq("Account.Id", id));
        }

        /// <summary>
        /// Create or update a license.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="license">new license</param>
        [WebMethod(Description = "Create a license.")]
        public int CreateOrUpdateAccountLicense(string ticket, TransitAccountLicense license)
        {
            return WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.CreateOrUpdate(
                ticket, license);
        }

        /// <summary>
        /// Delete a license.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">license id</param>
        [WebMethod(Description = "Delete a license.")]
        public void DeleteAccountLicense(string ticket, int id)
        {
            WebServiceImpl<TransitAccountLicense, ManagedAccountLicense, AccountLicense>.Delete(
                ticket, id);
        }

        #endregion
    }
}
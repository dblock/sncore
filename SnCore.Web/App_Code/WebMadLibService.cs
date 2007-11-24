using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.Web.Security;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Text;
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web mad libs services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebMadLibService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebMadLibService : WebService
    {
        public WebMadLibService()
        {

        }

        #region Mad Libs

        /// <summary>
        /// Gets mad libs count.
        /// </summary>
        [WebMethod(Description = "Get mad libs count.", CacheDuration = 60)]
        public int GetMadLibsCount(string ticket, int id)
        {
            return WebServiceImpl<TransitMadLib, ManagedMadLib, MadLib>.GetCount(
                 ticket, string.Format("WHERE MadLib.Account.Id = {0}", id));
        }

        /// <summary>
        /// Get all mad libs.
        /// </summary>
        [WebMethod(Description = "Get all mad libs.", CacheDuration = 60)]
        public List<TransitMadLib> GetMadLibs(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            Order[] orders = { Order.Desc("Created") };
            return WebServiceImpl<TransitMadLib, ManagedMadLib, MadLib>.GetList(
                ticket, options, expressions, orders); 
        }

        /// <summary>
        /// Create or update a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a mad lib.")]
        public int CreateOrUpdateMadLib(string ticket, TransitMadLib madlib)
        {
            return WebServiceImpl<TransitMadLib, ManagedMadLib, MadLib>.CreateOrUpdate(
                ticket, madlib);
        }

        /// <summary>
        /// Delete a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a mad lib.")]
        public void DeleteMadLib(string ticket, int id)
        {
            WebServiceImpl<TransitMadLib, ManagedMadLib, MadLib>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a mad lib by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a mad lib by id.")]
        public TransitMadLib GetMadLibById(string ticket, int id)
        {
            return WebServiceImpl<TransitMadLib, ManagedMadLib, MadLib>.GetById(
                ticket, id);
        }

        #endregion

        #region Mad Lib Instances

        /// <summary>
        /// Gets mad lib instances count.
        /// </summary>
        [WebMethod(Description = "Get mad lib instances count.", CacheDuration = 60)]
        public int GetMadLibInstancesCount(string ticket, string table, int id)
        {
            int object_id = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                object_id = ManagedDataObject.Find(session, table);
            }

            return WebServiceImpl<TransitMadLibInstance, ManagedMadLibInstance, MadLibInstance>.GetCount(
                ticket, string.Format("WHERE MadLibInstance.DataObject.Id = '{0}' AND MadLibInstance.ObjectId = {1}",
                    object_id, id)); 
        }

        /// <summary>
        /// Get all mad lib instances.
        /// </summary>
        [WebMethod(Description = "Get all mad lib instances.", CacheDuration = 60)]
        public List<TransitMadLibInstance> GetMadLibInstances(string ticket, string table, int id, ServiceQueryOptions options)
        {
            int object_id = 0;
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                object_id = ManagedDataObject.Find(session, table);
            }

            ICriterion[] expressions = 
            {
                Expression.Eq("DataObject.Id", object_id),
                Expression.Eq("ObjectId", id)
            };

            Order[] orders = { Order.Desc("Created") };

            return WebServiceImpl<TransitMadLibInstance, ManagedMadLibInstance, MadLibInstance>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Create or update a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Create or update a mad lib.")]
        public int CreateOrUpdateMadLibInstance(string ticket, TransitMadLibInstance madlibinstance)
        {
            return WebServiceImpl<TransitMadLibInstance, ManagedMadLibInstance, MadLibInstance>.CreateOrUpdate(
                ticket, madlibinstance);
        }

        /// <summary>
        /// Delete a mad lib.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Delete a mad lib.")]
        public void DeleteMadLibInstance(string ticket, int id)
        {
            WebServiceImpl<TransitMadLibInstance, ManagedMadLibInstance, MadLibInstance>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a mad lib instance by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        [WebMethod(Description = "Get a mad lib instance by id.")]
        public TransitMadLibInstance GetMadLibInstanceById(string ticket, int id)
        {
            return WebServiceImpl<TransitMadLibInstance, ManagedMadLibInstance, MadLibInstance>.GetById(
                ticket, id);
        }

        #endregion
    }
}
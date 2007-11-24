using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using SnCore.Services;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Expression;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using System.Reflection;
using System.Web.Services.Protocols;
using SnCore.Data.Hibernate;

namespace SnCore.WebServices
{
    /// <summary>
    /// System information services.
    /// </summary>
    /// 
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebSystemService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebSystemService : WebService
    {
        public WebSystemService()
        {

        }

        #region System
        /// <summary>
        /// System version.
        /// </summary>
        [WebMethod(Description = "System version.", CacheDuration = 120)]
        public string GetVersion()
        {
            return ManagedSystem.Version;
        }

        /// <summary>
        /// System title.
        /// </summary>
        [WebMethod(Description = "System title.", CacheDuration = 120)]
        public string GetTitle()
        {
            return ManagedSystem.Title;
        }

        /// <summary>
        /// Product copyright.
        /// </summary>
        [WebMethod(Description = "Product copyright.", CacheDuration = 120)]
        public string GetCopyright()
        {
            return ManagedSystem.Copyright;
        }

        /// <summary>
        /// Product description.
        /// </summary>
        [WebMethod(Description = "Product description.", CacheDuration = 120)]
        public string GetDescription()
        {
            return ManagedSystem.Description;
        }

        /// <summary>
        /// Product uptime in ticks.
        /// </summary>
        [WebMethod(Description = "Product uptime in ticks.")]
        public long GetUptime()
        {
            return ((TimeSpan)(DateTime.UtcNow - Global.Started)).Ticks;
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Create or update a configuration.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit configuration</param>
        [WebMethod(Description = "Create or update a configuration.")]
        public int CreateOrUpdateConfiguration(string ticket, TransitConfiguration configuration)
        {
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.CreateOrUpdate(
                ticket, configuration);
        }

        /// <summary>
        /// Get a configuration.
        /// </summary>
        /// <returns>transit configuration</returns>
        [WebMethod(Description = "Get a configuration.")]
        public TransitConfiguration GetConfigurationById(string ticket, int id)
        {
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all configurations count.
        /// </summary>
        /// <returns>number of configurations</returns>
        [WebMethod(Description = "Get all configurations count.")]
        public int GetConfigurationsCount(string ticket)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return ManagedConfiguration.GetAllConfigurations(session).Count;
            }
        }

        /// <summary>
        /// Get all configurations.
        /// </summary>
        /// <returns>list of transit configurations</returns>
        [WebMethod(Description = "Get all configurations.")]
        public List<TransitConfiguration> GetConfigurations(string ticket, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                return WebServiceQueryOptions<TransitConfiguration>.Apply(options,
                    WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetTransformedList(
                        session, sec, ManagedConfiguration.GetAllConfigurations(session)));
            }
        }

        /// <summary>
        /// Delete a configuration.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a configuration.")]
        public void DeleteConfiguration(string ticket, int id)
        {
            WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a configuration by name.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a configuration by name.")]
        public TransitConfiguration GetConfigurationByName(string ticket, string name)
        {
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetByCriterion(
                ticket, Expression.Eq("OptionName", name));
        }

        /// <summary>
        /// Get a configuration value by name.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a configuration value by name.")]
        public string GetConfigurationValue(string ticket, string name)
        {
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetByCriterion(
                ticket, Expression.Eq("OptionName", name)).Value;
        }

        /// <summary>
        /// Get a configuration value by name with default.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a configuration value by name with default.")]
        public string GetConfigurationByNameValueWithDefault(string ticket, string name, string defaultvalue)
        {
            return GetConfigurationByNameWithDefault(ticket, name, defaultvalue).Value;
        }

        /// <summary>
        /// Get a configuration by name with default.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a configuration value by name with default.")]
        public TransitConfiguration GetConfigurationByNameWithDefault(string ticket, string name, string defaultvalue)
        {
            TransitConfiguration result = WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetByCriterion(
                    ticket, Expression.Eq("OptionName", name));

            if (result == null)
            {
                result = new TransitConfiguration();
                result.Name = name;
                result.Value = defaultvalue;
            }

            return result;
        }

        #endregion

        #region DomainModel

        /// <summary>
        /// Get the underlying domain model for a type.
        /// </summary>
        [WebMethod(Description = "Get the underlying domain model for a type.")]
        public sp_column[] GetTypeColumns(string type)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                DomainClass dc = Session.Model[type];
                sp_column[] arr = new sp_column[dc.Columns.Count];
                dc.Columns.CopyTo(arr, 0);
                return arr;
            }
        }

        #endregion
    }
}
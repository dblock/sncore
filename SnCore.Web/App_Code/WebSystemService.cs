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
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all configurations.
        /// </summary>
        /// <returns>list of transit configurations</returns>
        [WebMethod(Description = "Get all configurations.")]
        public List<TransitConfiguration> GetConfigurations(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetList(
                ticket, options);
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
                ticket, Expression.Eq("Name", name));
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
                ticket, Expression.Eq("Name", name)).Value;
        }

        /// <summary>
        /// Get a configuration value by name with default.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        [WebMethod(Description = "Get a configuration value by name with default.")]
        public string GetConfigurationValueWithDefault(string ticket, string name, string defaultvalue)
        {
            try
            {
                return WebServiceImpl<TransitConfiguration, ManagedConfiguration, Configuration>.GetByCriterion(
                    ticket, Expression.Eq("Name", name)).Value;
            }
            catch (ObjectNotFoundException)
            {
                return defaultvalue;
            }
        }

        #endregion

        #region Schedule

        /// <summary>
        /// Create or update a schedule.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="schedule">transit  schedule</param>
        [WebMethod(Description = "Create or update a schedule.")]
        public int CreateOrUpdateSchedule(string ticket, TransitSchedule schedule)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.CreateOrUpdate(
                ticket, schedule);
        }

        /// <summary>
        /// Get a schedule.
        /// </summary>
        /// <returns>transit  schedule</returns>
        [WebMethod(Description = "Get a schedule.")]
        public TransitSchedule GetScheduleById(string ticket, int id)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all schedules.
        /// </summary>
        /// <returns>list of transit  schedules</returns>
        [WebMethod(Description = "Get all schedules.")]
        public List<TransitSchedule> GetSchedules(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all schedules count.
        /// </summary>
        /// <returns>list of transit  schedules</returns>
        [WebMethod(Description = "Get all schedules.")]
        public int GetSchedulesCount(string ticket)
        {
            return WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a schedule
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a schedule.")]
        public void DeleteSchedule(string ticket, int id)
        {
            WebServiceImpl<TransitSchedule, ManagedSchedule, Schedule>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get a humanly readable representation of a schedule.
        /// </summary>
        [WebMethod(Description = "Get a humanly readable representation of a schedule.")]
        public string GetScheduleString(string ticket, TransitSchedule schedule, int offset)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedSchedule m_schedule = new ManagedSchedule(session, schedule.GetInstance(session, sec));
                return m_schedule.ToString(offset);
            }
        }

        #endregion

        #region Attribute

        /// <summary>
        /// Create or update an attribute.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="PropertyGroup">transit attribute</param>
        [WebMethod(Description = "Create or update an attribute.")]
        public int CreateOrUpdateAttribute(string ticket, TransitAttribute attr)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.CreateOrUpdate(
                ticket, attr);
        }

        /// <summary>
        /// Get an attribute.
        /// </summary>
        /// <returns>transit attribute</returns>
        [WebMethod(Description = "Get an attribute.")]
        public TransitAttribute GetAttributeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get attribute data if modified since.
        /// </summary>
        /// <param name="id">attribute id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit attribute with bitmap</returns>
        [WebMethod(Description = "Get attribute data if modified since.", BufferResponse = true)]
        public TransitAttribute GetAttributeIfModifiedSince(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAttribute t_instance = WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        /// <summary>
        /// Get all attributes count.
        /// </summary>
        /// <returns>list of transit attributes</returns>
        [WebMethod(Description = "Get all attributes count.")]
        public int GetAttributesCount(string ticket)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all attributes.
        /// </summary>
        /// <returns>list of transit attributes</returns>
        [WebMethod(Description = "Get all attributes.")]
        public List<TransitAttribute> GetAttributes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Delete an attribute
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an attribute.")]
        public void DeleteAttribute(string ticket, int id)
        {
            WebServiceImpl<TransitAttribute, ManagedAttribute, Attribute>.Delete(
                ticket, id);
        }

        #endregion
    }
}
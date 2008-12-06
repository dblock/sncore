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
    /// Managed web event services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebEventService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebEventService : WebService
    {

        public WebEventService()
        {

        }

        #region AccountEvent

        /// <summary>
        /// Get account avents count by account id.
        /// </summary>
        [WebMethod(Description = "Get account events count by account id.")]
        public int GetAccountEventsCountByAccountId(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            return WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get account events.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account events</returns>
        [WebMethod(Description = "Get account events.", CacheDuration = 60)]
        public List<TransitAccountEvent> GetAccountEventsByAccountId(string ticket, int id, int utcoffset, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("Account.Id", id) };
            List<TransitAccountEvent> result = WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.GetList(
                ticket, options, expressions, null);

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                foreach (TransitAccountEvent t_instance in result)
                {
                    t_instance.CreateSchedule(session, utcoffset, sec);
                }
            }

            return result;
        }

        /// <summary>
        /// Get account event by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">event id</param>
        /// <returns>transit account event</returns>
        [WebMethod(Description = "Get account event by id.")]
        public TransitAccountEvent GetAccountEventById(string ticket, int id, int utcoffset)
        {
            TransitAccountEvent t_instance = WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.GetById(
                ticket, id);

            if (t_instance != null)
            {
                using (SnCore.Data.Hibernate.Session.OpenConnection())
                {
                    ISession session = SnCore.Data.Hibernate.Session.Current;
                    ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                    t_instance.CreateSchedule(session, utcoffset, sec);
                }
            }

            return t_instance;
        }

        /// <summary>
        /// Get account event VCalendar by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">event id</param>
        /// <returns>account event VCalendar</returns>
        [WebMethod(Description = "Get account event VCalendar by id.")]
        public string GetAccountEventVCalendarById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountEvent m_instance = new ManagedAccountEvent(session, id);
                return m_instance.ToVCalendarString(sec);
            }
        }

        /// <summary>
        /// Create or update an event.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="event">new event</param>
        [WebMethod(Description = "Create an event.")]
        public int CreateOrUpdateAccountEvent(string ticket, TransitAccountEvent evt)
        {
            return WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.CreateOrUpdate(
                ticket, evt);
        }

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventid">event id</param>
        [WebMethod(Description = "Delete an event.")]
        public void DeleteAccountEvent(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get account event count.
        /// </summary>
        /// <returns>transit account envets count</returns>
        [WebMethod(Description = "Get all account events count.", CacheDuration = 60)]
        public int GetAccountEventsCount(string ticket, TransitAccountEventQueryOptions qopt)
        {
            return WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.GetCount(
                ticket, qopt.CreateCountQuery());
        }

        /// <summary>
        /// Get all account events.
        /// </summary>
        /// <returns>list of transit account events</returns>
        [WebMethod(Description = "Get all account events.", CacheDuration = 60)]
        public List<TransitAccountEvent> GetAccountEvents(string ticket, int utcoffset, TransitAccountEventQueryOptions qopt, ServiceQueryOptions options)
        {
            List<TransitAccountEvent> result = WebServiceImpl<TransitAccountEvent, ManagedAccountEvent, AccountEvent>.GetList(
                ticket, options, qopt.CreateQuery());

            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                foreach (TransitAccountEvent t_instance in result)
                {
                    t_instance.CreateSchedule(session, utcoffset, sec);
                }
            }

            return result;
        }

        #endregion    

        #region AccountEventType

        /// <summary>
        /// Create or update an event type.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventtype">transit event type</param>
        [WebMethod(Description = "Create or update an event type.")]
        public int CreateOrUpdateAccountEventType(string ticket, TransitAccountEventType eventtype)
        {
            return WebServiceImpl<TransitAccountEventType, ManagedAccountEventType, AccountEventType>.CreateOrUpdate(
                ticket, eventtype);
        }

        /// <summary>
        /// Get an event type.
        /// </summary>
        /// <returns>transit event type</returns>
        [WebMethod(Description = "Get an event type.")]
        public TransitAccountEventType GetAccountEventTypeById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEventType, ManagedAccountEventType, AccountEventType>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all event types.
        /// </summary>
        /// <returns>list of transit event types</returns>
        [WebMethod(Description = "Get all event types.")]
        public List<TransitAccountEventType> GetAccountEventTypes(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitAccountEventType, ManagedAccountEventType, AccountEventType>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all event types count.
        /// </summary>
        /// <returns>list of transit event types</returns>
        [WebMethod(Description = "Get all event types count.")]
        public int GetAccountEventTypesCount(string ticket)
        {
            return WebServiceImpl<TransitAccountEventType, ManagedAccountEventType, AccountEventType>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete an event type
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">event type id</param>
        [WebMethod(Description = "Delete an event type.")]
        public void DeleteAccountEventType(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEventType, ManagedAccountEventType, AccountEventType>.Delete(
                ticket, id);
        }

        #endregion

        #region AccountEventPicture

        /// <summary>
        /// Create or update an event picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventpicture">transit event picture</param>
        [WebMethod(Description = "Create or update an event picture.")]
        public int CreateOrUpdateAccountEventPicture(string ticket, TransitAccountEventPicture eventpicture)
        {
            return WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.CreateOrUpdate(
                ticket, eventpicture);
        }

        /// <summary>
        /// Get account event picture by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">event picture id</param>
        /// <returns>transit account event picture</returns>
        [WebMethod(Description = "Get account event picture by id.")]
        public TransitAccountEventPicture GetAccountEventPictureById(string ticket, int id)
        {
            return WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete a event picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventpictureid">event picture id</param>
        [WebMethod(Description = "Delete a event picture.")]
        public void DeleteAccountEventPicture(string ticket, int id)
        {
            WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.Delete(
                ticket, id);
        }

        /// <summary>
        /// Get event pictures count by event id.
        /// </summary>
        [WebMethod(Description = "Get event pictures count.")]
        public int GetAccountEventPicturesCount(string ticket, int id)
        {
            ICriterion[] expressions = { Expression.Eq("AccountEvent.Id", id) };
            return WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.GetCount(
                ticket, expressions);
        }

        /// <summary>
        /// Get event pictures by event id.
        /// </summary>
        /// <param name="eventid">event id</param>
        /// <returns>transit event pictures</returns>
        [WebMethod(Description = "Get event pictures.", CacheDuration = 60)]
        public List<TransitAccountEventPicture> GetAccountEventPictures(string ticket, int id, ServiceQueryOptions options)
        {
            ICriterion[] expressions = { Expression.Eq("AccountEvent.Id", id) };
            Order[] orders = { Order.Asc("Position"), Order.Desc("Created") };
            return WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.GetList(
                ticket, options, expressions, orders);
        }

        /// <summary>
        /// Move an account event picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="disp">displace by positions</param>
        /// <param name="id">picture id</param>
        [WebMethod(Description = "Move an account event picture.")]
        public void MoveAccountEventPicture(string ticket, int id, int disp)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                ManagedAccountEventPicture m_instance = new ManagedAccountEventPicture(session, id);
                m_instance.Move(sec, disp);
            }
        }

        /// <summary>
        /// Get event picture if modified since.
        /// </summary>
        /// <param name="id">event picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get event picture data if modified since.", BufferResponse = true)]
        public TransitAccountEventPicture GetAccountEventPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            TransitAccountEventPicture t_instance = WebServiceImpl<TransitAccountEventPicture, ManagedAccountEventPicture, AccountEventPicture>.GetById(
                ticket, id);

            if (t_instance.Modified <= ifModifiedSince)
                return null;

            return t_instance;
        }

        #endregion

        #region AccountEventInstance

        /// <summary>
        /// Get all account event instances.
        /// </summary>
        /// <returns>list of transit account event instances</returns>
        [WebMethod(Description = "Get all account event instances.", CacheDuration = 60)]
        public List<TransitAccountEventInstance> GetAccountEventInstances(string ticket, TransitAccountEventInstanceQueryOptions qopt, ServiceQueryOptions options)
        {
            IList<ScheduleInstance> instances = WebServiceImpl<TransitScheduleInstance, ManagedScheduleInstance, ScheduleInstance>.GetDataList(
                ticket, options, qopt.CreateQuery());

            List<TransitAccountEventInstance> result = new List<TransitAccountEventInstance>(instances.Count);
            using (SnCore.Data.Hibernate.Session.OpenConnection())
            {
                foreach (ScheduleInstance t_instance in instances)
                {
                    result.Add(new TransitAccountEventInstance(t_instance));
                }
            }

            return result;
        }

        /// <summary>
        /// Get all account event instances count.
        /// </summary>
        /// <returns>number of account event instances</returns>
        [WebMethod(Description = "Get all account event instances count.", CacheDuration = 60)]
        public int GetAccountEventInstancesCount(string ticket, TransitAccountEventInstanceQueryOptions qopt)
        {
            return WebServiceImpl<TransitScheduleInstance, ManagedScheduleInstance, ScheduleInstance>.GetCount(
                ticket, qopt.CreateCountQuery());
        }

        #endregion
    }
}
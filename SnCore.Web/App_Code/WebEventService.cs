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
        /// Get account events.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit account events</returns>
        [WebMethod(Description = "Get account events.")]
        public List<TransitAccountEvent> GetAccountEvents(string ticket)
        {
            return GetAccountEventsById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account events.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account events</returns>
        [WebMethod(Description = "Get account events.", CacheDuration = 60)]
        public List<TransitAccountEvent> GetAccountEventsById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList list = session.CreateCriteria(typeof(AccountEvent))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"))
                    .List();

                ManagedAccount user = new ManagedAccount(session, id);

                List<TransitAccountEvent> result = new List<TransitAccountEvent>(list.Count);
                foreach (AccountEvent e in list)
                {
                    TransitAccountEvent evt = new TransitAccountEvent(e);
                    ManagedSchedule schedule = new ManagedSchedule(session, evt.ScheduleId);
                    evt.Schedule = schedule.ToString(user.TransitAccount.UtcOffset);
                    result.Add(evt);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get account event by id.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">event id</param>
        /// <returns>transit account event</returns>
        [WebMethod(Description = "Get account event by id.")]
        public TransitAccountEvent GetAccountEventById(string ticket, int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for Event
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountEvent(session, id).TransitAccountEvent;
            }
        }

        /// <summary>
        /// Create or update an event.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="event">new event</param>
        [WebMethod(Description = "Create an event.")]
        public int CreateOrUpdateAccountEvent(string ticket, TransitAccountEvent ev)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount a = new ManagedAccount(session, id);
                int result = a.CreateOrUpdate(ev);
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an event.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventid">event id</param>
        [WebMethod(Description = "Delete an event.")]
        public void DeleteAccountEvent(string ticket, int eventid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEvent s = new ManagedAccountEvent(session, eventid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.Account.Id && ! acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedAccountEventType m_eventtype = new ManagedAccountEventType(session);
                m_eventtype.CreateOrUpdate(eventtype);
                SnCore.Data.Hibernate.Session.Flush();
                return m_eventtype.Id;
            }
        }

        /// <summary>
        /// Get an event type.
        /// </summary>
        /// <returns>transit event type</returns>
        [WebMethod(Description = "Get an event type.")]
        public TransitAccountEventType GetAccountEventTypeById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountEventType result = new ManagedAccountEventType(session, id).TransitAccountEventType;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }


        /// <summary>
        /// Get all event types.
        /// </summary>
        /// <returns>list of transit event types</returns>
        [WebMethod(Description = "Get all event types.")]
        public List<TransitAccountEventType> GetAccountEventTypes()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList eventtypes = session.CreateCriteria(typeof(AccountEventType)).List();
                List<TransitAccountEventType> result = new List<TransitAccountEventType>(eventtypes.Count);
                foreach (AccountEventType eventtype in eventtypes)
                {
                    result.Add(new ManagedAccountEventType(session, eventtype).TransitAccountEventType);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete an event type
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete an event type.")]
        public void DeleteAccountEventType(string ticket, int id)
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

                ManagedAccountEventType m_eventtype = new ManagedAccountEventType(session, id);
                m_eventtype.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion

    }
}
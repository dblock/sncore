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
        public List<TransitAccountEvent> GetAccountEvents(string ticket, ServiceQueryOptions options, int utcoffset)
        {
            return GetAccountEventsById(ManagedAccount.GetAccountId(ticket), utcoffset, options);
        }

        /// <summary>
        /// Get account events count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>number of account events</returns>
        [WebMethod(Description = "Get account events count.")]
        public int GetAccountEventsCount(string ticket)
        {
            return GetAccountEventsCountById(ManagedAccount.GetAccountId(ticket));
        }

        /// <summary>
        /// Get account avents count by account id.
        /// </summary>
        [WebMethod(Description = "Get account events count by account id.")]
        public int GetAccountEventsCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM AccountEvent s WHERE s.Account.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get account events.
        /// </summary>
        /// <param name="id">account id</param>
        /// <returns>transit account events</returns>
        [WebMethod(Description = "Get account events.", CacheDuration = 60)]
        public List<TransitAccountEvent> GetAccountEventsById(int id, int utcoffset, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountEvent))
                    .Add(Expression.Eq("Account.Id", id))
                    .AddOrder(Order.Desc("Created"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();

                List<TransitAccountEvent> result = new List<TransitAccountEvent>(list.Count);
                foreach (AccountEvent e in list)
                {
                    TransitAccountEvent evt = new TransitAccountEvent(e);
                    evt.CreateSchedule(session, utcoffset);
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
        public TransitAccountEvent GetAccountEventById(string ticket, int id, int user_utcoffset)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for Event
                ISession session = SnCore.Data.Hibernate.Session.Current;
                int user_id = ManagedAccount.GetAccountId(ticket, 0);
                TransitAccountEvent tav = new ManagedAccountEvent(session, id).TransitAccountEvent;
                tav.CreateSchedule(session, user_utcoffset);
                return tav;
            }
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: permissions for Event
                ISession session = SnCore.Data.Hibernate.Session.Current;
                int user_id = ManagedAccount.GetAccountId(ticket, 0);
                ManagedAccountEvent mav = new ManagedAccountEvent(session, id);
                return mav.ToVCalendarString();
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

                if (!a.HasVerifiedEmail)
                    throw new ManagedAccount.NoVerifiedEmailException();

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

        #region Event Pictures

        /// <summary>
        /// Create or update an event picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventpicture">transit event picture</param>
        [WebMethod(Description = "Create or update an event picture.")]
        public int CreateOrUpdateAccountEventPicture(string ticket, TransitAccountEventPicture eventpicture)
        {
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if ((eventpicture.Id != 0) && (!user.IsAdministrator()))
                {
                    // deny unless event owner
                    ManagedAccountEvent m_event = new ManagedAccountEvent(session, eventpicture.AccountEventId);
                    if (m_event.Account.Id != user.Id)
                    {
                        throw new ManagedAccount.AccessDeniedException();
                    }
                }

                ManagedAccountEventPicture m_eventpicture = new ManagedAccountEventPicture(session);
                m_eventpicture.CreateOrUpdate(eventpicture);
                SnCore.Data.Hibernate.Session.Flush();
                return m_eventpicture.Id;
            }
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
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                // todo: persmissions for event picture
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountEventPicture(session, id).TransitAccountEventPicture;
            }
        }

        /// <summary>
        /// Delete a event picture.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="eventpictureid">event picture id</param>
        [WebMethod(Description = "Delete a event picture.")]
        public void DeleteAccountEventPicture(string ticket, int eventpictureid)
        {
            int id = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccountEventPicture s = new ManagedAccountEventPicture(session, eventpictureid);
                ManagedAccount acct = new ManagedAccount(session, id);
                if (acct.Id != s.AccountId && !acct.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }
                s.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        /// <summary>
        /// Get event pictures count by event id.
        /// </summary>
        [WebMethod(Description = "Get event pictures count.")]
        public int GetAccountEventPicturesCountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) session.CreateQuery(string.Format(
                    "SELECT COUNT(*) FROM AccountEventPicture p WHERE p.AccountEvent.Id = {0}",
                    id)).UniqueResult();
            }
        }

        /// <summary>
        /// Get event pictures by event id.
        /// </summary>
        /// <param name="eventid">event id</param>
        /// <returns>transit event pictures</returns>
        [WebMethod(Description = "Get event pictures.", CacheDuration = 60)]
        public List<TransitAccountEventPicture> GetAccountEventPicturesById(int eventid, ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(AccountEventPicture))
                    .Add(Expression.Eq("AccountEvent.Id", eventid))
                    .AddOrder(Order.Desc("Created"))
                    .AddOrder(Order.Desc("Id"));

                if (options != null)
                {
                    c.SetFirstResult(options.FirstResult);
                    c.SetMaxResults(options.PageSize);
                }

                IList list = c.List();
                List<TransitAccountEventPicture> result = new List<TransitAccountEventPicture>(list.Count);
                foreach (AccountEventPicture p in list)
                {
                    result.Add(new ManagedAccountEventPicture(session, p).TransitAccountEventPicture);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get event picture picture data.
        /// </summary>
        /// <param name="id">event picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get event picture picture data.", BufferResponse = true)]
        public TransitAccountEventPictureWithPicture GetAccountEventPictureWithPictureById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountEventPicture(session, id).TransitAccountEventPictureWithPicture;
            }
        }

        /// <summary>
        /// Get event picture picture data if modified since.
        /// </summary>
        /// <param name="id">event picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit picture</returns>
        [WebMethod(Description = "Get event picture picture data if modified since.", BufferResponse = true)]
        public TransitAccountEventPictureWithPicture GetAccountEventPictureWithPictureIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountEventPictureWithPicture p = new ManagedAccountEventPicture(session, id).TransitAccountEventPictureWithPicture;

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p;
            }
        }

        /// <summary>
        /// Get event picture thumbnail data.
        /// </summary>
        /// <param name="id">event picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit event picture with thumbnail</returns>
        [WebMethod(Description = "Get event picture Thumbnail data.", BufferResponse = true)]
        public TransitAccountEventPictureWithThumbnail GetAccountEventPictureWithThumbnailById(string ticket, int id)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new ManagedAccountEventPicture(session, id).TransitAccountEventPictureWithThumbnail;
            }
        }

        /// <summary>
        /// Get event picture thumbnail data if modified since.
        /// </summary>
        /// <param name="id">event picture id</param>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="ifModifiedSince">last update date/time</param>
        /// <returns>transit event picture with thumbnail</returns>
        [WebMethod(Description = "Get event picture thumbnail data if modified since.", BufferResponse = true)]
        public TransitAccountEventPictureWithThumbnail GetAccountEventPictureWithThumbnailIfModifiedSinceById(string ticket, int id, DateTime ifModifiedSince)
        {
            // todo: check permissions with ticket
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitAccountEventPictureWithThumbnail p = new ManagedAccountEventPicture(session, id).TransitAccountEventPictureWithThumbnail;

                if (p.Modified <= ifModifiedSince)
                {
                    return null;
                }

                return p;
            }
        }

        #endregion

        #region All Account Events
        /// <summary>
        /// Get account event count.
        /// </summary>
        /// <returns>transit account envets count</returns>
        [WebMethod(Description = "Get all account events count.", CacheDuration = 60)]
        public int GetAllAccountEventsCount(TransitAccountEventQueryOptions queryoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) queryoptions.CreateCountQuery(session).UniqueResult();
            }
        }

        /// <summary>
        /// Get all account events.
        /// </summary>
        /// <returns>list of transit account events</returns>
        [WebMethod(Description = "Get all account events.", CacheDuration = 60)]
        public List<TransitAccountEvent> GetAllAccountEvents(
            string ticket,
            int user_utcoffset,
            TransitAccountEventQueryOptions queryoptions, 
            ServiceQueryOptions serviceoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                int user_id = ManagedAccount.GetAccountId(ticket, 0);

                IQuery q = queryoptions.CreateQuery(session);

                if (serviceoptions != null)
                {
                    q.SetMaxResults(serviceoptions.PageSize);
                    q.SetFirstResult(serviceoptions.PageNumber * serviceoptions.PageSize);
                }

                IList list = q.List();
                List<TransitAccountEvent> result = new List<TransitAccountEvent>(list.Count);

                foreach (AccountEvent p in list)
                {
                    ManagedAccountEvent mav = new ManagedAccountEvent(session, p);
                    TransitAccountEvent tav = mav.TransitAccountEvent;
                    tav.CreateSchedule(session, user_utcoffset);
                    result.Add(tav);
                }

                return result;
            }
        }

        #endregion

        /// <summary>
        /// Get all account event instances.
        /// </summary>
        /// <returns>list of transit account event instances</returns>
        [WebMethod(Description = "Get all account event instances.", CacheDuration = 60)]
        public List<TransitAccountEventInstance> GetAccountEventInstances(
            string ticket,
            TransitAccountEventInstanceQueryOptions queryoptions,
            ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IQuery q = queryoptions.CreateQuery(session);

                if (options != null)
                {
                    q.SetFirstResult(options.FirstResult);
                    q.SetMaxResults(options.PageSize);
                }

                IList list = q.List();
                List<TransitAccountEventInstance> result = new List<TransitAccountEventInstance>(list.Count);

                foreach (ScheduleInstance si in list)
                {
                    result.Add(new TransitAccountEventInstance(si));
                }

                return result;
            }
        }

        /// <summary>
        /// Get all account event instances count.
        /// </summary>
        /// <returns>number of account event instances</returns>
        [WebMethod(Description = "Get all account event instances count.", CacheDuration = 60)]
        public int GetAccountEventInstancesCount(
            string ticket,
            TransitAccountEventInstanceQueryOptions queryoptions)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int) queryoptions.CreateCountQuery(session).UniqueResult();
            }
        } 
    }
}
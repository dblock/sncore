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
using System.Web.Services.Protocols;

namespace SnCore.WebServices
{
    /// <summary>
    /// Managed web stats and tracking services.
    /// </summary>
    [WebService(Namespace = "http://www.vestris.com/sncore/ns/", Name = "WebStatsService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebStatsService : WebService
    {
        public WebStatsService()
        {

        }

        #region Track Requests

        /// <summary>
        /// Track a single request.
        /// </summary>
        /// <param name="request">client request</param>
        [WebMethod(Description = "Track a request.")]
        public void TrackSingleRequest(TransitStatsRequest request)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedStats stats = new ManagedStats(session);
                try
                {
                    stats.Track(request);
                    SnCore.Data.Hibernate.Session.Flush();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(string.Format("Error tracking {0} from {1}.\n{2}",
                        (request.RequestUri != null) ? request.RequestUri.ToString() : "an unknown url",
                        (request.RefererUri != null) ? request.RefererUri.ToString() : "an unknown referer",
                        ex.Message),
                        EventLogEntryType.Warning);
                }
            }
        }

        /// <summary>
        /// Track multiple requests.
        /// </summary>
        /// <param name="requests">client requests</param>
        [WebMethod(Description = "Track multiple requests.")]
        public void TrackMultipleRequests(TransitStatsRequest[] requests)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                ManagedStats stats = new ManagedStats(session);
                foreach (TransitStatsRequest request in requests)
                {
                    try
                    {
                        stats.Track(request);
                        SnCore.Data.Hibernate.Session.Flush();
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(string.Format("Error tracking {0} from {1}.\n{2}",
                            (request.RequestUri != null) ? request.RequestUri.ToString() : "an unknown url",
                            (request.RefererUri != null) ? request.RefererUri.ToString() : "an unknown referer",
                            ex.Message),
                            EventLogEntryType.Warning);
                    }
                }
            }
        }

        #endregion

        #region Stats

        /// <summary>
        /// Get stats summary.
        /// </summary>
        [WebMethod(Description = "Get stats summary.")]
        public TransitStatsSummary GetSummary()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedStats stats = new ManagedStats(session);
                return stats.GetSummary();
            }
        }

        #endregion

        #region Referer Hosts

        /// <summary>
        /// Get referer hosts count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit referer hosts</returns>
        [WebMethod(Description = "Get referer hosts count.", CacheDuration = 60)]
        public int GetRefererHostsCount(string ticket)
        {
            return WebServiceImpl<TransitRefererHost, ManagedRefererHost, RefererHost>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all referer hosts.
        /// </summary>
        /// <returns>transit referer hosts</returns>
        [WebMethod(Description = "Get referer hosts.", CacheDuration = 60)]
        public List<TransitRefererHost> GetRefererHosts(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Desc("Total") };
            return WebServiceImpl<TransitRefererHost, ManagedRefererHost, RefererHost>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Create or update a referer host.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit referer host</param>
        [WebMethod(Description = "Create or update a referer host.")]
        public int CreateOrUpdateRefererHost(string ticket, TransitRefererHost refererhost)
        {
            return WebServiceImpl<TransitRefererHost, ManagedRefererHost, RefererHost>.CreateOrUpdate(
                ticket, refererhost);
        }

        /// <summary>
        /// Get a referer host.
        /// </summary>
        /// <returns>transit referer host</returns>
        [WebMethod(Description = "Get a referer host.")]
        public TransitRefererHost GetRefererHostById(string ticket, int id)
        {
            return WebServiceImpl<TransitRefererHost, ManagedRefererHost, RefererHost>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete a referer host.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer host.")]
        public void DeleteRefererHost(string ticket, int id)
        {
            WebServiceImpl<TransitRefererHost, ManagedRefererHost, RefererHost>.Delete(
                ticket, id);
        }

        #endregion

        #region Referer Queries

        /// <summary>
        /// Get referer queries count.
        /// </summary>
        /// <returns>transit referer queries count</returns>
        [WebMethod(Description = "Get referer queries count.", CacheDuration = 60)]
        public int GetRefererQueriesCount(string ticket)
        {
            return WebServiceImpl<TransitRefererQuery, ManagedRefererQuery, RefererQuery>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get referer queries.
        /// </summary>
        /// <returns>transit referer queries</returns>
        [WebMethod(Description = "Get referer queries.", CacheDuration = 60)]
        public List<TransitRefererQuery> GetRefererQueries(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Desc("Total") };
            return WebServiceImpl<TransitRefererQuery, ManagedRefererQuery, RefererQuery>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Create or update a referer query.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit referer query</param>
        [WebMethod(Description = "Create or update a referer query.")]
        public int CreateOrUpdateRefererQuery(string ticket, TransitRefererQuery refererquery)
        {
            return WebServiceImpl<TransitRefererQuery, ManagedRefererQuery, RefererQuery>.CreateOrUpdate(
                ticket, refererquery);
        }

        /// <summary>
        /// Get a referer query.
        /// </summary>
        /// <returns>transit referer query</returns>
        [WebMethod(Description = "Get a referer query.")]
        public TransitRefererQuery GetRefererQueryById(string ticket, int id)
        {
            return WebServiceImpl<TransitRefererQuery, ManagedRefererQuery, RefererQuery>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete a referer query.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer query.")]
        public void DeleteRefererQuery(string ticket, int id)
        {
            WebServiceImpl<TransitRefererQuery, ManagedRefererQuery, RefererQuery>.Delete(
                ticket, id);
        }

        #endregion

        #region Counters

        /// <summary>
        /// Get counter for an url.
        /// </summary>
        /// <returns>transit referer queries</returns>
        [WebMethod(Description = "Get counter for an url.", CacheDuration = 60)]
        public TransitCounter GetCounterByUri(string ticket, string uri)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedSecurityContext sec = new ManagedSecurityContext(session, ticket);
                return ManagedCounter.FindByUri(session, uri, sec);
            }
        }

        /// <summary>
        /// Get counters count.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <returns>transit counters</returns>
        [WebMethod(Description = "Get counters count.", CacheDuration = 60)]
        public int GetCountersCount(string ticket)
        {
            return WebServiceImpl<TransitCounter, ManagedCounter, Counter>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all counters.
        /// </summary>
        /// <returns>transit counters</returns>
        [WebMethod(Description = "Get counters.", CacheDuration = 60)]
        public List<TransitCounter> GetCounters(string ticket, ServiceQueryOptions options)
        {
            Order[] orders = { Order.Desc("Total") };
            return WebServiceImpl<TransitCounter, ManagedCounter, Counter>.GetList(
                ticket, options, null, orders);
        }

        /// <summary>
        /// Create or update a counter.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="type">transit counter</param>
        [WebMethod(Description = "Create or update a counter.")]
        public int CreateOrUpdateCounter(string ticket, TransitCounter counter)
        {
            return WebServiceImpl<TransitCounter, ManagedCounter, Counter>.CreateOrUpdate(
                ticket, counter);
        }

        /// <summary>
        /// Get a counter.
        /// </summary>
        /// <returns>transit counter</returns>
        [WebMethod(Description = "Get a counter.")]
        public TransitCounter GetCounterById(string ticket, int id)
        {
            return WebServiceImpl<TransitCounter, ManagedCounter, Counter>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Delete a counter.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a counter.")]
        public void DeleteCounter(string ticket, int id)
        {
            WebServiceImpl<TransitCounter, ManagedCounter, Counter>.Delete(
                ticket, id);
        }

        #endregion

        #region Referer Host Dups

        /// <summary>
        /// Create or update a referer host dup.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="refererhostdup">transit referer host dup</param>
        [WebMethod(Description = "Create or update a referer host dup.")]
        public int CreateOrUpdateRefererHostDup(string ticket, TransitRefererHostDup refererhostdup)
        {
            return WebServiceImpl<TransitRefererHostDup, ManagedRefererHostDup, RefererHostDup>.CreateOrUpdate(
                ticket, refererhostdup);
        }

        /// <summary>
        /// Get a referer host dup.
        /// </summary>
        /// <returns>transit referer host dup</returns>
        [WebMethod(Description = "Get a referer host dup.")]
        public TransitRefererHostDup GetRefererHostDupById(string ticket, int id)
        {
            return WebServiceImpl<TransitRefererHostDup, ManagedRefererHostDup, RefererHostDup>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get all referer host dups.
        /// </summary>
        /// <returns>list of transit referer host dups</returns>
        [WebMethod(Description = "Get all referer host dups.", CacheDuration = 60)]
        public List<TransitRefererHostDup> GetRefererHostDups(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitRefererHostDup, ManagedRefererHostDup, RefererHostDup>.GetList(
                ticket, options);
        }

        /// <summary>
        /// Get all referer host dups count.
        /// </summary>
        /// <returns>number of transit referer host dups</returns>
        [WebMethod(Description = "Get referer host dups count.", CacheDuration = 60)]
        public int GetRefererHostDupsCount(string ticket)
        {
            return WebServiceImpl<TransitRefererHostDup, ManagedRefererHostDup, RefererHostDup>.GetCount(
                ticket);
        }

        /// <summary>
        /// Delete a referer host dup.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer host dup.")]
        public void DeleteRefererHostDup(string ticket, int id)
        {
            WebServiceImpl<TransitRefererHostDup, ManagedRefererHostDup, RefererHostDup>.Delete(
                ticket, id);
        }

        #endregion

        #region Referer Accounts

        /// <summary>
        /// Create or update a referer account.
        /// </summary>
        /// <param name="ticket">authentication ticket</param>
        /// <param name="refererhostaccount">transit referer account</param>
        [WebMethod(Description = "Create or update a referer account.")]
        public int CreateOrUpdateRefererAccount(string ticket, TransitRefererAccount refererhostaccount)
        {
            return WebServiceImpl<TransitRefererAccount, ManagedRefererAccount, RefererAccount>.CreateOrUpdate(
                ticket, refererhostaccount);
        }

        /// <summary>
        /// Get a referer account.
        /// </summary>
        /// <returns>transit referer account</returns>
        [WebMethod(Description = "Get a referer account.")]
        public TransitRefererAccount GetRefererAccountById(string ticket, int id)
        {
            return WebServiceImpl<TransitRefererAccount, ManagedRefererAccount, RefererAccount>.GetById(
                ticket, id);
        }

        /// <summary>
        /// Get referer accounts count.
        /// </summary>
        /// <returns>number of referer accounts</returns>
        [WebMethod(Description = "Get referer accounts count.")]
        public int GetRefererAccountsCount(string ticket)
        {
            return WebServiceImpl<TransitRefererAccount, ManagedRefererAccount, RefererAccount>.GetCount(
                ticket);
        }

        /// <summary>
        /// Get all referer accounts.
        /// </summary>
        /// <returns>list of transit referer accounts</returns>
        [WebMethod(Description = "Get all referer accounts.", CacheDuration = 60)]
        public List<TransitRefererAccount> GetRefererAccounts(string ticket, ServiceQueryOptions options)
        {
            return WebServiceImpl<TransitRefererAccount, ManagedRefererAccount, RefererAccount>.GetList(
                ticket, options, "SELECT {ra.*} FROM RefererAccount {ra}, RefererHost rh" +
                    " WHERE ra.RefererHost_Id = rh.RefererHost_Id" +
                    " ORDER BY rh.Total DESC", "ra");
        }

        /// <summary>
        /// Delete a referer account.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer account.")]
        public void DeleteRefererAccount(string ticket, int id)
        {
            WebServiceImpl<TransitRefererAccount, ManagedRefererAccount, RefererAccount>.Delete(
                ticket, id);
        }

        #endregion
    }
}

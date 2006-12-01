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

        /// <summary>
        /// Get referer hosts count.
        /// </summary>
        /// <returns>transit referer hosts</returns>
        [WebMethod(Description = "Get referer hosts count.", CacheDuration = 60)]
        public int GetRefererHostsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(rh) from RefererHost rh").UniqueResult();
            }
        }

        /// <summary>
        /// Get referer hosts.
        /// </summary>
        /// <returns>transit referer hosts</returns>
        [WebMethod(Description = "Get referer hosts.", CacheDuration = 60)]
        public List<TransitRefererHost> GetRefererHosts(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(RefererHost))
                    .AddOrder(Order.Desc("Total"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList refererhosts = c.List();
                List<TransitRefererHost> result = new List<TransitRefererHost>(refererhosts.Count);
                foreach (RefererHost h in refererhosts)
                {
                    result.Add(new ManagedRefererHost(session, h).TransitRefererHost);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get referer queries count.
        /// </summary>
        /// <returns>transit referer queries count</returns>
        [WebMethod(Description = "Get referer queries count.", CacheDuration = 60)]
        public int GetRefererQueriesCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(rq) from RefererQuery rq").UniqueResult();
            }
        }

        /// <summary>
        /// Get referer queries.
        /// </summary>
        /// <returns>transit referer queries</returns>
        [WebMethod(Description = "Get referer queries.", CacheDuration = 60)]
        public List<TransitRefererQuery> GetRefererQueries(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ICriteria c = session.CreateCriteria(typeof(RefererQuery))
                    .AddOrder(Order.Desc("Total"));

                if (options != null)
                {
                    c.SetMaxResults(options.PageSize);
                    c.SetFirstResult(options.FirstResult);
                }

                IList refererqueries = c.List();
                List<TransitRefererQuery> result = new List<TransitRefererQuery>(refererqueries.Count);
                foreach (RefererQuery q in refererqueries)
                {
                    result.Add(new ManagedRefererQuery(session, q).TransitRefererQuery);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        #endregion

        #region Counters

        /// <summary>
        /// Get counter for an url.
        /// </summary>
        /// <returns>transit referer queries</returns>
        [WebMethod(Description = "Get counter for an url.", CacheDuration = 60)]
        public TransitCounter GetCounterByUri(string uri)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return new TransitCounter((Counter)session.CreateCriteria(typeof(Counter))
                        .Add(Expression.Eq("Uri", uri))
                        .UniqueResult());
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedRefererHostDup m_refererhostdup = new ManagedRefererHostDup(session);
                m_refererhostdup.CreateOrUpdate(refererhostdup);
                SnCore.Data.Hibernate.Session.Flush();
                return m_refererhostdup.Id;
            }
        }

        /// <summary>
        /// Get a referer host dup.
        /// </summary>
        /// <returns>transit referer host dup</returns>
        [WebMethod(Description = "Get a referer host dup.")]
        public TransitRefererHostDup GetRefererHostDupById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitRefererHostDup result = new ManagedRefererHostDup(session, id).TransitRefererHostDup;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get all referer host dups.
        /// </summary>
        /// <returns>list of transit referer host dups</returns>
        [WebMethod(Description = "Get all referer host dups.", CacheDuration = 60)]
        public List<TransitRefererHostDup> GetRefererHostDups()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                IList refererhostdup = session.CreateCriteria(typeof(RefererHostDup)).List();
                List<TransitRefererHostDup> result = new List<TransitRefererHostDup>(refererhostdup.Count);
                foreach (RefererHostDup rhd in refererhostdup)
                {
                    result.Add(new ManagedRefererHostDup(session, rhd).TransitRefererHostDup);
                }
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a referer host dup.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer host dup.")]
        public void DeleteRefererHostDup(string ticket, int id)
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

                ManagedRefererHostDup m_refererhostdup = new ManagedRefererHostDup(session, id);
                m_refererhostdup.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
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
            int userid = ManagedAccount.GetAccountId(ticket);
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                ManagedAccount user = new ManagedAccount(session, userid);

                if (!user.IsAdministrator())
                {
                    throw new ManagedAccount.AccessDeniedException();
                }

                ManagedRefererAccount m_refererhostaccount = new ManagedRefererAccount(session);
                m_refererhostaccount.CreateOrUpdate(refererhostaccount);
                SnCore.Data.Hibernate.Session.Flush();
                return m_refererhostaccount.Id;
            }
        }

        /// <summary>
        /// Get a referer account.
        /// </summary>
        /// <returns>transit referer account</returns>
        [WebMethod(Description = "Get a referer account.")]
        public TransitRefererAccount GetRefererAccountById(int id)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                TransitRefererAccount result = new ManagedRefererAccount(session, id).TransitRefererAccount;
                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Get referer account count.
        /// </summary>
        /// <returns>number of referer accounts</returns>
        [WebMethod(Description = "Get referer accounts count.")]
        public int GetRefererAccountsCount()
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;
                return (int)session.CreateQuery("SELECT COUNT(ra) from RefererAccount ra").UniqueResult();
            }
        }

        /// <summary>
        /// Get all referer accounts.
        /// </summary>
        /// <returns>list of transit referer accounts</returns>
        [WebMethod(Description = "Get all referer accounts.", CacheDuration = 60)]
        public List<TransitRefererAccount> GetRefererAccounts(ServiceQueryOptions options)
        {
            using (SnCore.Data.Hibernate.Session.OpenConnection(GetNewConnection()))
            {
                ISession session = SnCore.Data.Hibernate.Session.Current;

                IQuery query = session.CreateSQLQuery(
                    "SELECT {ra.*} FROM RefererAccount {ra}, RefererHost rh" +
                    " WHERE ra.RefererHost_Id = rh.RefererHost_Id" +
                    " ORDER BY rh.Total DESC",
                    "ra",
                    typeof(RefererAccount));

                if (options != null)
                {
                    query.SetMaxResults(options.PageSize);
                    query.SetFirstResult(options.FirstResult);
                }

                IList refererhostaccount = query.List();
                List<TransitRefererAccount> result = new List<TransitRefererAccount>(refererhostaccount.Count);
                foreach (RefererAccount rhd in refererhostaccount)
                {
                    result.Add(new ManagedRefererAccount(session, rhd).TransitRefererAccount);
                }

                SnCore.Data.Hibernate.Session.Flush();
                return result;
            }
        }

        /// <summary>
        /// Delete a referer account.
        /// <param name="ticket">authentication ticket</param>
        /// <param name="id">id</param>
        /// </summary>
        [WebMethod(Description = "Delete a referer account.")]
        public void DeleteRefererAccount(string ticket, int id)
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

                ManagedRefererAccount m_refererhostaccount = new ManagedRefererAccount(session, id);
                m_refererhostaccount.Delete();
                SnCore.Data.Hibernate.Session.Flush();
            }
        }

        #endregion
    }
}

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
                }
                catch (Exception ex)
                {
                    if (request.SinkException)
                    {
                        EventLog.WriteEntry(string.Format("Error tracking {0} from {1}.\n{2}",
                            (request.RequestUri != null) ? request.RequestUri.ToString() : "an unknown url",
                            (request.ReferrerUri != null) ? request.ReferrerUri.ToString() : "an unknown referrer",
                            ex.Message), 
                            EventLogEntryType.Warning);
                    }
                    else
                    {
                        throw;
                    }
                }
                SnCore.Data.Hibernate.Session.Flush();
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
                    }
                    catch (Exception ex)
                    {
                        if (request.SinkException)
                        {
                            EventLog.WriteEntry(string.Format("Error tracking {0} from {1}.\n{2}",
                                (request.RequestUri != null) ? request.RequestUri.ToString() : "an unknown url", 
                                (request.ReferrerUri != null) ? request.ReferrerUri.ToString() : "an unknown referrer",
                                ex.Message),
                                EventLogEntryType.Warning);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                SnCore.Data.Hibernate.Session.Flush();
            }
        }

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
    }
}

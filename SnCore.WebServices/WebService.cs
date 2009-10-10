using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.Web.Hosting;
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    public class WebService : System.Web.Services.WebService
    {
        private EventLog mEventLogManager = null;

        public WebService()
        {

        }

        public EventLog EventLogManager
        {
            get
            {
                if (mEventLogManager == null)
                {
                    mEventLogManager = HostedApplication.CreateEventLog();
                }
                return mEventLogManager;
            }
        }
    }

    public abstract class WebServiceQueryOptions<T>
    {
        public static List<T> Apply(ServiceQueryOptions options, IList<T> collection)
        {
            if (options == null)
            {
                List<T> result = new List<T>();
                result.AddRange(collection);
                return result;
            }

            return SnCore.Data.Hibernate.Collection<T>.ApplyServiceOptions(
                options.FirstResult, options.PageSize, collection);
        }
    }
}

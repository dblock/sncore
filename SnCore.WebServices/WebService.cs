using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Messaging;
using Microsoft.Web.Services3.Design;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.Web.Hosting;
using SnCore.Tools.Web;

namespace SnCore.WebServices
{
    public class WebService : SoapService
    {
        private EventLog mEventLog = null;

        public WebService()
        {

        }

        public EventLog EventLog
        {
            get
            {
                if (mEventLog == null)
                {
                    mEventLog = HostedApplication.CreateEventLog();
                }
                return mEventLog;
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

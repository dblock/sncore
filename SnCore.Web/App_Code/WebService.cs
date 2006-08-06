using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Messaging;
using Microsoft.Web.Services3.Design;
using SnCore.Data.Hibernate;
using System.Web.Services.Protocols;
using System.Diagnostics;
using System.Web.Hosting;

namespace SnCore.WebServices
{
    public class ServiceQueryOptions
    {
        public int PageSize = -1;
        public int PageNumber = 0;

        public int FirstResult
        {
            get
            {
                return PageSize * PageNumber;
            }
        }

        public ServiceQueryOptions()
        {
        }

        public ServiceQueryOptions(int pagesize, int pagenumber)
        {
            PageSize = pagesize;
            PageNumber = pagenumber;
        }

        public override int GetHashCode()
        {
            string hash = string.Format("{0}:{1}", PageSize, PageNumber);
            return hash.GetHashCode();
        }
    };

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
                    string eventLogName = HostingEnvironment.ApplicationVirtualPath.Trim("/".ToCharArray());
                    if (eventLogName.Length == 0) eventLogName = HostingEnvironment.SiteName;
                    if (eventLogName.Length == 0) eventLogName = "Application";

                    if (!EventLog.SourceExists(eventLogName))
                    {
                        EventLog.CreateEventSource(eventLogName, "Application");
                    }

                    mEventLog = new EventLog();
                    mEventLog.Source = eventLogName;
                }
                return mEventLog;
            }
        }

        public static IDbConnection GetNewConnection()
        {
            return new SqlConnection(
             SnCore.Data.Hibernate.Session.Configuration.GetProperty(
              "hibernate.connection.connection_string"));
        }
    }
}

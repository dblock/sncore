using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using System.Web.Hosting;

namespace SnCore.Tools.Web
{
    public class HostedApplication : System.Web.HttpApplication
    {
        private static DateTime mStarted;
        private EventLog mEventLog = null;

        static HostedApplication()
        {
            mStarted = DateTime.UtcNow;
        }

        public static DateTime Started
        {
            get
            {
                return mStarted;
            }
        }

        public HostedApplication()
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

        protected virtual void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();

            EventLog.WriteEntry(String.Format("Application error in {0}.\n{1}\n\n{2}",
                Request.Url.ToString(),
                ex.Message,
                ex.StackTrace), 
                EventLogEntryType.Error);
        }

        protected virtual void Application_Start(Object sender, EventArgs e)
        {
            EventLog.WriteEntry("Application starting.");
        }

        protected virtual void Application_End(Object sender, EventArgs e)
        {
            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember(
                "_theRuntime",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                null, null, null);

            if (runtime == null)
                return;

            string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);

            string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);

            EventLog.WriteEntry(String.Format("{0}\n{1}\n\n{2}",
                System.Web.Hosting.HostingEnvironment.ShutdownReason,
                shutDownMessage,
                shutDownStack),
                EventLogEntryType.Warning);
        }

    }
}

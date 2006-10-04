using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using SnCore.Data.Hibernate;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Web.Hosting;

namespace SnCore.BackEndServices
{
    public abstract class SystemService
    {
        public delegate void SessionJobDelegate(ISession session);
        private int mInterruptInterval = 1;
        private int mSleepInterval = 60;
        private Thread mThread = null;
        private bool mIsStopping = false;
        private List<SessionJobDelegate> mJobs = new List<SessionJobDelegate>();
        private static EventLog mEventLog = null;

        public SystemService()
        {

        }

        public int InterruptInterval
        {
            get
            {
                return mInterruptInterval;
            }
            set
            {
                mInterruptInterval = value;
            }
        }

        public int SleepInterval
        {
            get
            {
                return mSleepInterval;
            }
            set
            {
                mSleepInterval = value;
            }
        }

        public bool IsStopping
        {
            get
            {
                return mIsStopping;
            }
        }

        public static IDbConnection GetNewConnection()
        {
            return new SqlConnection(
                SnCore.Data.Hibernate.Session.Configuration.GetProperty(
                    "hibernate.connection.connection_string"));
        }

        public static bool SystemServicesEnabled
        {
            get
            {
                object enabled = ConfigurationManager.AppSettings["SystemServices.Enabled"];
                return (enabled == null) || bool.Parse(enabled.ToString());
            }
        }

        public void Start()
        {
            mThread = new Thread(ThreadProc);
            mThread.Start(this);
        }

        protected void AddJob(SessionJobDelegate job)
        {
            mJobs.Add(job);
        }

        public void Stop()
        {
            mIsStopping = true;

            if (mThread != null)
            {
                if (mThread.IsAlive)
                {
                    Thread.Sleep(500);
                    mThread.Abort();
                }
                mThread.Join();
            }            
        }

        public static void ThreadProc(object service)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            SystemService s = (SystemService) service;
            Thread.Sleep(1000 * 30); // let the system come up
            s.SetUp();
            s.Run();
        }

        private static object _locker = new object();
        private static ISessionFactory mFactory = null;

        protected static ISessionFactory Factory
        {
            get
            {
                if (mFactory == null)
                {
                    NHibernate.Cfg.Configuration config = Session.Configuration;
                    lock (_locker)
                    {
                        if (mFactory == null)
                        {
                            mFactory = config.BuildSessionFactory();
                        }
                    }
                }
                return mFactory;
            }
        }

        private void RunJob(SessionJobDelegate job)
        {
            IDbConnection conn = GetNewConnection();
            conn.Open();

            ISession session = Factory.OpenSession(conn);

            try
            {
                job(session);
                session.Flush();
            }
            finally
            {
                conn.Close();
                session.Close();
            }
        }

        public void Run()
        {
            EventLog.WriteEntry(string.Format("Running {0} with {1} job(s).", this.GetType().Name, mJobs.Count),
                EventLogEntryType.Information);

            while (!IsStopping)
            {
                foreach (SessionJobDelegate job in mJobs)
                {
                    try
                    {
                        RunJob(job);
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(string.Format("{0}: Error running job.\n{1}", this.GetType().Name, ex.Message),
                            EventLogEntryType.Error);
                    }

                    if (IsStopping)
                    {
                        break;
                    }

                    Thread.Sleep(1000 * SleepInterval);
                }
            }
        }

        public static EventLog EventLog
        {
            get
            {
                if (mEventLog == null)
                {
                    lock (_locker)
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
                    }
                }
                return mEventLog;
            }
        }

        public abstract void SetUp();
    }
}

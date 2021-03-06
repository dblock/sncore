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
using SnCore.Tools.Web;
using SnCore.Services;
using NHibernate.Dialect.Function;
using System.ServiceProcess;
using System.Windows.Forms;

namespace SnCore.BackEndServices
{
    public abstract class SystemService : ServiceBase
    {
        public delegate void SessionJobDelegate(ISession session, ManagedSecurityContext sec);
        private int mInterruptInterval = 1;
        private int mSleepInterval = 60;
        private Thread mThread = null;
        private bool mIsStopping = false;
        private List<SessionJobDelegate> mJobs = new List<SessionJobDelegate>();
        private EventLog mEventLogManager = null;
        public const int MaxAbortRetryCount = 3;

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
            set
            {
                mIsStopping = value;
            }
        }

        public static IDbConnection GetNewConnection()
        {
            return new SqlConnection(
                SnCore.Data.Hibernate.Session.Configuration.GetProperty(
                    "hibernate.connection.connection_string"));
        }

        public bool GetAppSetting(string name, bool defaultvalue)
        {
            bool result = defaultvalue;
            object enabled = ConfigurationManager.AppSettings[name];
            if (enabled != null)
            {
                if (!bool.TryParse(enabled.ToString(), out result))
                {
                    EventLogManager.WriteEntry(string.Format("Configuration setting {0}=\"{1}\" is invalid.", name, enabled.ToString()),
                        EventLogEntryType.Warning);
                }
            }
            return result;
        }

        public bool SystemServicesEnabled
        {
            get
            {
                return GetAppSetting("SystemServices.Enabled", true);
            }
        }

        public bool IsEnabled
        {
            get
            {
                if (!SystemServicesEnabled)
                    return false;

                return GetAppSetting(string.Format("{0}.Enabled", GetType().Name), true);
            }
        }

        public bool SystemServicesDebug
        {
            get
            {
                return GetAppSetting("SystemServices.Debug", false);
            }
        }

        public bool IsDebug
        {
            get
            {
                if (!SystemServicesDebug)
                    return false;

                return GetAppSetting(string.Format("{0}.Debug", GetType().Name), false);
            }
        }

        public void Start()
        {
            if (!IsEnabled)
            {
                EventLogManager.WriteEntry(string.Format("Service {0} is disabled.", this.GetType().Name),
                    EventLogEntryType.Information);

                return;
            }
            else
            {
                EventLogManager.WriteEntry(string.Format("Service {0} is starting.", this.GetType().Name),
                    EventLogEntryType.Information);
            }

            mThread = new Thread(ThreadProc);
            mThread.Name = this.GetType().Name;
            mThread.Start(this);
        }

        public void Wait()
        {
            mThread.Join();
        }

        protected void AddJob(SessionJobDelegate job)
        {
            mJobs.Add(job);
        }

        public new void Stop()
        {
            if (IsDebug)
            {
                Console.WriteLine(string.Format("Service {0} is stopping.", this.GetType().Name));
                EventLogManager.WriteEntry(string.Format("Service {0} is stopping.", this.GetType().Name),
                    EventLogEntryType.Information);
            }

            mIsStopping = true;

            if (mThread != null)
            {
                if (mThread.IsAlive)
                {
                    if (IsDebug)
                    {
                        EventLogManager.WriteEntry(string.Format("Service {0} is stopping (forcefully).", this.GetType().Name),
                            EventLogEntryType.Warning);
                    }

                    Application.DoEvents();
                    Thread.Sleep(500);
                    mThread.Abort();
                }

                int retry = 0;
                while (!mThread.Join(1000))
                {
                    if (retry++ >= MaxAbortRetryCount)
                        break;

                    if (IsDebug)
                    {
                        Console.WriteLine(string.Format("Service {0} failed to abort thread, retrying {1}.", this.GetType().Name, retry));
                        EventLogManager.WriteEntry(string.Format("Service {0} failed to abort thread, retrying {1}.", this.GetType().Name, retry),
                            EventLogEntryType.Warning);
                    }

                    Application.DoEvents();
                    mThread.Abort();
                }

                if (IsDebug)
                {
                    EventLogManager.WriteEntry(string.Format("Service {0} has been stopped.", this.GetType().Name),
                        EventLogEntryType.Information);
                }
            }
        }

        public static void ThreadProc(object service)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            SystemService s = (SystemService) service;
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
                            mFactory.Dialect.Functions["count"] = new ClassicCountFunction();
                            mFactory.Dialect.Functions["sum"] = new ClassicSumFunction();
                            mFactory.Dialect.Functions["avg"] = new ClassicAvgFunction();
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
                job(session, ManagedAccount.GetAdminSecurityContext(session));
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
            EventLogManager.WriteEntry(string.Format("Running {0} with {1} job(s).", this.GetType().Name, mJobs.Count),
                EventLogEntryType.Information);

            if (SystemServicesDebug)
            {
                EventLogManager.WriteEntry(string.Format("{0}, SystemServicesDebug=\"{1}\", ServiceDebug=\"{2}\".", this.GetType().Name,
                    SystemServicesDebug, IsDebug), EventLogEntryType.Information);
            }

            while (!IsStopping)
            {
                foreach (SessionJobDelegate job in mJobs)
                {
                    DateTime start = DateTime.UtcNow;

                    if (IsDebug)
                    {
                        EventLogManager.WriteEntry(string.Format("Service {0} is running \"{1}\".", this.GetType().Name, job.Method.Name),
                            EventLogEntryType.Information);
                    }
                    
                    try
                    {
                        RunJob(job);

                        if (IsDebug)
                        {
                            EventLogManager.WriteEntry(string.Format("Service {0} finished running \"{1}\". Total run time was {2}.", 
                                this.GetType().Name, job.Method.Name, DateTime.UtcNow.Subtract(start)),
                                EventLogEntryType.Information);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        EventLogManager.WriteEntry(string.Format("{0}: Error running job.\n{1}", this.GetType().Name, ex.Message),
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

        public EventLog EventLogManager
        {
            get
            {
                if (mEventLogManager == null)
                {
                    lock (_locker)
                    {
                        if (mEventLogManager == null)
                        {
                            mEventLogManager = HostedApplication.CreateEventLog();
                        }
                    }
                }
                return mEventLogManager;
            }
            set
            {
                mEventLogManager = value;
            }
        }

        public abstract void SetUp();

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            EventLogManager = EventLog;
            Start();
        }

        protected override void OnStop()
        {
            Stop();
            base.OnStop();
        }
    }
}

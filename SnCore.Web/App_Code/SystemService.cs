using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace SnCore.BackEndServices
{
    public abstract class SystemService
    {
        private Thread mThread = null;
        private bool mIsStopping = false;

        public SystemService()
        {

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

        public void Start()
        {
            mThread = new Thread(ThreadProc);
            mThread.Start(this);
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
            s.Run();
        }

        public abstract void Run();
    }
}

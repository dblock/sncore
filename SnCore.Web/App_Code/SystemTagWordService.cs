using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Threading;
using System.Net.Mail;
using System.Diagnostics;

namespace SnCore.BackEndServices
{
    public class SystemTagWordService : SystemService
    {
        private int mSleepInterval = 60 * 60 * 2;
        private int mInterruptInterval = 1;

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

        public SystemTagWordService()
        {

        }

        public override void Run()
        {
            Nullable<DateTime> lastRun = null;
            ISessionFactory Factory = SnCore.Data.Hibernate.Session.Configuration.BuildSessionFactory();

            while (!IsStopping)
            {
                DateTime newRun = DateTime.UtcNow;
                try
                {

                    IDbConnection conn = GetNewConnection();
                    conn.Open();

                    ISession session = Factory.OpenSession(conn);

                    try
                    {
                        ICriteria c = session.CreateCriteria(typeof(Account));
                        if (lastRun.HasValue)
                        {
                            // last run is updated on interval, add some safe time
                            c.Add(Expression.Gt("LastLogin", lastRun.Value.AddMinutes(-30)));
                        }

                        IList accounts = c.List();

                        foreach (Account account in accounts)
                        {
                            try
                            {
                                ManagedAccount ma = new ManagedAccount(session, account);
                                ma.UpdateTagWords();
                            }
                            catch
                            {

                            }

                            Thread.Sleep(1000 * InterruptInterval);
                        }
                        session.Flush();
                    }
                    finally
                    {
                        conn.Close();
                        session.Close();
                    }
                }
                catch
                {

                }

                lastRun = newRun;
                Thread.Sleep(1000 * SleepInterval);
            }
        }
    }

}

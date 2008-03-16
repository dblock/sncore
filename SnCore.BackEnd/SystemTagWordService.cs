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
        public static string Name = "SnCore System Tag Service";
        public static string Description = "Indexes content into meta tags.";
        
        public SystemTagWordService()
        {
            ServiceName = Name;
        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunTagWords));
        }

        private Nullable<DateTime> mLastRun = null;

        public void RunTagWords(ISession session, ManagedSecurityContext sec)
        {
            DateTime newRun = DateTime.UtcNow;

            ICriteria c = session.CreateCriteria(typeof(Account));
            if (mLastRun.HasValue)
            {
                // last run is updated on interval, add some safe time
                c.Add(Expression.Gt("LastLogin", mLastRun.Value.AddMinutes(-30)));
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
            mLastRun = newRun;
        }
    }

}

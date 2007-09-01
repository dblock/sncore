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
using System.Collections.Generic;
using System.Threading;
using System.Net.Mail;
using System.Diagnostics;
using System.Reflection;
using SnCore.Tools.Web;

namespace SnCore.BackEndServices
{
    public class SystemSyndicationService : SystemService
    {
        public SystemSyndicationService()
        {

        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunSyndication));
        }

        public void RunSyndication(ISession session, ManagedSecurityContext sec)
        {
            IQuery query = session.CreateSQLQuery(
                "SELECT {AccountFeed.*} FROM AccountFeed" +
                " WHERE (NOT EXISTS ( SELECT AccountFeedItem_Id FROM AccountFeedItem item WHERE item.AccountFeed_Id = AccountFeed.AccountFeed_Id ))" +
                " OR ( AccountFeed.LastError NOT LIKE '' )" +
                " OR ( DATEDIFF(hour, AccountFeed.Updated, getutcdate()) > AccountFeed.UpdateFrequency )" +
                " ORDER BY AccountFeed.Updated ASC")
            .AddEntity("AccountFeed", typeof(AccountFeed));

            IList<AccountFeed> list = query.List<AccountFeed>();

            foreach(AccountFeed feed in list)
            {
                try
                {
                    ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feed);
                    EventLog.WriteEntry(string.Format("Syndication service updating {0} ({1}).",
                        feed.Name, feed.Id), EventLogEntryType.Information);
                    m_feed.Update(sec);
                    m_feed.UpdateImages(sec);
                    m_feed.UpdateMedias(sec);
                }
                catch (Exception ex)
                {
                    feed.LastError = ex.Message;
                    session.Save(feed);
                }

                session.Flush();
                Thread.Sleep(1000 * InterruptInterval);
            }
        }
    }
}

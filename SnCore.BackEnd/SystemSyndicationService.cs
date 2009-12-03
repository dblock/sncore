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
using System.ServiceProcess;
using System.Globalization;

namespace SnCore.BackEndServices
{
    public class SystemSyndicationService : SystemService
    {
        public static string Name = "SnCore System Syndication Service";
        public static string Description = "Runs RSS feed syndication updates.";

        public SystemSyndicationService()
        {
            ServiceName = Name;
        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunCleanupImg));
            AddJob(new SessionJobDelegate(RunCleanupMedia));
            AddJob(new SessionJobDelegate(RunSyndication));
        }

        public void RunCleanupImg(ISession session, ManagedSecurityContext sec)
        {
            session.Delete(string.Format(
                "FROM AccountFeedItemImg img " +
                " WHERE img.Modified < '{0}'", DateTime.UtcNow.AddMonths(-1).ToString(
                    DateTimeFormatInfo.InvariantInfo)));
            session.Flush();
        }

        public void RunCleanupMedia(ISession session, ManagedSecurityContext sec)
        {
            session.Delete(string.Format(
                "FROM AccountFeedItemMedia media " +
                " WHERE media.Modified < '{0}'", DateTime.UtcNow.AddMonths(-1).ToString(
                    DateTimeFormatInfo.InvariantInfo)));
            session.Flush();
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
                if (IsStopping)
                    break;

                try
                {
                    ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feed);
                    if (IsDebug)
                    {
                        EventLogManager.WriteEntry(string.Format("Syndication service updating {0} ({1}).",
                            feed.Name, feed.Id), EventLogEntryType.Information);
                    }
                    m_feed.Update(sec);
                    m_feed.UpdateImages(sec);
                    m_feed.UpdateMedias(sec);
                }
                catch (ThreadAbortException)
                {
                    throw;
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

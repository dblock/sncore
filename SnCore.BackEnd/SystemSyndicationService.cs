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
            AddJob(new SessionJobDelegate(RunSubscriptions));
            AddJob(new SessionJobDelegate(RunSyndication));
        }

        public void RunSyndication(ISession session, ManagedSecurityContext sec)
        {
            IQuery query = session.CreateSQLQuery(
                "SELECT {AccountFeed.*} FROM AccountFeed" +
                " WHERE (NOT EXISTS ( SELECT AccountFeedItem_Id FROM AccountFeedItem item WHERE item.AccountFeed_Id = AccountFeed.AccountFeed_Id ))" +
                " OR ( AccountFeed.LastError NOT LIKE '' )" +
                " OR ( DATEDIFF(hour, AccountFeed.Updated, getutcdate()) > AccountFeed.UpdateFrequency )",
                "AccountFeed",
                typeof(AccountFeed));

            IList<AccountFeed> list = query.List<AccountFeed>();

            foreach (AccountFeed feed in list)
            {
                try
                {
                    ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feed);
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

        public void RunSubscriptions(ISession session, ManagedSecurityContext sec)
        {
            IEnumerable<AccountRssWatch> rsswatchs = session.CreateQuery(
                "FROM AccountRssWatch AccountRssWatch" +
                " WHERE AccountRssWatch.Enabled = 1" +
                " AND DATEDIFF(hour, AccountRssWatch.Sent, getutcdate()) > AccountRssWatch.UpdateFrequency")
                .Enumerable<AccountRssWatch>();

            IEnumerator<AccountRssWatch> enumerator = rsswatchs.GetEnumerator();
            while (enumerator.MoveNext())
            {
                AccountRssWatch rsswatch = enumerator.Current;
                rsswatch.Sent = DateTime.UtcNow;
                rsswatch.LastError = string.Empty;
                try
                {
                    ManagedAccountRssWatch m_rsswatch = new ManagedAccountRssWatch(session, rsswatch);
                    if (m_rsswatch.HasSubscriptionUpdates(sec))
                    {
                        ManagedAccount ma = new ManagedAccount(session, rsswatch.Account);
                        ManagedSiteConnector.TrySendAccountEmailMessageUriAsAdmin(
                            session, ma, string.Format("AccountRssWatchView.aspx?id={0}", m_rsswatch.Id));
                    }
                }
                catch (Exception ex)
                {
                    rsswatch.LastError = ex.Message;
                }
                finally
                {
                    session.Save(rsswatch);
                    session.Flush();
                }

                Thread.Sleep(1000 * InterruptInterval);
            }
        }
    }

}

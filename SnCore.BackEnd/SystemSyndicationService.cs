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

        public void RunSyndication(ISession session)
        {
            // get feeds
            // todo: restrict to only feeds that need update
            IList list = session.CreateCriteria(typeof(AccountFeed)).List();

            foreach (AccountFeed feed in list)
            {
                try
                {
                    // todo: move into SQL
                    if (feed.AccountFeedItems != null
                        && string.IsNullOrEmpty(feed.LastError)
                        && feed.Updated.AddHours(feed.UpdateFrequency) > DateTime.UtcNow)
                    {
                        continue;
                    }

                    ManagedAccountFeed m_feed = new ManagedAccountFeed(session, feed);
                    m_feed.Update();
                    m_feed.UpdateImages();
                }
                catch (Exception ex)
                {
                    feed.LastError = ex.Message;
                    session.SaveOrUpdate(feed);
                }

                Thread.Sleep(1000 * InterruptInterval);
            }
        }
    }

}

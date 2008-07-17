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
using Microsoft.Win32;

namespace SnCore.BackEndServices
{
    public class SystemThumbnailService : SystemService
    {
        public static string Name = "SnCore System Thumbnail Generation Service";
        public static string Description = "Updates website and feed thumbnails.";

        public SystemThumbnailService()
        {
            ServiceName = Name;
        }

        public override void SetUp()
        {
            AddJob(new SessionJobDelegate(RunThumbnail));
        }

        private void RunThumbnail(ISession session, AccountWebsite website, ManagedSecurityContext sec)
        {
            try
            {
                ManagedAccountWebsite m_website = new ManagedAccountWebsite(session, website);

                if (IsDebug)
                {
                    EventLogManager.WriteEntry(string.Format("Thumbnail service updating {0}: {1}, {2}",
                        website.Id, website.Name, website.Url), EventLogEntryType.Information);
                }

                m_website.UpdateThumbnail();
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception)
            {
                website.Modified = DateTime.UtcNow;
                session.Save(website);
            }
        }

        private void RunThumbnail(ISession session, PlaceWebsite website, ManagedSecurityContext sec)
        {
            try
            {
                ManagedPlaceWebsite m_website = new ManagedPlaceWebsite(session, website);

                if (IsDebug)
                {
                    EventLogManager.WriteEntry(string.Format("Thumbnail service updating {0}: {1}, {2}",
                        website.Id, website.Name, website.Url), EventLogEntryType.Information);
                }

                m_website.UpdateThumbnail();
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception)
            {
                website.Modified = DateTime.UtcNow;
                session.Save(website);
            }
        }

        private void RunThumbnail_Websites(ISession session, ManagedSecurityContext sec)
        {
            // websites
            IQuery query = session.CreateSQLQuery(
                "SELECT {AccountWebsite.*} FROM AccountWebsite" +
                " WHERE ( AccountWebsite.Bitmap IS NULL )" +
                " OR ( DATEDIFF(hour, AccountWebsite.Modified, getutcdate()) > 24 )" +
                " ORDER BY AccountWebsite.Modified ASC")
            .AddEntity("AccountWebsite", typeof(AccountWebsite));

            IList<AccountWebsite> list = query.List<AccountWebsite>();

            foreach (AccountWebsite website in list)
            {
                if (IsStopping)
                    break;

                RunThumbnail(session, website, sec);
                session.Flush();
            }
        }

        private void RunThumbnail_PlaceWebsites(ISession session, ManagedSecurityContext sec)
        {
            // websites
            IQuery query = session.CreateSQLQuery(
                "SELECT {PlaceWebsite.*} FROM PlaceWebsite" +
                " WHERE ( PlaceWebsite.Bitmap IS NULL )" +
                " OR ( DATEDIFF(hour, PlaceWebsite.Modified, getutcdate()) > 24 )" +
                " ORDER BY PlaceWebsite.Modified ASC")
            .AddEntity("PlaceWebsite", typeof(PlaceWebsite));

            IList<PlaceWebsite> list = query.List<PlaceWebsite>();

            foreach (PlaceWebsite website in list)
            {
                if (IsStopping)
                    break;

                RunThumbnail(session, website, sec);
                session.Flush();
            }
        }

        public void RunThumbnail(ISession session, ManagedSecurityContext sec)
        {
            // check that script debugging is disabled
            object dsd = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main", "Disable Script Debugger", "yes");
            if (dsd != null && dsd.ToString() != "yes")
            {
                throw new Exception(@"Script debugging must be disabled for this service to start.\n" +
                    @"Set HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\Disable Script Debugger to 'yes'.");
            }

            object dsd_ie = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main", "DisableScriptDebuggerIE", "yes");
            if (dsd_ie != null && dsd_ie.ToString() != "yes")
            {
                throw new Exception(@"Script debugging must be disabled for this service to start.\n" +
                    @"Set HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\DisableScriptDebuggerIE to 'yes'.");
            }
 
            // update every 24 hours at the most

            RunThumbnail_Websites(session, sec);
            RunThumbnail_PlaceWebsites(session, sec);
        }
    }
}

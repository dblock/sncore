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
            IQuery query = session.CreateSQLQuery(
                "SELECT {AccountWebsite.*} FROM AccountWebsite" +
                " WHERE ( AccountWebsite.Bitmap IS NULL )" +
                " OR ( DATEDIFF(hour, AccountWebsite.Modified, getutcdate()) > 24 )" +
                " ORDER BY AccountWebsite.Modified ASC")
            .AddEntity("AccountWebsite", typeof(AccountWebsite));

            IList<AccountWebsite> list = query.List<AccountWebsite>();

            foreach (AccountWebsite website in list)
            {
                try
                {
                    ManagedAccountWebsite m_website = new ManagedAccountWebsite(session, website);
                    
                    if (IsDebug)
                    {
                        EventLogManager.WriteEntry(string.Format("Thumbnail service updating {0} ({1}).",
                            website.Name, website.Id), EventLogEntryType.Information);
                    }

                    m_website.UpdateThumbnail();
                }
                catch (Exception)
                {
                    website.Modified = DateTime.UtcNow;
                    session.Save(website);
                }

                session.Flush();
                Thread.Sleep(1000 * InterruptInterval);
            }
        }
    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using System.Diagnostics;
using SnCore.WebServices;
using SnCore.BackEndServices;
using NHibernate;
using NHibernate.Expression;
using SnCore.Services;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Hosting;

public class Global : SnCore.Tools.Web.HostedApplication
{
    private SystemMailMessageService mMailMessageService = new SystemMailMessageService();
    private SystemTagWordService mTagWordService = new SystemTagWordService();
    private SystemReminderService mSystemReminderService = new SystemReminderService();
    private SystemSyndicationService mSystemSyndicationService = new SystemSyndicationService();
    private SystemSMTPMessageService mSMTPMessageService = new SystemSMTPMessageService();

    public Global()
    {

    }

    protected override void Application_Start(Object sender, EventArgs e)
    {
        base.Application_Start(sender, e);

        // initialize http nhibernate session
        SnCore.Data.Hibernate.Session.Initialize(true);

        // ProtectAppConfig();

        using (SnCore.Data.Hibernate.Session.OpenConnection())
        {
            CreateDataObjects();
            CreateAdministrator();
            ManagedAccountAuditEntryCollection.SetMaxMessageLength();
        }

        if (! mMailMessageService.SystemServicesEnabled)
        {
            EventLogManager.WriteEntry("System services disabled by configuration setting.",
                EventLogEntryType.Warning);

            return;
        }

        mMailMessageService.Start();
        mTagWordService.Start();
        mSystemReminderService.Start();
        mSystemSyndicationService.Start();
        mSMTPMessageService.Start();

        WebSystemService system = new WebSystemService();
        EventLogManager.WriteEntry(string.Format("Running with web services {0}.", system.GetVersion()), EventLogEntryType.Information);
    }

    protected void Session_Start(Object sender, EventArgs e)
    {

    }

    protected void Application_BeginRequest(Object sender, EventArgs e)
    {
        SnCore.Data.Hibernate.Session.BeginRequest();
    }

    protected void Application_EndRequest(Object sender, EventArgs e)
    {
        SnCore.Data.Hibernate.Session.EndRequest();
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {

    }

    protected override void Application_Error(Object sender, EventArgs e)
    {
        base.Application_Error(sender, e);
    }

    protected override void Application_End(Object sender, EventArgs e)
    {
        base.Application_End(sender, e);
    }

    protected void Session_End(Object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Create an administrator.
    /// </summary>
    private void CreateAdministrator()
    {
        ISession session = SnCore.Data.Hibernate.Session.Current;
        Account a = (Account)session.CreateCriteria(typeof(Account))
            .SetMaxResults(1)
            .UniqueResult();

        if (a != null)
        {
            return;
        }

        a = new Account();
        a.Name = "Administrator";
        a.Password = ManagedAccount.GetPasswordHash("password");
        a.IsAdministrator = true;
        a.IsPasswordExpired = true;
        a.Created = a.Modified = a.LastLogin = DateTime.UtcNow;
        a.Birthday = DateTime.UtcNow;
        a.TimeZone = -1;
        session.Save(a);

        AccountEmail ae = new AccountEmail();
        ae.Account = a;
        ae.Address = "admin@localhost.com";
        ae.Created = ae.Modified = a.Created;
        ae.Principal = true;
        ae.Verified = true;
        session.Save(ae);
        session.Flush();

        ManagedAccount ma = new ManagedAccount(session, a);
        ma.CreateAccountSystemMessageFolders(ManagedAccount.GetAdminSecurityContext(session));
    }

    /// <summary>
    /// Create objects for basic permissions.
    /// </summary>
    private void CreateDataObjects()
    {
        ISession session = SnCore.Data.Hibernate.Session.Current;
        ITransaction t = session.BeginTransaction();
        try
        {
            IList dataobjects = session.CreateCriteria(typeof(DataObject)).List();
            Assembly a = Assembly.GetAssembly(typeof(DataObject));
            foreach (Type type in a.GetTypes())
            {
                bool found = false;
                if (dataobjects != null)
                {
                    foreach (DataObject dao in dataobjects)
                    {
                        if (dao.Name == type.Name)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    DataObject o = new DataObject();
                    o.Name = type.Name;
                    session.Save(o);
                }
            }

            t.Commit();
        }
        catch
        {
            t.Rollback();
            throw;
        }
        finally
        {
            t.Dispose();
        }
    }

    public static bool EncryptAppSettings
    {
        get
        {
            string encrypt = ConfigurationManager.AppSettings["System.EncryptAppSettings"];
            bool result = true;
            bool.TryParse(encrypt, out result);
            return result;
        }
    }

    private void ProtectAppConfig()
    {
        try
        {
            if (!EncryptAppSettings)
            {
                EventLogManager.WriteEntry("Not protecting App.config due to System.EncryptAppSettings setting.", EventLogEntryType.Information);
                return;
            }

            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            AppSettingsSection section = (AppSettingsSection)config.GetSection("appSettings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
                EventLogManager.WriteEntry(string.Format("Protected AppSettings in {0}", HostingEnvironment.ApplicationPhysicalPath), EventLogEntryType.Information);
            }
        }
        catch (Exception ex)
        {
            EventLogManager.WriteEntry(string.Format("Error protecting App.config.\n{0}", ex.Message), EventLogEntryType.Warning);
        }
    }
}

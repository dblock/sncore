using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using System.Net.Mail;
using SnCore.WebControls;

public partial class SystemConfigurationEmailEdit : AuthenticatedPage
{
    private struct SmtpDeliveryMethodType
    {
        private SmtpDeliveryMethod mMethod;
        private string mDescription;

        public SmtpDeliveryMethod Method
        {
            get
            {
                return mMethod;
            }
        }

        public string Description
        {
            get
            {
                return mDescription;
            }
        }

        public SmtpDeliveryMethodType(SmtpDeliveryMethod method, string description)
        {
            mMethod = method;
            mDescription = description;
        }
    };

    private static SmtpDeliveryMethodType[] smtpdeliverymethods = {
            new SmtpDeliveryMethodType(SmtpDeliveryMethod.Network, "SMTP Server on Network"),
            new SmtpDeliveryMethodType(SmtpDeliveryMethod.PickupDirectoryFromIis, "Pickup from IIS"),
            new SmtpDeliveryMethodType(SmtpDeliveryMethod.SpecifiedPickupDirectory, "Local Directory")
        };

    public void Page_Load(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            throw new ManagedAccount.AccessDeniedException();
        }

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Settings", Request, "SystemConfigurationsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("EMail Subsystem", Request.Url));
            StackSiteMap(sitemapdata);

            DomainClass cs = SessionManager.GetDomainClass("Configuration");
            int len = cs["OptionName"].MaxLengthInChars;
            inputServer.MaxLength = len;
            inputUsername.MaxLength = len;
            inputPassword.MaxLength = len;
            inputPort.MaxLength = len;
            inputPickupDirectoryLocation.MaxLength = len;
            inputDelivery.DataSource = smtpdeliverymethods;
            inputDelivery.DataBind();

            ListItemManager.TrySelect(inputDelivery, SessionManager.GetCachedConfiguration("SnCore.Mail.Delivery", SmtpDeliveryMethod.Network.ToString()));

            inputServer.Text = SessionManager.GetCachedConfiguration("SnCore.Mail.Server", "localhost");
            inputUsername.Text = SessionManager.GetCachedConfiguration("SnCore.Mail.Username", string.Empty);
            inputPort.Text = SessionManager.GetCachedConfiguration("SnCore.Mail.Port", "25");
            inputPickupDirectoryLocation.Text = SessionManager.GetCachedConfiguration("SnCore.Mail.PickupDirectoryLocation", string.Empty);

            save.Enabled = false;
        }

        SetDefaultButton(save);
    }

    public void test_Click(object sender, EventArgs e)
    {
        TransitAccountEmailMessage message = new TransitAccountEmailMessage();
        message.Body = string.Format("This e-mail was sent {0}.", SessionManager.Adjust(DateTime.UtcNow));
        message.Subject = "SnCore mail message system test";
        message.DeleteSent = true;
        message.MailTo = message.MailFrom = SessionManager.AccountService.GetActiveEmailAddress(
            SessionManager.Ticket, SessionManager.AccountId);
        SessionManager.AccountService.SendAccountEmailMessage(
            SessionManager.Ticket, message);
        ReportInfo("Test successful.");
    }

    protected override void OnPreRender(EventArgs e)
    {
        SmtpDeliveryMethod method = SmtpDeliveryMethod.Network;
        if (!string.IsNullOrEmpty(inputDelivery.SelectedValue))
        {
            method = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), inputDelivery.SelectedValue);
        }

        switch (method)
        {
            case SmtpDeliveryMethod.Network:
                panelServerPort.Visible = true;
                panelUsernamePassword.Visible = true;
                panelPickupDirectoryLocation.Visible = false;
                break;
            case SmtpDeliveryMethod.PickupDirectoryFromIis:
                panelServerPort.Visible = false;
                panelUsernamePassword.Visible = false;
                panelPickupDirectoryLocation.Visible = false;
                break;
            case SmtpDeliveryMethod.SpecifiedPickupDirectory:
                panelServerPort.Visible = false;
                panelUsernamePassword.Visible = false;
                panelPickupDirectoryLocation.Visible = true;
                break;
        }

        base.OnPreRender(e);
    }

    public void inputDelivery_SelectedIndexChanged(object sender, EventArgs e)
    {
        test.Enabled = false;
        save.Enabled = true;
    }

    public void save_Click(object sender, EventArgs e)
    {
        // delivery method
        TransitConfiguration t_delivery = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.Delivery", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
        t_delivery.Value = inputDelivery.SelectedValue;
        SessionManager.CreateOrUpdate<TransitConfiguration>(t_delivery, 
            SessionManager.SystemService.CreateOrUpdateConfiguration);
        // server
        TransitConfiguration t_server = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.Server", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
        t_server.Value = inputServer.Text;
        SessionManager.CreateOrUpdate<TransitConfiguration>(t_server,
            SessionManager.SystemService.CreateOrUpdateConfiguration);
        // port
        TransitConfiguration t_port = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.Port", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
        t_port.Value = inputPort.Text;
        SessionManager.CreateOrUpdate<TransitConfiguration>(t_port,
            SessionManager.SystemService.CreateOrUpdateConfiguration);
        // username
        TransitConfiguration t_username = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.Username", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
        t_username.Value = inputUsername.Text;
        SessionManager.CreateOrUpdate<TransitConfiguration>(t_username,
            SessionManager.SystemService.CreateOrUpdateConfiguration);
        // password
        if (!string.IsNullOrEmpty(inputPassword.Text))
        {
            TransitConfiguration t_password = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.Password", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
            t_password.Value = inputPassword.Text;
            t_password.Password = true;
            SessionManager.CreateOrUpdate<TransitConfiguration>(t_password,
                SessionManager.SystemService.CreateOrUpdateConfiguration);
        }
        // username
        TransitConfiguration t_pickupdirectorylocation = SessionManager.GetInstance<TransitConfiguration, string, string>(
            "SnCore.Mail.PickupDirectoryLocation", string.Empty, SessionManager.SystemService.GetConfigurationByNameWithDefault);
        t_pickupdirectorylocation.Value = inputPickupDirectoryLocation.Text;
        SessionManager.CreateOrUpdate<TransitConfiguration>(t_pickupdirectorylocation,
            SessionManager.SystemService.CreateOrUpdateConfiguration);

        test.Enabled = true;
        save.Enabled = false;
        
        ReportInfo("Configuration saved.");
    }
}

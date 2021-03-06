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

public partial class SystemConfigurationEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Settings", Request, "SystemConfigurationsManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Configuration");
            inputName.MaxLength = cs["OptionName"].MaxLengthInChars;

            int id = RequestId;

            if (id > 0)
            {
                TransitConfiguration tw = SessionManager.SystemService.GetConfigurationById(
                    SessionManager.Ticket, id);
                inputName.Text = Renderer.Render(tw.Name);
                inputValue.Text = Renderer.Render(tw.Value);
                inputPassword.Checked = tw.Password;
                sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Setting", Request.Url));
                inputName.Text = Request["name"];
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitConfiguration tw = new TransitConfiguration();
        tw.Name = inputName.Text;
        tw.Id = RequestId;
        tw.Value = inputValue.Text;
        tw.Password = inputPassword.Checked;
        SessionManager.CreateOrUpdate<TransitConfiguration>(
            tw, SessionManager.SystemService.CreateOrUpdateConfiguration);
        Page.Cache.Remove(string.Format("settings:{0}", tw.Name));
        Redirect("SystemConfigurationsManage.aspx");

    }
}

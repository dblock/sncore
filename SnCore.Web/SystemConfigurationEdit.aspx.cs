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

public partial class SystemConfigurationEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Settings", Request, "SystemConfigurationsManage.aspx"));

                int id = RequestId;

                if (id > 0)
                {
                    TransitConfiguration tw = SessionManager.SystemService.GetConfigurationById(id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputValue.Text = Renderer.Render(tw.Value);
                    inputPassword.Checked = tw.Password;
                    sitemapdata.Add(new SiteMapDataAttributeNode(tw.Name, Request.Url));
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("New Setting", Request.Url));
                }

                StackSiteMap(sitemapdata);
            }

            SetDefaultButton(manageAdd);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitConfiguration tw = new TransitConfiguration();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            tw.Value = inputValue.Text;
            tw.Password = inputPassword.Checked;
            SessionManager.SystemService.AddConfiguration(SessionManager.Ticket, tw);
            Page.Cache.Remove(string.Format("settings:{0}", tw.Name));
            Redirect("SystemConfigurationsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

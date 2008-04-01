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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;
using SnCore.Tools.Web;

public partial class SystemConfigurationsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Settings", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.SystemService.GetConfigurations(
            SessionManager.Ticket, null);
    }


    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.CommandArgument.ToString());
                TransitConfiguration tc = SessionManager.SystemService.GetConfigurationById(
                    SessionManager.Ticket, id);
                Page.Cache.Remove(string.Format("settings:{0}", tc.Name));
                SessionManager.Delete<TransitConfiguration>(id, SessionManager.SystemService.DeleteConfiguration);
                ReportInfo(string.Format("Configuration \"{0}\" deleted.", base.Render(tc.Name)));
                gridManage.CurrentPageIndex = 0;
                gridManage_OnGetDataSource(sender, e);
                gridManage.DataBind();
                break;
        }
    }

    public string GetValue(bool password, string value)
    {
        if (password) 
            return "**********";

        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return Renderer.GetSummary(value, 0, 24);
    }
}

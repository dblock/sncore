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
        gridManage.DataSource = SessionManager.SystemService.GetConfigurations();
    }


    private enum Cells
    {
        id = 0
    };

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    TransitConfiguration tc = SessionManager.SystemService.GetConfigurationById(id);
                    Page.Cache.Remove(string.Format("settings:{0}", tc.Name));
                    SessionManager.SystemService.DeleteConfiguration(SessionManager.Ticket, id);
                    ReportInfo(string.Format("Configuration \"{0}\" deleted.", base.Render(tc.Name)));
                    gridManage.CurrentPageIndex = 0;
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                    break;
            }
    }
}

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
using Wilco.Web.UI.WebControls;
using System.Drawing;
using SnCore.Tools.Drawing;
using System.IO;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Tools.Web;

public partial class SystemRefererHosts : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            if (!SessionManager.IsAdministrator)
            {
                ReportWarning("This page is only available to the system administrator.");
                return;
            }

            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Statistics", Request, "SystemStatsHits.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Referer Hosts", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    RefererHostQueryOptions GetOptions()
    {
        RefererHostQueryOptions qopt = new RefererHostQueryOptions();
        qopt.NewOnly = inputNewOnly.Checked;
        qopt.Hidden = inputHidden.Checked;
        return qopt;
    }

    private void GetData(object sender, EventArgs e)
    {
        RefererHostQueryOptions qopt = GetOptions();
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitRefererHost, RefererHostQueryOptions>(
            qopt, SessionManager.StatsService.GetRefererHostsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        RefererHostQueryOptions qopt = GetOptions();
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitRefererHost, RefererHostQueryOptions>(
            qopt, options, SessionManager.StatsService.GetRefererHosts);
    }

    public void optionsChanged(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Show":
            case "Hide":
                int id = int.Parse(e.CommandArgument.ToString());
                TransitRefererHost t_instance = SessionManager.GetInstance<TransitRefererHost, int>(
                    id, SessionManager.StatsService.GetRefererHostById);
                if (e.CommandName == "Show") t_instance.Hidden = false;
                else if (e.CommandName == "Hide") t_instance.Hidden = true;
                SessionManager.CreateOrUpdate<TransitRefererHost>(
                    t_instance, SessionManager.StatsService.CreateOrUpdateRefererHost);
                ReportInfo(string.Format("Referer \"{0}\" {1}.", 
                    Renderer.Render(t_instance.Host), t_instance.Hidden ? "hidden" : "shown"));
                GetData(source, e);
                break;
        }
    }
}

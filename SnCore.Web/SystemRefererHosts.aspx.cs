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

    private void GetData(object sender, EventArgs e)
    {
        RefererHostQueryOptions qopt = new RefererHostQueryOptions();
        qopt.NewOnly = inputNewOnly.Checked;
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitRefererHost, RefererHostQueryOptions>(
            qopt, SessionManager.StatsService.GetRefererHostsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        RefererHostQueryOptions qopt = new RefererHostQueryOptions();
        qopt.NewOnly = inputNewOnly.Checked;
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
}

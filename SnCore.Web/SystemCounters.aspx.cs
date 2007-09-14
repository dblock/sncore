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

public partial class SystemCounters : AuthenticatedPage
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
            sitemapdata.Add(new SiteMapDataAttributeNode("Counters", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitCounter>(
            SessionManager.StatsService.GetCountersCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitCounter>(
            options, SessionManager.StatsService.GetCounters);
    }

    public string GetUri(string uri)
    {
        string result = uri;

        if (uri.StartsWith(SessionManager.WebsiteUri.ToString()))
        {
            uri = uri.Substring(SessionManager.WebsiteUri.ToString().Length - 1)
                .TrimStart("/".ToCharArray());
        }

        if (uri.Length > 32)
        {
            uri = string.Format("{0} ...", uri.Substring(0, 32));
        }

        return uri;
    }
}

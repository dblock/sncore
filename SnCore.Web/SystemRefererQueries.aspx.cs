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

public partial class SystemRefererQueries : AuthenticatedPage
{
    public void Page_Load()
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                gridManage.VirtualItemCount = StatsService.GetRefererQueriesCount();
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = StatsService.GetRefererQueries(options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

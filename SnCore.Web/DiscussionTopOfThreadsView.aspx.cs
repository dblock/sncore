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
using SnCore.WebServices;
using SnCore.Services;

public partial class DiscussionTopOfThreadsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_OnGetDataSource(object sender, EventArgs e)
    {        
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        object[] args = { SessionManager.Ticket, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitDiscussionPost>(
            DiscussionService, "GetDiscussionTopOfThreads", args);
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            DiscussionService, "GetDiscussionTopOfThreadsCount", null);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }
}

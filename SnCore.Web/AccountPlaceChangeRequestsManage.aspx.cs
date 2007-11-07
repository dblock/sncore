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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;

public partial class AccountPlaceChangeRequestsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Place Change Requests", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.PlaceService.GetPlaceChangeRequests(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.PlaceService.GetPlaceChangeRequestsCount(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    SessionManager.PlaceService.DeletePlaceChangeRequest(SessionManager.Ticket, id);
                    GetData(sender, e);
                    ReportInfo("Place change request has been deleted.");
                    break;
                }
        }
    }
}

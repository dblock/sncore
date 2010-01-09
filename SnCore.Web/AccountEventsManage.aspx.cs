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
using SnCore.SiteMap;
using SnCore.Services;

public partial class AccountEventsManage : AuthenticatedPage
{

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    private enum Cells
    {
        id = 0
    };

    public void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.EventService.GetAccountEventsCountByAccountId(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageSize = gridManage.PageSize;
        options.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.EventService.GetAccountEventsByAccountId(
            SessionManager.Ticket, SessionManager.AccountId, SessionManager.UtcOffset.Ticks, options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                SessionManager.Delete<TransitAccountEvent>(id, SessionManager.EventService.DeleteAccountEvent);
                SessionManager.InvalidateCache<TransitAccountEventInstance>();
                ReportInfo("Event deleted.");
                GetData();
                break;
        }
    }
}

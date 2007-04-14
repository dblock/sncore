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
using System.Text;
using SnCore.Services;

public partial class AccountRssWatchsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Subscriptions", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.SyndicationService.GetAccountRssWatchsCount(
            SessionManager.Ticket, SessionManager.AccountId);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.SyndicationService.GetAccountRssWatchs(
            SessionManager.Ticket, SessionManager.AccountId, options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    SessionManager.Delete<TransitAccountRssWatch>(id, SessionManager.SyndicationService.DeleteAccountRssWatch);
                    ReportInfo("RssWatch deleted.");
                    GetData(sender, e);
                }
                break;
        }
    }
}

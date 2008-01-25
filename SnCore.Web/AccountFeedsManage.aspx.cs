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

public partial class AccountFeedsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            GetData(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Syndication", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    TransitAccountFeedQueryOptions GetOptions()
    {
        TransitAccountFeedQueryOptions qopt = new TransitAccountFeedQueryOptions();
        qopt.AccountId = SessionManager.AccountId;
        qopt.Hidden = true;
        return qopt;
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.SyndicationService.GetAccountFeedsCount(
            SessionManager.Ticket, GetOptions());
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
        gridManage.DataSource = SessionManager.SyndicationService.GetAccountFeeds(
            SessionManager.Ticket, GetOptions(), options);
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    SessionManager.Delete<TransitAccountFeed>(id, SessionManager.SyndicationService.DeleteAccountFeed);
                    ReportInfo("Feed deleted.");
                    GetData(sender, e);
                }
                break;
            case "Update":
                {
                    int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                    int item_count = SessionManager.SyndicationService.UpdateAccountFeedItems(SessionManager.Ticket, id);
                    int image_count = SessionManager.SyndicationService.UpdateAccountFeedItemImgs(SessionManager.Ticket, id);
                    int media_count = SessionManager.SyndicationService.UpdateAccountFeedItemMedias(SessionManager.Ticket, id);
                    SessionManager.InvalidateCache<TransitAccountFeedItem>();
                    SessionManager.InvalidateCache<TransitAccountFeedItemImg>();
                    SessionManager.InvalidateCache<TransitAccountFeedItemMedia>();
                    StringBuilder s = new StringBuilder("Feed updated with ");
                    s.AppendFormat("{0} new item{1}", item_count, item_count == 1 ? string.Empty : "s");
                    s.AppendFormat(", {0} new image{1}", image_count, image_count == 1 ? string.Empty : "s");
                    s.AppendFormat(" and {0} new media item{1}.", media_count, media_count == 1 ? string.Empty : "s");
                    ReportInfo(s.ToString());
                    gridManage_OnGetDataSource(sender, e);
                    gridManage.DataBind();
                }
                break;
        }
    }
}

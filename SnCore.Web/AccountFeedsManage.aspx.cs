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

public partial class AccountFeedsManage : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            if (!IsPostBack)
            {
                GetData(sender, e);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode("Syndication", Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SyndicationService.GetAccountFeedsCount(SessionManager.Ticket);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private enum Cells
    {
        id = 0
    };

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = SyndicationService.GetAccountFeeds(SessionManager.Ticket, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        SyndicationService.DeleteAccountFeed(SessionManager.Ticket, id);
                        ReportInfo("Feed deleted.");
                        GetData(sender, e);
                    }
                    break;
                case "Update":
                    {
                        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
                        int item_count = SyndicationService.UpdateAccountFeedItems(SessionManager.Ticket, id);
                        int image_count = SyndicationService.UpdateAccountFeedItemImgs(SessionManager.Ticket, id);
                        ReportInfo(string.Format("Feed updated with {0} new item{1} and {2} new image{3}.",
                            item_count, item_count == 1 ? string.Empty : "s",
                            image_count, image_count == 1 ? string.Empty : "s"));
                        gridManage_OnGetDataSource(sender, e);
                        gridManage.DataBind();
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

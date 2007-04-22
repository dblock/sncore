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
using System.Text;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using nStuff.UpdateControls;
using System.Collections.Specialized;

public partial class AccountFeedsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        ((SnCoreMasterPage)Master).History.Navigate += new HistoryEventHandler(History_Navigate);
        if (!IsPostBack)
        {
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("All Blogs", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
        }
        else
        {
            gridManage.CurrentPageIndex = 0;
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeed>(
            SessionManager.SyndicationService.GetAllAccountFeedsCount);

        int feedItemsCount = SessionManager.GetCount<TransitAccountFeedItem>(
            SessionManager.SyndicationService.GetAllAccountFeedItemsCount);

        labelCount.Text = string.Format("{0} blog{1} with <a href='AccountFeedItemsView.aspx'>{2} {3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedItemsCount, feedItemsCount == 1 ? "entry" : "entries");

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeed>(
            serviceoptions, SessionManager.SyndicationService.GetAllAccountFeeds);

        string args = string.Format("page={0}", gridManage.CurrentPageIndex);
        if (!(e is HistoryEventArgs)) ((SnCoreMasterPage)Master).History.AddEntry(Convert.ToBase64String(Encoding.Default.GetBytes(args)));
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }
}

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
using SnCore.WebServices;
using SnCore.Services;
using SnCore.SiteMap;

[SiteMapDataAttribute("Blogs")]
public partial class AccountFeedItemsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        SetDefaultButton(search);
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["q"]))
            {
                inputSearch.Text = Request.QueryString["q"];
            }

            panelSearchInternal.Visible = !string.IsNullOrEmpty(inputSearch.Text);
            GetData();
        }
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;

        gridManage.VirtualItemCount = string.IsNullOrEmpty(inputSearch.Text)
            ? SessionManager.GetCount<TransitAccountFeedItem>(
                SessionManager.SyndicationService.GetAllAccountFeedItemsCount)
            : SessionManager.GetCount<TransitAccountFeedItem, string>(
                inputSearch.Text, SessionManager.SyndicationService.SearchAccountFeedItemsCount);

        int feedsCount = SessionManager.SyndicationService.GetAllAccountFeedsCount(SessionManager.Ticket);

        labelCount.Text = string.Format("{0} post{1} from <a href='AccountFeedsView.aspx'>{2} blog{3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? string.Empty : "s",
            feedsCount, feedsCount == 1 ? string.Empty : "s");

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = string.IsNullOrEmpty(inputSearch.Text)
            ? SessionManager.GetCollection<TransitAccountFeedItem>(
                serviceoptions, SessionManager.SyndicationService.GetAllAccountFeedItems)
            : SessionManager.GetCollection<TransitAccountFeedItem, string>(
                inputSearch.Text, serviceoptions, SessionManager.SyndicationService.SearchAccountFeedItems);
    }

    protected void search_Click(object sender, EventArgs e)
    {
        GetData();
        panelLinks.Update();
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read 1 comment";
        else return string.Format("read {0} comments", count);
    }

    public string GetSummary(string summary, string link)
    {
        Uri uri = null;
        Uri.TryCreate(link, UriKind.Absolute, out uri);
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUrl.TrimEnd("/".ToCharArray()) + "/AccountFeedItemPicture.aspx?Src={url}", UriKind.Absolute);
        return Renderer.CleanHtml(summary, uri, imgrewriteuri);
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }
}

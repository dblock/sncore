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

public partial class AccountFeedItemsView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            SetDefaultButton(search);
            if (!IsPostBack)
            {
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetData()
    {
        gridManage.CurrentPageIndex = 0;
        
        if (string.IsNullOrEmpty(SearchQuery))
        {
            gridManage.VirtualItemCount = SyndicationService.GetAccountFeedItemsCount();
        }
        else
        {
            gridManage.VirtualItemCount = SyndicationService.SearchAccountFeedItemsCount(SearchQuery);
        }

        int feedsCount = SyndicationService.GetUpdatedAccountFeedsCount();

        labelCount.Text = string.Format("{0} feed {1} from <a href='AccountFeedsView.aspx'>{2} feed{3}</a>",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount == 1 ? "entry" : "entries",
            feedsCount, feedsCount == 1 ? string.Empty : "s" );
        
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPageIndex;
            if (string.IsNullOrEmpty(SearchQuery))
            {
                gridManage.DataSource = SyndicationService.GetAccountFeedItems(serviceoptions);
            }
            else
            {
                gridManage.DataSource = SyndicationService.SearchAccountFeedItems(SearchQuery, serviceoptions);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string SearchQuery
    {
        get
        {
            object query = Request.QueryString["q"];
            return query == null ? string.Empty : query.ToString();
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            Redirect(string.Format("AccountFeedItemsView.aspx?q={0}",
                Renderer.UrlEncode(inputSearch.Text)));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read 1 comment";
        else return string.Format("read {0} comments", count);
    }
}

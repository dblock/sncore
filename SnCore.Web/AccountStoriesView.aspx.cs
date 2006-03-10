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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountStoriesView : Page
{
    public void Page_Load()
    {
        try
        {
            SetDefaultButton(search);
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(SearchQuery))
                {
                    gridManage.VirtualItemCount = StoryService.GetLatestAccountStoriesCount();
                }
                else
                {
                    gridManage.VirtualItemCount = StoryService.SearchAccountStoriesCount(SearchQuery);
                }

                labelCount.Text = string.Format("{0} {1}",
                    gridManage.VirtualItemCount, gridManage.VirtualItemCount != 1 ? "stories" : "story");
                
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetComments(int count)
    {
        if (count == 0)
        {
            return string.Empty;
        }
        else if (count == 1)
        {
            return "1 comment";
        }
        else
        {
            return string.Format("{0} comments", count);
        }
    }

    public string GetSummary(string summary)
    {
        string result = SessionManager.ClearMarkups(Renderer.RemoveHtml(summary));
        result = Renderer.Render(result);
        if (result.Length > 256) result = result.Substring(0, 256) + " ...";
        return result;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;

            if (string.IsNullOrEmpty(SearchQuery))
            {
                gridManage.DataSource = StoryService.GetLatestAccountStories(options);
            }
            else
            {
                gridManage.DataSource = StoryService.SearchAccountStories(SearchQuery, options);
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
            Redirect(string.Format("AccountStoriesView.aspx?q={0}", 
                Renderer.UrlEncode(inputSearch.Text)));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

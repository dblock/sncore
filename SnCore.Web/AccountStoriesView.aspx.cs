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
using SnCore.SiteMap;

[SiteMapDataAttribute("Stories")]
public partial class AccountStoriesView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["q"]))
                {
                    inputSearch.Text = Request.QueryString["q"];
                }

                panelSearchInternal.Visible = ! string.IsNullOrEmpty(inputSearch.Text);
                GetData(sender, e);
            }

            SetDefaultButton(search);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        labelCount.Text = string.Format("{0} {1}",
            gridManage.VirtualItemCount, gridManage.VirtualItemCount != 1 ? "stories" : "story");

        base.OnPreRender(e);
    }

    public string GetComments(int count)
    {
        if (count == 0)
        {
            return "no comments";
        }
        else if (count == 1)
        {
            return "one comment";
        }
        else
        {
            return string.Format("{0} comments", count);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            if (string.IsNullOrEmpty(inputSearch.Text))
            {
                object[] args = { options };
                gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountStory>(
                    SessionManager.StoryService, "GetLatestAccountStories", args);
            }
            else
            {
                object[] args = { inputSearch.Text, options };
                gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountStory>(
                    SessionManager.StoryService, "SearchAccountStories", args);
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

        if (string.IsNullOrEmpty(inputSearch.Text))
        {
            gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                SessionManager.StoryService, "GetLatestAccountStoriesCount", null);
        }
        else
        {
            object[] args = { inputSearch.Text };
            gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                SessionManager.StoryService, "SearchAccountStoriesCount", args);
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData(sender, e);

            labelCount.Text = string.Format("{0} {1}",
                gridManage.VirtualItemCount, gridManage.VirtualItemCount != 1 ? "stories" : "story");

            panelLinks.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        try
        {
            panelSearchInternal.Visible = false;
            inputSearch.Text = string.Empty;
            GetData(sender, e);
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        try
        {
            panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }
}

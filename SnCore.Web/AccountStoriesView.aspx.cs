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
                if (!string.IsNullOrEmpty(Request.QueryString["q"]))
                {
                    inputSearch.Text = Request.QueryString["q"];
                }
                else
                {
                    panelSearchInternal.Attributes.Add("style", "display: none;");
                }

                GetData();
            }
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
            options.PageNumber = gridManage.CurrentPage;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = string.IsNullOrEmpty(inputSearch.Text)
                ? StoryService.GetLatestAccountStories(options)
                : StoryService.SearchAccountStories(inputSearch.Text, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void GetData()
    {
        gridManage.VirtualItemCount = string.IsNullOrEmpty(inputSearch.Text)
            ? StoryService.GetLatestAccountStoriesCount()
            : StoryService.SearchAccountStoriesCount(inputSearch.Text);

        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    protected void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData();

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
            panelSearchInternal.Attributes["style"] = "display: none;";
            inputSearch.Text = string.Empty;
            GetData();
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
            panelSearchInternal.Attributes["style"] =
                (string.IsNullOrEmpty(panelSearchInternal.Attributes["style"]) ? "display: none;" : string.Empty);

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

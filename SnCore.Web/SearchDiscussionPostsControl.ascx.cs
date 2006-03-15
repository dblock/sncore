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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;
using System.Collections.Generic;

public partial class SearchDiscussionPostsControl : Control
{
    public void Page_Load()
    {
        try
        {
            gridResults.OnGetDataSource += new EventHandler(gridResults_OnGetDataSource);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(SearchQuery))
                {
                    GetResults();
                }
                else
                {
                    this.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public new string RenderEx(object s)
    {
        return base.RenderEx(SessionManager.RenderComments(s));
    }

    public void GetResults()
    {
        gridResults.CurrentPageIndex = 0;
        
        if (DiscussionId > 0)
        {
            gridResults.VirtualItemCount = DiscussionService.SearchDiscussionPostsByIdCount(DiscussionId, SearchQuery);
        }
        else
        {
            gridResults.VirtualItemCount = DiscussionService.SearchDiscussionPostsCount(SearchQuery);
        }

        labelResults.Text = string.Format("{0} result{1}", 
            gridResults.VirtualItemCount, gridResults.VirtualItemCount != 1 ? "s" : string.Empty);
        
        gridResults_OnGetDataSource(this, null);
        gridResults.DataBind();
    }

    public int DiscussionId
    {
        get
        {
            object id = Request.QueryString["id"];
            return id == null ? 0 : int.Parse(id.ToString());
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

    public int ResultsCount
    {
        get
        {
            return gridResults.VirtualItemCount;
        }
    }

    void gridResults_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridResults.CurrentPageIndex;
            options.PageSize = gridResults.PageSize;

            List<TransitDiscussionPost> posts = null;

            if (DiscussionId > 0)
            {
                posts = DiscussionService.SearchDiscussionPostsById(DiscussionId, SearchQuery, options);
            }
            else
            {
                posts = DiscussionService.SearchDiscussionPosts(SearchQuery, options);
            }
                
            gridResults.DataSource = posts;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

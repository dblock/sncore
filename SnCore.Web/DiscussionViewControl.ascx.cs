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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;
using System.Collections.Generic;
using SnCore.WebServices;

public partial class DiscussionViewControl : Control
{
    public string PostNewText
    {
        get
        {
            return postNew.Text;
        }
        set
        {
            postNew.Text = value;
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = DiscussionService.GetDiscussionThreadsCountById(DiscussionId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            if (DiscussionId > 0)
            {
                TransitDiscussion d = DiscussionService.GetDiscussionById(DiscussionId);
                discussionLabel.Text = Renderer.Render(d.Name);
                discussionDescription.Text = Renderer.Render(d.Description);
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageNumber = gridManage.CurrentPageIndex;
                options.PageSize = gridManage.PageSize;
                gridManage.DataSource = DiscussionService.GetDiscussionThreadsById(SessionManager.Ticket, DiscussionId, options);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        try
        {
            if (!IsPostBack)
            {
                postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit",
                    DiscussionId, Renderer.UrlEncode(Request.Url.PathAndQuery));

                linkSearch.NavigateUrl = string.Format("SearchDiscussionPosts.aspx?id={0}",
                    DiscussionId);

                linkRss.NavigateUrl = string.Format("DiscussionRss.aspx?id={0}", DiscussionId);

                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public int DiscussionId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "DiscussionId", 0);
        }
        set
        {
            ViewState["DiscussionId"] = value;
        }
    }
}

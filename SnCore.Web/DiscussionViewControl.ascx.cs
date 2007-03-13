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
    private string mCssClass = "sncore_table";

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
        gridManage.VirtualItemCount = SessionManager.DiscussionService.GetDiscussionThreadsCountByDiscussionId(
            SessionManager.Ticket, DiscussionId);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        if (DiscussionId > 0)
        {
            TransitDiscussion d = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, DiscussionId);
            discussionLabel.Text = Renderer.Render(d.Name);
            discussionDescription.Text = Renderer.Render(d.Description);
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = SessionManager.DiscussionService.GetDiscussionThreadsByDiscussionId(
                SessionManager.Ticket, DiscussionId, options);
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        PageManager.SetDefaultButton(search, panelSearch.Controls);
        if (!IsPostBack)
        {
            postNew.NavigateUrl = string.Format("DiscussionPost.aspx?did={0}&ReturnUrl={1}&#edit",
                DiscussionId, Renderer.UrlEncode(Request.Url.PathAndQuery));

            linkRelRss.NavigateUrl = string.Format("DiscussionRss.aspx?id={0}", DiscussionId);

            gridManage.CssClass = mCssClass;
            tableSearch.Attributes["class"] = mCssClass;

            GetData(sender, e);
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

    public string CssClass
    {
        get
        {
            return mCssClass;
        }
        set
        {
            mCssClass = value;
        }
    }

    protected void search_Click(object sender, EventArgs e)
    {
        Redirect(string.Format("SearchDiscussionPosts.aspx?id={0}&q={1}",
            RequestId,
            Renderer.UrlEncode(inputSearch.Text)));
    }
}

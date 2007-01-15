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
using SnCore.WebServices;
using SnCore.BackEndServices;
using SnCore.Services;

public partial class AccountBlogPreviewControl : Control
{
    public bool ShowTitle
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "ShowTitle", true);
        }
        set
        {
            ViewState["ShowTitle"] = value;
        }
    }

    public bool LinkDescription
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "LinkDescription", false);
        }
        set
        {
            ViewState["LinkDescription"] = value;
        }
    }

    public int BlogId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "BlogId", 0);
        }
        set
        {
            ViewState["BlogId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            if (BlogId > 0)
            {
                object[] b_args = { SessionManager.Ticket, BlogId };
                TransitAccountBlog tb = SessionManager.GetCachedItem<TransitAccountBlog>(
                    SessionManager.BlogService, "GetAccountBlogById", b_args);
                linkRelRss.NavigateUrl = string.Format("AccountBlogRss.aspx?id={0}", BlogId);
                linkRelRss.Title = Renderer.Render(tb.Name);

                object[] args = { SessionManager.Ticket, BlogId };
                gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount<TransitAccountBlogPost>(
                    SessionManager.BlogService, "GetAccountBlogPostsCount", args);
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();
            }
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        object[] args = { SessionManager.Ticket, BlogId, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountBlogPost>(
            SessionManager.BlogService, "GetAccountBlogPosts", args);
    }

    public string GetTitle(object title)
    {
        if (!ShowTitle)
            return string.Empty;

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(object description)
    {
        return Renderer.RenderEx(Renderer.CleanHtml(description));
    }
}

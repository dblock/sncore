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
using System.Text;

public partial class AccountBlogPreviewControl : Control
{
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
                TransitAccountBlog tb = SessionManager.GetInstance<TransitAccountBlog, int>(
                    BlogId, SessionManager.BlogService.GetAccountBlogById);

                blogName.Text = Renderer.Render(tb.Name);
                blogDescription.Text = Renderer.Render(tb.Description);
                linkRelRss.NavigateUrl = string.Format("AccountBlogRss.aspx?id={0}", BlogId);
                linkRelRss.Title = Renderer.Render(tb.Name);
                linkRead.NavigateUrl = string.Format("AccountBlogView.aspx?id={0}", BlogId);

                gridManage.RepeatRows = tb.DefaultViewRows;

                if (tb.DefaultViewRows > 0)
                {
                    gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountBlogPost, int>(
                        BlogId, SessionManager.BlogService.GetAccountBlogPostsCount);
                    gridManage_OnGetDataSource(this, null);
                    gridManage.DataBind();
                }

                StringBuilder sb = new StringBuilder();
                if (tb.PostCount > 0)
                {
                    sb.AppendFormat(" &#187; {0} post{1}", tb.PostCount, tb.PostCount == 1 ? string.Empty : "s");
                    sb.AppendFormat(" <span class='{0}'>&#187; last post {1}</span>",
                        DateTime.UtcNow.Subtract(tb.Updated).TotalDays < 3 ? "sncore_datetime_highlight" : string.Empty,
                        SessionManager.ToAdjustedString(tb.Updated));
                }
                labelPosts.Text = sb.ToString();
            }
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountBlogPost, int>(
            BlogId, options, SessionManager.BlogService.GetAccountBlogPosts);
    }
}

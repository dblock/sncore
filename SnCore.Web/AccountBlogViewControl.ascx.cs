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

public partial class AccountBlogViewControl : Control
{
    private int mItemsShown = 0;
    private TransitAccountBlog mAccountBlog = null;

    public TransitAccountBlog Blog
    {
        get
        {
            if (mAccountBlog == null)
            {
                mAccountBlog = (TransitAccountBlog)
                    Cache[string.Format("accountblog:{0}", BlogId)];

                if (mAccountBlog == null)
                {
                    mAccountBlog = BlogService.GetAccountBlogById(string.Empty, BlogId);
                    Cache.Insert(string.Format("accountblog:{0}", BlogId),
                        mAccountBlog, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mAccountBlog;
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
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                if (BlogId > 0)
                {
                    TransitAccountBlog f = BlogService.GetAccountBlogById(
                        SessionManager.IsLoggedIn ? SessionManager.Ticket : string.Empty, BlogId);

                    // limit number of items
                    gridManage.VirtualItemCount = Math.Min(gridManage.PageSize, BlogService.GetAccountBlogPostsCountById(BlogId));
                    gridManage_OnGetDataSource(this, null);
                    gridManage.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPage;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = BlogService.GetAccountBlogPostsById(BlogId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetTitle(object title)
    {
        if (title == null || string.IsNullOrEmpty(title.ToString()))
            return "Untitled";

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(object description)
    {
        if (mItemsShown++ >= 1)
            return string.Empty;

        return Renderer.CleanHtml(description);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }
}

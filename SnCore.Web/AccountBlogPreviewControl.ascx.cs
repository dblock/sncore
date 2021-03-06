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
using SnCore.Tools.Web.Html;
using System.Collections.Generic;

public partial class AccountBlogPreviewControl : Control
{
    private int mItemsCollapseAfter = 1;
    private int mItemsShown = 0;
    private int mImagesShown = 0;
    private TransitAccountBlog mAccountBlog = null;

    public int ItemsCollapseAfter
    {
        get
        {
            return mItemsCollapseAfter;
        }
        set
        {
            mItemsCollapseAfter = value;
        }
    }

    public TransitAccountBlog Blog
    {
        get
        {
            if (mAccountBlog == null && BlogId > 0)
            {
                mAccountBlog = SessionManager.GetInstance<TransitAccountBlog, int>(
                    BlogId, SessionManager.BlogService.GetAccountBlogById);
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

    private TransitAccountBlogPostQueryOptions GetBlogPostsOptions()
    {
        TransitAccountBlogPostQueryOptions qopt = new TransitAccountBlogPostQueryOptions();
        qopt.PublishedOnly = true;
        qopt.BlogId = BlogId;
        return qopt;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            this.Visible = (BlogId > 0);

            if (BlogId > 0)
            {
                TransitAccountBlog blog = SessionManager.GetInstance<TransitAccountBlog, int>(
                    BlogId, SessionManager.BlogService.GetAccountBlogById);

                lblBlogName.Text = Renderer.Render(blog.Name);

                gridManage.RepeatRows = blog.DefaultViewRows;
                gridManage.VirtualItemCount = Math.Min(gridManage.PageSize, SessionManager.GetCount<TransitAccountBlogPost, TransitAccountBlogPostQueryOptions>(
                    GetBlogPostsOptions(), SessionManager.BlogService.GetAccountBlogPostsCount));
                gridManage_OnGetDataSource(this, null);
                gridManage.DataBind();

                linkRelRss.NavigateUrl = string.Format("AccountBlogRss.aspx?id={0}", BlogId);
                linkRelRss.Title = Renderer.Render(blog.Name);
            }
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountBlogPost, TransitAccountBlogPostQueryOptions>(
            GetBlogPostsOptions(), options, SessionManager.BlogService.GetAccountBlogPosts);
    }

    public string GetTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "Untitled";

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= mItemsCollapseAfter)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return base.GetSummary(description);
    }

    public string GetImage(string description)
    {
        if (mImagesShown++ >= mItemsCollapseAfter)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        List<HtmlImage> list = HtmlImageExtractor.Extract(description);

        if (list.Count == 0)
            return string.Empty;

        return string.Format("<img class='sncore_blog_image' border='0' src='{0}'>", list[0].Src);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }
}

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

public partial class AccountBlogViewControl : Control
{
    private int mItemsShown = 0;
    private int mImagesShown = 0;
    private TransitAccountBlog mAccountBlog = null;
    private ListItemCollection mContentLinkIds = new ListItemCollection();

    public TransitAccountBlog Blog
    {
        get
        {
            if (mAccountBlog == null)
            {
                mAccountBlog = (TransitAccountBlog)
                    Cache[string.Format("accountblog:{0}", BlogId)];

                if (mAccountBlog == null && BlogId > 0)
                {
                    mAccountBlog = BlogService.GetAccountBlogById(string.Empty, BlogId);
                    Cache.Insert(string.Format("accountblog:{0}", BlogId),
                        mAccountBlog, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
                }
            }

            return mAccountBlog;
        }
    }

    public string BlogName
    {
        get
        {
            if (BlogId == 0)
                return string.Empty;

            return Renderer.Render(Blog.Name);
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

    public ListItemCollection ContentLinkIds
    {
        get
        {
            return mContentLinkIds;
        }
        set
        {
            mContentLinkIds = value;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        foreach (ListItem item in ContentLinkIds)
        {
            AccountContentGroupLinkControl link = (AccountContentGroupLinkControl) Page.LoadControl("AccountContentGroupLinkControl.ascx");
            link.LowerCase = true;
            link.ConfigurationName = item.Value;
            divLinks.Controls.Add(link);
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
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            gridManage.DataSource = BlogService.GetAccountBlogPostsById(BlogId, options);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public string GetTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "Untitled";

        return Renderer.Render(Renderer.RemoveHtml(title));
    }

    public string GetDescription(string description)
    {
        if (mItemsShown++ >= 1)
            return string.Empty;

        if (string.IsNullOrEmpty(description))
            return string.Empty;

        return Renderer.GetSummary(description);
    }

    public string GetImage(string description)
    {
        if (mImagesShown++ >= 1)
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

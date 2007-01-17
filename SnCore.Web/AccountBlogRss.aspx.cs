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

public partial class AccountBlogRss : Page
{
    private TransitAccountBlog mBlog = null;

    private TransitAccountBlog Blog
    {
        get
        {
            if (mBlog == null)
            {
                mBlog = SessionManager.GetInstance<TransitAccountBlog, int>(
                    RequestId, SessionManager.BlogService.GetAccountBlogById);
            }
            return mBlog;
        }
    }

    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} {1}",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Blog.Name));
        }
    }

    public string Description
    {
        get
        {
            return Renderer.Render(Blog.Description);
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl.TrimEnd('/') + string.Format("/AccountBlogView.aspx?id={0}", RequestId);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 25;
            rssRepeater.DataSource = SessionManager.GetCollection<TransitAccountBlogPost, int>(
                RequestId, options, SessionManager.BlogService.GetAccountBlogPosts);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}

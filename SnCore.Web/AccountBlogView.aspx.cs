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
using SnCore.SiteMap;

public partial class AccountBlogView : Page
{
    private TransitFeature mAccountBlogFeature = null;
    private TransitAccountBlog mAccountBlog = null;

    public TransitAccountBlog AccountBlog
    {
        get
        {
            if (mAccountBlog == null)
            {
                mAccountBlog = SessionManager.GetInstance<TransitAccountBlog, int>(
                    RequestId, SessionManager.BlogService.GetAccountBlogById);
            }
            return mAccountBlog;
        }
    }

    public TransitFeature LatestAccountBlogFeature
    {
        get
        {
            if (mAccountBlogFeature == null)
            {
                mAccountBlogFeature = SessionManager.GetInstance<TransitFeature, string, int>(
                    "AccountBlog", RequestId, SessionManager.ObjectService.FindLatestFeature);
            }
            return mAccountBlogFeature;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            TransitAccountBlog blog = AccountBlog;

            if (blog == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            labelBlog.Text = Renderer.Render(blog.Name);
            labelBlogDescription.Text = Renderer.Render(blog.Description);

            TransitAccount a = SessionManager.GetInstance<TransitAccount, int>(
                blog.AccountId, SessionManager.AccountService.GetAccountById);

            labelAccountName.Text = Renderer.Render(a.Name);
            linkAccount.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);

            licenseView.AccountId = a.Id;

            linkRelRss.Title = this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(blog.Name));

            GetData(sender, e);

            if (SessionManager.IsAdministrator)
            {
                linkFeature.Text = (LatestAccountBlogFeature != null)
                    ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountBlogFeature.Created).ToString("d"))
                    : "Feature &#187; Never Featured";
            }

            linkRelRss.NavigateUrl = string.Format("AccountBlogRss.aspx?id={0}", RequestId);
            linkEdit.NavigateUrl = string.Format("AccountBlogEdit.aspx?id={0}", RequestId);
            linkPostNew.NavigateUrl = string.Format("AccountBlogPost.aspx?bid={0}", RequestId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(blog.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountBlogPost, int>(
            RequestId, SessionManager.BlogService.GetAccountBlogPostsCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountBlogPost, int>(
            RequestId, options, SessionManager.BlogService.GetAccountBlogPosts);
    }

    public string GetComments(int count)
    {
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }

    protected override void OnPreRender(EventArgs e)
    {
        panelAdmin.Visible = SessionManager.IsAdministrator;
        if (panelAdmin.Visible)
        {
            linkDeleteFeatures.Visible = (LatestAccountBlogFeature != null);
        }

        panelOwner.Visible = SessionManager.IsAdministrator
            || ((SessionManager.Account != null) && (SessionManager.Account.Id == AccountBlog.AccountId));

        base.OnPreRender(e);
    }

    public void feature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature blogs.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountBlog";
        t_feature.DataRowId = RequestId;
        SessionManager.CreateOrUpdate<TransitFeature>(
            t_feature, SessionManager.ObjectService.CreateOrUpdateFeature);
        Redirect(Request.Url.PathAndQuery);
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature blogs.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountBlog";
        t_feature.DataRowId = RequestId;
        SessionManager.ObjectService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        SessionManager.InvalidateCache<TransitFeature>();
        Redirect(Request.Url.PathAndQuery);
    }

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                int id = int.Parse(e.CommandArgument.ToString());
                SessionManager.Delete<TransitAccountBlogPost>(id, SessionManager.BlogService.DeleteAccountBlogPost);
                ReportInfo("Post Deleted");
                GetData(source, e);
                break;
        }
    }


}

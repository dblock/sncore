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
                object[] args = { SessionManager.Ticket, RequestId };
                mAccountBlog = SessionManager.GetCachedItem<TransitAccountBlog>(
                    SessionManager.BlogService, "GetAccountBlogById", args);
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
                object[] args = { "AccountBlog", RequestId };
                mAccountBlogFeature = SessionManager.GetCachedItem<TransitFeature>(
                    SessionManager.SystemService, "FindLatestFeature", args);
            }
            return mAccountBlogFeature;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                TransitAccountBlog f = AccountBlog;
                labelBlog.Text = Renderer.Render(f.Name);
                labelBlogDescription.Text = Renderer.Render(f.Description);

                object[] args = { f.AccountId };
                TransitAccount a = SessionManager.GetCachedItem<TransitAccount>(
                    SessionManager.AccountService, "GetAccountById", args);

                labelAccountName.Text = Renderer.Render(a.Name);
                linkAccount.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
                imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);

                licenseView.AccountId = a.Id;

                linkRelRss.Title = this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(f.Name));

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
                sitemapdata.Add(new SiteMapDataAttributeNode(f.Name, Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        object[] args = { RequestId };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            SessionManager.BlogService, "GetAccountBlogPostsCountById", args);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = gridManage.CurrentPageIndex;
            options.PageSize = gridManage.PageSize;
            object[] args = { SessionManager.Ticket, RequestId, options };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountBlogPost>(
                SessionManager.BlogService, "GetAccountBlogPostsById", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature blogs.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountBlog";
            t_feature.DataRowId = RequestId;
            SessionManager.SystemService.CreateOrUpdateFeature(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsAdministrator)
            {
                // avoid round-trip
                throw new Exception("You must be an administrator to feature blogs.");
            }

            TransitFeature t_feature = new TransitFeature();
            t_feature.DataObjectName = "AccountBlog";
            t_feature.DataRowId = RequestId;
            SessionManager.SystemService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
            Redirect(Request.Url.PathAndQuery);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridManage_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    SessionManager.BlogService.DeleteAccountBlogPost(SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()));
                    ReportInfo("Post Deleted");
                    GetData(source, e);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }


}

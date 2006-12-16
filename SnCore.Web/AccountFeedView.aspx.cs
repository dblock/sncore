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

public partial class AccountFeedView : Page
{
    private TransitFeature mAccountFeedFeature = null;
    private TransitAccountFeed mAccountFeed = null;

    public TransitFeature LatestAccountFeedFeature
    {
        get
        {
            if (mAccountFeedFeature == null)
            {
                object[] args = { "AccountFeed", RequestId };
                mAccountFeedFeature = SessionManager.GetCachedItem<TransitFeature>(
                    SessionManager.SystemService, "FindLatestFeature", args);
            }
            return mAccountFeedFeature;
        }
    }

    public TransitAccountFeed AccountFeed
    {
        get
        {
            if (mAccountFeed == null)
            {
                object[] args = { SessionManager.Ticket, RequestId };
                mAccountFeed = SessionManager.GetCachedItem<TransitAccountFeed>(
                    SessionManager.SyndicationService, "GetAccountFeedById", args);
            }
            return mAccountFeed;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            TransitAccountFeed f = AccountFeed;
            labelFeed.Text = Renderer.Render(f.Name);
            labelFeed.NavigateUrl = f.LinkUrl;
            labelFeedDescription.Text = Renderer.Render(f.Description);

            licenseView.AccountId = f.AccountId;

            GetDataPublish(sender, e);

            object[] f_args = { f.FeedType };
            TransitFeedType t = SessionManager.GetCachedItem<TransitFeedType>(
                SessionManager.SyndicationService, "GetFeedTypeByName", f_args);
            gridManage.RepeatColumns = t.SpanColumns;
            gridManage.RepeatRows = t.SpanRows;

            object[] a_args = { f.AccountId };
            TransitAccount a = SessionManager.GetCachedItem<TransitAccount>(
                SessionManager.AccountService, "GetAccountById", a_args);

            labelAccountName.Text = Renderer.Render(a.Name);
            linkAccount.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);

            this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(f.Name));

            object[] args = { RequestId };
            gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
                SessionManager.SyndicationService, "GetAccountFeedItemsCountById", args);
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            GetDataFeature(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(f.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void GetDataPublish(object sender, EventArgs e)
    {
        linkPublish.Text = AccountFeed.Publish ? "&#187; Do Not Publish" : "&#187; Publish";
        linkPublishImgs.Text = AccountFeed.PublishImgs ? "&#187; Do Not Publish Pictures" : "&#187; Publish Pictures";
    }

    void GetDataFeature(object sender, EventArgs e)
    {
        if (SessionManager.IsAdministrator)
        {
            linkFeature.Text = (LatestAccountFeedFeature != null)
                ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountFeedFeature.Created).ToString("d"))
                : "Feature &#187; Never Featured";
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        object[] args = { RequestId, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountFeedItem>(
            SessionManager.SyndicationService, "GetAccountFeedItemsById", args);
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
            linkDeleteFeatures.Visible = (LatestAccountFeedFeature != null);
        }
        base.OnPreRender(e);
    }

    public void feature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature other users.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountFeed";
        t_feature.DataRowId = RequestId;
        SessionManager.SystemService.CreateOrUpdateFeature(SessionManager.Ticket, t_feature);
        GetDataFeature(sender, e);
        panelAdminUpdate.Update();
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature feeds.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountFeed";
        t_feature.DataRowId = RequestId;
        SessionManager.SystemService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        GetDataFeature(sender, e);
        panelAdminUpdate.Update();
    }

    public void publish_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to toggle feed publishing.");
        }

        AccountFeed.Publish = !AccountFeed.Publish;
        SessionManager.SyndicationService.CreateOrUpdateAccountFeed(SessionManager.Ticket, AccountFeed);
        GetDataPublish(sender, e);
        panelAdminUpdate.Update();
    }

    public void publishImgs_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to toggle feed picture publishing.");
        }

        AccountFeed.PublishImgs = !AccountFeed.PublishImgs;
        SessionManager.SyndicationService.CreateOrUpdateAccountFeed(SessionManager.Ticket, AccountFeed);
        GetDataPublish(sender, e);
        panelAdminUpdate.Update();
    }

    public string GetDescription(string value)
    {
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUrl.TrimEnd("/".ToCharArray()) + "/AccountFeedItemPicture.aspx?Src={url}", UriKind.Absolute);
        return Renderer.CleanHtml(value,
            Uri.IsWellFormedUriString(AccountFeed.LinkUrl, UriKind.Absolute) ? new Uri(AccountFeed.LinkUrl) : null,
            imgrewriteuri);
    }
}

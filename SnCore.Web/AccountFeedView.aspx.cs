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
                mAccountFeedFeature = SessionManager.GetInstance<TransitFeature, string, int>(
                    "AccountFeed", RequestId, SessionManager.ObjectService.FindLatestFeature);
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
                mAccountFeed = SessionManager.GetInstance<TransitAccountFeed, int>(
                    RequestId, SessionManager.SyndicationService.GetAccountFeedById);
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

            TransitFeedType t = SessionManager.GetInstance<TransitFeedType, string>(
                f.FeedType, SessionManager.SyndicationService.GetFeedTypeByName);

            gridManage.RepeatColumns = t.SpanColumns;
            gridManage.RepeatRows = t.SpanRows;

            TransitAccount a = SessionManager.GetInstance<TransitAccount, int>(
                f.AccountId, SessionManager.AccountService.GetAccountById);

            labelAccountName.Text = Renderer.Render(a.Name);
            linkAccount.HRef = string.Format("AccountView.aspx?id={0}", a.Id);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", a.PictureId);

            this.Title = string.Format("{0}'s {1}", Renderer.Render(a.Name), Renderer.Render(f.Name));

            gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountFeedItem, int>(
                RequestId, SessionManager.SyndicationService.GetAccountFeedItemsCount);
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            GetDataFeature(sender, e);

            redirect.AccountId = f.AccountId;
            redirect.TargetUri = string.Format("AccountFeedView.aspx?id={0}", f.Id);

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
        linkPublishMedia.Text = AccountFeed.PublishMedia ? "&#187; Do Not Publish Media" : "&#187; Publish Media";
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
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountFeedItem, int>(
            RequestId, options, SessionManager.SyndicationService.GetAccountFeedItems);
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
        SessionManager.CreateOrUpdate<TransitFeature>(
            t_feature, SessionManager.ObjectService.CreateOrUpdateFeature);
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
        SessionManager.ObjectService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        SessionManager.InvalidateCache<TransitFeature>();
        GetDataFeature(sender, e);
        panelAdminUpdate.Update();
    }

    public void publish_Click(object sender, EventArgs e)
    {
        AccountFeed.Publish = !AccountFeed.Publish;
        SessionManager.CreateOrUpdate<TransitAccountFeed>(
            AccountFeed, SessionManager.SyndicationService.CreateOrUpdateAccountFeed);
        GetDataPublish(sender, e);
        panelAdminUpdate.Update();
    }

    public void publishImgs_Click(object sender, EventArgs e)
    {
        AccountFeed.PublishImgs = !AccountFeed.PublishImgs;
        SessionManager.CreateOrUpdate<TransitAccountFeed>(
            AccountFeed, SessionManager.SyndicationService.CreateOrUpdateAccountFeed);
        GetDataPublish(sender, e);
        panelAdminUpdate.Update();
    }

    public void publishMedia_Click(object sender, EventArgs e)
    {
        AccountFeed.PublishMedia = !AccountFeed.PublishMedia;
        SessionManager.CreateOrUpdate<TransitAccountFeed>(
            AccountFeed, SessionManager.SyndicationService.CreateOrUpdateAccountFeed);
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

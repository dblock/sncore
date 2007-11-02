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
using SnCore.Tools.Web.Html;

public partial class AccountFeedItemView : Page
{
    public AccountFeedItemView()
    {
        mIsMobileEnabled = true;
    }

    private TransitFeature mAccountFeedItemFeature = null;
    private TransitFeature mAccountFeedItemByAccountFeedIdFeature = null;

    private TransitAccountFeedItem GetAccountFeedItem()
    {
        return SessionManager.GetInstance<TransitAccountFeedItem, int>(
            RequestId, SessionManager.SyndicationService.GetAccountFeedItemById);
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountFeedItem tfi = GetAccountFeedItem();

            if (tfi == null)
            {
                Response.Redirect("AccountFeedItemsView.aspx");
                return;
            }

            licenseView.AccountId = tfi.AccountId;

            labelAccountName.Text = Renderer.Render(tfi.AccountName);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", tfi.AccountPictureId);

            this.Title = string.Format("{0}'s {1} in {2}",
                Renderer.Render(tfi.AccountName),
                Renderer.Render(tfi.Title),
                Renderer.Render(tfi.AccountFeedName));

            FeedTitle.Text = Renderer.Render(tfi.AccountFeedName);
            FeedItemCreated.Text = base.Adjust(tfi.Created).ToString();
            FeedItemTitle.NavigateUrl = tfi.Link;

            linkAccountView.HRef = string.Format("AccountView.aspx?id={0}", tfi.AccountId);
            FeedTitle.NavigateUrl = string.Format("AccountFeedView.aspx?id={0}", tfi.AccountFeedId);
            FeedXPosted.NavigateUrl = Render(tfi.Link);

            FeedItemTitle.Text = Renderer.Render(tfi.Title);

            Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");

            HtmlWriterOptions options = new HtmlWriterOptions();
            options.BaseHref = Uri.IsWellFormedUriString(tfi.AccountFeedLinkUrl, UriKind.Absolute) ? new Uri(tfi.AccountFeedLinkUrl) : null;
            options.RewriteImgSrc = imgrewriteuri;
            FeedItemDescription.Text = Renderer.CleanHtml(tfi.Description, options);

            FeedItemComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
                typeof(AccountFeedItem).Name, RequestId, SessionManager.DiscussionService.GetOrCreateDiscussionId);

            GetDataFeature(sender, e);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.AccountFeedName, Request, string.Format("AccountFeedView.aspx?id={0}", tfi.AccountFeedId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.Title, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public TransitFeature LatestAccountFeedItemByAccountFeedIdFeature
    {
        get
        {
            if (mAccountFeedItemByAccountFeedIdFeature == null)
            {
                TransitAccountFeedItem tfi = GetAccountFeedItem();

                if (tfi != null)
                {
                    mAccountFeedItemByAccountFeedIdFeature = SessionManager.GetInstance<TransitFeature, int>(
                        tfi.AccountFeedId, SessionManager.SyndicationService.GetLatestAccountFeedItemFeatureByAccountFeedId);
                }
            }

            return mAccountFeedItemByAccountFeedIdFeature;
        }
    }

    public TransitFeature LatestAccountFeedItemFeature
    {
        get
        {
            if (mAccountFeedItemFeature == null)
            {
                mAccountFeedItemFeature = SessionManager.GetInstance<TransitFeature, string, int>(
                    "AccountFeedItem", RequestId, SessionManager.ObjectService.FindLatestFeature);
            }

            return mAccountFeedItemFeature;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        linkFeature.Visible = SessionManager.IsAdministrator;
        linkDeleteFeatures.Visible = SessionManager.IsAdministrator && (LatestAccountFeedItemFeature != null);
        base.OnPreRender(e);
    }

    void GetDataFeature(object sender, EventArgs e)
    {
        if (SessionManager.IsAdministrator)
        {

            linkFeature.Text = (LatestAccountFeedItemFeature != null)
                ? string.Format("Feature &#187; Last on {0}", Adjust(LatestAccountFeedItemFeature.Created).ToString("d"))
                : "Feature &#187; Never Featured";

            labelAccountFeedItemByAccountFeedIdFeature.Text = (LatestAccountFeedItemByAccountFeedIdFeature != null)
                ? string.Format("&#187; Last Blog Feature on {0}", Adjust(LatestAccountFeedItemByAccountFeedIdFeature.Created).ToString("d"))
                : "&#187; Blog Posts Never Featured";
        }
    }

    public void feature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature feed items.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountFeedItem";
        t_feature.DataRowId = RequestId;
        SessionManager.CreateOrUpdate<TransitFeature>(
            t_feature, SessionManager.ObjectService.CreateOrUpdateFeature);
        GetDataFeature(sender, e);
        panelAdmin.Update();
    }

    public void deletefeature_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            // avoid round-trip
            throw new Exception("You must be an administrator to feature feed items.");
        }

        TransitFeature t_feature = new TransitFeature();
        t_feature.DataObjectName = "AccountFeedItem";
        t_feature.DataRowId = RequestId;
        SessionManager.ObjectService.DeleteAllFeatures(SessionManager.Ticket, t_feature);
        SessionManager.InvalidateCache<TransitFeature>();
        GetDataFeature(sender, e);
        panelAdmin.Update();
    }
}

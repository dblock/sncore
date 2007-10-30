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
using SnCore.SiteMap;

public partial class AccountFeedItemView : Page
{
    private SyndicationService.TransitAccountFeedItem GetAccountFeedItem()
    {
        return SessionManager.GetInstance<SyndicationService.TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, int>(
            RequestId, SessionManager.SyndicationService.GetAccountFeedItemById);
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SyndicationService.TransitAccountFeedItem tfi = GetAccountFeedItem();

            if (tfi == null)
            {
                Response.Redirect("AccountFeedItemsView.aspx");
                return;
            }

            this.Title = string.Format("{0}'s {1} in {2}",
                Renderer.Render(tfi.AccountName),
                Renderer.Render(tfi.Title),
                Renderer.Render(tfi.AccountFeedName));

            feeditemCreated.Text = SessionManager.Adjust(tfi.Created).ToString();
            feeditemXPosted.NavigateUrl = Renderer.Render(tfi.Link);

            Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
            feeditemDescription.Text = Renderer.CleanHtml(tfi.Description,
                Uri.IsWellFormedUriString(tfi.AccountFeedLinkUrl, UriKind.Absolute) ? new Uri(tfi.AccountFeedLinkUrl) : null,
                imgrewriteuri);

            int discussion_id = SessionManager.GetCount<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, string, int>(
                "AccountFeedItem", RequestId, SessionManager.DiscussionService.GetOrCreateDiscussionId);

            linkDiscussion.NavigateUrl = string.Format("DiscussionView.aspx?id={0}", discussion_id);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.AccountFeedName, Request, string.Format("AccountFeedView.aspx?id={0}", tfi.AccountFeedId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.Title, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }
}

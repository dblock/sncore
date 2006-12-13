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

public partial class AccountFeedItemView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                object[] args = { SessionManager.Ticket, RequestId };
                TransitAccountFeedItem tfi = SessionManager.GetCachedItem<TransitAccountFeedItem>(
                    SyndicationService, "GetAccountFeedItemById", args);

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

                Uri imgrewriteuri = new Uri(SessionManager.WebsiteUrl.TrimEnd("/".ToCharArray()) + "/AccountFeedItemPicture.aspx?Src={url}", UriKind.Absolute);
                FeedItemDescription.Text = Renderer.CleanHtml(tfi.Description,
                    Uri.IsWellFormedUriString(tfi.Link, UriKind.Absolute) ? new Uri(tfi.Link) : null,
                    imgrewriteuri);

                object[] d_args = { RequestId };
                FeedItemComments.DiscussionId = SessionManager.GetCachedCollectionCount(
                    DiscussionService, "GetAccountFeedItemDiscussionId", d_args);

                SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
                sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
                sitemapdata.Add(new SiteMapDataAttributeNode(tfi.AccountFeedName, Request, string.Format("AccountFeedView.aspx?id={0}", tfi.AccountFeedId)));
                sitemapdata.Add(new SiteMapDataAttributeNode(tfi.Title, Request.Url));
                StackSiteMap(sitemapdata);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

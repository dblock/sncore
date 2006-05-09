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

public partial class AccountFeedItemView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                TransitAccountFeedItem tfi = SyndicationService.GetAccountFeedItemById(
                    SessionManager.Ticket, RequestId);

                labelAccountName.Text = Renderer.Render(tfi.AccountName);
                imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", tfi.AccountPictureId);

                this.Title = string.Format("{0}'s {1} in {2}", 
                    Renderer.Render(tfi.AccountName), 
                    Renderer.Render(tfi.Title), 
                    Renderer.Render(tfi.AccountFeedName));

                linkAccountFeedItem.Text = Renderer.Render(tfi.Title);
                FeedTitle.Text = linkAccountFeed.Text = Renderer.Render(tfi.AccountFeedName);
                linkAccount.Text = Renderer.Render(tfi.AccountName);
                FeedItemCreated.Text = base.Adjust(tfi.Created).ToString();
                FeedItemTitle.NavigateUrl = tfi.Link;

                linkAccountView.HRef = linkAccount.NavigateUrl = string.Format("AccountView.aspx?id={0}", tfi.AccountId);
                FeedTitle.NavigateUrl = linkAccountFeed.NavigateUrl = string.Format("AccountFeedView.aspx?id={0}", tfi.AccountFeedId);
                FeedXPosted.NavigateUrl = Render(tfi.Link);

                FeedItemTitle.Text = Renderer.Render(tfi.Title);
                FeedItemDescription.Text = Renderer.CleanHtml(tfi.Description, 
                    Uri.IsWellFormedUriString(tfi.Link, UriKind.Absolute) ? new Uri(tfi.Link) : null);

                FeedItemComments.DiscussionId = DiscussionService.GetAccountFeedItemDiscussionId(RequestId);
                FeedItemComments.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

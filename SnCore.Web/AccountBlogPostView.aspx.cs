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

public partial class AccountBlogPostView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountBlogPost tfi = SessionManager.GetInstance<TransitAccountBlogPost, int>(
                RequestId, SessionManager.BlogService.GetAccountBlogPostById);

            labelAccountName.Text = Renderer.Render(tfi.AccountName);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", tfi.AccountPictureId);

            this.Title = string.Format("{0}'s {1} in {2}",
                Renderer.Render(tfi.AccountName),
                Renderer.Render(tfi.Title),
                Renderer.Render(tfi.AccountBlogName));

            licenseView.AccountId = tfi.AccountId;

            BlogTitle.Text = Renderer.Render(tfi.AccountBlogName);
            BlogPostCreated.Text = base.Adjust(tfi.Created).ToString();

            linkEdit.Visible = tfi.CanEdit;
            linkEdit.NavigateUrl = string.Format("AccountBlogPost.aspx?bid={0}&id={1}&ReturnUrl={2}",
                tfi.AccountBlogId, tfi.Id, Renderer.UrlEncode(Request.Url.PathAndQuery));
            linkDelete.Visible = tfi.CanDelete;

            linkAccountView.HRef = string.Format("AccountView.aspx?id={0}", tfi.AccountId);
            BlogTitle.NavigateUrl = string.Format("AccountBlogView.aspx?id={0}", tfi.AccountBlogId);

            BlogPostTitle.Text = Renderer.Render(string.IsNullOrEmpty(tfi.Title) ? "Untitled" : tfi.Title);
            BlogPostBody.Text = RenderEx(tfi.Body);

            BlogPostComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, int>(
                RequestId, SessionManager.DiscussionService.GetOrCreateAccountBlogPostDiscussionId);
            BlogPostComments.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.AccountBlogName, Request, string.Format("AccountBlogView.aspx?id={0}", tfi.AccountBlogId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(tfi.Title, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        TransitAccountBlogPost tfi = SessionManager.GetInstance<TransitAccountBlogPost, int>(
            RequestId, SessionManager.BlogService.GetAccountBlogPostById);
        SessionManager.BlogService.DeleteAccountBlogPost(SessionManager.Ticket, tfi.Id);
        Redirect(string.Format("AccountBlogView.aspx?id={0}", tfi.AccountBlogId));
    }
}

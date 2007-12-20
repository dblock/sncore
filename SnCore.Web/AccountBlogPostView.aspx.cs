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
            TransitAccountBlogPost post = SessionManager.GetInstance<TransitAccountBlogPost, int>(
                RequestId, SessionManager.BlogService.GetAccountBlogPostById);

            if (post == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            labelAccountName.Text = Renderer.Render(post.AccountName);
            imageAccount.Src = string.Format("AccountPictureThumbnail.aspx?id={0}", post.AccountPictureId);

            this.Title = string.Format("{0}'s {1} in {2}",
                Renderer.Render(post.AccountName),
                Renderer.Render(post.Title),
                Renderer.Render(post.AccountBlogName));

            licenseView.AccountId = post.AccountId;

            BlogTitle.Text = Renderer.Render(post.AccountBlogName);
            BlogPostCreated.Text = base.Adjust(post.Created).ToString();

            linkEdit.Visible = post.CanEdit;
            linkEdit.NavigateUrl = string.Format("AccountBlogPost.aspx?bid={0}&id={1}&ReturnUrl={2}",
                post.AccountBlogId, post.Id, Renderer.UrlEncode(Request.Url.PathAndQuery));

            linkMove.Visible = post.CanEdit && post.CanDelete;
            linkMove.NavigateUrl = string.Format("AccountBlogPostMove.aspx?id={0}&ReturnUrl={1}",
                post.Id, Renderer.UrlEncode(Request.Url.PathAndQuery));

            linkDelete.Visible = post.CanDelete;

            linkAccountView.HRef = string.Format("AccountView.aspx?id={0}", post.AccountId);
            BlogTitle.NavigateUrl = string.Format("AccountBlogView.aspx?id={0}", post.AccountBlogId);

            BlogPostTitle.Text = Renderer.Render(string.IsNullOrEmpty(post.Title) ? "Untitled" : post.Title);
            BlogPostBody.Text = RenderEx(post.Body);

            BlogPostComments.DiscussionId = SessionManager.GetCount<TransitDiscussion, string, int>(
                typeof(AccountBlogPost).Name, RequestId, SessionManager.DiscussionService.GetOrCreateDiscussionId);
            BlogPostComments.DataBind();

            if (!post.EnableComments) BlogPostComments.PostNewText = string.Empty;

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(post.AccountBlogName, Request, string.Format("AccountBlogView.aspx?id={0}", post.AccountBlogId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(post.Title, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        TransitAccountBlogPost post = SessionManager.GetInstance<TransitAccountBlogPost, int>(
            RequestId, SessionManager.BlogService.GetAccountBlogPostById);
        SessionManager.Delete<TransitAccountBlogPost>(post.Id, SessionManager.BlogService.DeleteAccountBlogPost);
        Redirect(string.Format("AccountBlogView.aspx?id={0}", post.AccountBlogId));
    }
}

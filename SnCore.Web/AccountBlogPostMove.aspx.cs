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
using SnCore.WebControls;
using System.Collections.Generic;

public partial class AccountBlogPostMove : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitAccountBlogPost t_post = SessionManager.BlogService.GetAccountBlogPostById(
                SessionManager.Ticket, RequestId);

            // discussions
            listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussions(
                SessionManager.Ticket, null);
            listDiscussions.DataBind();

            // blogs
            List<TransitAccountBlog> t_blogs = SessionManager.BlogService.GetAccountBlogs(
                SessionManager.Ticket, SessionManager.AccountId, null);
            t_blogs.AddRange(SessionManager.BlogService.GetAuthoredAccountBlogs(
                SessionManager.Ticket, SessionManager.AccountId, null));
            listBlogs.DataSource = t_blogs;
            listBlogs.DataBind();

            // ----

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(t_post.AccountBlogName, Request, string.Format("AccountBlogView.aspx?id={0}", t_post.AccountBlogId)));
            sitemapdata.Add(new SiteMapDataAttributeNode(t_post.Title, Request, string.Format("AccountBlogPostView.aspx?id={0}", t_post.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Move", Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    public void moveToBlog_Click(object sender, EventArgs args)
    {
        SessionManager.BlogService.MoveAccountBlogPost(
            SessionManager.Ticket,
            RequestId,
            int.Parse(listBlogs.SelectedValue));

        SessionManager.InvalidateCache<TransitAccountBlogPost>();
        SessionManager.InvalidateCache<TransitAccountBlog>();

        Redirect(string.Format("AccountBlogPostView.aspx?id={0}", RequestId));
    }

    public void moveToDiscussion_Click(object sender, EventArgs args)
    {
        int id = SessionManager.DiscussionService.MoveAccountBlogPost(
            SessionManager.Ticket,
            RequestId,
            int.Parse(listDiscussions.SelectedValue));

        TransitDiscussionPost t_post = SessionManager.DiscussionService.GetDiscussionPostById(
            SessionManager.Ticket, id);

        SessionManager.InvalidateCache<TransitDiscussion>();
        SessionManager.InvalidateCache<TransitDiscussionPost>();
        SessionManager.InvalidateCache<TransitAccountBlog>();
        SessionManager.InvalidateCache<TransitAccountBlogPost>();

        Redirect(string.Format("DiscussionThreadView.aspx?id={0}&did={1}", 
            t_post.DiscussionThreadId, t_post.DiscussionId));
    }
}

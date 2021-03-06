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

public partial class DiscussionPostMove : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TransitDiscussionPost t_post = SessionManager.DiscussionService.GetDiscussionPostById(
                SessionManager.Ticket, RequestId);

            TransitDiscussion t_discussion = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, t_post.DiscussionId);

            // discussions

            if (t_discussion.ObjectId != 0 && ! string.IsNullOrEmpty(t_discussion.ParentObjectType))
            {
                listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussionsByObjectId(
                    SessionManager.Ticket, t_discussion.ParentObjectType, t_discussion.ObjectId, null);
            }
            else
            {
                listDiscussions.DataSource = SessionManager.DiscussionService.GetDiscussions(
                    SessionManager.Ticket, null);
            }

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
            sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(t_discussion.Name, Request, string.Format("DiscussionView.aspx?id={0}", t_discussion.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode(t_post.Subject, Request, string.Format("DiscussionThreadView.aspx?id={0}&did={1}", t_post.DiscussionThreadId, t_discussion.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Move", Request.Url));
            StackSiteMap(sitemapdata);

            ReportWarning("Warning! This will move the post out of its thread, including all its replies. " +
                "This cannot be undone.");
        }
    }

    public void moveToBlog_Click(object sender, EventArgs args)
    {
        int blogpost_id = SessionManager.BlogService.MoveDiscussionPost(
            SessionManager.Ticket,
            RequestId,
            int.Parse(listBlogs.SelectedValue));

        SessionManager.InvalidateCache<TransitDiscussion>();
        SessionManager.InvalidateCache<TransitDiscussionPost>();
        SessionManager.InvalidateCache<TransitAccountBlog>();
        SessionManager.InvalidateCache<TransitAccountBlogPost>();

        Redirect(string.Format("AccountBlogPostView.aspx?id={0}", blogpost_id));
    }

    public void moveToDiscussion_Click(object sender, EventArgs args)
    {
        int discussion_id = int.Parse(listDiscussions.SelectedValue);
        int thread_id = SessionManager.DiscussionService.MoveDiscussionPost(
            SessionManager.Ticket,
            RequestId,
            discussion_id);

        SessionManager.InvalidateCache<TransitDiscussion>();
        SessionManager.InvalidateCache<TransitDiscussionPost>();

        Redirect(string.Format("DiscussionThreadView.aspx?id={0}&did={1}", 
            thread_id, discussion_id));
    }
}

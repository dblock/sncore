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

public partial class DiscussionThreadView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        discussionThreadView.OnGetDataSource += new EventHandler(discussionThreadView_OnGetDataSource);
        if (!IsPostBack)
        {
            if (RequestId == 0)
            {
                Redirect("Default.aspx");
                return;
            }

            DiscussionService.TransitDiscussionThread t = SessionManager.DiscussionService.GetDiscussionThreadById(
                SessionManager.Ticket, RequestId);

            if (t == null)
            {
                ReportWarning("Discussion has been deleted or invalid discussion thread.");
                return;
            }

            DiscussionService.TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, t.DiscussionId);

            this.Title = Renderer.Render(td.Name);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            sitemapdata.Add(new SiteMapDataAttributeNode(td.ParentObjectName, Request, td.ParentObjectUri));
            if (td.Personal)
            {
                string uri = SessionManager.DiscussionService.GetDiscussionRedirectUri(
                    SessionManager.Ticket, td.Id);

                if (!string.IsNullOrEmpty(uri))
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("...", Request, uri));
                }
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
            }

            sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", td.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Thread", Request.Url));
            StackSiteMap(sitemapdata);

            discussionDescription.Text = Renderer.Render(td.Description);

            GetData(sender, e);
        }
    }

    public int DiscussionThreadId
    {
        get
        {
            return RequestId;
        }
    }

    protected void GetData(object sender, EventArgs e)
    {
        if (DiscussionThreadId <= 0)
            return;

        discussionThreadView.CurrentPageIndex = 0;
        discussionThreadView.VirtualItemCount = SessionManager.DiscussionService.GetDiscussionThreadPostsCount(
            SessionManager.Ticket, DiscussionThreadId);
        discussionThreadView_OnGetDataSource(sender, e);
        discussionThreadView.DataBind();
    }

    void discussionThreadView_OnGetDataSource(object sender, EventArgs e)
    {
        discussionThreadView.DataSource = SessionManager.DiscussionService.GetDiscussionThreadPosts(
            SessionManager.Ticket, DiscussionThreadId);
    }

    public int OuterWidth
    {
        get
        {
            return 200;
        }
    }
}

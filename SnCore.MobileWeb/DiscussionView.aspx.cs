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
using System.Collections.Generic;

public partial class DiscussionView : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DiscussionService.TransitDiscussion td = SessionManager.DiscussionService.GetDiscussionById(
                SessionManager.Ticket, RequestId);

            if (td == null)
            {
                Response.StatusCode = 404;
                Response.End();
                return;
            }

            this.Title = Renderer.Render(td.Name);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();

            if (td.Personal)
            {
                string uri = SessionManager.DiscussionService.GetDiscussionRedirectUri(
                    SessionManager.Ticket, td.Id);

                if (!string.IsNullOrEmpty(uri))
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("...", Request, uri));
                    linkBack.NavigateUrl = uri;
                }
                else
                {
                    sitemapdata.Add(new SiteMapDataAttributeNode("Discussions", Request, "DiscussionsView.aspx"));
                    linkBack.NavigateUrl = "DiscussionsView.aspx";
                }
            }

            sitemapdata.Add(new SiteMapDataAttributeNode(td.Name, Request, string.Format("DiscussionView.aspx?id={0}", RequestId)));
            StackSiteMap(sitemapdata);

            GetData(sender, e);
        }
    }

    public bool IsThreaded
    {
        get
        {
            DiscussionViewTypes type = GetDiscussionViewType();
            switch (type)
            {
                case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
                case DiscussionViewTypes.ThreadedWithNewestOnTop:
                    return true;
            }

            return false;
        }
    }

    public DiscussionViewTypes GetDiscussionViewType()
    {
        DiscussionService.TransitDiscussion d = GetDiscussion();
        DiscussionViewTypes type = DiscussionViewTypes.ThreadedWithNewestOnTop;
        if (!string.IsNullOrEmpty(d.DefaultView))
        {
            type = (DiscussionViewTypes)Enum.Parse(typeof(DiscussionViewTypes), d.DefaultView);
        }
        return type;
    }

    public bool IsFull
    {
        get
        {
            DiscussionViewTypes type = GetDiscussionViewType();
            switch (type)
            {
                case DiscussionViewTypes.FlatFullWithNewestOnTop:
                case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
                    return true;
            }

            return false;
        }
    }

    public int DiscussionId
    {
        get
        {
            return RequestId;
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        if (DiscussionId <= 0)
            return;

        DiscussionService.TransitDiscussion d = GetDiscussion();
        discussionDescription.Text = Renderer.Render(d.Description);
        DiscussionService.ServiceQueryOptions options = new DiscussionService.ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;

        DiscussionViewTypes type = GetDiscussionViewType();
        switch (type)
        {
            case DiscussionViewTypes.FlatWithNewestOnTop:
            case DiscussionViewTypes.FlatFullWithNewestOnTop:
                gridManage.DataSource = SessionManager.GetCollection<DiscussionService.TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int>(
                    DiscussionId, options, SessionManager.DiscussionService.GetLatestDiscussionPostsById);
                break;
            case DiscussionViewTypes.ThreadedWithNewestOnTop:
            case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
            default:
                gridManage.DataSource = SessionManager.GetCollection<DiscussionService.TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int>(
                    DiscussionId, options, SessionManager.DiscussionService.GetDiscussionThreadsByDiscussionId);
                break;
        }
    }

    public DiscussionService.TransitDiscussion GetDiscussion()
    {
        return SessionManager.GetInstance<DiscussionService.TransitDiscussion, DiscussionService.ServiceQueryOptions, int>(
            DiscussionId, SessionManager.DiscussionService.GetDiscussionById);
    }

    void GetData(object sender, EventArgs e)
    {
        if (DiscussionId <= 0)
            return;

        gridManage.CurrentPageIndex = 0;

        DiscussionViewTypes type = GetDiscussionViewType();
        switch (type)
        {
            case DiscussionViewTypes.FlatWithNewestOnTop:
            case DiscussionViewTypes.FlatFullWithNewestOnTop:
                gridManage.VirtualItemCount = SessionManager.GetCount<DiscussionService.TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int>(
                    DiscussionId, SessionManager.DiscussionService.GetDiscussionPostsCount);
                break;
            case DiscussionViewTypes.ThreadedWithNewestOnTop:
            case DiscussionViewTypes.ThreadedFullWithNewestOnTop:
            default:
                gridManage.VirtualItemCount = SessionManager.GetCount<DiscussionService.TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int>(
                    DiscussionId, SessionManager.DiscussionService.GetDiscussionThreadsCountByDiscussionId);
                break;
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();

        linkBack.Text = (gridManage.VirtualItemCount == 0 ? "No Posts &#187; Back" : "&#187; Back");
    }

    public int OuterWidth
    {
        get
        {
            return 200;
        }
    }
}

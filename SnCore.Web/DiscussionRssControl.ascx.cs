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
using Wilco.Web.UI;

public partial class DiscussionRssControl : Control
{
    public int DiscussionId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "DiscussionId", 0);
        }
        set
        {
            ViewState["DiscussionId"] = value;
        }
    }

    private TransitDiscussion mDiscussion = null;

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    private TransitDiscussion Discussion
    {
        get
        {
            if (mDiscussion == null)
            {
                mDiscussion = SessionManager.DiscussionService.GetDiscussionById(
                    SessionManager.Ticket, DiscussionId);
            }
            return mDiscussion;
        }
    }

    public string Description
    {
        get
        {
            return Renderer.Render(Discussion.Description);
        }
    }

    public string RssControlTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} {1}",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Discussion.Name));
        }
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, string.Format("DiscussionView.aspx?id={0}",
                DiscussionId)).ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = 50;
            options.PageNumber = 0;
            rssRepeater.DataSource = SessionManager.DiscussionService.GetLatestDiscussionPostsById(
                SessionManager.Ticket, DiscussionId, options);
            rssRepeater.DataBind();
        }
    }
}

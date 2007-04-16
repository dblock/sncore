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

public partial class DiscussionThreadsRss : Page
{
    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    public string RssTitle
    {
        get
        {
            return Renderer.Render(string.Format("{0} Discussion Posts",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, "DiscussionThreadsView.aspx").ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageSize = 50;
            options.PageNumber = 0;
            rssRepeater.DataSource = SessionManager.GetCollection<TransitDiscussionPost>(
                options, SessionManager.DiscussionService.GetDiscussionThreads);
            rssRepeater.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}

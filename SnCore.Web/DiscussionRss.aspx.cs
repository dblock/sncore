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

public partial class DiscussionRss : Page
{
    private string mWebsiteUrl = string.Empty;
    private TransitDiscussion mDiscussion = null;

    public string WebsiteUrl
    {
        get
        {
            if (string.IsNullOrEmpty(mWebsiteUrl))
            {
                mWebsiteUrl = SystemService.GetConfigurationByNameWithDefault(
                    "SnCore.WebSite.Url", "http://localhost/SnCoreWeb").Value;
            }

            return mWebsiteUrl;
        }
    }

    private TransitDiscussion Discussion
    {
        get
        {
            if (mDiscussion == null)
            {
                mDiscussion = DiscussionService.GetDiscussionById(RequestId);
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

    public new string Title
    {
        get
        {
            return Renderer.Render(Discussion.Name);
        }
    }

    public string Link
    {
        get
        {
            return WebsiteUrl + string.Format("/DiscussionView.aspx?id={0}", Discussion.Id);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageSize = 50;
                options.PageNumber = 0;
                rssRepeater.DataSource = DiscussionService.GetDiscussionPosts(
                    SessionManager.Ticket, Discussion.Id, options);
                rssRepeater.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }
}

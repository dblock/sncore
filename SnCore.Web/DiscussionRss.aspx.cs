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
using NHibernate.Exceptions;

public partial class DiscussionRss : Page
{
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
                mDiscussion = SessionManager.DiscussionService.GetDiscussionById(RequestId);
            }
            return mDiscussion;
        }
    }

    public string Description
    {
        get
        {
            try
            {
                return Renderer.Render(Discussion.Description);
            }
            catch (Exception ex)
            {
                return Renderer.Render(ex.Message);
            }
        }
    }

    public string RssTitle
    {
        get
        {
            try
            {
                return Renderer.Render(string.Format("{0} {1}",
                    SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore"), Discussion.Name));
            }
            catch (Exception ex)
            {
                return Renderer.Render(ex.Message);
            }
        }
    }

    public string Link
    {
        get
        {
            try
            {
                return WebsiteUrl + string.Format("/DiscussionView.aspx?id={0}", Discussion.Id);
            }
            catch (Exception ex)
            {
                return Renderer.Render(ex.Message);
            }
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
                rssRepeater.DataSource = SessionManager.DiscussionService.GetLatestDiscussionPostsById(Discussion.Id, options);
                rssRepeater.DataBind();
            }
        }
        catch (NHibernate.ObjectNotFoundException ex)
        {
            Response.StatusCode = 404;
            Response.StatusDescription = Renderer.Render(ex.Message);
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

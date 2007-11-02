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
using SnCore.BackEndServices;
using SnCore.Services;
using SnCore.Tools.Web.Html;

public partial class AccountFeedRss : Page
{
    private TransitAccountFeed mAccountFeed = null;

    public TransitAccountFeed AccountFeed
    {
        get
        {
            if (mAccountFeed == null)
            {
                mAccountFeed = SessionManager.GetInstance<TransitAccountFeed, int>(
                    RequestId, SessionManager.SyndicationService.GetAccountFeedById);
            }
            return mAccountFeed;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 10;
            rssRepeater.DataSource = SessionManager.GetCollection<TransitAccountFeedItem, int>(
            RequestId, options, SessionManager.SyndicationService.GetAccountFeedItems);
            rssRepeater.DataBind();
        }
    }

    public string WebsiteUrl
    {
        get
        {
            return SessionManager.WebsiteUrl;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Response.ContentType = "text/xml";
        base.OnPreRender(e);
    }

    public string GetSummary(string summary, string link)
    {
        Uri uri = null;
        Uri.TryCreate(link, UriKind.Absolute, out uri);
        Uri imgrewriteuri = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        HtmlWriterOptions options = new HtmlWriterOptions();
        options.BaseHref = uri;
        options.RewriteImgSrc = imgrewriteuri;
        return Renderer.CleanHtml(summary, options);
    }

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, string.Format(
                "AccountFeedView.aspx?id={0}", AccountFeed.Id)).ToString();
        }
    }
}

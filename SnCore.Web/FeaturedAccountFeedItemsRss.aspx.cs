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
using System.Collections.Generic;
using SnCore.Tools.Web.Html;

public partial class FeaturedAccountFeedItemsRss : Page
{
    public string Name
    {
        get
        {
            return Renderer.Render(string.Format("{0} Featured Blog Posts",
                SessionManager.GetCachedConfiguration("SnCore.Title", "SnCore")));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ServiceQueryOptions options = new ServiceQueryOptions();
            options.PageNumber = 0;
            options.PageSize = 10;
            rssRepeater.DataSource = SessionManager.GetCollection<TransitFeature, string>(
                "AccountFeedItem", options, SessionManager.ObjectService.GetFeatures);
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

    public string Link
    {
        get
        {
            return new Uri(SessionManager.WebsiteUri, "FeaturedAccountFeedItemsView.aspx").ToString();
        }
    }

    public TransitAccountFeedItem GetAccountFeedItem(int id)
    {
        return SessionManager.GetInstance<TransitAccountFeedItem, int>(
            id, SessionManager.SyndicationService.GetAccountFeedItemById);
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
}

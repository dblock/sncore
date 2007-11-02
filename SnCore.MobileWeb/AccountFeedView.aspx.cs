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
using SnCore.Tools.Web.Html;
using System.Drawing;

public partial class AccountFeedView : Page
{
    private SyndicationService.TransitAccountFeed mAccountFeed = null;

    public SyndicationService.TransitAccountFeed AccountFeed
    {
        get
        {
            if (mAccountFeed == null)
            {
                mAccountFeed = SessionManager.GetInstance<SyndicationService.TransitAccountFeed, SyndicationService.ServiceQueryOptions, int>(
                    RequestId, SessionManager.SyndicationService.GetAccountFeedById);
            }
            return mAccountFeed;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
        if (!IsPostBack)
        {
            SyndicationService.TransitAccountFeed f = AccountFeed;
            feedDescription.Text = (f.Description.Length > 0 ? string.Format("&#187; {0}", Renderer.Render(f.Description)) : string.Empty);

            this.Title = string.Format("{0}'s {1}", Renderer.Render(f.AccountName), Renderer.Render(f.Name));

            gridManage.VirtualItemCount = SessionManager.GetCount<SyndicationService.TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, int>(
                RequestId, SessionManager.SyndicationService.GetAccountFeedItemsCount);
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Blogs", Request, "AccountFeedItemsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode(f.Name, Request.Url));
            StackSiteMap(sitemapdata);
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        SyndicationService.ServiceQueryOptions options = new SyndicationService.ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<SyndicationService.TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, int>(
            RequestId, options, SessionManager.SyndicationService.GetAccountFeedItems);
    }

    public string GetDescription(string value)
    {
        HtmlWriterOptions options = new HtmlWriterOptions();
        options.BaseHref = Uri.IsWellFormedUriString(AccountFeed.LinkUrl, UriKind.Absolute) ? new Uri(AccountFeed.LinkUrl) : null;
        options.RewriteImgSrc = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        return Renderer.CleanHtml(value, options);
    }

    public string GetValue(string s, string defaultvalue)
    {
        return string.IsNullOrEmpty(s) ? defaultvalue : s;
    }

    public string GetDescription(string s, string uri)
    {
        HtmlWriterOptions options = new HtmlWriterOptions();
        options.RewriteImgSize = new Size(0, 0);
        options.BaseHref = Uri.IsWellFormedUriString(uri, UriKind.Absolute) ? new Uri(uri) : null;
        options.RewriteImgSrc = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        return Renderer.CleanHtml(s, options);
    }
}

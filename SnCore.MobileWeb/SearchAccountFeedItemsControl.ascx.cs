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
using SnCore.WebControls;
using SyndicationService;
using System.Drawing;
using SnCore.Tools.Web.Html;

public partial class SearchAccountFeedItemsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        return SessionManager.GetCount<TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, string>(
            SearchQuery, SessionManager.SyndicationService.SearchAccountFeedItemsCount);
    }

    protected override IEnumerable GetResults()
    {
        SyndicationService.ServiceQueryOptions options = new SyndicationService.ServiceQueryOptions();
        options.PageNumber = Grid.CurrentPageIndex;
        options.PageSize = Grid.PageSize;
        return SessionManager.GetCollection<TransitAccountFeedItem, SyndicationService.ServiceQueryOptions, string>(
            SearchQuery, options, SessionManager.SyndicationService.SearchAccountFeedItems);
    }

    protected override IPagedControl Grid
    {
        get
        {
            return gridResults;
        }
    }

    public string GetComments(int count)
    {
        if (count == 0) return "read and comment";
        else if (count == 1) return "read 1 comment";
        else return string.Format("read {0} comments", count);
    }

    public string GetSummary(string summary, string link)
    {
        Uri uri = null;
        Uri.TryCreate(link, UriKind.Absolute, out uri);        
        HtmlWriterOptions options = new HtmlWriterOptions();
        options.RewriteImgSize = new Size(0, 0);
        options.BaseHref = uri;
        options.RewriteImgSrc = new Uri(SessionManager.WebsiteUri, "AccountFeedItemPicture.aspx?src={url}");
        return Renderer.CleanHtml(summary, options);
    }

    protected override Label Label
    {
        get
        {
            return labelResults;
        }
    }

    public string GetValue(string s, string defaultvalue)
    {
        return string.IsNullOrEmpty(s) ? defaultvalue : s;
    }
}

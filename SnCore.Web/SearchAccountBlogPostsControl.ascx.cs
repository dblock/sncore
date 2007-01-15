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
using SnCore.WebControls;

public partial class SearchAccountBlogPostsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        object[] args = { SessionManager.Ticket, SearchQuery };
        return SessionManager.GetCachedCollectionCount<TransitAccountBlogPost>(
            SessionManager.BlogService, "SearchAccountBlogPostsCount", args);
    }

    protected override IEnumerable GetResults()
    {
        return SessionManager.GetCachedCollection<TransitAccountBlogPost>(
            SessionManager.BlogService, "SearchAccountBlogPosts", GetSearchQueryArgs());
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
        if (count == 0) return "post a comment";
        else if (count == 1) return "1 comment";
        else return string.Format("{0} comments", count);
    }

    protected override Label Label
    {
        get
        {
            return labelResults;
        }
    }
}

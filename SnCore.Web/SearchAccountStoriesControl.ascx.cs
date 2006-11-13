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

public partial class SearchAccountStoriesControl : SearchControl
{
    protected override int GetResultsCount()
    {
        object[] args = { SearchQuery };
        return SessionManager.GetCachedCollectionCount(
            StoryService, "SearchAccountStoriesCount", args);
    }

    protected override IEnumerable GetResults()
    {
        return SessionManager.GetCachedCollection<TransitAccountStory>(
            StoryService, "SearchAccountStories", GetSearchQueryArgs());
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
        if (count == 0) return "no comments";
        else if (count == 1) return "one comment";
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

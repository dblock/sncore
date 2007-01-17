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
        return SessionManager.GetCount<TransitAccountStory, string>(
            SearchQuery, SessionManager.StoryService.SearchAccountStoriesCount);
    }

    protected override IEnumerable GetResults()
    {
        return SessionManager.GetCollection<TransitAccountStory, string>(
            SearchQuery, GetServiceQueryOptions(), SessionManager.StoryService.SearchAccountStories);
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

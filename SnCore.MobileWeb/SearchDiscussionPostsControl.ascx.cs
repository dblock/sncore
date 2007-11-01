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
using System.Collections.Generic;
using SnCore.WebControls;
using DiscussionService;

public partial class SearchDiscussionPostsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        if (RequestId > 0)
        {
            return SessionManager.GetCount<TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int, string>(
                RequestId, SearchQuery, SessionManager.DiscussionService.SearchDiscussionPostsByIdCount);
        }
        else
        {
            return SessionManager.GetCount<TransitDiscussionPost, DiscussionService.ServiceQueryOptions, string>(
                SearchQuery, SessionManager.DiscussionService.SearchDiscussionPostsCount);
        }
    }

    protected override IEnumerable GetResults()
    {
        DiscussionService.ServiceQueryOptions options = new DiscussionService.ServiceQueryOptions();
        options.PageNumber = Grid.CurrentPageIndex;
        options.PageSize = Grid.PageSize;
        if (RequestId > 0)
        {
            return SessionManager.GetCollection<TransitDiscussionPost, DiscussionService.ServiceQueryOptions, int, string>(
                RequestId, SearchQuery, options, SessionManager.DiscussionService.SearchDiscussionPostsById);
        }
        else
        {
            return SessionManager.GetCollection<TransitDiscussionPost, DiscussionService.ServiceQueryOptions, string>(
                SearchQuery, options, SessionManager.DiscussionService.SearchDiscussionPosts);
        }
    }

    protected override IPagedControl Grid
    {
        get
        {
            return gridResults;
        }
    }

    protected override Label Label
    {
        get
        {
            return labelResults;
        }
    }

    public int OuterWidth
    {
        get
        {
            return 200;
        }
    }
}

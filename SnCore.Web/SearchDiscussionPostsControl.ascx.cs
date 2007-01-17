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
using SnCore.WebControls;

public partial class SearchDiscussionPostsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        if (RequestId > 0)
        {
            return SessionManager.GetCount<TransitDiscussionPost, int, string>(
                RequestId, SearchQuery, SessionManager.DiscussionService.SearchDiscussionPostsByIdCount);
        }
        else
        {
            return SessionManager.GetCount<TransitDiscussionPost, string>(
                SearchQuery, SessionManager.DiscussionService.SearchDiscussionPostsCount);
        }
    }

    protected override IEnumerable GetResults()
    {
        if (RequestId > 0)
        {
            return SessionManager.GetCollection<TransitDiscussionPost, int, string>(
                RequestId, SearchQuery, GetServiceQueryOptions(), SessionManager.DiscussionService.SearchDiscussionPostsById);
        }
        else
        {
            return SessionManager.GetCollection<TransitDiscussionPost, string>(
                SearchQuery, GetServiceQueryOptions(), SessionManager.DiscussionService.SearchDiscussionPosts);
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
}

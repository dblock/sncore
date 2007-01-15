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
            object[] args = { SessionManager.Ticket, RequestId, SearchQuery };
            return SessionManager.GetCachedCollectionCount<TransitDiscussionPost>(
                SessionManager.DiscussionService, "SearchDiscussionPostsByIdCount", args);
        }
        else
        {
            object[] args = { SessionManager.Ticket, SearchQuery };
            return SessionManager.GetCachedCollectionCount<TransitDiscussionPost>(
                SessionManager.DiscussionService, "SearchDiscussionPostsCount", args);
        }
    }

    protected override IEnumerable GetResults()
    {
        if (RequestId > 0)
        {
            List<object> args = new List<object>();
            args.Add(RequestId);
            args.AddRange(GetSearchQueryArgs());
            return SessionManager.GetCachedCollection<TransitDiscussionPost>(
                SessionManager.DiscussionService, "SearchDiscussionPostsById", args.ToArray());
        }
        else
        {
            return SessionManager.GetCachedCollection<TransitDiscussionPost>(
                SessionManager.DiscussionService, "SearchDiscussionPosts", GetSearchQueryArgs());
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

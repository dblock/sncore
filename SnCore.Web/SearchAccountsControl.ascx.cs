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

public partial class SearchAccountsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        object[] args = { SearchQuery };
        return SessionManager.GetCachedCollectionCount(
            SessionManager.AccountService, "SearchAccountsCount", args);
    }

    protected override IEnumerable GetResults()
    {
        return SessionManager.GetCachedCollection<TransitAccountActivity>(
            SessionManager.AccountService, "SearchAccounts", GetSearchQueryArgs());
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

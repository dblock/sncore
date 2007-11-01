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
using AccountService;

public partial class SearchAccountsControl : SearchControl
{
    protected override int GetResultsCount()
    {
        return SessionManager.GetCount<TransitAccount, AccountService.ServiceQueryOptions, string>(
            SearchQuery, SessionManager.AccountService.SearchAccountsCount);
    }

    protected override IEnumerable GetResults()
    {
        AccountService.ServiceQueryOptions options = new AccountService.ServiceQueryOptions();
        options.PageNumber = Grid.CurrentPageIndex;
        options.PageSize = Grid.PageSize;
        return SessionManager.GetCollection<TransitAccountActivity, AccountService.ServiceQueryOptions, string>(
            SearchQuery, options, SessionManager.AccountService.SearchAccounts);
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

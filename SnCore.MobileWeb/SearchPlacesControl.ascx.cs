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
using PlaceService;

public partial class SearchPlacesControl : SearchControl
{
    protected override int GetResultsCount()
    {
        return SessionManager.GetCount<TransitPlace, PlaceService.ServiceQueryOptions, string>(
            SearchQuery, SessionManager.PlaceService.SearchPlacesCount);
    }

    protected override IEnumerable GetResults()
    {
        PlaceService.ServiceQueryOptions options = new PlaceService.ServiceQueryOptions();
        options.PageNumber = Grid.CurrentPageIndex;
        options.PageSize = Grid.PageSize;
        return SessionManager.GetCollection<TransitPlace, PlaceService.ServiceQueryOptions, string>(
            SearchQuery, options, SessionManager.PlaceService.SearchPlaces);
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

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
using Wilco.Web.UI;

public abstract class SearchControl : Control
{
    public int ResultsCount
    {
        get
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                return 0;
            }

            int result = ViewStateUtility.GetViewStateValue<int>(ViewState, "ResultsCount", -1);
            if (result < 0)
            {
                result = GetResultsCount();
                ViewState["ResultsCount"] = result;
            }

            return result;
        }
        set
        {
            ViewState["ResultsCount"] = value;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        Grid.OnGetDataSource += new EventHandler(Grid_OnGetDataSource);

        if (!IsPostBack)
        {
            Grid.CurrentPageIndex = 0;
            Grid.VirtualItemCount = ResultsCount;
            if (string.IsNullOrEmpty(SearchQuery))
            {
                Label.Text = "Nothing to search";
            }
            else
            {
                Label.Text = string.Format("Searching for \"<b>{0}</b>\": {1} result{2}",
                    Renderer.Render(SearchQuery),
                    Grid.VirtualItemCount == 0 ? "no" : Grid.VirtualItemCount.ToString(),
                    Grid.VirtualItemCount == 1 ? string.Empty : "s");
            }
        }

        base.OnLoad(e);
    }

    void Grid_OnGetDataSource(object sender, EventArgs e)
    {
        Grid.DataSource = GetResults();
    }

    public override void DataBind()
    {
        Grid.DataSource = GetResults();
        Grid.DataBind();
        base.DataBind();
    }

    public ServiceQueryOptions GetServiceQueryOptions()
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = Grid.CurrentPageIndex;
        options.PageSize = Grid.PageSize;
        return options;
    }

    protected abstract int GetResultsCount();    
    protected abstract IPagedControl Grid { get; }
    protected abstract IEnumerable GetResults();
    protected abstract Label Label { get; }

    public string SearchQuery
    {
        get
        {
            object query = Request.QueryString["q"];
            return query == null ? string.Empty : query.ToString();
        }
    }
}

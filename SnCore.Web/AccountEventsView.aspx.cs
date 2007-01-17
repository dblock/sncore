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
using System.Text;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;

[SiteMapDataAttribute("Events")]
public partial class AccountEventsView : Page
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;
        public string Neighborhood;
        public string Type;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City, string.Empty /* TODO */, string.Empty)
        {

        }

        public SelectLocationEventArgs(HttpRequest request)
            : this(request["country"], request["state"], request["city"], request["neighborhood"], request["type"])
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city,
            string neighborhood,
            string type)
        {
            Country = country;
            State = state;
            City = city;
            Neighborhood = neighborhood;
            Type = type;
        }
    }

    private TransitAccountEventQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            List<TransitAccountEventType> types = new List<TransitAccountEventType>();
            types.Add(new TransitAccountEventType());
            types.AddRange(SessionManager.GetCollection<TransitAccountEventType>(
                (ServiceQueryOptions) null, SessionManager.EventService.GetAccountEventTypes));
            inputType.DataSource = types;
            inputType.DataBind();

            List<TransitCountry> countries = new List<TransitCountry>();
            countries.Add(new TransitCountry());
            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault));
            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
            {
                SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
            }
            else
            {
                SelectLocation(sender, new SelectLocationEventArgs(Request));
            }

            GetData();
        }

        SetDefaultButton(search);
    }

    private void GetData()
    {
        mOptions = null;

        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountEvent, TransitAccountEventQueryOptions>(
            QueryOptions, SessionManager.EventService.GetAccountEventsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<TransitState> states = new List<TransitState>();
        states.Add(new TransitState());
        states.AddRange(SessionManager.GetCollection<TransitState, string>(
            inputCountry.SelectedValue, (ServiceQueryOptions) null, SessionManager.LocationService.GetStatesByCountryName));
        inputState.DataSource = states;
        inputState.DataBind();
        inputState_SelectedIndexChanged(sender, e);
    }


    public void inputState_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<TransitCity> cities = new List<TransitCity>();
        cities.Add(new TransitCity());
        cities.AddRange(SessionManager.GetCollection<TransitCity, string, string>(
            inputCountry.SelectedValue, inputState.SelectedValue, (ServiceQueryOptions) null, 
            SessionManager.LocationService.GetCitiesByLocation));
        inputCity.DataSource = cities;
        inputCity.DataBind();
    }

    public void inputCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<TransitNeighborhood> neighborhoods = new List<TransitNeighborhood>();
        neighborhoods.Add(new TransitNeighborhood());
        neighborhoods.AddRange(SessionManager.GetCollection<TransitNeighborhood, string, string, string>(
            inputCountry.SelectedValue, inputState.SelectedValue, inputCity.SelectedValue, (ServiceQueryOptions) null, 
            SessionManager.LocationService.GetNeighborhoodsByLocation));
        inputNeighborhood.DataSource = neighborhoods;
        inputNeighborhood.DataBind();
    }

    private TransitAccountEventQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new TransitAccountEventQueryOptions();
                mOptions.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
                mOptions.SortOrder = listboxSelectSortOrder.SelectedValue;
                mOptions.City = inputCity.Text;
                mOptions.Country = inputCountry.SelectedValue;
                mOptions.State = inputState.SelectedValue;
                mOptions.Neighborhood = inputNeighborhood.SelectedValue;
                mOptions.Name = inputName.Text;
                mOptions.Type = inputType.SelectedValue;
            }
            return mOptions;
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountEventQueryOptions options = QueryOptions;

        linkRelRss.NavigateUrl =
            string.Format("AccountEventsRss.aspx?order={0}&asc={1}&city={2}&country={3}&state={4}&name={5}&type={6}&neighborhood={7}",
                Renderer.UrlEncode(QueryOptions.SortOrder),
                Renderer.UrlEncode(QueryOptions.SortAscending),
                Renderer.UrlEncode(QueryOptions.City),
                Renderer.UrlEncode(QueryOptions.Country),
                Renderer.UrlEncode(QueryOptions.State),
                Renderer.UrlEncode(QueryOptions.Name),
                Renderer.UrlEncode(QueryOptions.Type),
                Renderer.UrlEncode(QueryOptions.Neighborhood));

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountEvent, int, TransitAccountEventQueryOptions>(
            SessionManager.UtcOffset, options, serviceoptions, SessionManager.EventService.GetAccountEvents);
    }

    public void SelectLocation(object sender, SelectLocationEventArgs e)
    {
        try
        {
            inputType.ClearSelection();
            inputType.Items.FindByValue(e.Type).Selected = true;
        }
        catch
        {

        }

        try
        {
            inputCountry.ClearSelection();
            inputCountry.Items.FindByValue(e.Country).Selected = true;
            inputCountry_SelectedIndexChanged(sender, e);
            inputState.ClearSelection();
            inputState.Items.FindByValue(e.State).Selected = true;
            inputState_SelectedIndexChanged(sender, e);
            inputCity.ClearSelection();
            inputCity.Items.FindByValue(e.City).Selected = true;
            inputNeighborhood.ClearSelection();
            inputNeighborhood.Items.FindByValue(e.Neighborhood).Selected = true;
        }
        catch
        {

        }
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData();
        panelGrid.Update();
        panelLinks.Update();
    }

    public void linkShowAll_Click(object sender, EventArgs e)
    {
        inputName.Text = string.Empty;
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.ClearSelection();
        inputNeighborhood.ClearSelection();
        inputType.ClearSelection();
        GetData();
        panelSearch.Update();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }
}

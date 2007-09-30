using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using SnCore.WebServices;
using System.Collections.Specialized;
using SnCore.Tools.Web;

public class CityLocationEventArgs : EventArgs
{
    private string mCity;

    public string City
    {
        get
        {
            return mCity;
        }
        set
        {
            mCity = value;
        }
    }

    public CityLocationEventArgs(string city)
    {
        mCity = city;
    }
}

public class LocationEventArgs : EventArgs
{
    public string Country;
    public string State;
    public string City;
    public string Neighborhood;

    public LocationEventArgs(AccountService.TransitAccount account)
        : this(account.Country, account.State, account.City, string.Empty /* TODO */)
    {

    }

    public LocationEventArgs(LocationService.TransitNeighborhood neighborhood)
        : this(neighborhood.Country, neighborhood.State, neighborhood.City, neighborhood.Name)
    {

    }

    public LocationEventArgs(LocationService.TransitCity city)
        : this(city.Country, city.State, city.Name, string.Empty)
    {

    }

    public LocationEventArgs(PlaceService.TransitPlace place)
        : this(place.Country, place.State, place.City, place.Neighborhood)
    {

    }

    public LocationEventArgs(HttpRequest request)
        : this(request["country"], request["state"], request["city"], request["neighborhood"])
    {

    }

    public LocationEventArgs(NameValueCollection coll)
        : this(coll["country"], coll["state"], coll["city"], coll["neighborhood"])
    {

    }

    public LocationEventArgs(
        string country,
        string state,
        string city,
        string neighborhood)
    {
        Country = country;
        State = state;
        City = city;
        Neighborhood = neighborhood;
    }
}

public class LocationSelector
{
    protected Page mPage;

    public LocationSelector(Page page)
    {
        mPage = page;
    }

    public virtual string GetUri()
    {
        return string.Empty;
    }

    protected bool AppendUriPart(ITextControl list, string name, ref string result)
    {
        if (list == null || string.IsNullOrEmpty(list.Text))
            return false;

        if (!string.IsNullOrEmpty(result)) result += "&";
        result += string.Format("{0}={1}", name, Renderer.UrlEncode(list.Text));
        return true;
    }
}

public class LocationSelectorCountryState : LocationSelector
{
    public event EventHandler CountryChanged;
    public event EventHandler StateChanged;

    private bool mInsertEmptySelection = false;
    protected DropDownList mCountry;
    protected DropDownList mState;

    public bool InsertEmptySelection
    {
        get
        {
            return mInsertEmptySelection;
        }
        set
        {
            mInsertEmptySelection = value;
        }
    }

    public LocationSelectorCountryState(Page page, bool empty, DropDownList country, DropDownList state)
        : base(page)
    {
        mInsertEmptySelection = empty;
        mCountry = country;
        mState = state;

        if (mCountry != null && mState != null)
        {
            mCountry.SelectedIndexChanged += new EventHandler(Country_SelectedIndexChanged);
            mState.SelectedIndexChanged += new EventHandler(State_SelectedIndexChanged);
        }

        if (!mPage.IsPostBack)
        {
            List<LocationService.TransitCountry> countries = new List<LocationService.TransitCountry>();
            if (InsertEmptySelection) countries.Add(new LocationService.TransitCountry());
            string defaultcountry = mPage.SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(mPage.SessionManager.GetCollection<LocationService.TransitCountry, LocationService.ServiceQueryOptions, string>(
                defaultcountry, null, mPage.SessionManager.LocationService.GetCountriesWithDefault));
            mCountry.DataSource = countries;
            mCountry.DataBind();
        }
    }

    public virtual bool SelectLocation(object sender, LocationEventArgs e)
    {
        bool result = false;

        if (mCountry != null && !string.IsNullOrEmpty(e.Country))
        {
            mCountry.ClearSelection();
            ListItem country = mCountry.Items.FindByValue(e.Country);
            if (country != null)
            {
                country.Selected = true;
                Country_SelectedIndexChanged(sender, e);
                result = true;
            }
        }

        if (mState != null && !string.IsNullOrEmpty(e.State))
        {
            mState.ClearSelection();
            ListItem state = mState.Items.FindByValue(e.State);
            if (state != null)
            {
                state.Selected = true;
                State_SelectedIndexChanged(sender, e);
                result = true;
            }
        }

        return result;
    }

    public void ChangeCountry(object sender, EventArgs e)
    {
        Country_SelectedIndexChanged(sender, e);
    }

    protected virtual void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (mState != null)
            {
                List<LocationService.TransitState> states = new List<LocationService.TransitState>();
                if (InsertEmptySelection) states.Add(new LocationService.TransitState());
                states.AddRange(mPage.SessionManager.GetCollection<LocationService.TransitState, LocationService.ServiceQueryOptions, string>(
                    mCountry.SelectedValue, null, mPage.SessionManager.LocationService.GetStatesByCountryName));
                mState.DataSource = states;
                mState.DataBind();

                State_SelectedIndexChanged(sender, e);
            }

            if (CountryChanged != null)
            {
                CountryChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            mPage.ReportException(ex);
        }
    }

    protected virtual void State_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (StateChanged != null)
        {
            StateChanged(sender, e);
        }
    }

    public virtual void ClearSelection()
    {
        mState.ClearSelection();
        mCountry.ClearSelection();
    }

    public override string GetUri()
    {
        string result = base.GetUri();
        AppendUriPart(mCountry, "country", ref result);
        AppendUriPart(mState, "state", ref result);
        return result;
    }
}

public class LocationSelectorCountryStateCityText : LocationSelectorCountryState
{
    protected TextBox mCity;

    public LocationSelectorCountryStateCityText(Page page, bool empty, DropDownList country, DropDownList state, TextBox city)
        : base(page, empty, country, state)
    {
        mCity = city;
    }

    public override bool SelectLocation(object sender, LocationEventArgs e)
    {
        bool result = base.SelectLocation(sender, e);

        if (mCity != null && !string.IsNullOrEmpty(e.City))
        {
            mCity.Text = e.City;
            result = true;
        }

        return result;
    }

    public void ChangeCityWithAccountDefault(object sender, CityLocationEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.City))
        {
            LocationService.TransitCity t_city = mPage.SessionManager.LocationService.GetCityByTag(
                mPage.SessionManager.Ticket, e.City);
            if (t_city != null)
            {
                SelectLocation(sender, new LocationEventArgs(t_city));
            }
            else
            {
                mCity.Text = e.City;
            }
        }
        else
        {
            SelectLocation(sender, new LocationEventArgs(mPage.SessionManager.Account));
        }
    }

    public override void ClearSelection()
    {
        mCity.Text = string.Empty;
        base.ClearSelection();
    }

    public override string GetUri()
    {
        string result = base.GetUri();
        AppendUriPart(mCity, "city", ref result);
        return result;
    }
}

public class LocationSelectorCountryStateCity : LocationSelectorCountryState
{
    protected DropDownList mCity;
    public event EventHandler CityChanged;

    public LocationSelectorCountryStateCity(Page page, bool empty, DropDownList country, DropDownList state, DropDownList city)
        : base(page, empty, country, state)
    {
        mCity = city;
        mCity.SelectedIndexChanged += new EventHandler(City_SelectedIndexChanged);
    }

    protected virtual void City_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CityChanged != null)
        {
            CityChanged(sender, e);
        }
    }

    public override bool SelectLocation(object sender, LocationEventArgs e)
    {
        bool result = base.SelectLocation(sender, e);

        if (mCity != null && !string.IsNullOrEmpty(e.City))
        {
            mCity.ClearSelection();
            ListItem city = mCity.Items.FindByValue(e.City);
            if (city != null)
            {
                city.Selected = true;
                City_SelectedIndexChanged(sender, e);
                result = true;
            }
        }

        return result;
    }

    protected override void State_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (mCity != null)
            {
                List<LocationService.TransitCity> cities = new List<LocationService.TransitCity>();
                if (InsertEmptySelection) cities.Add(new LocationService.TransitCity());
                cities.AddRange(mPage.SessionManager.GetCollection<LocationService.TransitCity, LocationService.ServiceQueryOptions, string, string>(
                    mCountry.SelectedValue, mState.SelectedValue, null,
                    mPage.SessionManager.LocationService.GetCitiesByLocation));
                mCity.DataSource = cities;
                mCity.DataBind();

                City_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            mPage.ReportException(ex);
        }

        base.State_SelectedIndexChanged(sender, e);
    }

    public override void ClearSelection()
    {
        mCity.ClearSelection();
        base.ClearSelection();
    }

    public override string GetUri()
    {
        string result = base.GetUri();
        AppendUriPart(mCity, "city", ref result);
        return result;
    }
}

public class LocationSelectorCountryStateCityNeighborhood : LocationSelectorCountryStateCity
{
    protected DropDownList mNeighborhood;
    public event EventHandler NeighborhoodChanged;

    public LocationSelectorCountryStateCityNeighborhood(Page page, bool empty, DropDownList country, DropDownList state, DropDownList city, DropDownList neighborhood)
        : base(page, empty, country, state , city)
    {
        mNeighborhood = neighborhood;
        mNeighborhood.SelectedIndexChanged += new EventHandler(Neighborhood_SelectedIndexChanged);
    }

    void Neighborhood_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (NeighborhoodChanged != null)
        {
            NeighborhoodChanged(sender, e);
        }
    }

    public override bool SelectLocation(object sender, LocationEventArgs e)
    {
        bool result = base.SelectLocation(sender, e);

        if (mNeighborhood != null && !string.IsNullOrEmpty(e.Neighborhood))
        {
            mNeighborhood.ClearSelection();
            ListItem neighborhood = mNeighborhood.Items.FindByValue(e.Neighborhood);
            if (neighborhood != null)
            {
                neighborhood.Selected = true;
                result = true;
            }
        }

        return result;
    }

    protected override void City_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (mNeighborhood != null)
            {
                List<LocationService.TransitNeighborhood> neighborhoods = new List<LocationService.TransitNeighborhood>();
                if (InsertEmptySelection) neighborhoods.Add(new LocationService.TransitNeighborhood());
                neighborhoods.AddRange(mPage.SessionManager.GetCollection<LocationService.TransitNeighborhood, LocationService.ServiceQueryOptions, string, string, string>(
                    mCountry.SelectedValue, mState.SelectedValue, mCity.SelectedValue, null,
                    mPage.SessionManager.LocationService.GetNeighborhoodsByLocation));
                mNeighborhood.DataSource = neighborhoods;
                mNeighborhood.DataBind();
            }
        }
        catch (Exception ex)
        {
            mPage.ReportException(ex);
        }

        base.City_SelectedIndexChanged(sender, e);
    }

    public override void ClearSelection()
    {
        mNeighborhood.ClearSelection();
        base.ClearSelection();
    }

    public override string GetUri()
    {
        string result = base.GetUri();
        AppendUriPart(mNeighborhood, "neighborhood", ref result);
        return result;
    }
}

public class LocationSelectorCountryStateCityNeighborhoodText : LocationSelectorCountryStateCityText
{
    protected TextBox mNeighborhood;

    public LocationSelectorCountryStateCityNeighborhoodText(Page page, bool empty, DropDownList country, DropDownList state, TextBox city, TextBox neighborhood)
        : base(page, empty, country, state , city)
    {
        mNeighborhood = neighborhood;
    }

    public override bool SelectLocation(object sender, LocationEventArgs e)
    {
        bool result = base.SelectLocation(sender, e);

        if (mNeighborhood != null && !string.IsNullOrEmpty(e.Neighborhood))
        {
            mNeighborhood.Text = e.Neighborhood;
            result = true;
        }

        return result;
    }

    public override void ClearSelection()
    {
        mNeighborhood.Text = string.Empty;
        base.ClearSelection();
    }

    public override string GetUri()
    {
        string result = base.GetUri();
        AppendUriPart(mNeighborhood, "neighborhood", ref result);
        return result;
    }
}


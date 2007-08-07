using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using System.Collections.Generic;
using SnCore.WebServices;
using System.Collections.Specialized;

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

    public LocationEventArgs(TransitAccount account)
        : this(account.Country, account.State, account.City, string.Empty /* TODO */)
    {

    }

    public LocationEventArgs(TransitNeighborhood neighborhood)
        : this(neighborhood.Country, neighborhood.State, neighborhood.City, neighborhood.Name)
    {

    }

    public LocationEventArgs(TransitCity city)
        : this(city.Country, city.State, city.Name, string.Empty)
    {

    }

    public LocationEventArgs(TransitPlace place)
        : this(place.Country, place.State, place.City, place.Neighborhood)
    {

    }

    public LocationEventArgs(TransitPlaceChangeRequest request)
        : this(request.Country, request.State, request.City, request.Neighborhood)
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
            List<TransitCountry> countries = new List<TransitCountry>();
            if (InsertEmptySelection) countries.Add(new TransitCountry());
            string defaultcountry = mPage.SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(mPage.SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, mPage.SessionManager.LocationService.GetCountriesWithDefault));
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
                List<TransitState> states = new List<TransitState>();
                if (InsertEmptySelection) states.Add(new TransitState());
                states.AddRange(mPage.SessionManager.GetCollection<TransitState, string>(
                    mCountry.SelectedValue, (ServiceQueryOptions) null, mPage.SessionManager.LocationService.GetStatesByCountryName));
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
}

public class LocationSelectorCountryStateCityText : LocationSelectorCountryState
{
    protected TextBox mCity;

    public LocationSelectorCountryStateCityText(Page page, bool empty, DropDownList country, DropDownList state, TextBox city)
        : base( page, empty, country, state)
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
            TransitCity t_city = mPage.SessionManager.LocationService.GetCityByTag(
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
}

public class LocationSelectorCountryStateCity : LocationSelectorCountryState
{
    protected DropDownList mCity;
    public event EventHandler CityChanged;

    public LocationSelectorCountryStateCity(Page page, bool empty, DropDownList country, DropDownList state, DropDownList city)
        : base( page, empty, country, state)
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

        if (mCity != null && ! string.IsNullOrEmpty(e.City))
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
                List<TransitCity> cities = new List<TransitCity>();
                if (InsertEmptySelection) cities.Add(new TransitCity());
                cities.AddRange(mPage.SessionManager.GetCollection<TransitCity, string, string>(
                    mCountry.SelectedValue, mState.SelectedValue, (ServiceQueryOptions) null, 
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
}

public class LocationSelectorCountryStateCityNeighborhood : LocationSelectorCountryStateCity
{
    protected DropDownList mNeighborhood;
    public event EventHandler NeighborhoodChanged;

    public LocationSelectorCountryStateCityNeighborhood(Page page, bool empty, DropDownList country, DropDownList state, DropDownList city, DropDownList neighborhood)
        : base( page, empty, country, state, city)
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
                List<TransitNeighborhood> neighborhoods = new List<TransitNeighborhood>();
                if (InsertEmptySelection) neighborhoods.Add(new TransitNeighborhood());
                neighborhoods.AddRange(mPage.SessionManager.GetCollection<TransitNeighborhood, string, string, string>(
                    mCountry.SelectedValue, mState.SelectedValue, mCity.SelectedValue, (ServiceQueryOptions) null, 
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
}

public class LocationSelectorCountryStateCityNeighborhoodText : LocationSelectorCountryStateCityText
{
    protected TextBox mNeighborhood;

    public LocationSelectorCountryStateCityNeighborhoodText(Page page, bool empty, DropDownList country, DropDownList state, TextBox city, TextBox neighborhood)
        : base( page, empty, country, state, city)
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
}


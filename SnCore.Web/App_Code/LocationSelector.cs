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

    public LocationEventArgs(TransitAccount account)
        : this(account.Country, account.State, account.City)
    {

    }

    public LocationEventArgs(TransitCity city)
        : this(city.Country, city.State, city.Name)
    {

    }

    public LocationEventArgs(TransitPlace place)
        : this(place.Country, place.State, place.City)
    {

    }

    public LocationEventArgs(HttpRequest request)
        : this(request["country"], request["state"], request["city"])
    {

    }

    public LocationEventArgs(
        string country,
        string state,
        string city)
    {
        Country = country;
        State = state;
        City = city;
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

        if (! mPage.IsPostBack)
        {
            List<TransitCountry> countries = new List<TransitCountry>();
            if (InsertEmptySelection) countries.Add(new TransitCountry());
            countries.AddRange(mPage.SessionManager.GetCachedCollection<TransitCountry>(
                mPage.SessionManager.LocationService, "GetCountries", null));
            mCountry.DataSource = countries;
            mCountry.DataBind();
        }
    }

    public virtual void SelectLocation(object sender, LocationEventArgs e)
    {
        if (mCountry != null)
        {
            mCountry.ClearSelection();
            ListItem country = mCountry.Items.FindByValue(e.Country);
            if (country != null)
            {
                country.Selected = true;
                Country_SelectedIndexChanged(sender, e);
            }
        }

        if (mState != null)
        {
            mState.ClearSelection();
            ListItem state = mState.Items.FindByValue(e.State);
            if (state != null)
            {
                state.Selected = true;
                State_SelectedIndexChanged(sender, e);
            }
        }
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
                object[] args = { mCountry.SelectedValue };
                List<TransitState> states = new List<TransitState>();
                if (InsertEmptySelection) states.Add(new TransitState());
                states.AddRange(mPage.SessionManager.GetCachedCollection<TransitState>(
                    mPage.SessionManager.LocationService, "GetStatesByCountry", args));
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
        : base(page, empty, country, state)
    {
        mCity = city;
    }

    public override void SelectLocation(object sender, LocationEventArgs e)
    {
        base.SelectLocation(sender, e);

        if (mCity != null)
        {
            mCity.Text = e.City;
        }
    }

    public void ChangeCityWithAccountDefault(object sender, CityLocationEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.City))
        {
            TransitCity t_city = mPage.SessionManager.LocationService.GetCityByTag(e.City);
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
        : base(page, empty, country, state)
    {
        mCity = city;
        mCity.SelectedIndexChanged += new EventHandler(City_SelectedIndexChanged);
    }

    void City_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (CityChanged != null)
        {
            CityChanged(sender, e);
        }
    }

    public override void SelectLocation(object sender, LocationEventArgs e)
    {
        base.SelectLocation(sender, e);

        if (mCity != null)
        {
            mCity.ClearSelection();
            ListItem city = mCity.Items.FindByValue(e.City);
            if (city != null)
            {
                city.Selected = true;
            }
        }
    }

    protected override void State_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (mCity != null)
            {
                object[] args = { mCountry.SelectedValue, mState.SelectedValue };
                List<TransitCity> cities = new List<TransitCity>();
                if (InsertEmptySelection) cities.Add(new TransitCity());
                cities.AddRange(mPage.SessionManager.GetCachedCollection<TransitCity>(
                    mPage.SessionManager.LocationService, "GetCitiesByLocation", args));
                mCity.DataSource = cities;
                mCity.DataBind();
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
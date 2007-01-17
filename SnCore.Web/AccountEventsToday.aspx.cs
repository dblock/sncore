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

public partial class AccountEventsToday : Page
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;
        public string Neighborhood;
        public string Type;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City, string.Empty, string.Empty)
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

    private TransitAccountEventInstanceQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        if (!IsPostBack)
        {
            List<TransitAccountEventType> types = new List<TransitAccountEventType>();
            types.Add(new TransitAccountEventType());
            types.AddRange(SessionManager.GetCollection<TransitAccountEventType>(
                (ServiceQueryOptions)null, SessionManager.EventService.GetAccountEventTypes));
            inputType.DataSource = types;
            inputType.DataBind();

            List<TransitCountry> countries = new List<TransitCountry>();
            countries.Add(new TransitCountry());
            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault));
            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; {0} Events", Renderer.Render(SessionManager.Account.City));
            }

            //if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
            //{
            //    SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
            //}
            //else
            {
                SelectLocation(sender, new SelectLocationEventArgs(Request));
            }

            SelectWeek();
            GetData();

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events This Week", "AccountEventsToday.aspx"));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(search);
    }

    private void SelectWeek()
    {
        DateTime adjustedNow = base.Adjust(DateTime.UtcNow);

        calendarEvents.VisibleDate = adjustedNow;

        int count = 0;
        do
        {
            calendarEvents.SelectedDates.Add(adjustedNow.Date);
            adjustedNow = adjustedNow.AddDays(1);
            count++;
        } while ((adjustedNow.DayOfWeek != DayOfWeek.Monday) || (count < 7));
    }

    private void GetData()
    {
        mOptions = null;
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            QueryOptions, SessionManager.EventService.GetAccountEventInstancesCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            QueryOptions, options, SessionManager.EventService.GetAccountEventInstances);
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
        panelCountryState.Update();
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
        panelCity.Update();
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
        panelNeighborhood.Update();
    }

    private TransitAccountEventInstanceQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new TransitAccountEventInstanceQueryOptions();
                mOptions.Neighborhood = inputNeighborhood.Text;
                mOptions.City = inputCity.Text;
                mOptions.Country = inputCountry.SelectedValue;
                mOptions.State = inputState.SelectedValue;
                mOptions.Type = inputType.SelectedValue;
                mOptions.StartDateTime = DateTime.MinValue;
                mOptions.EndDateTime = DateTime.MaxValue;
                if (calendarEvents.SelectedDates.Count > 0)
                {
                    foreach (DateTime dt in calendarEvents.SelectedDates)
                    {
                        DateTime d = base.ToUTC(dt);

                        if (mOptions.StartDateTime == DateTime.MinValue || d < mOptions.StartDateTime)
                            mOptions.StartDateTime = d;

                        // include the entire day (adddays(1))
                        if (mOptions.EndDateTime == DateTime.MaxValue || d.AddDays(1) > mOptions.EndDateTime)
                            mOptions.EndDateTime = d.AddDays(1);
                    }
                }
                else
                {
                    mOptions.StartDateTime = mOptions.EndDateTime = calendarEvents.SelectedDate;
                }

                if (mOptions.StartDateTime == mOptions.EndDateTime)
                {
                    mOptions.EndDateTime = mOptions.EndDateTime.AddDays(1);
                }
            }
            return mOptions;
        }
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

        panelSearch.Update();
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData();
    }

    protected void calendarEvents_SelectionChanged(object sender, EventArgs e)
    {
        GetData();
    }

    public void linkShowAll_Click(object sender, EventArgs e)
    {
        inputCountry.ClearSelection();
        inputState.ClearSelection();
        inputCity.ClearSelection();
        inputNeighborhood.ClearSelection();
        inputType.ClearSelection();
        SelectWeek();
        GetData();
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
        GetData();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }
}

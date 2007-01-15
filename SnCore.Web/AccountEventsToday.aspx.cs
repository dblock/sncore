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
            object[] args = { SessionManager.Ticket, null };
            ArrayList types = new ArrayList();
            types.Add(new TransitAccountEventType());
            types.AddRange(SessionManager.GetCachedCollection<TransitAccountEventType>(
                SessionManager.EventService, "GetAccountEventTypes", args));
            inputType.DataSource = types;
            inputType.DataBind();

            ArrayList countries = new ArrayList();
            countries.Add(new TransitCountry());
            object[] c_args = { SessionManager.Ticket, null };
            countries.AddRange(SessionManager.GetCachedCollection<TransitCountry>(
                SessionManager.LocationService, "GetCountries", c_args));
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
        object[] args = { SessionManager.Ticket, QueryOptions };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount<TransitAccountEventInstance>(
            SessionManager.EventService, "GetAccountEventInstancesCount", args);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = gridManage.CurrentPageIndex;
        options.PageSize = gridManage.PageSize;
        object[] args = { SessionManager.Ticket, QueryOptions, options };
        gridManage.DataSource = SessionManager.GetCachedCollection<TransitAccountEventInstance>(
            SessionManager.EventService, "GetAccountEventInstances", args);
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList states = new ArrayList();
        states.Add(new TransitState());
        object[] args = { SessionManager.Ticket, inputCountry.SelectedValue, null };
        states.AddRange(SessionManager.GetCachedCollection<TransitState>(
            SessionManager.LocationService, "GetStatesByCountryName", args));
        inputState.DataSource = states;
        inputState.DataBind();
        inputState_SelectedIndexChanged(sender, e);
        panelCountryState.Update();
    }


    public void inputState_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList cities = new ArrayList();
        cities.Add(new TransitCity());
        object[] args = { SessionManager.Ticket, inputCountry.SelectedValue, inputState.SelectedValue };
        cities.AddRange(SessionManager.GetCachedCollection<TransitCity>(
            SessionManager.LocationService, "GetCitiesByLocation", args));
        inputCity.DataSource = cities;
        inputCity.DataBind();
        panelCity.Update();
    }

    public void inputCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        ArrayList neighborhoods = new ArrayList();
        neighborhoods.Add(new TransitNeighborhood());
        object[] args = { SessionManager.Ticket, inputCountry.SelectedValue, inputState.SelectedValue, inputCity.SelectedValue };
        neighborhoods.AddRange(SessionManager.GetCachedCollection<TransitNeighborhood>(
            SessionManager.LocationService, "GetNeighborhoodsByLocation", args));
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

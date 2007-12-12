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
using System.Collections.Specialized;
using SnCore.WebControls;

public partial class AccountEventsToday : Page
{
    public class LocationWithOptionsEventArgs : LocationEventArgs
    {
        private string mType;

        public string Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        public LocationWithOptionsEventArgs(TransitAccount account)
            : base(account)
        {
        }

        public LocationWithOptionsEventArgs(HttpRequest request)
            : base(request)
        {
            mType = request["type"];
        }

        public LocationWithOptionsEventArgs(NameValueCollection coll)
            : base(coll)
        {
            mType = coll["type"];
        }
    }

    public class LocationSelectorWithOptions : LocationSelectorCountryStateCityNeighborhood
    {
        private DropDownList mType;

        public LocationSelectorWithOptions(
            Page page,
            bool empty,
            DropDownList country,
            DropDownList state,
            DropDownList city,
            DropDownList neighborhood,
            DropDownList type)
            :
            base(page, empty, country, state, city, neighborhood)
        {
            mType = type;

            if (!mPage.IsPostBack)
            {
                List<TransitAccountEventType> types = new List<TransitAccountEventType>();
                types.Add(new TransitAccountEventType());
                types.AddRange(mPage.SessionManager.GetCollection<TransitAccountEventType>(
                    (ServiceQueryOptions)null, mPage.SessionManager.EventService.GetAccountEventTypes));
                mType.DataSource = types;
                mType.DataBind();
            }
        }

        public bool SelectLocation(object sender, LocationWithOptionsEventArgs e)
        {
            bool result = base.SelectLocation(sender, e);

            if (mType != null && !string.IsNullOrEmpty(e.Type))
            {
                result = ListItemManager.TrySelect(mType, e.Type);
            }

            return result;
        }

        public override void ClearSelection()
        {
            mType.ClearSelection();
            base.ClearSelection();
        }
    }

    private LocationSelectorWithOptions mLocationSelector = null;

    public LocationSelectorWithOptions LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorWithOptions(
                    this, true, inputCountry, inputState, inputCity, inputNeighborhood, inputType);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

        LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
        LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);
        LocationSelector.CityChanged += new EventHandler(LocationSelector_CityChanged);

        if (!IsPostBack)
        {
            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; {0} Events", Renderer.Render(SessionManager.Account.City));
            }

            if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
            {
                LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(SessionManager.Account));
            }

            SelectWeek();
            GetData(sender, e);

            if (gridManage.VirtualItemCount == 0)
            {
                LocationSelector.ClearSelection();
                GetData(sender, e);

                if (gridManage.VirtualItemCount == 0)
                {
                    SelectMonth();
                    GetData(sender, e);
                }
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsView.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events This Week", "AccountEventsToday.aspx"));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(search);
    }

    private void SelectWeek()
    {
        calendarEvents.SelectedDates.Clear();
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

    private void SelectMonth()
    {
        calendarEvents.SelectedDates.Clear();
        DateTime adjustedNow = base.Adjust(DateTime.UtcNow);
        adjustedNow = adjustedNow.AddDays(1 - adjustedNow.Day);
        calendarEvents.VisibleDate = adjustedNow;

        do
        {
            calendarEvents.SelectedDates.Add(adjustedNow.Date);
            adjustedNow = adjustedNow.AddDays(1);
        } while (adjustedNow.Day != 1);
    }

    private void GetData(object sender, EventArgs e)
    {
        TransitAccountEventInstanceQueryOptions options = GetQueryOptions();
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            options, SessionManager.EventService.GetAccountEventInstancesCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountEventInstanceQueryOptions options = GetQueryOptions();

        ServiceQueryOptions service_options = new ServiceQueryOptions();
        service_options.PageNumber = gridManage.CurrentPageIndex;
        service_options.PageSize = gridManage.PageSize;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountEventInstance, TransitAccountEventInstanceQueryOptions>(
            options, service_options, SessionManager.EventService.GetAccountEventInstances);

        Title = titleEvents.Text = (string.IsNullOrEmpty(options.City)
            ? titleEvents.DefaultText
            : string.Format("{0}: {1}", titleEvents.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titleEvents.Text);
        }

        panelLinks.Update();
    }

    void LocationSelector_CityChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
        panelCity.Update();
        panelNeighborhood.Update();
    }

    void LocationSelector_StateChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
        panelCity.Update();
    }

    void LocationSelector_CountryChanged(object sender, EventArgs e)
    {
        panelCountryState.Update();
    }

    private TransitAccountEventInstanceQueryOptions GetQueryOptions()
    {
        TransitAccountEventInstanceQueryOptions options = new TransitAccountEventInstanceQueryOptions();
        options.Neighborhood = inputNeighborhood.Text;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Type = inputType.SelectedValue;
        options.StartDateTime = DateTime.MinValue;
        options.EndDateTime = DateTime.MaxValue;
        if (calendarEvents.SelectedDates.Count > 0)
        {
            foreach (DateTime dt in calendarEvents.SelectedDates)
            {
                DateTime d = base.ToUTC(dt);

                if (options.StartDateTime == DateTime.MinValue || d < options.StartDateTime)
                    options.StartDateTime = d;

                // include the entire day (adddays(1))
                if (options.EndDateTime == DateTime.MaxValue || d.AddDays(1) > options.EndDateTime)
                    options.EndDateTime = d.AddDays(1);
            }
        }
        else
        {
            options.StartDateTime = options.EndDateTime = calendarEvents.SelectedDate;
        }

        if (options.StartDateTime == options.EndDateTime)
        {
            options.EndDateTime = options.EndDateTime.AddDays(1);
        }

        return options;
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    protected void calendarEvents_SelectionChanged(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void linkShowAll_Click(object sender, EventArgs e)
    {
        LocationSelector.ClearSelection();
        SelectWeek();
        GetData(sender, e);
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
        GetData(sender, e);
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }


    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
        GetData(sender, e);
    }
}

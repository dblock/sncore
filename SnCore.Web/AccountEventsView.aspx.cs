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
using nStuff.UpdateControls;
using SnCore.WebControls;

[SiteMapDataAttribute("Events")]
public partial class AccountEventsView : Page
{
    public class LocationWithOptionsEventArgs : LocationEventArgs
    {
        public string Type;

        public LocationWithOptionsEventArgs(TransitAccount account)
            : base(account)
        {

        }

        public LocationWithOptionsEventArgs(HttpRequest request)
            : base(request)
        {
            Type = request["type"];
        }

        public LocationWithOptionsEventArgs(NameValueCollection coll)
            : base(coll)
        {
            Type = coll["type"];
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
                if (InsertEmptySelection) types.Add(new TransitAccountEventType());
                types.AddRange(page.SessionManager.GetCollection<TransitAccountEventType>(
                    (ServiceQueryOptions)null, page.SessionManager.EventService.GetAccountEventTypes));
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
            if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
            {
                LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(SessionManager.Account));
            }
            else
            {
                if (LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(Request)))
                {
                    panelSearchInternal.Visible = true;
                }
            }

            GetData(sender, e);
        }

        SetDefaultButton(search);
    }

    private void GetData(object sender, EventArgs e)
    {
        TransitAccountEventQueryOptions options = GetQueryOptions();
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitAccountEvent, TransitAccountEventQueryOptions>(
            options, SessionManager.EventService.GetAccountEventsCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private TransitAccountEventQueryOptions GetQueryOptions()
    {
        TransitAccountEventQueryOptions options = new TransitAccountEventQueryOptions();
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Neighborhood = inputNeighborhood.SelectedValue;
        options.Name = inputName.Text;
        options.Type = inputType.SelectedValue;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitAccountEventQueryOptions options = GetQueryOptions();

        string args = string.Format("city={0}&country={1}&state={2}&name={3}&type={4}&neighborhood={5}",
                Renderer.UrlEncode(options.City),
                Renderer.UrlEncode(options.Country),
                Renderer.UrlEncode(options.State),
                Renderer.UrlEncode(options.Name),
                Renderer.UrlEncode(options.Type),
                Renderer.UrlEncode(options.Neighborhood));

        linkRelRss.NavigateUrl = string.Format("AccountEventsRss.aspx?{0}", args);
        linkPermalink.NavigateUrl = string.Format("AccountEventsView.aspx?{0}", args);

        Title = titleEvents.Text = (string.IsNullOrEmpty(options.City)
            ? titleEvents.DefaultText
            : string.Format("{0}: {1}", titleEvents.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titleEvents.Text);
        }

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<TransitAccountEvent, int, TransitAccountEventQueryOptions>(
            SessionManager.UtcOffset, options, serviceoptions, SessionManager.EventService.GetAccountEvents);

        panelLinks.Update();
    }


    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
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
        GetData(sender, e);
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

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        string s = Encoding.Default.GetString(Convert.FromBase64String(e.EntryName));
        if (!string.IsNullOrEmpty(s))
        {
            NameValueCollection args = Renderer.ParseQueryString(s);
            LocationWithOptionsEventArgs l_args = new LocationWithOptionsEventArgs(args);
            l_args.Clear = true;
            LocationSelector.SelectLocation(sender, l_args);
            gridManage.CurrentPageIndex = int.Parse(args["page"]);
            gridManage_OnGetDataSource(sender, e);
            gridManage.DataBind();
        }
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

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
        GetData(sender, e);
    }
}

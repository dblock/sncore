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
using System.Collections.Generic;
using SnCore.SiteMap;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using SnCore.WebControls;

public partial class PlacesView : Page
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

        private string mPicturesOnly;

        public string PicturesOnly
        {
            get
            {
                return mPicturesOnly;
            }
            set
            {
                mPicturesOnly = value;
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
            mPicturesOnly = request["pictures"];
        }

        public LocationWithOptionsEventArgs(NameValueCollection coll)
            : base(coll)
        {
            mType = coll["type"];
            mPicturesOnly = coll["pictures"];
        }
    }

    public class LocationSelectorWithOptions : LocationSelectorCountryStateCityNeighborhood
    {
        private DropDownList mType;
        private CheckBox mPicturesOnly;

        public LocationSelectorWithOptions(
            Page page,
            bool empty,
            DropDownList country,
            DropDownList state,
            DropDownList city,
            DropDownList neighborhood,
            DropDownList type,
            CheckBox picturesonly)
            :
            base(page, empty, country, state, city, neighborhood)
        {
            mType = type;
            mPicturesOnly = picturesonly;

            if (!mPage.IsPostBack)
            {
                List<TransitPlaceType> types = new List<TransitPlaceType>();
                if (InsertEmptySelection) types.Add(new TransitPlaceType());
                types.AddRange(page.SessionManager.GetCollection<TransitPlaceType>(
                    (ServiceQueryOptions)null, page.SessionManager.PlaceService.GetPlaceTypes));
                mType.DataSource = types;
                mType.DataBind();
            }
        }

        public bool SelectLocation(object sender, LocationWithOptionsEventArgs e)
        {
            bool result = base.SelectLocation(sender, e);

            if (e.Clear || (mType != null && !string.IsNullOrEmpty(e.Type)))
            {
                mType.ClearSelection();
                result = ListItemManager.TrySelect(mType, e.Type);
            }

            if (e.Clear || (mPicturesOnly != null && !string.IsNullOrEmpty(e.PicturesOnly)))
            {
                bool picturesonly = false;
                if (bool.TryParse(e.PicturesOnly, out picturesonly))
                {
                    mPicturesOnly.Checked = picturesonly;
                    result = true;
                }
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
                    this, true, inputCountry, inputState, inputCity, inputNeighborhood, inputType, checkboxPicturesOnly);
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
        LocationSelector.LocationChanged += new EventHandler(LocationSelector_LocationChanged);

        ((SnCoreMasterPage)Master).ScriptManager.Navigate += new EventHandler<HistoryEventArgs>(History_Navigate);

        neighborhoods.OnChange += new EventHandler(neighborhoods_OnChange);

        if (!IsPostBack)
        {
            linkLocal.Visible = SessionManager.IsLoggedIn && !string.IsNullOrEmpty(SessionManager.Account.City);

            if (SessionManager.IsLoggedIn)
            {
                linkLocal.Text = string.Format("&#187; All {0} Places", Renderer.Render(SessionManager.Account.City));
            }

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

            GetNeighborhoodsData(sender, e);

            GetData(sender, e);

            if ((gridManage.VirtualItemCount == 0) && (Request.QueryString.Count == 0))
            {
                panelSearchInternal.Visible = false;
                LocationSelector.ClearSelection();
                GetData(sender, e);
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Eat Out", Request, "PlacesView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx", inputCountry.SelectedValue, inputState.SelectedValue, inputCity.SelectedValue, inputNeighborhood.SelectedValue, inputType.SelectedValue));
            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(search);
    }

    void LocationSelector_LocationChanged(object sender, EventArgs e)
    {
        linkAdd.NavigateUrl =
            string.Format("PlaceEdit.aspx?city={0}&state={1}&country={2}&neighborhood={3}&type={4}",
                Renderer.UrlEncode(inputCity.SelectedValue),
                Renderer.UrlEncode(inputState.SelectedValue),
                Renderer.UrlEncode(inputCountry.SelectedValue),
                Renderer.UrlEncode(inputNeighborhood.SelectedValue),
                Renderer.UrlEncode(inputType.SelectedValue));

        panelLinks.Update();
    }

    void History_Navigate(object sender, HistoryEventArgs e)
    {
        if (e.State.HasKeys())
        {
            LocationWithOptionsEventArgs l_args = new LocationWithOptionsEventArgs(e.State);
            l_args.Clear = true;
            LocationSelector.SelectLocation(sender, l_args);
            gridManage.CurrentPageIndex = int.Parse(e.State["page"]);
            inputName.Text = e.State["name"];
            atoz.SelectIfChar(inputName.Text);
        }
        else
        {
            LocationSelector.ClearSelection();
            inputName.Text = string.Empty;
            atoz.ClearSelection();
        }

        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    void GetNeighborhoodsData(object sender, EventArgs e)
    {
        neighborhoods.City = inputCity.SelectedValue;
        neighborhoods.State = inputState.SelectedValue;
        neighborhoods.Country = inputCountry.SelectedValue;
        neighborhoods.DataBind();
        panelNeighborhoods.Update();
    }

    void LocationSelector_CityChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
        panelCountryState.Update();
        panelCity.Update();
        panelNeighborhood.Update();
    }

    void LocationSelector_StateChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
        panelCountryState.Update();
        panelCity.Update();
    }

    void LocationSelector_CountryChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
        panelCountryState.Update();
    }

    public void gridManage_DataBinding(object sender, EventArgs e)
    {
        panelGrid.Update();
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<TransitPlace, TransitPlaceQueryOptions>(
            GetQueryOptions(), SessionManager.PlaceService.GetPlacesCount);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        if (!SessionManager.IsLoggedIn)
            return;

        checkboxPicturesOnly.Checked = false;
        inputName.Text = string.Empty;
        LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
        GetData(sender, e);
        panelSearch.Update();
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        checkboxPicturesOnly.Checked = false;
        LocationSelector.ClearSelection();
        inputName.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    private TransitPlaceQueryOptions GetQueryOptions()
    {
        TransitPlaceQueryOptions options = new TransitPlaceQueryOptions();
        options.PicturesOnly = checkboxPicturesOnly.Checked;
        options.Neighborhood = inputNeighborhood.Text;
        options.City = inputCity.Text;
        options.Country = inputCountry.SelectedValue;
        options.State = inputState.SelectedValue;
        options.Name = inputName.Text;
        options.Type = inputType.SelectedValue;
        return options;
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        TransitPlaceQueryOptions options = GetQueryOptions();

        string args = string.Format("city={0}&country={1}&state={2}&name={3}&type={4}&pictures={5}&neighborhood={6}&page={7}",
                Renderer.UrlEncode(options.City),
                Renderer.UrlEncode(options.Country),
                Renderer.UrlEncode(options.State),
                Renderer.UrlEncode(options.Name),
                Renderer.UrlEncode(options.Type),
                Renderer.UrlEncode(options.PicturesOnly),
                Renderer.UrlEncode(options.Neighborhood),
                gridManage.CurrentPageIndex);

        Title = titlePlaces.Text = (string.IsNullOrEmpty(options.City)
            ? titlePlaces.DefaultText
            : string.Format("{0}: {1}", titlePlaces.DefaultText, options.City));

        if (IsPostBack)
        {
            Title = string.Format("{0} - {1}", SessionManager.GetCachedConfiguration(
                "SnCore.Title", "SnCore"), titlePlaces.Text);
        }

        linkRelRss.NavigateUrl = string.Format("PlacesRss.aspx?{0}", args);
        linkPermalink.NavigateUrl = string.Format("PlacesView.aspx?{0}", args);

        if (((SnCoreMasterPage)Master).ScriptManager.IsInAsyncPostBack && 
            !((SnCoreMasterPage)Master).ScriptManager.IsNavigating)
        {
            NameValueCollection history = new NameValueCollection();
            history.Add("city", options.City);
            history.Add("country", options.Country);
            history.Add("state", options.State);
            history.Add("name", options.Name);
            history.Add("type", options.Type);
            history.Add("picturesonly", options.PicturesOnly.ToString());
            history.Add("neighborhood", options.Neighborhood);
            history.Add("page", gridManage.CurrentPageIndex.ToString());
            ((SnCoreMasterPage)Master).ScriptManager.AddHistoryPoint(history, Page.Title);
        }

        ServiceQueryOptions serviceoptions = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
        gridManage.DataSource = SessionManager.GetCollection<TransitPlace, TransitPlaceQueryOptions>(
            options, serviceoptions, SessionManager.PlaceService.GetPlaces);

        panelLinks.Update();
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
        panelSearch.Update();
    }

    public void neighborhoods_OnChange(object sender, EventArgs e)
    {
        LocationSelector.SelectLocation(sender, new LocationEventArgs(
            neighborhoods.Country, neighborhoods.State, neighborhoods.City, neighborhoods.Neighborhood));
        checkboxPicturesOnly.Checked = false;
        inputName.Text = string.Empty;
        GetData(sender, e);
        panelSearch.Update();
    }

    public void atoz_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        inputName.Text = e.CommandArgument.ToString();
        GetData(sender, e);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        panelSearch.Update();
        NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
        GetData(sender, e);
    }
}

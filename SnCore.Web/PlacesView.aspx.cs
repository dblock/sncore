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

public partial class PlacesView : Page
{
    public class LocationWithTypeEventArgs : LocationEventArgs
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

        public LocationWithTypeEventArgs(TransitAccount account)
            : base(account)
        {
        }

        public LocationWithTypeEventArgs(HttpRequest request)
            : base(request)
        {
            mType = request["type"];
        }

        public LocationWithTypeEventArgs(string country, string state, string city, string neighborhood, string type)
            : base(country, state, city, neighborhood)
        {
            mType = type;
        }
    }

    public class LocationSelectorWithType : LocationSelectorCountryStateCityNeighborhood
    {
        private DropDownList mType;

        public LocationSelectorWithType(
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
                List<TransitPlaceType> types = new List<TransitPlaceType>();
                if (InsertEmptySelection) types.Add(new TransitPlaceType());
                types.AddRange(mPage.SessionManager.GetCachedCollection<TransitPlaceType>(
                    mPage.SessionManager.PlaceService, "GetPlaceTypes", null));
                mType.DataSource = types;
                mType.DataBind();
            }
        }

        public void SelectLocation(object sender, LocationWithTypeEventArgs e)
        {
            base.SelectLocation(sender, e);

            if (mType != null)
            {
                mType.ClearSelection();
                ListItem type = mType.Items.FindByValue(e.Type);
                if (type != null)
                {
                    type.Selected = true;
                }
            }
        }

        public override void ClearSelection()
        {
            mType.ClearSelection();
            base.ClearSelection();
        }
    }

    private LocationSelectorWithType mLocationSelector = null;

    public LocationSelectorWithType LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorWithType(
                    this, true, inputCountry, inputState, inputCity, inputNeighborhood, inputType);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(search);
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);

            LocationSelector.CountryChanged += new EventHandler(LocationSelector_CountryChanged);
            LocationSelector.StateChanged += new EventHandler(LocationSelector_StateChanged);
            LocationSelector.CityChanged += new EventHandler(LocationSelector_CityChanged);

            neighborhoods.OnChange += new EventHandler(neighborhoods_OnChange);
            
            if (!IsPostBack)
            {
                linkLocal.Visible = SessionManager.IsLoggedIn && ! string.IsNullOrEmpty(SessionManager.Account.City);

                if (SessionManager.IsLoggedIn)
                {
                    linkLocal.Text = string.Format("&#187; All {0} Places", Renderer.Render(SessionManager.Account.City));
                }

                if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
                {
                    LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
                }
                else
                {
                    LocationSelector.SelectLocation(sender, new LocationEventArgs(Request));
                    bool picturesOnly = true;
                    if (bool.TryParse(Request["PicturesOnly"], out picturesOnly))
                    {
                        checkboxPicturesOnly.Checked = picturesOnly;
                    }
                }

                GetNeighborhoodsData(sender, e);

                GetData(sender, e);
                
                if (gridManage.VirtualItemCount == 0)
                {
                    LocationSelector.ClearSelection();
                    GetData(sender, e);
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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
        object[] args = { GetQueryOptions() };
        gridManage.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            PlaceService, "GetPlacesCount", args);
        gridManage_OnGetDataSource(sender, e);
        gridManage.DataBind();
    }

    public void linkLocal_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SessionManager.IsLoggedIn)
                return;

            checkboxPicturesOnly.Checked = false;
            inputName.Text = string.Empty;
            LocationSelector.SelectLocation(sender, new LocationEventArgs(SessionManager.Account));
            GetData(sender, e);
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkAll_Click(object sender, EventArgs e)
    {
        try
        {
            checkboxPicturesOnly.Checked = false;
            LocationSelector.ClearSelection();
            inputName.Text = string.Empty;
            GetData(sender, e);
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private TransitPlaceQueryOptions GetQueryOptions()
    {
        TransitPlaceQueryOptions options = new TransitPlaceQueryOptions();
        options.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
        options.SortOrder = listboxSelectSortOrder.SelectedValue;
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
        try
        {
            TransitPlaceQueryOptions options = GetQueryOptions();

            linkRss.NavigateUrl = linkRelRss.Attributes["href"] =
                string.Format("PlacesRss.aspx?order={0}&asc={1}&city={2}&country={3}&state={4}&name={5}&type={6}&pictures={7}&neighborhood={8}",
                    Renderer.UrlEncode(options.SortOrder),
                    Renderer.UrlEncode(options.SortAscending),
                    Renderer.UrlEncode(options.City),
                    Renderer.UrlEncode(options.Country),
                    Renderer.UrlEncode(options.State),
                    Renderer.UrlEncode(options.Name),
                    Renderer.UrlEncode(options.Type),
                    Renderer.UrlEncode(options.PicturesOnly),
                    Renderer.UrlEncode(options.Neighborhood));

            ServiceQueryOptions serviceoptions = new ServiceQueryOptions(gridManage.PageSize, gridManage.CurrentPageIndex);
            object[] args = { options, serviceoptions };
            gridManage.DataSource = SessionManager.GetCachedCollection<TransitPlace>(PlaceService, "GetPlaces", args);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        try
        {
            panelSearchInternal.PersistentVisible = !panelSearchInternal.PersistentVisible;
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void neighborhoods_OnChange(object sender, EventArgs e)
    {
        try
        {
            LocationSelector.SelectLocation(sender, new LocationEventArgs(
                neighborhoods.Country, neighborhoods.State, neighborhoods.City, neighborhoods.Neighborhood));
            checkboxPicturesOnly.Checked = false;
            inputName.Text = string.Empty;
            GetData(sender, e);
            panelSearch.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

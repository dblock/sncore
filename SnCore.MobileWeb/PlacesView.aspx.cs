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
using SnCore.WebServices;
using SnCore.SiteMap;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;

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

        public LocationWithOptionsEventArgs(AccountService.TransitAccount account)
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
                List<PlaceService.TransitPlaceType> types = new List<PlaceService.TransitPlaceType>();
                if (InsertEmptySelection) types.Add(new PlaceService.TransitPlaceType());
                types.AddRange(page.SessionManager.GetCollection<PlaceService.TransitPlaceType, PlaceService.ServiceQueryOptions>(
                    null, page.SessionManager.PlaceService.GetPlaceTypes));
                mType.DataSource = types;
                mType.DataBind();
            }
        }

        public bool SelectLocation(object sender, LocationWithOptionsEventArgs e)
        {
            bool result = base.SelectLocation(sender, e);

            if (mType != null && !string.IsNullOrEmpty(e.Type))
            {
                mType.ClearSelection();
                ListItem type = mType.Items.FindByValue(e.Type);
                if (type != null)
                {
                    type.Selected = true;
                    result = true;
                }
            }

            if (mPicturesOnly != null && !string.IsNullOrEmpty(e.PicturesOnly))
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

        public override string GetUri()
        {
            string result = base.GetUri();
            AppendUriPart(mType, "type", ref result);
            return result;
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

        neighborhoods.OnChange += new EventHandler(neighborhoods_OnChange);

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
                    // panelSearchInternal.Visible = true;
                }
            }

            GetNeighborhoodsData(sender, e);
            GetData(sender, e);

            if ((gridManage.VirtualItemCount == 0) && (Request.QueryString.Count == 0))
            {
                // panelSearchInternal.Visible = false;
                LocationSelector.ClearSelection();
                GetData(sender, e);
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            if (Request.QueryString.Count != 0)
            {
                sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx",
                    inputCountry.SelectedValue, inputState.SelectedValue, inputCity.SelectedValue, inputNeighborhood.SelectedValue, inputType.SelectedValue));
            }
            StackSiteMap(sitemapdata);
        }
    }

    void LocationSelector_CityChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
    }

    void LocationSelector_StateChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
    }

    void LocationSelector_CountryChanged(object sender, EventArgs e)
    {
        GetNeighborhoodsData(sender, e);
    }

    private void GetData(object sender, EventArgs e)
    {
        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = SessionManager.GetCount<
            PlaceService.TransitPlace, PlaceService.ServiceQueryOptions, PlaceService.TransitPlaceQueryOptions>(
            GetQueryOptions(), SessionManager.PlaceService.GetPlacesCount);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();
    }

    private PlaceService.TransitPlaceQueryOptions GetQueryOptions()
    {
        PlaceService.TransitPlaceQueryOptions options = new PlaceService.TransitPlaceQueryOptions();
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
        PlaceService.TransitPlaceQueryOptions options = GetQueryOptions();

        PlaceService.ServiceQueryOptions serviceoptions = new PlaceService.ServiceQueryOptions();
        serviceoptions.PageSize = gridManage.PageSize;
        serviceoptions.PageNumber = gridManage.CurrentPageIndex;
        gridManage.DataSource = SessionManager.GetCollection<
            PlaceService.TransitPlace, PlaceService.ServiceQueryOptions, PlaceService.TransitPlaceQueryOptions>(
            options, serviceoptions, SessionManager.PlaceService.GetPlaces);
    }

    public void cities_SelectedChanged(object sender, CommandEventArgs e)
    {
        Redirect(string.Format("PlacesView.aspx?{0}", e.CommandArgument));
        //NameValueCollection args = Renderer.ParseQueryString(e.CommandArgument.ToString());
        //LocationSelector.SelectLocation(sender, new LocationWithOptionsEventArgs(args));
        //GetData(sender, e);
    }

    public void search_Click(object sender, EventArgs e)
    {
        GetData(sender, e);
    }

    void GetNeighborhoodsData(object sender, EventArgs e)
    {
        neighborhoods.City = inputCity.SelectedValue;
        neighborhoods.State = inputState.SelectedValue;
        neighborhoods.Country = inputCountry.SelectedValue;
        neighborhoods.DataBind();
    }

    public void neighborhoods_OnChange(object sender, EventArgs e)
    {
        LocationSelector.SelectLocation(sender, new LocationEventArgs(neighborhoods.Country, neighborhoods.State, neighborhoods.City, neighborhoods.Neighborhood));
        Redirect(string.Format("PlacesView.aspx?{0}", LocationSelector.GetUri()));
    }
}

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

public partial class PlacesView : Page
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;
        public string Type;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City, string.Empty)
        {

        }

        public SelectLocationEventArgs(HttpRequest request)
            : this(request["country"], request["state"], request["city"], request["type"])
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city,
            string type)
        {
            Country = country;
            State = state;
            City = city;
            Type = type;
        }
    }

    private TransitPlaceQueryOptions mOptions = null;

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(search);
            gridManage.OnGetDataSource += new EventHandler(gridManage_OnGetDataSource);
            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitPlaceType());
                types.AddRange(PlaceService.GetPlaceTypes());
                inputType.DataSource = types;
                inputType.DataBind();

                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                countries.AddRange(LocationService.GetCountries());
                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                if (SessionManager.IsLoggedIn && (Request.QueryString.Count == 0))
                {
                    SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
                }
                else
                {
                    SelectLocation(sender, new SelectLocationEventArgs(Request));
                }
                GetData();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private void GetData()
    {
        mOptions = null;

        gridManage.CurrentPageIndex = 0;
        gridManage.VirtualItemCount = PlaceService.GetPlacesCount(QueryOptions);
        gridManage_OnGetDataSource(this, null);
        gridManage.DataBind();

        if (gridManage.VirtualItemCount == 0)
        {
            labelCount.Text = "no places";
        }
        else if (gridManage.VirtualItemCount == 1)
        {
            labelCount.Text = "1 place";
        }
        else
        {
            labelCount.Text = string.Format("{0} places !", gridManage.VirtualItemCount);
        }
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList states = new ArrayList();
            states.Add(new TransitState());
            states.AddRange(LocationService.GetStatesByCountry(inputCountry.SelectedValue));
            inputState.DataSource = states;
            inputState.DataBind();
            inputState_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }


    public void inputState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ArrayList cities = new ArrayList();
            cities.Add(new TransitCity());
            cities.AddRange(LocationService.GetCitiesByLocation(inputCountry.SelectedValue, inputState.SelectedValue));
            inputCity.DataSource = cities;
            inputCity.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private TransitPlaceQueryOptions QueryOptions
    {
        get
        {
            if (mOptions == null)
            {
                mOptions = new TransitPlaceQueryOptions();
                mOptions.SortAscending = bool.Parse(listboxSelectOrderBy.SelectedValue);
                mOptions.SortOrder = listboxSelectSortOrder.SelectedValue;
                mOptions.PicturesOnly = checkboxPicturesOnly.Checked;
                mOptions.City = inputCity.Text;
                mOptions.Country = inputCountry.SelectedValue;
                mOptions.State = inputState.SelectedValue;
                mOptions.Name = inputName.Text;
                mOptions.Type = inputType.SelectedValue;
            }
            return mOptions;
        }
    }

    void gridManage_OnGetDataSource(object sender, EventArgs e)
    {
        try
        {
            TransitPlaceQueryOptions options = QueryOptions;

            linkRss.NavigateUrl = linkRelRss.Attributes["href"] =
                string.Format("PlacesRss.aspx?order={0}&asc={1}&city={2}&country={3}&state={4}&name={5}&type={6}&pictures={7}",
                    Renderer.UrlEncode(QueryOptions.SortOrder),
                    Renderer.UrlEncode(QueryOptions.SortAscending),
                    Renderer.UrlEncode(QueryOptions.City),
                    Renderer.UrlEncode(QueryOptions.Country),
                    Renderer.UrlEncode(QueryOptions.State),
                    Renderer.UrlEncode(QueryOptions.Name),
                    Renderer.UrlEncode(QueryOptions.Type),
                    Renderer.UrlEncode(QueryOptions.PicturesOnly));

            ServiceQueryOptions serviceoptions = new ServiceQueryOptions();
            serviceoptions.PageSize = gridManage.PageSize;
            serviceoptions.PageNumber = gridManage.CurrentPageIndex;
            gridManage.DataSource = PlaceService.GetPlaces(options, serviceoptions);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void SelectLocation(object sender, SelectLocationEventArgs e)
    {
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
            inputType.ClearSelection();
            inputType.Items.FindByValue(e.Type).Selected = true;
        }
        catch
        {

        }
    }

    public void search_Click(object sender, EventArgs e)
    {
        try
        {
            GetData();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

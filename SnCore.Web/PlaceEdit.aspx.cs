using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class PlaceEdit : AuthenticatedPage
{
    private LocationSelectorCountryStateCityNeighborhoodText mLocationSelector = null;

    public LocationSelectorCountryStateCityNeighborhoodText LocationSelector
    {
        get
        {
            if (mLocationSelector == null)
            {
                mLocationSelector = new LocationSelectorCountryStateCityNeighborhoodText(
                    this, false, inputCountry, inputState, inputCity, inputNeighborhood);
            }

            return mLocationSelector;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        gridPlaceNamesManage.OnGetDataSource += new EventHandler(gridPlaceNamesManage_OnGetDataSource);
        LocationSelector.LocationChanged += new EventHandler(LocationSelector_LocationChanged);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("Place");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputStreet.MaxLength = cs["Street"].MaxLengthInChars;
            inputZip.MaxLength = cs["Zip"].MaxLengthInChars;
            inputCrossStreet.MaxLength = cs["CrossStreet"].MaxLengthInChars;
            inputPhone.MaxLength = cs["Phone"].MaxLengthInChars;
            inputFax.MaxLength = cs["Fax"].MaxLengthInChars;
            inputEmail.MaxLength = cs["Email"].MaxLengthInChars;

            DomainClass acs = SessionManager.GetDomainClass("PlaceName");
            inputAltName.MaxLength = acs["Name"].MaxLengthInChars;

            gridPlaceNamesManage_OnGetDataSource(sender, e);
            gridPlaceNamesManage.DataBind();

            ppg.PlaceId = RequestId;
            ppg.DataBind();

            GetPlaceTypes(sender, e);

            if (RequestId > 0)
            {
                TransitPlace place = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, RequestId);
                labelName.Text = Renderer.Render(place.Name);
                inputName.Text = place.Name;
                inputDescription.Text = place.Description;
                inputCrossStreet.Text = place.CrossStreet;
                inputEmail.Text = place.Email;
                inputFax.Text = place.Fax;
                inputPhone.Text = place.Phone;
                inputStreet.Text = place.Street;
                inputZip.Text = place.Zip;
                ListItemManager.TrySelect(selectType, place.Type);
                LocationSelector.SelectLocation(sender, new LocationEventArgs(place));
                linkEditAttributes.NavigateUrl = string.Format("PlaceAttributesManage.aspx?id={0}", place.Id);
                linkEditPictures.NavigateUrl = string.Format("PlacePicturesManage.aspx?id={0}", place.Id);
                sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request.Url));
                placeredirect.TargetUri = string.Format("PlaceView.aspx?id={0}", place.Id);
            }
            else
            {
                panelPlaceAltName.Visible = false;
                LocationSelector.ChangeCountry(sender, e);

                string type = Request.QueryString["type"];
                ListItemManager.TrySelect(selectType, type);

                string name = Request.QueryString["name"];
                if (!string.IsNullOrEmpty(name)) inputName.Text = name;

                if (!LocationSelector.SelectLocation(sender, new LocationEventArgs(Request)))
                {
                    LocationSelector.ChangeCityWithAccountDefault(sender, new CityLocationEventArgs(
                        Request.QueryString["city"]));
                }

                linkDelete.Visible = false;
                linkEditAttributes.Visible = false;
                linkEditPictures.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New Place", Request.Url));

                placeredirect.Visible = false;
            }

            LocationSelector_LocationChanged(sender, e);
            StackSiteMap(sitemapdata);
        }

        if (!SessionManager.HasVerifiedEmailAddress())
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before submitting places.");

            manageAdd.Enabled = false;
        }

        SetDefaultButton(manageAdd);
    }

    void LocationSelector_LocationChanged(object sender, EventArgs e)
    {
        autoCompleteCity.ContextKey = string.Format("{0};{1}",
            inputCountry.Text, inputState.Text);
        panelCity.Update();
        autoCompleteNeighborhood.ContextKey = string.Format("{0};{1};{2}",
            inputCountry.Text, inputState.Text, inputCity.Text);
        panelNeighborhood.Update();
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitPlace t = new TransitPlace();
        t.Name = inputName.Text;
        t.Type = selectType.SelectedValue;
        t.Id = RequestId;
        t.Description = inputDescription.Text;
        t.CrossStreet = inputCrossStreet.Text;
        t.Email = inputEmail.Text;
        t.Fax = inputFax.Text;
        t.Phone = inputPhone.Text;
        t.Street = inputStreet.Text;
        t.Zip = inputZip.Text;
        t.City = inputCity.Text;
        t.Neighborhood = inputNeighborhood.Text;
        t.State = inputState.SelectedValue;
        t.Country = inputCountry.SelectedValue;

        if (string.IsNullOrEmpty(t.Name)) throw new Exception("Please enter a name.");
        if (string.IsNullOrEmpty(t.City)) throw new Exception("Please enter a city.");
        if (string.IsNullOrEmpty(t.Country)) throw new Exception("Please enter a country.");

        int place_id = SessionManager.CreateOrUpdate<TransitPlace>(
            t, SessionManager.PlaceService.CreateOrUpdatePlace);

        ppg.PlaceId = place_id;
        ppg.save_Click(sender, e);

        Redirect(string.Format("PlaceView.aspx?id={0}", place_id));
    }

    void gridPlaceNamesManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridPlaceNamesManage.DataSource = SessionManager.PlaceService.GetPlaceNames(
            SessionManager.Ticket, RequestId, null);
    }

    public void altname_save_Click(object sender, EventArgs e)
    {
        TransitPlaceName tn = new TransitPlaceName();
        tn.Name = inputAltName.Text;
        tn.PlaceId = RequestId;
        SessionManager.CreateOrUpdate<TransitPlaceName>(
            tn, SessionManager.PlaceService.CreateOrUpdatePlaceName);
        ReportInfo("Alternate name added.");
        gridPlaceNamesManage_OnGetDataSource(sender, e);
        gridPlaceNamesManage.DataBind();
        inputAltName.Text = string.Empty;
    }

    private enum Cells
    {
        id = 0
    }

    public void gridPlaceNamesManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitPlaceName>(id, SessionManager.PlaceService.DeletePlaceName);
                ReportInfo("Alternate place name deleted.");
                gridPlaceNamesManage.CurrentPageIndex = 0;
                gridPlaceNamesManage_OnGetDataSource(sender, e);
                gridPlaceNamesManage.DataBind();
                break;
        }
    }

    public void linkDelete_Click(object sender, EventArgs e)
    {
        SessionManager.Delete<TransitPlace>(RequestId, SessionManager.PlaceService.DeletePlace);
        Redirect("AccountPlacesManage.aspx");
    }

    public void linkLookup_Click(object sender, EventArgs e)
    {
        panelLookup.Update();

        if (string.IsNullOrEmpty(inputName.Text))
        {
            labelLookup.Text = "Please enter a name.";
            return;
        }

        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = 0;
        options.PageSize = 10;
        gridLookupPlaces.DataSource = SessionManager.GetCollection<TransitPlace, string>(
            inputName.Text, options, SessionManager.PlaceService.SearchPlaces);
        gridLookupPlaces.DataBind();

        if (gridLookupPlaces.Items.Count == 0)
        {
            labelLookup.Text = string.Format("No places matching '{0}'.",
                base.Render(inputName.Text));
        }
    }

    private void GetPlaceTypes(object sender, EventArgs e)
    {
        IList<TransitPlaceType> types = SessionManager.GetCollection<TransitPlaceType>(
            null, SessionManager.PlaceService.GetPlaceTypes);

        TransitPlaceType selected = null;
        foreach (TransitPlaceType tp in types)
        {
            if (tp.DefaultType)
            {
                selected = tp;
                break;
            }
        }

        if (selected == null)
        {
            types.Insert(0, new TransitPlaceType());
        }

        selectType.DataSource = types;
        selectType.DataBind();

        if (selected != null)
        {
            ListItemManager.TrySelect(selectType, selected.Name);
        }
    }
}

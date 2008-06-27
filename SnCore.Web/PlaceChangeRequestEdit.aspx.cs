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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using SnCore.WebControls;

public partial class PlaceChangeRequestEdit : AuthenticatedPage
{
    private int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }
               
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
        if (RequestId <= 0 && GetId("pid") == 0)
        {
            throw new Exception("Missing Place");
        }

        LocationSelector.LocationChanged += new EventHandler(LocationSelector_LocationChanged);

        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("PlaceChangeRequest");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;
            inputStreet.MaxLength = cs["Street"].MaxLengthInChars;
            inputZip.MaxLength = cs["Zip"].MaxLengthInChars;
            inputCrossStreet.MaxLength = cs["CrossStreet"].MaxLengthInChars;
            inputPhone.MaxLength = cs["Phone"].MaxLengthInChars;
            inputFax.MaxLength = cs["Fax"].MaxLengthInChars;
            inputEmail.MaxLength = cs["Email"].MaxLengthInChars;

            ArrayList types = new ArrayList();
            types.Add(new TransitAccountPlaceType());
            types.AddRange(SessionManager.PlaceService.GetPlaceTypes(SessionManager.Ticket, null));
            selectType.DataSource = types;
            selectType.DataBind();

            TransitPlace place = null;

            if (RequestId > 0)
            {
                TransitPlaceChangeRequest request = SessionManager.PlaceService.GetPlaceChangeRequestById(
                    SessionManager.Ticket, RequestId);
                labelName.Text = Renderer.Render(request.Name);
                inputName.Text = request.Name;
                inputDescription.Text = request.Description;
                inputCrossStreet.Text = request.CrossStreet;
                inputEmail.Text = request.Email;
                inputFax.Text = request.Fax;
                inputPhone.Text = request.Phone;
                inputStreet.Text = request.Street;
                inputZip.Text = request.Zip;
                ListItemManager.TrySelect(selectType, request.Type);
                LocationSelector.SelectLocation(sender, new LocationEventArgs(request));

                place = SessionManager.PlaceService.GetPlaceById(
                    SessionManager.Ticket, request.PlaceId);
            }
            else
            {
                place = SessionManager.PlaceService.GetPlaceById(
                    SessionManager.Ticket, PlaceId);
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
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx", place.Country, place.State, place.City, place.Neighborhood, place.Type));
            sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Request Changes", Request.Url));
            StackSiteMap(sitemapdata);

            linkBack.NavigateUrl = (RequestId > 0 ? 
                string.Format("AccountPlaceChangeRequestsManage.aspx?id={0}", RequestId)
                : string.Format("PlaceView.aspx?id={0}", place.Id));
        }

        if (!SessionManager.HasVerifiedEmailAddress())
        {
            ReportWarning("You don't have any verified e-mail addresses.\n" +
                "You must add/confirm a valid e-mail address before submitting place changes.");

            save.Enabled = false;
        }

        LocationSelector_LocationChanged(sender, e);
        SetDefaultButton(save);
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
        TransitPlaceChangeRequest t = (RequestId > 0) ? SessionManager.PlaceService.GetPlaceChangeRequestById(
            SessionManager.Ticket, RequestId) : new TransitPlaceChangeRequest();

        if (PlaceId > 0) t.PlaceId = PlaceId;
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

        SessionManager.CreateOrUpdate<TransitPlaceChangeRequest>(
            t, SessionManager.PlaceService.CreateOrUpdatePlaceChangeRequest);

        Redirect(linkBack.NavigateUrl);
    }
}

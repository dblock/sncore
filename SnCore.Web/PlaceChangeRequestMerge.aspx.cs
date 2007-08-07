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

public partial class PlaceChangeRequestMerge : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RequestId == 0)
            {
                throw new Exception("Missing place merge request.");
            }

            TransitPlaceChangeRequest request = SessionManager.PlaceService.GetPlaceChangeRequestById(
                SessionManager.Ticket, RequestId);

            TransitPlace place = SessionManager.PlaceService.GetPlaceById(
                SessionManager.Ticket, request.PlaceId);

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            sitemapdata.AddRange(SiteMapDataAttribute.GetLocationAttributeNodes(Request, "PlacesView.aspx", place.Country, place.State, place.City, place.Neighborhood, place.Type));
            sitemapdata.Add(new SiteMapDataAttributeNode(place.Name, Request, string.Format("PlaceView.aspx?id={0}", place.Id)));
            sitemapdata.Add(new SiteMapDataAttributeNode("Merge Changes", Request.Url));
            StackSiteMap(sitemapdata);

            Title = labelName.Text = string.Format("{0}: Merge Changes", Renderer.Render(place.Name));
            linkBack.NavigateUrl = string.Format("PlaceChangeRequestsManage.aspx?id={0}", place.Id);

            AddListItem(place.Name, request.Name, selectName);
            AddListItem(place.Type, request.Type, selectType);
            AddListItem(place.Description, request.Description, selectDescription);
            AddListItem(place.Country, request.Country, selectCountry);
            AddListItem(place.State, request.State, selectState);
            AddListItem(place.City, request.City, selectCity);
            AddListItem(place.Neighborhood, request.Neighborhood, selectNeighborhood);
            AddListItem(place.Phone, request.Phone, selectPhone);
            AddListItem(place.Fax, request.Fax, selectFax);
            AddListItem(place.Website, request.Website, selectWebsite);
            AddListItem(place.Street, request.Street, selectStreet);
            AddListItem(place.Zip, request.Zip, selectZip);
            AddListItem(place.CrossStreet, request.CrossStreet, selectCrossStreet);
            AddListItem(place.Email, request.Email, selectEmail);
        }

        SetDefaultButton(merge);
    }

    private void AddListItem(string to, string from, RadioButtonList list)
    {
        list.Items.Add(new ListItem(to));

        if (to != from)
        {
            list.Items.Add(new ListItem(from));
        }

        list.Items[0].Selected = true;
    }

    public void merge_Click(object sender, EventArgs e)
    {
        TransitPlaceChangeRequest request = SessionManager.PlaceService.GetPlaceChangeRequestById(
            SessionManager.Ticket, RequestId);

        TransitPlace place = SessionManager.PlaceService.GetPlaceById(
            SessionManager.Ticket, request.PlaceId);

        place.City = selectCity.SelectedValue;
        place.Country = selectCountry.SelectedValue;
        place.CrossStreet = selectCrossStreet.SelectedValue;
        place.Description = selectDescription.SelectedValue;
        place.Email = selectEmail.SelectedValue;
        place.Fax = selectFax.SelectedValue;
        place.Name = selectName.SelectedValue;
        place.Neighborhood = selectNeighborhood.SelectedValue;
        place.Phone = selectPhone.SelectedValue;
        place.State = selectState.SelectedValue;
        place.Street = selectStreet.SelectedValue;
        place.Type = selectType.SelectedValue;
        place.Website = selectWebsite.SelectedValue;
        place.Zip = selectZip.SelectedValue;

        SessionManager.CreateOrUpdate<TransitPlace>(place, SessionManager.PlaceService.CreateOrUpdatePlace);
        SessionManager.Delete<TransitPlaceChangeRequest>(request.Id, SessionManager.PlaceService.DeletePlaceChangeRequest);

        Redirect(string.Format("PlaceView.aspx?id={0}", place.Id));
    }
}

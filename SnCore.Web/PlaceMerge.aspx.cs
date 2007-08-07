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

public partial class PlaceMerge : AuthenticatedPage
{
    public int MergeId
    {
        get
        {
            return GetId("mid");
        }
    }

    public TransitPlace GetMergeTo()
    {
        return SessionManager.PlaceService.GetPlaceById(
            SessionManager.Ticket, RequestId);
    }

    public TransitPlace GetMergeFrom()
    {
        return SessionManager.PlaceService.GetPlaceById(
            SessionManager.Ticket, MergeId);
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Places", Request, "PlacesView.aspx"));
            StackSiteMap(sitemapdata);

            if (RequestId == 0)
            {
                throw new Exception("Missing Merge Place");
            }

            TransitPlace mergeto = GetMergeTo();
            linkMergeInto.Text = Renderer.Render(mergeto.Name);
            linkMergeInto.NavigateUrl = string.Format("PlaceView.aspx?id={0}", mergeto.Id);

            if (MergeId == 0)
            {
                panelLookup.Visible = true;
                panelMerge.Visible = false;
                panelSelectMergeFrom.Visible = true;
                return;
            }

            if (MergeId == RequestId)
            {
                throw new Exception("Cannot merge place into itself.");
            }

            TransitPlace mergefrom = GetMergeFrom();
            linkMergeFrom.Text = Renderer.Render(mergefrom.Name);
            linkMergeFrom.NavigateUrl = string.Format("PlaceView.aspx?id={0}", mergefrom.Id);

            AddListItem(mergeto.Name, mergefrom.Name, selectName);
            AddListItem(mergeto.Type, mergefrom.Type, selectType);
            AddListItem(mergeto.Description, mergefrom.Description, selectDescription);
            AddListItem(mergeto.Country, mergefrom.Country, selectCountry);
            AddListItem(mergeto.State, mergefrom.State, selectState);
            AddListItem(mergeto.City, mergefrom.City, selectCity);
            AddListItem(mergeto.Neighborhood, mergefrom.Neighborhood, selectNeighborhood);
            AddListItem(mergeto.Phone, mergefrom.Phone, selectPhone);
            AddListItem(mergeto.Fax, mergefrom.Fax, selectFax);
            AddListItem(mergeto.Website, mergefrom.Website, selectWebsite);
            AddListItem(mergeto.Street, mergefrom.Street, selectStreet);
            AddListItem(mergeto.Zip, mergefrom.Zip, selectZip);
            AddListItem(mergeto.CrossStreet, mergefrom.CrossStreet, selectCrossStreet);
            AddListItem(mergeto.Email, mergefrom.Email, selectEmail);
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
        TransitPlace t_instance = GetMergeTo();
        t_instance.City = selectCity.SelectedValue;
        t_instance.Country = selectCountry.SelectedValue;
        t_instance.CrossStreet = selectCrossStreet.SelectedValue;
        t_instance.Description = selectDescription.SelectedValue;
        t_instance.Email = selectEmail.SelectedValue;
        t_instance.Fax = selectFax.SelectedValue;
        t_instance.Name = selectName.SelectedValue;
        t_instance.Neighborhood = selectNeighborhood.SelectedValue;
        t_instance.Phone = selectPhone.SelectedValue;
        t_instance.State = selectState.SelectedValue;
        t_instance.Street = selectStreet.SelectedValue;
        t_instance.Type = selectType.SelectedValue;
        t_instance.Website = selectWebsite.SelectedValue;
        t_instance.Zip = selectZip.SelectedValue;

        SessionManager.PlaceService.MergePlaces(
            SessionManager.Ticket, MergeId, t_instance);

        SessionManager.InvalidateCache<TransitPlace>();
        SessionManager.InvalidateCache<TransitPlacePicture>();
        Redirect(string.Format("PlaceView.aspx?id={0}", RequestId));
    }

    public void select_MergeFrom_Click(object sender, EventArgs e)
    {
        panelLookup.Update();

        if (string.IsNullOrEmpty(selectMergeFrom.Text))
        {
            labelLookup.Text = "Please enter a name.";
            return;
        }

        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = 0;
        options.PageSize = 10;
        gridLookupPlaces.DataSource = SessionManager.GetCollection<TransitPlace, string>(
            selectMergeFrom.Text, options, SessionManager.PlaceService.SearchPlaces);
        gridLookupPlaces.DataBind();

        if (gridLookupPlaces.Items.Count == 0)
        {
            labelLookup.Text = string.Format("No places matching '{0}'.",
                base.Render(selectMergeFrom.Text));
        }
    }
}

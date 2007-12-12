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
using SnCore.Services;
using Wilco.Web.UI;
using System.Text;
using SnCore.WebServices;
using SnCore.WebControls;

public partial class SelectPlaceControl : Control
{
    public class SelectLocationEventArgs : EventArgs
    {
        public string Country;
        public string State;
        public string City;

        public SelectLocationEventArgs(TransitAccount account)
            : this(account.Country, account.State, account.City)
        {

        }

        public SelectLocationEventArgs(
            string country,
            string state,
            string city)
        {
            Country = country;
            State = state;
            City = city;
        }
    }

    public event EventHandler Choose;

    public TransitPlace Place
    {
        get
        {
            TransitPlace result = (TransitPlace)ViewState["Place"];
            if (result == null)
            {
                result = new TransitPlace();
                ViewState["Place"] = result;
            }
            return result;
        }
        set
        {
            ViewState["Place"] = value;
        }
    }

    private void GetPlaceTypes(object sender, EventArgs e)
    {
        List<TransitPlaceType> types = SessionManager.PlaceService.GetPlaceTypes(SessionManager.Ticket, null);

        TransitPlaceType selected = null;
        foreach (TransitPlaceType Placetype in types)
        {
            if (Placetype.DefaultType)
            {
                selected = Placetype;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        PageManager.SetDefaultButton(buttonLookup, panelLookup.Controls);

        if (!IsPostBack)
        {
            GetPlaceTypes(sender, e);

            List<TransitCountry> countries = new List<TransitCountry>();
            countries.Add(new TransitCountry());
            string defaultcountry = SessionManager.GetCachedConfiguration("SnCore.Country.Default", "United States");
            countries.AddRange(SessionManager.GetCollection<TransitCountry, string>(
                defaultcountry, (ServiceQueryOptions)null, SessionManager.LocationService.GetCountriesWithDefault));

            ArrayList states = new ArrayList();
            states.Add(new TransitState());

            inputCountry.DataSource = countries;
            inputCountry.DataBind();

            inputState.DataSource = states;
            inputState.DataBind();

            if (Place.Id != 0)
            {
                ArrayList a = new ArrayList();
                a.Add(Place);
                chosenPlace.DataSource = a;
                chosenPlace.DataBind();
                panelAdd.Visible = false;
                panelLookup.Visible = false;
                lookupPlace.Enabled = true;
                addPlace.Enabled = true;
                IsChosen = true;
            }
            else if (!string.IsNullOrEmpty(Place.Name))
            {
                lookupPlace.Enabled = true;
                addPlace.Enabled = false;
                panelAdd.Visible = true;
                panelLookup.Visible = false;
                inputName.Text = Place.Name;
                inputStreet.Text = Place.Street;
                inputZip.Text = Place.Zip;
                inputWebsite.Text = Place.Website;
                inputCrossStreet.Text = Place.CrossStreet;
                inputPhone.Text = Place.Phone;
                inputFax.Text = Place.Fax;
                inputEmail.Text = Place.Email;
                SelectLocation(sender, new SelectLocationEventArgs(Place.Country, Place.State, Place.City));
                IsEditing = true;
            }
            else
            {
                SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
            }
        }

        UpdateEvents();
    }

    public void SavePlace(object sender, EventArgs e)
    {
        if (!IsEditing)
            return;

        Place = new TransitPlace();
        Place.Type = selectType.SelectedValue;
        if (string.IsNullOrEmpty(Place.Type))
        {
            throw new Exception("Please choose a type for this place.");
        }
        Place.Name = inputName.Text;
        Place.Country = inputCountry.SelectedValue;
        Place.City = inputCity.Text;
        Place.Neighborhood = inputNeighborhood.Text;
        if (string.IsNullOrEmpty(Place.Name)) throw new Exception("Please enter a place name.");
        if (string.IsNullOrEmpty(Place.Country)) throw new Exception("Please choose a country for this place.");
        if (string.IsNullOrEmpty(Place.City)) throw new Exception("Please choose a city for this place.");
        Place.State = inputState.Text;
        Place.Street = inputStreet.Text;
        Place.Zip = inputZip.Text;
        Place.CrossStreet = inputCrossStreet.Text;
        Place.Phone = inputPhone.Text;
        Place.Fax = inputFax.Text;
        Place.Email = inputEmail.Text;
        Place.Website = inputWebsite.Text;
        Place.Description = string.Empty;
        ArrayList a = new ArrayList();
        a.Add(Place);
        chosenPlace.DataSource = a;
        chosenPlace.DataBind();
        panelAdd.Visible = false;
        lookupPlace.Enabled = true;
        addPlace.Enabled = true;
        IsChosen = true;
        IsEditing = false;
        if (Choose != null) Choose(sender, e);
        panelSelectPlace.Update();
    }

    public void addPlace_Click(object sender, EventArgs e)
    {
        Place = null;
        panelAdd.Visible = true;
        panelLookup.Visible = false;
        chosenPlace.DataBind();
        gridLookupPlaces.DataBind();
        lookupPlace.Enabled = true;
        addPlace.Enabled = false;
        IsChosen = false;
        IsEditing = true;
        panelSelectPlace.Update();
    }

    public void lookupPlace_Click(object sender, EventArgs e)
    {
        Place = null;
        chosenPlace.DataBind();
        gridLookupPlaces.DataBind();
        panelLookup.Visible = true;
        panelAdd.Visible = false;
        inputLookupName.Text = string.Empty;
        lookupPlace.Enabled = false;
        addPlace.Enabled = true;
        IsChosen = false;
        panelSelectPlace.Update();
    }

    public bool IsChosen
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "IsChosen", false);
        }
        set
        {
            ViewState["IsChosen"] = value;
        }
    }

    public bool IsEditing
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "IsEditing", false);
        }
        set
        {
            ViewState["IsEditing"] = value;
        }
    }

    public void lookupChoose_Command(object sender, CommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());
        Place = SessionManager.GetInstance<TransitPlace, int>(
            id, SessionManager.PlaceService.GetPlaceById);
        ArrayList list = new ArrayList();
        list.Add(Place);
        chosenPlace.DataSource = list;
        chosenPlace.DataBind();
        panelLookup.Visible = false;
        lookupPlace.Enabled = true;
        addPlace.Enabled = true;
        IsChosen = true;
        panelSelectPlace.Update();
    }

    public void buttonLookup_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(inputLookupName.Text))
        {
            labelLookup.Text = "Please enter a name.";
            return;
        }

        IList<TransitPlace> list = SessionManager.GetCollection<TransitPlace, string>(
            inputLookupName.Text, (ServiceQueryOptions) null, SessionManager.PlaceService.SearchPlaces);

        gridLookupPlaces.DataSource = list;
        gridLookupPlaces.DataBind();

        if (list.Count == 0)
        {
            labelLookup.Text = string.Format("Cannot find any place matching '{0}'.",
                base.Render(inputLookupName.Text));
        }
        else if (list.Count == 1)
        {
            CommandEventArgs ca = new CommandEventArgs("Choose", list[0].Id);
            lookupChoose_Command(sender, ca);
        }

        panelSelectPlace.Update();
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        inputState.DataSource = SessionManager.GetCollection<TransitState, string>(
            inputCountry.SelectedValue, (ServiceQueryOptions) null, SessionManager.LocationService.GetStatesByCountryName);
        inputState.DataBind();

        panelSelectCountryState.Update();
    }

    public void SelectLocation(object sender, SelectLocationEventArgs e)
    {
        try
        {
            ListItemManager.TrySelect(inputCountry, e.Country);
            inputCountry_SelectedIndexChanged(sender, e);
            ListItemManager.TrySelect(inputState, e.State);
            inputCity.Text = e.City;
        }
        catch
        {

        }
    }

    private void UpdateEvents()
    {
        StringBuilder changeclick = new StringBuilder();
        changeclick.Append("if (this.href != '') {");
        changeclick.Append("this.disabled = true;");
        changeclick.AppendFormat("document.getElementById('{0}').style.cssText = 'display: none';", panelButtons.ClientID);
        changeclick.AppendFormat("document.getElementById('{0}').disabled = 'disabled';", panelPlace.ClientID);
        changeclick.AppendFormat("document.getElementById('{0}').innerText = '» please wait ...'", panelWorking.ClientID);
        changeclick.Append("}");

        lookupPlace.OnClientClick = changeclick.ToString();
        addPlace.OnClientClick = changeclick.ToString();
    }

    public void linkSearch_Click(object sender, EventArgs e)
    {
        lookupPlace_Click(sender, e);
        inputLookupName.Text = inputName.Text;
        buttonLookup_Click(sender, e);
    }

    public void add_Click(object sender, EventArgs e)
    {
        addPlace_Click(sender, e);
        inputName.Text = inputLookupName.Text;
    }
}

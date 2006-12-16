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

    protected void Page_Load(object sender, EventArgs e)
    {
            PageManager.SetDefaultButton(buttonLookup, panelLookup.Controls);

            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitAccountPlaceType());
                types.AddRange(SessionManager.GetCachedCollection<TransitPlaceType>(
                    SessionManager.PlaceService, "GetPlaceTypes", null));
                selectType.DataSource = types;
                selectType.DataBind();

                ArrayList countries = new ArrayList();
                countries.Add(new TransitCountry());
                object[] c_args = { null };
                countries.AddRange(SessionManager.GetCachedCollection<TransitCountry>(
                    SessionManager.LocationService, "GetCountries", c_args));

                ArrayList states = new ArrayList();
                states.Add(new TransitState());

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));

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
            }

            UpdateEvents();
    }

    public void SavePlace(object sender, EventArgs e)
    {
        if (! IsEditing)
            return;

        Place = new TransitPlace();
        Place.Type = selectType.SelectedValue;
        if (string.IsNullOrEmpty(Place.Type))
        {
            throw new Exception("Missing type.");
        }
        Place.Name = inputName.Text;
        Place.Country = inputCountry.SelectedValue;
        Place.City = inputCity.Text;
        Place.Neighborhood = inputNeighborhood.Text;
        if (string.IsNullOrEmpty(Place.Name)) throw new Exception("Missing name.");
        if (string.IsNullOrEmpty(Place.Country)) throw new Exception("Missing country.");
        if (string.IsNullOrEmpty(Place.City)) throw new Exception("Missing city.");
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
            object[] args = { SessionManager.Ticket, int.Parse(e.CommandArgument.ToString()) };
            Place = SessionManager.GetCachedItem<TransitPlace>(
                SessionManager.PlaceService, "GetPlaceById", args);
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

            object[] args = { inputLookupName.Text, null };
            List<TransitPlace> list = SessionManager.GetCachedCollection<TransitPlace>(
                SessionManager.PlaceService, "SearchPlaces", args);

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
            object[] args = { inputCountry.SelectedValue };
            inputState.DataSource = SessionManager.GetCachedCollection<TransitState>(
                SessionManager.LocationService, "GetStatesByCountry", args);
            inputState.DataBind();

            panelSelectCountryState.Update();
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
}

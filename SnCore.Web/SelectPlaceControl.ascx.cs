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
using Microsoft.Web.UI;

public partial class SelectPlaceControl : Control
{
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
        try
        {
            PageManager.SetDefaultButton(buttonLookup, panelLookup.Controls);
            PageManager.SetDefaultButton(savePlace, panelAdd.Controls);

            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitAccountPlaceType());
                types.AddRange(SessionManager.GetCachedCollection<TransitPlaceType>(PlaceService, "GetPlaceTypes", null));
                selectType.DataSource = types;
                selectType.DataBind();

                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(LocationService, "GetCountries", null);
                inputCountry.DataBind();

                inputCountry_SelectedIndexChanged(sender, e);

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
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void savePlace_Click(object sender, EventArgs e)
    {
        try
        {
            Place = new TransitPlace();
            Place.Type = selectType.SelectedValue;
            if (string.IsNullOrEmpty(Place.Type))
            {
                throw new Exception("Missing type.");
            }
            Place.Name = inputName.Text;
            Place.Country = inputCountry.SelectedValue;
            Place.City = inputCity.Text;
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
            if (Choose != null) Choose(sender, e);
            panelSelectPlace.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
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

    public void lookupChoose_Command(object sender, CommandEventArgs e)
    {
        try
        {
            object[] args = { int.Parse(e.CommandArgument.ToString()) };
            Place = SessionManager.GetCachedItem<TransitPlace>(PlaceService, "GetPlaceById", args);
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
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void buttonLookup_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(inputLookupName.Text))
            {
                labelLookup.Text = "Please enter a name.";
                return;
            }

            object[] args = { inputLookupName.Text, null };
            List<TransitPlace> list = SessionManager.GetCachedCollection<TransitPlace>(
                PlaceService, "SearchPlaces", args);

            gridLookupPlaces.DataSource = list;
            gridLookupPlaces.DataBind();

            if (list.Count == 0)
            {
                labelLookup.Text = string.Format("Cannot find any place matching '{0}'.",
                    base.Render(inputLookupName.Text));
            }

            panelSelectPlace.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void inputCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            object[] args = { inputCountry.SelectedValue };
            inputState.DataSource = SessionManager.GetCachedCollection<TransitState>(LocationService, "GetStatesByCountry", args);
            inputState.DataBind();

            panelSelectCountryState.Update();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

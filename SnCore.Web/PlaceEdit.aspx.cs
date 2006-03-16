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

public partial class PlaceEdit : AuthenticatedPage
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

        public SelectLocationEventArgs(TransitCity city)
            : this(city.Country, city.State, city.Name)
        {

        }

        public SelectLocationEventArgs(TransitPlace place)
            : this(place.Country, place.State, place.City)
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

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);

            gridPlaceNamesManage.OnGetDataSource += new EventHandler(gridPlaceNamesManage_OnGetDataSource);

            if (!IsPostBack)
            {
                gridPlaceNamesManage_OnGetDataSource(sender, e);
                gridPlaceNamesManage.DataBind();
            }

            if (!IsPostBack)
            {
                ArrayList types = new ArrayList();
                types.Add(new TransitAccountPlaceType());
                types.AddRange(PlaceService.GetPlaceTypes());
                selectType.DataSource = types;
                selectType.DataBind();

                inputCountry.DataSource = LocationService.GetCountries();
                inputCountry.DataBind();

                if (RequestId > 0)
                {
                    TransitPlace place = PlaceService.GetPlaceById(RequestId);
                    inputName.Text = place.Name;
                    inputDescription.Text = place.Description;
                    inputCrossStreet.Text = place.CrossStreet;
                    inputEmail.Text = place.Email;
                    inputFax.Text = place.Fax;
                    inputPhone.Text = place.Phone;
                    inputStreet.Text = place.Street;
                    inputWebsite.Text = place.Website;
                    inputZip.Text = place.Zip;
                    selectType.Items.FindByValue(place.Type).Selected = true;
                    SelectLocation(sender, new SelectLocationEventArgs(place));
                    linkPlaceId.Text = Renderer.Render(place.Name);
                }
                else
                {
                    panelPlaceAltName.Visible = false;
                    inputCountry_SelectedIndexChanged(sender, e);
                    string type = Request.QueryString["type"];
                    if (!string.IsNullOrEmpty(type)) selectType.Items.FindByValue(type).Selected = true;

                    string name = Request.QueryString["name"];
                    if (!string.IsNullOrEmpty(name)) inputName.Text = name;

                    string city = Request.QueryString["city"];
                    if (!string.IsNullOrEmpty(city))
                    {
                        TransitCity t_city = LocationService.GetCityByTag(city);
                        if (t_city != null)
                        {
                            SelectLocation(sender, new SelectLocationEventArgs(t_city));
                        }
                        else
                        {
                            inputCity.Text = city;
                        }
                    }
                    else
                    {
                        SelectLocation(sender, new SelectLocationEventArgs(SessionManager.Account));
                    }

                    linkPlaceId.Text = "New Place";
                }
            }
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
            inputCity.Text = e.City;
        }
        catch
        {

        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
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
            t.Website = inputWebsite.Text;
            t.Zip = inputZip.Text;
            t.City = inputCity.Text;
            t.State = inputState.SelectedValue;
            t.Country = inputCountry.SelectedValue;
            int place_id = PlaceService.CreateOrUpdatePlace(SessionManager.Ticket, t);
            Redirect(string.Format("PlaceView.aspx?id={0}", place_id));
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
            inputState.DataSource = LocationService.GetStatesByCountry(
                inputCountry.SelectedValue);
            inputState.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void gridPlaceNamesManage_OnGetDataSource(object sender, EventArgs e)
    {
        gridPlaceNamesManage.DataSource = PlaceService.GetPlaceNames(RequestId);
    }

    public void altname_save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitPlaceName tn = new TransitPlaceName();
            tn.Name = inputAltName.Text;
            tn.PlaceId = RequestId;
            PlaceService.CreateOrUpdatePlaceName(SessionManager.Ticket, tn);
            ReportInfo("Alternate name added.");
            gridPlaceNamesManage_OnGetDataSource(sender, e);
            gridPlaceNamesManage.DataBind();
            inputAltName.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    private enum Cells
    {
        id = 0
    }

    public void gridPlaceNamesManage_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            int id = int.Parse(e.Item.Cells[(int)Cells.id].Text);
            switch (e.CommandName)
            {
                case "Delete":
                    PlaceService.DeletePlace(SessionManager.Ticket, id);
                    ReportInfo("Alternate place name deleted.");
                    gridPlaceNamesManage.CurrentPageIndex = 0;
                    gridPlaceNamesManage_OnGetDataSource(sender, e);
                    gridPlaceNamesManage.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

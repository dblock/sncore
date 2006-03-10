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

public partial class SystemCityEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                inputCountry.DataSource = LocationService.GetCountries();
                inputCountry.DataBind();

                if (RequestId > 0)
                {
                    TransitCity tc = LocationService.GetCityById(RequestId);
                    inputName.Text = tc.Name;
                    inputTag.Text = tc.Tag;
                    inputCountry.Items.FindByValue(tc.Country).Selected = true;
                    inputCountry_SelectedIndexChanged(sender, e);
                    inputState.Items.FindByValue(tc.State).Selected = true;
                }
                else
                {
                    inputCountry_SelectedIndexChanged(sender, e);
                }
            }
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
            inputState.DataSource = LocationService.GetStatesByCountry(inputCountry.SelectedValue);
            inputState.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            TransitCity tc = new TransitCity();
            tc.Name = inputName.Text;
            tc.Id = RequestId;
            tc.Tag = inputTag.Text;
            tc.Country = inputCountry.SelectedValue;
            tc.State = inputState.SelectedValue;
            if (string.IsNullOrEmpty(tc.State) && inputState.Items.Count > 0)
            {
                throw new Exception("State is required.");
            }
            LocationService.AddCity(SessionManager.Ticket, tc);
            Redirect("SystemCitiesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

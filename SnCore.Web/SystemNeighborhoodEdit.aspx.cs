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

public partial class SystemNeighborhoodEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            PageManager.SetDefaultButton(mergeLookup, panelMerge.Controls);

            if (!IsPostBack)
            {

                object[] c_args = { null };
                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(LocationService, "GetCountries", c_args);
                inputCountry.DataBind();

                if (RequestId > 0)
                {
                    TransitNeighborhood tc = LocationService.GetNeighborhoodById(RequestId);
                    inputName.Text = tc.Name;
                    inputCountry.Items.FindByValue(tc.Country).Selected = true;
                    inputCountry_SelectedIndexChanged(sender, e);
                    inputState.Items.FindByValue(tc.State).Selected = true;
                    inputState_SelectedIndexChanged(sender, e);
                    inputCity.Items.FindByValue(tc.City).Selected = true;
                }
                else
                {
                    inputCountry_SelectedIndexChanged(sender, e);
                    panelMerge.Visible = false;
                }
            }
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
            object[] args = { inputCountry.SelectedValue, inputState.SelectedValue};
            inputCity.DataSource = SessionManager.GetCachedCollection<TransitCity>(LocationService, "GetCitiesByLocation", args);
            inputCity.DataBind();
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
            TransitNeighborhood tc = new TransitNeighborhood();
            tc.Name = inputName.Text;
            tc.Id = RequestId;
            tc.Country = inputCountry.SelectedValue;
            tc.State = inputState.SelectedValue;
            tc.City = inputCity.SelectedValue;
            if (string.IsNullOrEmpty(tc.City))
            {
                throw new Exception("City is required.");
            }
            LocationService.AddNeighborhood(SessionManager.Ticket, tc);
            Redirect("SystemNeighborhoodsManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void mergeLookup_Click(object sender, EventArgs e)
    {
        try
        {
            gridMergeLookup.CurrentPageIndex = 0;
            gridMergeLookup.DataSource = LocationService.SearchNeighborhoodsByName(inputMergeWhat.Text);
            gridMergeLookup.DataBind();
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void gridMergeLookup_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Merge":
                    int count = LocationService.MergeNeighborhoods(SessionManager.Ticket, 
                        RequestId, int.Parse(e.CommandArgument.ToString()));
                    ReportInfo(string.Format("Merged {0} records.", count));
                    mergeLookup_Click(source, e);
                    break;
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

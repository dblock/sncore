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

public partial class AccountAddressEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                TransitAccountAddress tw = null;

                int id = RequestId;

                if (id > 0)
                {
                    tw = AccountService.GetAccountAddressById(SessionManager.Ticket, id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputApt.Text = Renderer.Render(tw.Apt);
                    inputStreet.Text = Renderer.Render(tw.Street);
                    inputCity.Text = Renderer.Render(tw.City);
                    inputZip.Text = Renderer.Render(tw.Zip);
                }

                ArrayList countries = new ArrayList();
                if (tw == null || tw.Country.Length == 0) countries.Add(new TransitCountry());
                countries.AddRange(SessionManager.GetCachedCollection<TransitCountry>(LocationService, "GetCountries", null));

                ArrayList states = new ArrayList();
                if (tw == null || tw.State.Length == 0) states.Add(new TransitState());
                states.AddRange(SessionManager.GetCachedCollection<TransitState>(LocationService, "GetStates", null));

                inputCountry.DataSource = countries;
                inputCountry.DataBind();

                inputState.DataSource = states;
                inputState.DataBind();

                if (tw != null)
                {
                    inputCountry.Items.FindByValue(tw.Country).Selected = true;
                    inputState.Items.FindByValue(tw.State).Selected = true;
                }
            }
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
            TransitAccountAddress tw = new TransitAccountAddress();
            tw.Name = inputName.Text;
            tw.Street = inputStreet.Text;
            tw.Apt = inputApt.Text;
            tw.City = inputCity.Text;
            tw.Country = inputCountry.SelectedValue;
            tw.State = inputState.SelectedValue;
            tw.Zip = inputZip.Text;
            tw.Id = RequestId;
            AccountService.AddAccountAddress(SessionManager.Ticket, tw);
            Redirect("AccountAddressesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

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

public partial class SystemStateEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                int id = RequestId;

                inputCountry.DataSource = SessionManager.GetCachedCollection<TransitCountry>(LocationService, "GetCountries", null);
                inputCountry.DataBind();

                if (id > 0)
                {
                    TransitState tw = LocationService.GetStateById(id);
                    inputName.Text = Renderer.Render(tw.Name);
                    inputCountry.Items.FindByValue(tw.Country).Selected = true;
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
            TransitState tw = new TransitState();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            tw.Country = inputCountry.SelectedItem.Value;
            LocationService.AddState(SessionManager.Ticket, tw);
            Redirect("SystemStatesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

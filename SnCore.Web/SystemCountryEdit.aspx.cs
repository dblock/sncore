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

public partial class SystemCountryEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                int id = RequestId;

                if (id > 0)
                {
                    TransitCountry tw = LocationService.GetCountryById(id);
                    inputName.Text = Renderer.Render(tw.Name);
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
            TransitCountry tw = new TransitCountry();
            tw.Name = inputName.Text;
            tw.Id = RequestId;
            LocationService.AddCountry(SessionManager.Ticket, tw);
            Redirect("SystemCountriesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

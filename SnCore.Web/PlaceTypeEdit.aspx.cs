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
using SnCore.WebServices;
using SnCore.Services;

public partial class PlaceTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (RequestId > 0)
            {
                TransitPlaceType t = PlaceService.GetPlaceTypeById(RequestId);
                inputName.Text = t.Name;
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
            TransitPlaceType t = new TransitPlaceType();
            t.Name = inputName.Text;
            t.Id = RequestId;
            PlaceService.CreateOrUpdatePlaceType(SessionManager.Ticket, t);
            Redirect("PlaceTypesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

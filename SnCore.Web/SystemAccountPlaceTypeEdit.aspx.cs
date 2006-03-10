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

public partial class SystemAccountPlaceTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (RequestId > 0)
            {
                TransitAccountPlaceType t = PlaceService.GetAccountPlaceTypeById(RequestId);
                inputName.Text = t.Name;
                inputDescription.Text = t.Description;
                inputCanWrite.Checked = t.CanWrite;
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
            TransitAccountPlaceType t = new TransitAccountPlaceType();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.CanWrite = inputCanWrite.Checked;
            t.Id = RequestId;
            PlaceService.CreateOrUpdateAccountPlaceType(SessionManager.Ticket, t);
            Redirect("SystemAccountPlaceTypesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

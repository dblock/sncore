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

public partial class SystemPictureTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {

                if (RequestId > 0)
                {
                    TransitPictureType t = SystemService.GetPictureTypeById(RequestId);
                    inputName.Text = t.Name;
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
            TransitPictureType t = new TransitPictureType();
            t.Name = inputName.Text;
            t.Id = RequestId;
            SystemService.CreateOrUpdatePictureType(SessionManager.Ticket, t);
            Redirect("SystemPictureTypesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

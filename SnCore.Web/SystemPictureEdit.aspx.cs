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

public partial class SystemPictureEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                inputType.DataSource = SystemService.GetPictureTypes();
                inputType.DataBind();

                if (RequestId > 0)
                {
                    TransitPicture t = SystemService.GetPictureById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    inputType.Items.FindByValue(t.Type).Selected = true;
                    imageThumbnail.ImageUrl = string.Format("SystemPictureThumbnail.aspx?id={0}&CacheDuration=0", t.Id);
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
            TransitPicture t = new TransitPicture();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.Type = inputType.SelectedValue;
            t.Id = RequestId;
            SystemService.CreateOrUpdatePicture(SessionManager.Ticket, t);
            Redirect("SystemPicturesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

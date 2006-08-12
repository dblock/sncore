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
using System.Drawing;
using SnCore.Tools.Drawing;
using System.IO;

public partial class SystemAttributeEdit : AuthenticatedPage
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
                    TransitAttribute t = SystemService.GetAttributeById(RequestId);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    inputDefaultUrl.Text = t.DefaultUrl;
                    inputDefaultValue.Text = t.DefaultValue;
                    imageBitmap.ImageUrl = string.Format("SystemAttribute.aspx?id={0}&CacheDuration=0", t.Id);
                    imageBitmap.Visible = t.HasBitmap;
                }
                else
                {
                    imageBitmap.Visible = false;
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
            TransitAttributeWithBitmap t = new TransitAttributeWithBitmap();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.DefaultUrl = inputDefaultUrl.Text;
            t.DefaultValue = inputDefaultValue.Text;
            t.Id = RequestId;
            if (inputBitmap.HasFile) t.Bitmap = new ThumbnailBitmap(inputBitmap.FileContent, new Size(16, 16)).Bitmap;
            SystemService.CreateOrUpdateAttribute(SessionManager.Ticket, t);
            Redirect("SystemAttributesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

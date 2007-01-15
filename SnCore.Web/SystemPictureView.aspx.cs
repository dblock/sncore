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

public partial class SystemPictureView : AuthenticatedPage
{
    public void Page_Load()
    {
        if (!IsPostBack)
        {
            TransitPicture p = SessionManager.ObjectService.GetPictureById(
                SessionManager.Ticket, RequestId);
            inputPicture.Src = string.Format("SystemPicture.aspx?id={0}", RequestId);
            inputName.Text = Renderer.Render(p.Name);
            inputDescription.Text = Renderer.Render(p.Description);
            inputCreated.Text = Adjust(p.Created).ToString();
            inputType.Text = Renderer.Render(p.Type);
        }

    }
}

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

public partial class PlacePictureView : Page
{
    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                TransitPlacePicture p = PlaceService.GetPlacePictureById(RequestId);
                inputPicture.Src = string.Format("PlacePicture.aspx?id={0}", RequestId);
                inputName.Text = Renderer.Render(p.Name);
                inputDescription.Text = Renderer.Render(p.Description);
                inputCreated.Text = Adjust(p.Created).ToString();

                TransitPlace l = PlaceService.GetPlaceById(p.PlaceId);
                this.Title = string.Format("{0} {1}", Renderer.Render(l.Name), Renderer.Render(p.Name));
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }

    }
}

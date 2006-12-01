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

public partial class PlacePictureEdit : AuthenticatedPage
{
    public int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                TransitPlace p = PlaceService.GetPlaceById(SessionManager.Ticket, PlaceId);
                linkBack.NavigateUrl = linkSection.NavigateUrl = string.Format("PlacePicturesManage.aspx?id={0}", p.Id);
                linkPlace.Text = Renderer.Render(p.Name);
                linkPlace.NavigateUrl = string.Format("PlaceView.aspx?id={0}", p.Id);

                if (RequestId > 0)
                {
                    TransitPlacePicture t = PlaceService.GetPlacePictureById(RequestId);
                    linkItem.Text = Renderer.Render(t.Name);
                    inputName.Text = t.Name;
                    inputDescription.Text = t.Description;
                    imageFull.ImageUrl = string.Format("PlacePicture.aspx?id={0}&CacheDuration=0", t.Id);
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
            TransitPlacePicture t = new TransitPlacePicture();
            t.Name = inputName.Text;
            t.Description = inputDescription.Text;
            t.PlaceId = PlaceId;
            t.Id = RequestId;
            PlaceService.CreateOrUpdatePlacePicture(SessionManager.Ticket, t);
            Redirect(string.Format("PlacePicturesManage.aspx?id={0}", PlaceId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}

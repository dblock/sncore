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
using SnCore.Services;
using SnCore.Tools.Web;

public partial class EmailPlacePicture : AuthenticatedPage
{
    private TransitPlacePicture mPlacePicture;

    public TransitPlacePicture PlacePicture
    {
        get
        {
            if (mPlacePicture == null)
            {
                mPlacePicture = SessionManager.PlaceService.GetPlacePictureById(
                    SessionManager.Ticket, RequestId);
            }
            return mPlacePicture;
        }
    }

    private TransitPlace mPlace = null;

    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.PlaceService.GetPlaceById(SessionManager.Ticket, PlacePicture.PlaceId);
            }
            return mPlace;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("New picture uploaded to {0}", Renderer.Render(Place.Name));
    }
}


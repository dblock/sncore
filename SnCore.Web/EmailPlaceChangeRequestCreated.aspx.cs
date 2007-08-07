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

public partial class EmailPlaceChangeRequestCreated : AuthenticatedPage
{
    private TransitPlaceChangeRequest mPlaceChangeRequest;

    public TransitPlaceChangeRequest PlaceChangeRequest
    {
        get
        {
            if (mPlaceChangeRequest == null)
            {
                mPlaceChangeRequest = SessionManager.PlaceService.GetPlaceChangeRequestById(
                    SessionManager.Ticket, RequestId);
            }
            return mPlaceChangeRequest;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("{0} has submitted changes to {1}",
            Renderer.Render(PlaceChangeRequest.AccountName),
            Renderer.Render(PlaceChangeRequest.PlaceName));
    }
}


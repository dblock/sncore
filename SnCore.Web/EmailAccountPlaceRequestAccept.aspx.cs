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

public partial class EmailAccountPlaceRequestAccept : AuthenticatedPage
{
    private TransitAccountPlaceRequest mAccountPlaceRequest;

    public TransitAccountPlaceRequest AccountPlaceRequest
    {
        get
        {
            if (mAccountPlaceRequest == null)
            {
                mAccountPlaceRequest = SessionManager.PlaceService.GetAccountPlaceRequestById(
                    SessionManager.Ticket, RequestId);
            }
            return mAccountPlaceRequest;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        Title = string.Format("Ownership request for {0} accepted", Renderer.Render(AccountPlaceRequest.PlaceName));
        panelMessage.Visible = !string.IsNullOrEmpty(Request.QueryString["message"]);
    }
}


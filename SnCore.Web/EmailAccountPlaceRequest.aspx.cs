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

public partial class EmailAccountPlaceRequest : AuthenticatedPage
{
    private TransitAccountPlaceRequest mAccountPlaceRequest;

    public TransitAccountPlaceRequest AccountPlaceRequest
    {
        get
        {
            try
            {
                if (mAccountPlaceRequest == null)
                {
                    mAccountPlaceRequest = SessionManager.PlaceService.GetAccountPlaceRequestById(
                        SessionManager.Ticket, RequestId);
                }
            }
            catch (Exception ex)
            {
                ReportException(ex);
            }

            return mAccountPlaceRequest;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Title = string.Format("{0} is claiming ownership of {1}", 
                Renderer.Render(AccountPlaceRequest.AccountName),
                Renderer.Render(AccountPlaceRequest.PlaceName));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}


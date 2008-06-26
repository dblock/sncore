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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.WebServices;
using SnCore.Services;

public partial class PlaceWebsitesViewControl : Control
{
    private TransitPlace mPlace = null;

    public int PlaceId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "PlaceId", 0);
        }
        set
        {
            ViewState["PlaceId"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        PlaceWebsites.OnGetDataSource += new EventHandler(PlaceWebsites_OnGetDataSource);
        if (!IsPostBack)
        {
            linkNew.NavigateUrl = string.Format("PlaceWebsiteEdit.aspx?pid={0}", PlaceId);
            GetData(sender, e);
        }
    }

    public TransitPlace Place
    {
        get
        {
            if (mPlace == null)
            {
                mPlace = SessionManager.GetPrivateInstance<TransitPlace, int>(
                    PlaceId, SessionManager.PlaceService.GetPlaceById);
            }

            return mPlace;
        }
    }

    public bool CanWrite(int account_id)
    {
        return account_id == SessionManager.AccountId || Place.CanWrite || SessionManager.IsAdministrator;
    }

    public void linkDelete_Command(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitPlaceWebsite>(int.Parse(e.CommandArgument.ToString()),
                    SessionManager.PlaceService.DeletePlaceWebsite);
                GetData(sender, e);
                break;
        }
    }

    void GetData(object sender, EventArgs e)
    {
        PlaceWebsites.CurrentPageIndex = 0;
        PlaceWebsites.VirtualItemCount = SessionManager.GetCount<TransitPlaceWebsite, int>(
            PlaceId, SessionManager.PlaceService.GetPlaceWebsitesCount);
        PlaceWebsites_OnGetDataSource(sender, e);
        PlaceWebsites.DataBind();
    }

    void PlaceWebsites_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions(PlaceWebsites.PageSize, PlaceWebsites.CurrentPageIndex);
        PlaceWebsites.DataSource = SessionManager.GetCollection<TransitPlaceWebsite, int>(
            PlaceId, options, SessionManager.PlaceService.GetPlaceWebsites);
        panelGrid.Update();
    }
}
